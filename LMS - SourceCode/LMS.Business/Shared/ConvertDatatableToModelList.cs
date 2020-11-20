using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LMS.Business.Shared
{
    public class ConvertDatatableToModelList
    {
        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        public static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName && !Convert.IsDBNull(dr[column.ColumnName]))
                        if (!Convert.IsDBNull(dr[column.ColumnName]))
                        {
                            pro.SetValue(obj, dr[column.ColumnName],null);
                        }
                        else
                        continue;
                }
            }
            return obj;
        }

    }
}
