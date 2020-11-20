using LMS.Data.DataModel.Shared;
using LMS.Data.Helper;
using LMS.Data.Interfaces.Jobseeker;
using LMS.Model.DataViewModel.JobSeeker;
using LMS.Utility.Exceptions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;

namespace LMS.Data.Repositories.Jobseeker
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly string connectionString;

        public UserProfileRepository(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionStrings:NassComLMS.B"];
        }

        public bool AddNewExperience(int userId, string model)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {

                    SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@experienceDetails",model),
                new SqlParameter("@userId",userId)
            };
                    var result =
                        SqlHelper.ExecuteNonQuery
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "USP_InsertExperienceDetails",
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
            throw new DataNotUpdatedException("Unable to experience, please contact your teck deck with your details.");
        }
        public bool AddNeweducational(int userId, string model)
        {
            string educationalString = JsonConvert.SerializeObject(model);
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {

                    SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@educationalDetails",model),
                new SqlParameter("@userId",userId)
            };
                    var result =
                        SqlHelper.ExecuteNonQuery
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "USP_InsertEducationalDetails",
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
            throw new DataNotUpdatedException("Unable to insert education, please contact your teck deck with your details.");
        }
        public bool AddNewSkills(int userId, string skills)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {

                    SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@skillDetails",skills),
                new SqlParameter("@userId",userId)
            };
                    var result =
                        SqlHelper.ExecuteNonQuery
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_InsertSkillsDetails",
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
            throw new DataNotUpdatedException("Unable to skills, please contact your teck deck with your details.");
        }

        public bool AddNewProfileSummary(string profile, int userId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {

                    SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@profile",profile),
                new SqlParameter("@userId",userId)
            };
                    var result =
                        SqlHelper.ExecuteNonQuery
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_InsertProfileSummary",
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
            throw new UserNotCreatedException("Unable to create profile summary, please contact your teck deck with your details.");
        }

        public DataTable GetAllCities()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {

                    //SqlParameter[] parameters = new SqlParameter[] {
                    //   new SqlParameter("@cityFirstChar", cityFirstChar)
                    //};
                    var city =
                        SqlHelper.ExecuteDataset
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetAllCitiesWithoutState"
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
            throw new InvalidUserCredentialsException("Can not execute query");
        }

        public DataTable GetJobseekerData(int UserId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {

                    SqlParameter[] parameters = new SqlParameter[] {
                        new SqlParameter("@UserId",UserId)
                    };
                    var country =
                        SqlHelper.ExecuteDataset
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetJobseekerProfileData",
                             parameters
                            );
                    if (null != country && country.Tables.Count>0)
                    {
                        return country.Tables[0];
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("Unable to find data.");
        }

        public DataTable GetUserPreferredlocation(int userid)
        {
            using (var connection = new SqlConnection(connectionString))
            {

                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@UserId",userid)
            };
                    var locatiions =
                        SqlHelper.ExecuteDataset
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetuserPreferredlocations",
                             parameters
                            );
                    if (null != locatiions && locatiions.Tables.Count>0)
                    {
                        return locatiions.Tables[0];
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("User's preferred locations not found.");
        }

        public bool AddNewProfileDetail(UserModel model)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                        new SqlParameter("@userId",model.UserId),
                        new SqlParameter("@currentSalary",model.CTC),
                        new SqlParameter("@expectedSalary",model.ECTC),
                        new SqlParameter("@dateOfBirth",model.DOB),
                        new SqlParameter("@aboutMe",model.AboutMe),
                        new SqlParameter("@status",true),
                        new SqlParameter("@email",model.Email),
                        new SqlParameter("@mobileNo",model.MobileNo),
                        new SqlParameter("@jobTitleId",model.JobTitleId),
                        new SqlParameter("@address",model.Address1),
                        new SqlParameter("@maritalStatus",model.MaritalStatus),
                        new SqlParameter("@gender",model.Gender),
                        new SqlParameter("@jobCategory",model.JobIndustryArea),
                        new SqlParameter("@employmentStatus",model.EmploymentStatus),
                        new SqlParameter("@country",model.Country),
                        new SqlParameter("@state",model.State),
                        new SqlParameter("@city",model.City),
                         new SqlParameter("@TotalExperience",model.TotalExperience),
                         new SqlParameter("@LinkedinProfile",model.LinkedinProfile)
                    };
                    var result =
                        SqlHelper.ExecuteNonQuery
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_InsertUserProfessionalDetails",
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
            throw new UserNotCreatedException("Unable to experience, please contact your teck deck with your details.");
        }

        public bool AddNewUploadFile(string fName, int userId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {

                    SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@UserId",userId),
                new SqlParameter("@fName",fName)
            };
                    var result =
                        SqlHelper.ExecuteNonQuery
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_UploadResume",
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
            throw new DataNotFound("Unable to find data.");
        }
        public bool UploadProfilePicture(string fName, int userId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {

                    SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@UserId",userId),
                new SqlParameter("@fName",fName)
            };
                    var result =
                        SqlHelper.ExecuteNonQuery
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_UploadProfilePicture",
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
            throw new DataNotFound("Unable to find data.");
        }
        public bool ApplyJob(int userId, int jobPostId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                        new SqlParameter("@userId",userId),
                        new SqlParameter("@jobPostId",jobPostId)
                    };
                    var result =
                        SqlHelper.ExecuteNonQuery
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_InsertAppliedJobs",
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
            throw new FaildToApplyJob("Unable to apply job");
        }
        public bool CheckIfJobExist(string UserId, string jobPostId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {

                    SqlParameter[] parameters = new SqlParameter[] {
                        new SqlParameter("@userId",UserId),
                        new SqlParameter("@jobPostId",jobPostId)
                    };
                    var result =
                        SqlHelper.ExecuteReader
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_CheckIfJobSaved",
                            parameters
                            );
                    if (result != null && result.HasRows)
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

        public bool CheckIfskillEmpty(string UserId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@userId",UserId)
            };
                    var result =
                        SqlHelper.ExecuteReader
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_CheckIfSkillEmpty",
                            parameters
                            );
                    if (result != null && result.HasRows)
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
        public bool CheckIfResumeEmpty(string UserId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {

                    SqlParameter[] parameters = new SqlParameter[] {
                        new SqlParameter("@userId",UserId)
                    };
                    var result =
                        SqlHelper.ExecuteReader
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_CheckIfCandidateResumeExist",
                            parameters
                            );
                    if (result != null && result.HasRows)
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

        public bool CheckIfEducationDetailsEmpty(string UserId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {

                    SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@userId",UserId)
            };
                    var result =
                        SqlHelper.ExecuteReader
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_CheckIfCandidateEducationalDetailsExist",
                            parameters
                            );
                    if (result != null && result.HasRows)
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

        public bool CheckIfUserAvailableInUserProfessionalDetails(string UserId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                        new SqlParameter("@userId",UserId)
                    };
                    var result =
                        SqlHelper.ExecuteReader
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_CheckIfUserExistInUserProfessionalDetails",
                            parameters
                            );
                    if (result != null && result.HasRows)
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

        public DataTable GetEmployerDetailFromJobId(int jobId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@jobId",jobId)
                };
                    var dReader =
                        SqlHelper.ExecuteReader
                        (
                            connectionString,
                            CommandType.StoredProcedure,
                            "usp_GetEmployerDetailFromJobId",
                            parameters
                            );
                    if (dReader != null && dReader.HasRows)
                    {
                        var dt = new DataTable();
                        dt.Load(dReader);
                        return dt;
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("Employer details not found for this job");
        }
        public DataTable GetUserITSkills(int userid)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var dt = new DataTable();
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@UserId",userid)
            };
                    var ITSkills =
                        SqlHelper.ExecuteReader
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetuserITSkills",
                             parameters
                            );
                    if (null != ITSkills && ITSkills.HasRows)
                    {
                        dt.Load(ITSkills);
                        return dt;
                    }
                    else
                    {
                        return dt;
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("User IT Skills not Founds");
        }
        public bool UpdateItSkills(ITSkills ITSkills, int UserId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {

                    SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@ITSkillId",ITSkills.Id),
                new SqlParameter("@ITSkill",ITSkills.Skill),
                new SqlParameter("@SkillVersion",ITSkills.SkillVersion),
                new SqlParameter("@LastUsed",ITSkills.LastUsed),
                new SqlParameter("@ExperienceYear",ITSkills.ExperienceYear),
                new SqlParameter("@ExperienceMonth",ITSkills.ExperienceMonth),
                new SqlParameter("@UserId",UserId),
            };
                    var result =
                        SqlHelper.ExecuteNonQuery
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_UpdateITSkills",
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
            throw new UserNotCreatedException("Unable to create IT Skill, please contact your teck deck with your details.");
        }
        public bool DeleteITSkill(int ITSkillId, int UserId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {

                    SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@ITSkillId",ITSkillId),
                new SqlParameter("@UserId",UserId),
            };
                    var result =
                        SqlHelper.ExecuteNonQuery
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_DeleteITSkill",
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
            throw new UserNotCreatedException("Unable to create IT Skill, please contact your teck deck with your details.");
        }
        public DataSet GetJobseekerDashboard(int userId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@UserId",userId)
            };
                    var result =
                        SqlHelper.ExecuteDataset
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetJobSeekerDashboardStats",
                            parameters
                            );
                    if (null != result && result.Tables.Count > 0)
                    {
                        return result;
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("Dashboard data not found, please contact your tech deck.");
        }
        public DataTable GetJobseekerAppliedJobs(int userId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@UserId",userId)
            };
                    var appliedJobs =
                        SqlHelper.ExecuteDataset
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_JobSeekerAppliedJobs",
                            parameters
                            );
                    if (null != appliedJobs && appliedJobs.Tables.Count > 0)
                    {
                        return appliedJobs.Tables[0];
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("Can not execute query");
        }

        public DataTable GetJobseekerViewedProfile(int userId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@UserId",userId)
            };
                    var viewedProfile=
                        SqlHelper.ExecuteDataset
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetViewedProfiel",
                            parameters
                            );
                    if (null != viewedProfile && viewedProfile.Tables.Count > 0)
                    {
                        return viewedProfile.Tables[0];
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("Can not execute query");
        }
        public bool DeleteAppliedJob(int JobPostId, int UserId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {

                    SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@JobPostId",JobPostId),
                new SqlParameter("@UserId",UserId),
            };
                    var result =
                        SqlHelper.ExecuteNonQuery
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_DeleteAppliedJob",
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
            throw new UserNotCreatedException("Unable to delete applied job.");
        }
        public DataTable EmployerFollowers(int userId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@UserId",userId)
            };
                    var empFollower =
                        SqlHelper.ExecuteDataset
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetEmployerFollowingByJobseeker",
                            parameters
                            );
                    if (null != empFollower && empFollower.Tables.Count > 0)
                    {
                        return empFollower.Tables[0];
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("Can not execute query");
        }
        public bool UnfollowEmployer(int EmployerId, int UserId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {

                    SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@EmployerId",EmployerId),
                new SqlParameter("@UserId",UserId),
            };
                    var result =
                        SqlHelper.ExecuteNonQuery
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_UnfollowEmployerForJobseeker",
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
            throw new UserNotCreatedException("Unable to unfollow selected company");
        }

        public DataTable JobSeekerSkills(int userId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@UserId",userId)
            };
                    var skills =
                        SqlHelper.ExecuteDataset
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetJobSeekerSkills",
                            parameters
                            );
                    if (null != skills && skills.Tables.Count > 0)
                    {
                        return skills.Tables[0];
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("Can not execute query");
        }

        public DataTable JobSeekerJobsOnSkills(string skills,int UserId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@Skills",skills),
                new SqlParameter("@UserId",UserId)
            };
                    var Jobs =
                        SqlHelper.ExecuteDataset
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetSearchJobOnSkills",
                            parameters
                            );
                    if (null != Jobs && Jobs.Tables.Count > 0)
                    {
                        return Jobs.Tables[0];
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("Can not execute query");
        }

        public bool JobsAlert(int JobAlert,int UserId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {

                    SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@JobAlert",JobAlert),
                new SqlParameter("@UserId",UserId),
            };
                    var result =
                        SqlHelper.ExecuteNonQuery
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_JobSeekerJobsAlert",
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
            throw new UserNotCreatedException("Unable to update job alert, please contact your teck deck with your details.");
        }

        public DataTable ProfileScore(int UserId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@UserId",UserId)
            };
                    var score =
                        SqlHelper.ExecuteDataset
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetProfileScore",
                            parameters
                            );
                    if (null != score && score.Tables.Count > 0)
                    {
                        return score.Tables[0];
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("Can not execute query");
        }

        public DataTable JobSeekerContacted(int userId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@UserId",userId)
            };
                    var empFollower =
                        SqlHelper.ExecuteDataset
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetJobSeekerContactedDetails",
                            parameters
                            );
                    if (null != empFollower && empFollower.Tables.Count > 0)
                    {
                        return empFollower.Tables[0];
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
