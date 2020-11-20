using LMS.Data.DataModel.Shared;
using LMS.Data.Helper;
using LMS.Data.Interfaces.TrainingPartner;
using LMS.Utility.Exceptions;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LMS.Data.Repositories.TrainingPartner
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly string connectionString;
        public DashboardRepository(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionStrings:NassComLMS.B"];
        }

        public DataTable GetCandidates(int tpId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {

                    SqlParameter[] parameters = new SqlParameter[] {
                        new SqlParameter("@TpId",tpId)
                    };
                    var result =
                        SqlHelper.ExecuteDataset
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetTrainingPartnerCandidates",
                            parameters
                            );
                    if (null != result && result.Tables.Count > 0)
                    {
                        return result.Tables[0];
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("Candidates not found, please contact your tech deck.");
        }

        public DataTable GetCandidateDetails(int userid)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {

                    SqlParameter[] parameters = new SqlParameter[] {
                        new SqlParameter("@userid",userid)
                    };
                    var result =
                        SqlHelper.ExecuteDataset
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetCandidateDetailByUserid",
                            parameters
                            );
                    if (null != result && result.Tables.Count > 0)
                    {
                        return result.Tables[0];
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("Candidates not found, please contact your tech deck.");
        }
        public bool DeleteCandidate(int userid)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {

                    SqlParameter[] parameters = new SqlParameter[] {
                        new SqlParameter("@userid",userid)
                    };
                    var result =
                        SqlHelper.ExecuteNonQuery
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_DeleteCandidateByUserid",
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
            return false;
        }
        public bool UpdateCandidateDetails(UserModel model)
        {
            using (var connection = new SqlConnection(connectionString)) {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                        new SqlParameter("@userid",model.UserId),
                        new SqlParameter("@firstname",model.FirstName),
                        new SqlParameter("@lastname",model.LastName),
                        new SqlParameter("@password",model.Password),
                    };
                    var result =
                        SqlHelper.ExecuteNonQuery
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_updateCandidateByUserid",
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
            return false;
        }
    }
}
