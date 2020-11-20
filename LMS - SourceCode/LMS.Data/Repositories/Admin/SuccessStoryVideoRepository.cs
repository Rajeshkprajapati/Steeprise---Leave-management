using LMS.Data.Helper;
using LMS.Data.Interfaces.Admin;
using LMS.Model.DataViewModel.Admin.SuccessStory;
using LMS.Utility.Exceptions;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace LMS.Data.Repositories.Admin
{
    public class SuccessStoryVideoRepository : ISuccessStoryVideoRepository
    {
        private readonly string connectionString;
        public SuccessStoryVideoRepository(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionStrings:NassComLMS.B"];
        }
        public DataTable GetSuccessStoryVid()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    var Data =
                        SqlHelper.ExecuteReader
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetAllScucessStoryVid"
                            );
                    if (null != Data && Data.HasRows)
                    {
                        var dt = new DataTable();
                        dt.Load(Data);
                        //return dt.Rows[0];
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

        public bool InsertUpdateSuccessStoryVid(SuccessStoryVideoViewModel successStory)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                        new SqlParameter("@SSId",successStory.Id),
                        new SqlParameter("@SSTitle",successStory.Title),
                        new SqlParameter("@SSFileName",successStory.VideoFile),
                        new SqlParameter("@SSType",successStory.Type),
                        new SqlParameter("@UpdatedBy",successStory.UpdatedBy),
                        new SqlParameter("@DisplayOrder",successStory.DisplayOrder),
                     };
                    var data =
                        SqlHelper.ExecuteNonQuery
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_InsertUpdateSuccessStoryVid",
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
            throw new DataNotUpdatedException("Unable to update data");
        }
        public bool DeleteSuccessStoryVid(string id, string deletedBy)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                        new SqlParameter("@SSId",id),
                        new SqlParameter("@UpdatedBy",deletedBy),
                    };
                    var data =
                       SqlHelper.ExecuteNonQuery
                       (
                           connection,
                           CommandType.StoredProcedure,
                           "usp_DeleteSuccessStoryVid",
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
            throw new DataNotUpdatedException("Unable to delete data");
        }
    }
}
