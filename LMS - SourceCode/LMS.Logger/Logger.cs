using System;
using LMS.Utility;
using System.Data.SqlClient;
using System.Data;
using LMS.Utility.Helpers;
using Newtonsoft.Json;
using LMS.Data.Helper;
//using Microsoft.AspNetCore.Mvc;

namespace LMS.Logger
{

    public class Logger
    {
        private static readonly string connectionString;
        static Logger()
        {
            connectionString = ConfigurationHelper.Config["ConnectionStrings:NassComLMS.B"];
        }
       
        private static void AddToDB(Logtype log, string message, Type classdetails, int userid, Exception exception = null, string details = null)
        {
            try
            {
                NLogger.logger.FileLogger("Testing File Logger");
                string assemblyName = classdetails.Assembly.FullName;
                string className = classdetails.Name;
                string ex = JsonConvert.SerializeObject(exception);
                var connection = new SqlConnection(connectionString);
                SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@logtype",Enum.GetName(typeof(Logtype),log)),
                new SqlParameter("@classinfo",className),
                new SqlParameter("@asseblyinfo",assemblyName),
                new SqlParameter("@message",message),
                new SqlParameter("@userid",userid),
                new SqlParameter("@exception",ex),
                new SqlParameter("@data",details),
                };
                var result =
                    SqlHelper.ExecuteNonQuery
                    (
                        connection,
                        CommandType.StoredProcedure,
                        "usp_WriteToDB",
                        parameters
                        );
                if (result > 0)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                NLogger.logger.FileLogger(ex.Message);
            }
        }

        public static void WriteLog(Logtype log, string message, int userid, Type classdetails)
        {
            AddToDB(log, message, classdetails, userid);
        }
        public static void WriteLog(Logtype log, string message, int userid, Type classdetails, Exception exception)
        {
            AddToDB(log, message, classdetails, userid, exception);
        }
        public static void WriteLog(Logtype log, string message, int userid, Type classdetails, object data)
        {
            string details = JsonConvert.SerializeObject(data);//(json)data;
            AddToDB(log, message, classdetails, userid, null, details);
        }

    }
}
