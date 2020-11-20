using LMS.Data.DataModel.Shared;
using LMS.Data.Helper;
using LMS.Data.Interfaces.Jobseeker;
using LMS.Utility.Exceptions;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;

namespace LMS.Data.Repositories.Jobseeker
{
    public class ResumeBuilderRepository : IResumeBuilderRepository
    {
        private readonly string connectionString;
        public ResumeBuilderRepository(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionStrings:NassComLMS.B"];
        }

        public bool InsertExperienceDetails(int userId, string exp, string skills)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {

                    SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@userId",userId),
                new SqlParameter("@ExpDetails",exp),
                new SqlParameter("@Skills",skills)
            };
                    var result =
                        SqlHelper.ExecuteNonQuery
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_InsertUpdateUserExperienceDetails",
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
            throw new DataNotUpdatedException("Unable to save user employment information please try again or check with your admin.");
        }

        public bool InsertPersonalDetails(int userId, UserModel user)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {

                    SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@userId",userId),
                new SqlParameter("@FirstName",user.FirstName),
                new SqlParameter("@LastName",user.LastName),
                new SqlParameter("@MaritalStatus",user.MaritalStatus),
                new SqlParameter("@Gender",user.Gender),
                new SqlParameter("@Email",user.Email),
                new SqlParameter("@Address1",user.Address1),
                new SqlParameter("@Address2",user.Address2),
                new SqlParameter("@Address3",user.Address3),
                new SqlParameter("@City",user.City),
                new SqlParameter("@Country",user.Country),
                new SqlParameter("@DOB",user.DOB),
                new SqlParameter("@State",user.State),
                new SqlParameter("@MobileNo",user.MobileNo),
            };
                    var result =
                        SqlHelper.ExecuteNonQuery
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_InsertUpdateUserPersonalDetails",
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
            throw new DataNotUpdatedException("Unable to save user personal information please try again or check with your admin.");
        }

        public bool InsertEducationDetails(int userId, string educations)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {

                    SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@userId",userId),
                new SqlParameter("@EduDetails",educations)
            };
                    var result =
                        SqlHelper.ExecuteNonQuery
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_InsertUpdateUserEducationDetails",
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
            throw new DataNotUpdatedException("Unable to save user education information please try again or check with your admin.");
        }

        public DataSet GetUserDetails(int userId)
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
                            "usp_GetJobSeekerInformation",
                            parameters
                            );
                    if (null != result)
                    {
                        return result;
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("User Information not found, please contact your tech deck.");
        }

        public DataSet GetUserDetailsForResumeBuilder(int userId)
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
                            "usp_GetJobSeekerInformationForResumeBuilder",
                            parameters
                            );
                    if (null != result)
                    {
                        return result;
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("User Information not found, please contact your tech deck.");
        }

        public bool UpdateResumePath(int userId, string resumePath)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                        new SqlParameter("@UserId",userId),
                        new SqlParameter("@fName",resumePath)
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
            throw new DataNotUpdatedException("Unable to update resme path,please try again or check with your admin.");
        }

        public string GetResume(int userId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@UserId",userId)
            };
                    var reader =
                        SqlHelper.ExecuteReader
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetResume",
                            parameters
                            );
                    if (null != reader && reader.HasRows)
                    {
                        var dt = new DataTable();
                        dt.Load(reader);
                        return Convert.ToString(dt.Rows[0]["ResumePath"]);
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new FileNotFoundException("Resume file not found, please upload/create resume or contact your tech deck.");
        }
    }
}
