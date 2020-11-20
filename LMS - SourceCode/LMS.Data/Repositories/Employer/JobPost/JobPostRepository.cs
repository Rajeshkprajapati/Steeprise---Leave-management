using LMS.Data.DataModel.Employer.JobPost;
using LMS.Data.Helper;
using LMS.Data.Interfaces.Employer.JobPost;
using LMS.Utility.Exceptions;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;

namespace LMS.Data.Repositories.Employer.JobPost
{
    public class JobPostRepository : IJobPostRepository
    {
        private readonly string connectionString;

        public JobPostRepository(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionStrings:NassComLMS.B"];
        }

        public DataTable GetJobIndustryAreaDetail()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    var JobIndustryArea =
                        SqlHelper.ExecuteDataset
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetIndustryArea"
                            );
                    if (null != JobIndustryArea && JobIndustryArea.Tables.Count>0)
                    {
                     return JobIndustryArea.Tables[0];
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("Can not execute query");
        }

        public bool AddPreferredLocation(string location, int i, int userid)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {

                    SqlParameter[] parameters = new SqlParameter[]
                    {
                new SqlParameter("@locationid",location),
                new SqlParameter("@locationorder",i),
                new SqlParameter("@userid",userid),
                    };
                    var status =
                        SqlHelper.ExecuteNonQuery
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_AddPreferredlocation",
                            parameters
                            );
                    if (status > 0)
                    {
                        return true;
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
                return false;
            }
        }

        public DataTable GetJobIndustryAreaWithJobPost()
        {
            using (var connection = new SqlConnection(connectionString))
            {

                try
                {
                    var JobIndustryArea =
                        SqlHelper.ExecuteDataset
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetJobIndustryAreaWithPostData"
                            );
                    if (null != JobIndustryArea && JobIndustryArea.Tables.Count>0)
                    {
                        return JobIndustryArea.Tables[0];
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("Can not execute query");
        }

        public DataTable GetJobIndustryAreaWithStudentData()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {

                    var JobIndustryArea =
                        SqlHelper.ExecuteReader
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetJobIndustryAreaWithStudentData"
                            );
                    if (null != JobIndustryArea && JobIndustryArea.HasRows)
                    {
                        var dt = new DataTable();
                        dt.Load(JobIndustryArea);
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

        public DataTable GetJobJobEmploymentStatusDetail()
        {
            using (var connection = new SqlConnection(connectionString))
            {

                try
                {


                    var jobEmploymentStatus =
                        SqlHelper.ExecuteDataset
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetEmploymentStatus"
                            );
                    if (null != jobEmploymentStatus && jobEmploymentStatus.Tables.Count>0)
                    {
                        return jobEmploymentStatus.Tables[0];
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new InvalidUserCredentialsException("Can not execute query");
        }

        public DataTable GetJobJobEmploTypeDetail()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {

                    var jobJobEmploType =
                        SqlHelper.ExecuteDataset
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetEmploymentType"
                            );
                    if (null != jobJobEmploType && jobJobEmploType.Tables.Count>0)
                    {
                        return jobJobEmploType.Tables[0];
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("Can not execute query");
        }

        public DataTable GetStateListDetail(string countryCode)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {

                    SqlParameter[] parameters = new SqlParameter[] {
                        new SqlParameter("@countryCode",countryCode)
                    };
                    var country =
                        SqlHelper.ExecuteReader
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetStates",
                             parameters
                            );
                    if (null != country && country.HasRows)
                    {
                        var dt = new DataTable();
                        dt.Load(country);
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

        public DataTable GetCityListDetail(string StateCode)
        {
            using (var connection = new SqlConnection(connectionString))
            {

                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@stateCode",StateCode)
            };
                    var country =
                        SqlHelper.ExecuteReader
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetCities",
                             parameters
                            );
                    if (null != country && country.HasRows)
                    {
                        var dt = new DataTable();
                        dt.Load(country);
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

        public DataTable GetGenderListDetail()
        {
            using (var connection = new SqlConnection(connectionString))
            {

                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@IsAll",true)
            };
                    var genderData =
                        SqlHelper.ExecuteReader
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetGenderMaster",
                            parameters
                            );
                    if (null != genderData && genderData.HasRows)
                    {
                        var dt = new DataTable();
                        dt.Load(genderData);
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

        public bool AddJobPostData(JobPostModel model)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@CTC",model.CTC),
                new SqlParameter("@CityCode",model.CityCode),
                new SqlParameter("@CountryCode",model.CountryCode),
                new SqlParameter("@CreatedBy",model.CreatedBy),
                new SqlParameter("@EmploymentStatusId",model.EmploymentStatusId),
                new SqlParameter("@EmploymentTypeId",model.EmploymentTypeId),
                new SqlParameter("@Gender",model.Gender),
                new SqlParameter("@HiringCriteria",model.HiringCriteria),
                new SqlParameter("@Jobdetails",model.JobDetails),
                new SqlParameter("@JobIndustryAreaId",model.JobIndustryAreaId),
                new SqlParameter("@OtherJobIndustryArea",model.OtherJobIndustryArea),
                new SqlParameter("@JobTitleId",model.JobTitleId),
                new SqlParameter("@JobType",model.JobType),
                new SqlParameter("@MonthlySalary",model.MonthlySalary),
                new SqlParameter("@Nationality",model.Nationality),
                new SqlParameter("@IsWalkInJob",model.IsWalkin == "Yes"?true:false),
                new SqlParameter("@NoPosition",model.NoPosition),
                new SqlParameter("@PositionStartDate",model.PositionStartDate),
                new SqlParameter("@PositionEndDate",model.PositionEndDate),
                //new SqlParameter("@Quarter1",model.Quarter1),
                //new SqlParameter("@Quarter2",model.Quarter2),
                //new SqlParameter("@Quarter3",model.Quarter3),
                //new SqlParameter("@Quarter4",model.Quarter4),
                new SqlParameter("@Spoc",model.SPOC),
                new SqlParameter("@SpocContact",model.SPOCContact),
                new SqlParameter("@SpocEmail",model.SPOCEmail),
                new SqlParameter("@StateCode",model.StateCode),
                new SqlParameter("@PostedTo",model.Userid),
                new SqlParameter("@Skills",model.Skills),
                new SqlParameter("@JobTitleByEmployer",model.JobTitleByEmployer),
                new SqlParameter("@MinExp",model.MinExp),
                new SqlParameter("@MaxExp",model.MaxExp),
                new SqlParameter("@FinancialYear",model.FinancialYear),
                new SqlParameter("@IsFromBulkUpload",model.IsFromBulkUpload)

            };
                    var result =
                        SqlHelper.ExecuteNonQuery
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_InsertJobPost",
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
            throw new UserNotCreatedException("Unable to create job post, please contact your teck deck with your details.");
        }

        public DataTable GetJobDetails(int jobid)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {

                    SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@jobid",jobid)
            };
                    var jobdetail =
                        SqlHelper.ExecuteReader
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetJoBDetail",
                             parameters
                            );
                    if (null != jobdetail && jobdetail.HasRows)
                    {
                        var dt = new DataTable();
                        dt.Load(jobdetail);
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

        public DataTable RecommendedJobs(int roleid)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {

                    SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@roleId",roleid)
            };
                    var jobdetail =
                        SqlHelper.ExecuteReader
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_RecommendedJobsOnRole",
                             parameters
                            );
                    if (null != jobdetail && jobdetail.HasRows)
                    {
                        var dt = new DataTable();
                        dt.Load(jobdetail);
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
