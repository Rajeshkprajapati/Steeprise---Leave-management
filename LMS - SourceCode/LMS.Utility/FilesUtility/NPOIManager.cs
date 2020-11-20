using LMS.Utility.Exceptions;
using Microsoft.AspNetCore.Http;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Data;
using System.IO;
using System.Linq;

namespace LMS.Utility.FilesUtility
{
    public class NPOIManager
    {
        public static void ReadFile(IFormFile file, DataTable hTable, out DataTable dataTable, bool isFileHeadersAsFirstRow = false, string roleName="")
        {
            if (file.Length <= 0)
            {
                throw
                    new FileEmptyException("Data not found in file to insert or update");
            }
            var fileExtension =
                Path.GetExtension(file.FileName).ToLower();
            using (var stream = file.OpenReadStream())
            {
                ISheet sheet = null;
                switch (fileExtension)
                {
                    case ".xls":
                        var hssfwb = new HSSFWorkbook(stream);    //This will read the Excel 97-2000 formats
                        sheet = hssfwb.GetSheetAt(0);   //get first sheet from workbook
                        break;
                    case ".xlsx":
                    case ".csv":
                        var xssfwb = new XSSFWorkbook(stream);    //This will read the Excel 97-2000 formats
                        sheet = xssfwb.GetSheetAt(0);   //get first sheet from workbook
                        break;
                    default:
                        throw new UnSpecifiedFileFormatException("You are uploading file which is not supported in our system.");
                }
                int lastRowNumber = sheet.LastRowNum;
                //if (roleName == "Training Partner")
                //{
                //    //lastRowNumber = 5;
                //    lastRowNumber = sheet.LastRowNum;
                //}
                //else
                //{
                //    lastRowNumber = sheet.LastRowNum;
                //}
                 
                dataTable = new DataTable();

                for (int i = sheet.FirstRowNum; i <= lastRowNumber; i++)
                {
                    var row = sheet.GetRow(i);
                    if (row == null) continue;
                    int lastColumnNumber; /*= row.LastCellNum;*/
                    if (roleName == "Training Partner")
                    {
                        lastColumnNumber = 5;
                    }
                    else {
                        lastColumnNumber = row.LastCellNum;
                    }
                    //int lastColumnNumber = 5;
                    if (i == sheet.FirstRowNum)   // Get Header Row
                    {
                        //  We will create table, based on file header index
                        for (int j = row.FirstCellNum; j < lastColumnNumber; j++)
                        {
                            ////  Can be enumerated using System.Data.DataSetExtensions dll
                            //DataRow header = hTable
                            //    .AsEnumerable()
                            //    .Where(r => Convert.ToInt32(r.Field<string>("DataIndex")) == (j - 1))
                            //    .FirstOrDefault();
                            //  Can be enumerated using simple System.Linq
                            DataRow header = hTable.Rows
                                .Cast<DataRow>()
                                .Where(r => Convert.ToInt32(r.Field<string>("DataIndex").Trim()) == (j))
                                .FirstOrDefault();

                            dataTable.Columns.Add(Convert.ToString(header["DataKey"]).Trim())
                                .DefaultValue = Convert.ToString(header["DefaultValue"]).Trim();
                        }
                    }
                    if (isFileHeadersAsFirstRow || i > sheet.FirstRowNum)
                    {
                        if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                        var newRow = dataTable.NewRow();
                        for (int j = row.FirstCellNum; j < lastColumnNumber; j++)
                        {
                            var dataCell = row.GetCell(j);
                            if (null != dataCell)
                            {
                                if (dataCell.CellType == CellType.Formula)
                                {
                                    switch (dataCell.CachedFormulaResultType)
                                    {
                                        case CellType.Numeric:
                                            newRow[j] = dataCell.NumericCellValue;
                                            break;
                                        case CellType.String:
                                            newRow[j] = dataCell.StringCellValue.Trim();
                                            break;
                                        case CellType.Unknown:
                                        case CellType.Formula:
                                        case CellType.Blank:
                                        case CellType.Boolean:
                                        case CellType.Error:
                                        default:
                                            newRow[j] = dataCell.ToString().Trim();
                                            break;
                                    }
                                }
                                else
                                {
                                    newRow[j] = dataCell.ToString().Trim();
                                }
                            }
                        }
                        dataTable.Rows.Add(newRow);
                    }
                }
            }
        }

        public static IWorkbook CreateExcelWorkBook(DataTable data, FileExtensions fileExtensions, string sheetName)
        {
            IWorkbook workbook;
            switch (fileExtensions)
            {
                case FileExtensions.xls:
                    workbook = new HSSFWorkbook();
                    break;
                case FileExtensions.xlsx:
                default:
                    workbook = new XSSFWorkbook();
                    break;
            }

            ISheet sheet = workbook.CreateSheet(sheetName);

            //  Make Header Row

            IRow headerRow = sheet.CreateRow(0);
            int headerCellIndex = 0;
            foreach (DataColumn column in data.Columns)
            {
                ICell cell = headerRow.CreateCell(headerCellIndex++);
                string colName = column.ColumnName;
                cell.SetCellValue(colName);
                cell.CellStyle = CreateHeaderCellStyle(workbook);
            }

            // Loop Through Data
            int rowIndex = 0;
            foreach (DataRow row in data.Rows)
            {
                IRow dRow = sheet.CreateRow(++rowIndex);
                int dataCellIndex = 0;
                foreach (ICell headerCell in headerRow.Cells)
                {
                    ICell cell = dRow.CreateCell(dataCellIndex++);
                    string cellData = Convert.ToString(row[headerCell.ToString()]);
                    cell.SetCellValue(cellData);
                }
            }
            return workbook;
        }

        private static ICellStyle CreateHeaderCellStyle(IWorkbook workbook)
        {
            var boldFont = workbook.CreateFont();
            boldFont.Boldweight = (short)FontBoldWeight.Bold;
            ICellStyle boldStyle = workbook.CreateCellStyle();
            boldStyle.SetFont(boldFont);

            boldStyle.BorderBottom = BorderStyle.Medium;
            boldStyle.FillForegroundColor = HSSFColor.Grey25Percent.Index;
            boldStyle.FillPattern = FillPattern.SolidForeground;
            return boldStyle;
        }
    }
}
