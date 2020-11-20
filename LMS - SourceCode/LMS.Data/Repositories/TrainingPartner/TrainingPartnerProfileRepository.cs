using LMS.Data.DataModel.Shared;
using LMS.Data.Helper;
using LMS.Data.Interfaces.TrainingPartner;
using LMS.Utility.Exceptions;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LMS.Data.Repositories.TrainingPartner
{
    public class TrainingPartnerProfileRepository : ITrainingPartnerProfileRepository
    {
        private readonly string connectionString;
        public TrainingPartnerProfileRepository(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionStrings:NassComLMS.B"];
        }
        public DataTable GetTPDetail(int userid)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                        new SqlParameter("@userid",userid),
                    };
                    var result =
                        SqlHelper.ExecuteReader
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GETTPDetails",
                            parameters
                            );
                    if (result != null && result.HasRows)
                    {
                        var dt = new DataTable();
                        dt.Load(result);
                        return dt;
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("Unable to Find Training Partner");
        }
        public bool UpdateTPDetail(UserModel user)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                        new SqlParameter("@userid",user.UserId),
                        new SqlParameter("@firstname",user.FirstName),
                        new SqlParameter("@lastname",user.LastName),
                        new SqlParameter("@email",user.Email),
                        new SqlParameter("@picture",user.ProfilePic),
                    };
                    var result =
                        SqlHelper.ExecuteNonQuery
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_UpdateTPDetails",
                            parameters
                            );
                    if (result > 0)
                    {
                        return true;
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotUpdatedException("Unable to Update Training Partner Data");
        }
    }
}
