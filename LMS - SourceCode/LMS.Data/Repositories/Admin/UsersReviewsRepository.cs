using LMS.Data.DataModel.Admin.UserReviews;
using LMS.Data.Helper;
using LMS.Data.Interfaces.Admin;
using LMS.Utility.Exceptions;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;

namespace LMS.Data.Repositories.Admin
{
    public class UsersReviewsRepository : IUsersReviewsRepository
    {
        private readonly string connectionString;
        public UsersReviewsRepository(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionStrings:NassComLMS.B"];
        }
        public DataTable GetUsersReviews()
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
                            "usp_GetSuccessStoryReview"
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
        public bool DeleteUsersReviews(string id, string deletedBy)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {

                    SqlParameter[] parameters = new SqlParameter[] {
                        new SqlParameter("@id",id),
                        new SqlParameter("@UpdatedBy",deletedBy),
                    };
                    var data =
                       SqlHelper.ExecuteNonQuery
                       (
                           connection,
                           CommandType.StoredProcedure,
                           "usp_DeleteSuccessStory",
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
        public bool ApproveUsersReviews(string id, string approvedBy)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {

                    SqlParameter[] parameters = new SqlParameter[] {
                        new SqlParameter("@id",id),
                        new SqlParameter("@ApprovedBy",approvedBy),
                    };
                    var data =
                       SqlHelper.ExecuteNonQuery
                       (
                           connection,
                           CommandType.StoredProcedure,
                           "usp_ApprovesuccessStoryReview",
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
            throw new Exception("Unable to approve data");
        }
        public bool UpdateUsersReviews(UserReviewsModel model, string userid)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                        new SqlParameter("@id",model.Id),
                        new SqlParameter("@UserId",userid),
                        new SqlParameter("@name",model.Name),
                        new SqlParameter("@Email",model.Email),
                        new SqlParameter("@Tagline",model.Tagline),
                        new SqlParameter("@Message",model.Message),
                    };
                    var data =
                       SqlHelper.ExecuteNonQuery
                       (
                           connection,
                           CommandType.StoredProcedure,
                           "usp_UpdateSuccessStory",
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
