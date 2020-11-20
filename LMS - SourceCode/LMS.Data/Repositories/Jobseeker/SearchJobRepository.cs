using System.Data;
using System.Data.SqlClient;
using LMS.Data.DataModel.Shared;
using LMS.Data.Helper;
using LMS.Data.Interfaces.Jobseeker;
using LMS.Utility.Exceptions;
using Microsoft.Extensions.Configuration;

namespace LMS.Data.Repositories.Jobseeker
{
    public class SearchJobRepository : ISearchJobRepository
    {
        private readonly string connectionString;

        public SearchJobRepository(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionStrings:NassComLMS.B"];
        }

        public DataTable GetSearchJobList(JobSearchModel searches, int UserId, int quarterStartMonth)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {

                    SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@jobTitle",searches.JobRole),
                new SqlParameter("@jobCategory",searches.JobCategory),
                new SqlParameter("@Experience",searches.Experiance),
                new SqlParameter("@city",searches.City),
                new SqlParameter("@User",UserId),
                new SqlParameter("@Skills",searches.Skills),
                new SqlParameter("@FinancialYearStratMonth",quarterStartMonth),
                new SqlParameter("@CompanyUserId",searches.CompanyUserId)
            };
                    var searchList =
                        SqlHelper.ExecuteDataset
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetSearchList",
                            parameters
                            );
                    if (null != searchList && searchList.Tables.Count > 0)
                    {
                        return searchList.Tables[0];
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("Data Not found");
        }
    }
}
