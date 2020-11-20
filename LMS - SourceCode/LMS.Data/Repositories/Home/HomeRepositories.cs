using LMS.Data.Helper;
using LMS.Data.Interfaces.Home;
using LMS.Model.DataViewModel.Home;
using LMS.Utility.Exceptions;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LMS.Data.Repositories.Home
{
    public class HomeRepositories : IHomeRepositories
    {

        private readonly string connectionString;
        private readonly string ContactUsEmail;
        private readonly string TalentConnect;
        private readonly string CandidateBulkUploadPDFGuide;
        private readonly string TPRegistrationPDFGuide;
        public HomeRepositories(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionStrings:NassComLMS.B"];
            ContactUsEmail = configuration["ContactUsMail"];
            TalentConnect = configuration["TalentConnectGuide"];
            CandidateBulkUploadPDFGuide = configuration["CandidateBulkUploadPDFGuide"];
            TPRegistrationPDFGuide = configuration["TPRegistrationPDFGuide"];
        }
        public DataTable GetCityListDetail()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    //SqlParameter[] parameters = new SqlParameter[] {
                    //   new SqlParameter("@cityFirstChar", cityFirstChar)
                    //};
                    var city =
                        SqlHelper.ExecuteReader
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetCitiesWithoutState"
                            );
                    if (null != city && city.HasRows)
                    {
                        var dt = new DataTable();
                        dt.Load(city);
                        return dt;
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new InvalidUserCredentialsException("Can not execute query");
        }
        public DataTable GetCityListByChar(string cityFirstChar)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {

                    SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@cityFirstChar", cityFirstChar)
            };
                    var city =
                        SqlHelper.ExecuteReader
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetCitiesListWithFirstChar",
                            parameters
                            );
                    if (null != city && city.HasRows)
                    {
                        var dt = new DataTable();
                        dt.Load(city);
                        return dt;
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("Can not execute query");
        }

        public DataTable GetJobListByChar(string jobFirstChar)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {

                    SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@jobFirstChar", jobFirstChar)
            };
                    var jobs =
                        SqlHelper.ExecuteReader
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetJobListWithFirstChar",
                            parameters
                            );
                    if (null != jobs && jobs.HasRows)
                    {
                        var dt = new DataTable();
                        dt.Load(jobs);
                        return dt;
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("Can not execute query");
        }

        public DataTable GetCityHasJobPostId()
        {
            using (var connection = new SqlConnection(connectionString))
            {

                try
                {
                    var city =
                        SqlHelper.ExecuteDataset
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetCitiesWithJobPostUserId"
                            //"usp_GetCities"
                            );
                    if (null != city && city.Tables.Count>0)
                    {
                        return city.Tables[0];
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("Can not execute query");
        }

        public DataTable GetCitiesWithJobSeekerInfo()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {

                    //SqlParameter[] parameters = new SqlParameter[] {
                    //   new SqlParameter("@cityFirstChar", cityFirstChar)
                    //};
                    var city =
                        SqlHelper.ExecuteReader
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetCitiesWithJobSeekerInfo"
                            );
                    if (null != city && city.HasRows)
                    {
                        var dt = new DataTable();
                        dt.Load(city);
                        return dt;
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("Can not execute query");
        }

        public DataTable GetSuccessStory()
        {
            using (var connection = new SqlConnection(connectionString))
            {

                try
                {

                    var successStory =
                        SqlHelper.ExecuteDataset
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetSuccessStory"
                            );
                    if (null != successStory && successStory.Tables.Count>0)
                    {
                        return successStory.Tables[0];
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("Can not execute query");
        }

        public DataTable GetAplliedJobs(int userid)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {

                    SqlParameter[] parameters = new SqlParameter[]
                    {
                new SqlParameter("@userid",userid),
                    };
                    var appliedjobs =
                        SqlHelper.ExecuteDataset(
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetAppliedJobs",
                            parameters
                            );
                    if (appliedjobs != null && appliedjobs.Tables.Count>0)
                    {
                        return appliedjobs.Tables[0];
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            return null;//user has not applied any jobs
        }
        public bool PostSuccsessStory(SuccessStoryViewModel successStory)
        {
            using (var connection = new SqlConnection(connectionString))
            {

                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                 new SqlParameter("@Name",successStory.name),
                 new SqlParameter("@Email",successStory.Email),
                 new SqlParameter("@Message",successStory.Message),
                 new SqlParameter("@UserId",successStory.UserId),
                 //new SqlParameter("@Tagline",successStory.Tagline),
              };
                    var result =
                        SqlHelper.ExecuteNonQuery
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_PostSuccessStory",
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
            throw new UserCanNotPostData("Unable to post data");
        }

        public DataTable GetFeaturedJobs()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    var successStory =
                        SqlHelper.ExecuteDataset
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetFeaturedJobs"
                            );
                    if (null != successStory && successStory.Tables.Count>0)
                    {
                        return successStory.Tables[0];
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("Can not execute query");
        }

        public DataTable ViewAllFeaturedJobs()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {

                    var successStory =
                        SqlHelper.ExecuteReader
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_ViewAllFeaturedJobs"
                            );
                    if (null != successStory && successStory.HasRows)
                    {
                        var dt = new DataTable();
                        dt.Load(successStory);
                        return dt;
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("Can not execute query");
        }
        public DataTable PopulerSearchesCategory()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {

                    var successStory =
                        SqlHelper.ExecuteDataset
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetPopulerSearchCategory"
                            );
                    if (null != successStory && successStory.Tables.Count>0)
                    {
                        return successStory.Tables[0];
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("Can not execute query");
        }

        public DataTable PopulerSearchesCity()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {

                    var successStory =
                        SqlHelper.ExecuteDataset
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetPopulerSearchCity"
                            );
                    if (null != successStory && successStory. Tables.Count>0)
                    {
                        return successStory.Tables[0];
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("Can not execute query");
        }

        public DataTable AllJobsByCategory(int categoryId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {

                    SqlParameter[] parameters = new SqlParameter[] {
                 new SqlParameter("@Id",categoryId),

            };
                    var successStory =
                        SqlHelper.ExecuteReader
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetJobListByCategory",
                            parameters
                            );
                    if (null != successStory && successStory.HasRows)
                    {
                        var dt = new DataTable();
                        dt.Load(successStory);
                        return dt;
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("Can not execute query");
        }

        public DataTable AllJobsByCity(string CityCode)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {

                    SqlParameter[] parameters = new SqlParameter[] {
                         new SqlParameter("@CityCode",CityCode),
                    };
                    var successStory =
                        SqlHelper.ExecuteReader
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetJobListByCity",
                            parameters
                            );
                    if (null != successStory && successStory.HasRows)
                    {
                        var dt = new DataTable();
                        dt.Load(successStory);
                        return dt;
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("Can not execute query");
        }

        public DataTable GetCategory()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    var successStory =
                        SqlHelper.ExecuteReader
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_getCategory"
                            );
                    if (null != successStory && successStory.HasRows)
                    {
                        var dt = new DataTable();
                        dt.Load(successStory);
                        return dt;
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("Can not execute query");
        }

        public DataTable TopEmployer()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    var successStory =
                        SqlHelper.ExecuteDataset
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetTopEmployer"
                            );
                    if (null != successStory && successStory.Tables.Count>0)
                    {
                        return successStory.Tables[0];
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("Can not execute query");
        }

        public DataTable GetAllCompanyList()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    var compnayList =
                        SqlHelper.ExecuteReader
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetEmployers"
                            );
                    if (null != compnayList)
                    {
                        var dt = new DataTable();
                        dt.Load(compnayList);
                        return dt;
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("Can not execute query");
        }

        public DataTable NasscomJobsList()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    var nasscomjobs =
                        SqlHelper.ExecuteReader
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetAllNasscomJobs"
                            );
                    if (null != nasscomjobs)
                    {
                        var dt = new DataTable();
                        dt.Load(nasscomjobs);
                        return dt;
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("Can not execute query");
        }
        public DataTable GetSuccessStoryVideo()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    var successStory =
                        SqlHelper.ExecuteDataset
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetSuccessStoryVideoPosted"
                            );
                    if (null != successStory && successStory.Tables.Count>0)
                    {
                        return successStory.Tables[0];
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("Can not execute query");
        }

        public DataTable GetCompanyHasJobPostId()
        {
            using (var connection = new SqlConnection(connectionString))
            {

                try
                {
                    var city =
                        SqlHelper.ExecuteDataset
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetCompanyNamehaveJobPost"
                            //"usp_GetCities"
                            );
                    if (null != city && city.Tables.Count > 0)
                    {
                        return city.Tables[0];
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("Can not execute query");
        }
        public string GetContactUsEmail()
        {
            string Email;
            try
            {
                Email = ContactUsEmail;
            }
            catch {
                throw new DataNotFound("Not get contact us email");
             }
            return Email;
        }

        public string TalentConnectLink()
        {
            string TalentConnectLink;
            try
            {
                TalentConnectLink = TalentConnect;
            }
            catch
            {
                throw new DataNotFound("Not get talent connect");
            }
            return TalentConnectLink;
        }
        public string CandidateBulkUpload()
        {
            string CandidateBulkUploadPDF;
            try
            {
                CandidateBulkUploadPDF = CandidateBulkUploadPDFGuide;
            }
            catch
            {
                throw new DataNotFound("Not get talent connect");
            }
            return CandidateBulkUploadPDF;
        }
        public string TPRegistrationGuide()
        {
            string TPRegistrationPDF;
            try
            {
                TPRegistrationPDF = TPRegistrationPDFGuide;
            }
            catch
            {
                throw new DataNotFound("Not get talent connect");
            }
            return TPRegistrationPDF;
        }

        public DataTable GetRecentJobs()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    var recentJobs =
                        SqlHelper.ExecuteDataset
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetRecentJobs"
                            );
                    if (null != recentJobs && recentJobs.Tables.Count > 0)
                    {
                        return recentJobs.Tables[0];
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("Can not execute query");
        }
        public DataTable GetWalkInJobs()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    var walkinJobs =
                        SqlHelper.ExecuteDataset
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetWalkinJobs"
                            );
                    if (null != walkinJobs && walkinJobs.Tables.Count > 0)
                    {
                        return walkinJobs.Tables[0];
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("Can not execute query");
        }
        public bool EmployerFollower(int EmployeeId, int UserId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {

                    SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@EmployerId",EmployeeId),
                new SqlParameter("@JobSeekerId",UserId),
            };
                    var result =
                        SqlHelper.ExecuteNonQuery
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_InsertEmployerFollower",
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
            throw new UserNotCreatedException("Unable to follow company, please contact your teck deck with your details.");
        }

        public DataTable CategoryJobVacancies()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {

                    var category =
                        SqlHelper.ExecuteDataset
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetCategoryJobVacancies"
                            );
                    if (null != category && category.Tables.Count > 0)
                    {
                        return category.Tables[0];
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("Can not execute query");
        }

        public DataTable CityJobVacancies()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {

                    var city =
                        SqlHelper.ExecuteDataset
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetCityJobVacancies"
                            );
                    if (null != city && city.Tables.Count > 0)
                    {
                        return city.Tables[0];
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("Can not execute query");
        }

        public DataTable CompanyJobVacancies()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {

                    var company =
                        SqlHelper.ExecuteDataset
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetCompanyJobs"
                            );
                    if (null != company && company.Tables.Count > 0)
                    {
                        return company.Tables[0];
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("Can not execute query");
        }

        public DataTable AllJobsByCompany(int UserId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {

                    SqlParameter[] parameters = new SqlParameter[] {
                         new SqlParameter("@UserId",UserId),
                    };
                    var companyJobs =
                        SqlHelper.ExecuteReader
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetJobListByCompany",
                            parameters
                            );
                    if (null != companyJobs && companyJobs.HasRows)
                    {
                        var dt = new DataTable();
                        dt.Load(companyJobs);
                        return dt;
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("Can not execute query");
        }
    }
}
