using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LMS.Utility.FilesUtility
{
    public class XmlProcessor
    {
        public static DataTable XmlToTable(string filePath)
        {
            var ds = new DataSet();
            ds.ReadXml(filePath);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }
    }
}
