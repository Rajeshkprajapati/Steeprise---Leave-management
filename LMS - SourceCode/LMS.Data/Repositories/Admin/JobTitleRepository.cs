using LMS.Data.DataModel.Admin.JobTitle;
using LMS.Data.Helper;
using LMS.Data.Interfaces.Admin;
using LMS.Utility.Exceptions;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;

namespace LMS.Data.Repositories.Admin
{
    public class JobTitleRepository: IJobTitleRepositroy
    {
        private readonly string connectionString;
        public JobTitleRepository(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionStrings:NassComLMS.B"];
        }

        public DataTable GetJobTitle()
        {
            using(var connection = new SqlConnection(connectionString))
            {
                try
                {
                    var Data =
                        SqlHelper.ExecuteReader
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetAllJobTitle"
                            );
                    if (null != Data && Data.HasRows)
                    {
                        var dt = new DataTable();
                        dt.Load(Data);
                        return dt;
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("Data not found");
        }

        public bool InsertUpdateJobTile(JobTitleModel jobTitle)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@JobTitleId",jobTitle.JobTitleId),
                    new SqlParameter("@JobTitleName",jobTitle.JobTitleName),
                    new SqlParameter("@UpdatedBy",jobTitle.UpdatedBy),
                    };
                    var data =
                        SqlHelper.ExecuteNonQuery
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_InsertUpdateJobTitle",
                            parameters
                            );
                    if (data > 0)
                    {
                        return true;
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new Exception("Unable to update data");
        }

        public bool DeleteJobTitle(string jobTileId, string deletedBy)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@JobTitleId",jobTileId),
                    new SqlParameter("@UpdatedBy",deletedBy),
                    };
                    var data =
                       SqlHelper.ExecuteNonQuery
                       (
                           connection,
                           CommandType.StoredProcedure,
                           "usp_DeleteJobTitle",
                           parameters
                           );
                    if (data > 0)
                    {
                        return true;
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new Exception("Unable to delete data");
        }
    }

}
