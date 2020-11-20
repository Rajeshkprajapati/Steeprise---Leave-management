using LMS.Data.DataModel.Shared;
using LMS.Data.Helper;
using LMS.Data.Interfaces.Auth;
using LMS.Utility.Exceptions;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;

namespace LMS.Data.Repositories.Auth
{
    public class AuthRepository : IAuthRepository
    {
        private readonly string connectionString;

        public AuthRepository(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionStrings:NassComLMS.B"];
        }

        public DataRow Login(string userName, string password)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {

                    SqlParameter[] parameters = new SqlParameter[] {
                        new SqlParameter("@Email",userName),
                        new SqlParameter("@Password",password)
                    };
                    var user =
                        SqlHelper.ExecuteDataset
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_UserLogin",
                            parameters
                            );
                    if (null != user && user.Tables.Count>0 && user.Tables[0].Rows.Count>0)
                    {
                        return user.Tables[0].Rows[0];
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new InvalidUserCredentialsException("Entered user credentials are not valid");
        }

        public int RegisterUser(UserModel user)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {

                    var outParam = new SqlParameter
                    {
                        ParameterName = "@OutUserId",
                        Direction = ParameterDirection.Output,
                        SqlDbType = SqlDbType.Int
                    };
                    SqlParameter[] parameters = new SqlParameter[] {
                        new SqlParameter("@Email",user.Email),
                        new SqlParameter("@FName",user.FirstName),
                        new SqlParameter("@LName",user.LastName),
                        new SqlParameter("@MobileNo",user.MobileNo),
                        new SqlParameter("@Password",user.Password),
                        new SqlParameter("@RoleId",user.RoleId),                        
                        new SqlParameter("@IsActive",user.IsActive),
                        new SqlParameter("@ActivationKey",user.ActivationKey),
                        new SqlParameter("@CreatedBy",user.CreatedBy),
                         new SqlParameter("@IsApproved",user.IsApproved),
                        outParam
                    };
                    var result =
                        SqlHelper.ExecuteNonQuery
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_RegisterUser",
                            parameters
                            );
                    if (result > 0)
                    {
                        return Convert.ToInt32(outParam.Value);
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new UserNotCreatedException("Unable to register, please contact your teck deck with your details.");
        }

        public bool RegisterEmployer(UserModel user, bool isRegisterOnlyForDemandAggregationData)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {

                    SqlParameter[] parameters = new SqlParameter[] {
                        new SqlParameter("@CompanyName",user.CompanyName),
                        new SqlParameter("@Email",user.Email),
                        new SqlParameter("@Password",user.Password),
                        new SqlParameter("@RoleId",user.RoleId),
                        new SqlParameter("@profilepic",user.ProfilePic),
                        new SqlParameter("@isRegisterOnlyForDemandAggregationData",isRegisterOnlyForDemandAggregationData),
                        new SqlParameter("@IsApproved",user.IsApproved),
                        new SqlParameter("@IsActive",user.IsActive)
                    };
                    var result =
                        SqlHelper.ExecuteNonQuery
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_RegisterEmployer",
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
            throw new UserNotCreatedException("Unable to register, please contact your teck deck with your details.");
        }


        public DataRow CandidateResult(string id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                        new SqlParameter("@candidateid",id)
                    };
                    var user =
                        SqlHelper.ExecuteReader
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetCandidateDetail",
                            parameters
                            );
                    if (null != user && user.HasRows)
                    {
                        var dt = new DataTable();
                        dt.Load(user);
                        return dt.Rows[0];
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("*Candidate not found in Sector Skills Council Nasscom");
        }

        public DataRow TPResult(string id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                        new SqlParameter("@tpid",id)
                    };
                    var user =
                        SqlHelper.ExecuteReader
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetTPDetail",
                            parameters
                            );
                    if (null != user && user.HasRows)
                    {
                        var dt = new DataTable();
                        dt.Load(user);
                        return dt.Rows[0];
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("*Training Partner not found in Internal SDMS");
        }

        public bool CheckCandidateIdExist(string id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                        new SqlParameter("@candidateid",id),
                    };
                    var result =
                        SqlHelper.ExecuteReader
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_CheckIfCandidateIdExists",
                            parameters
                            );
                    if (null != result && result.HasRows)
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

        public bool CheckTPIdExist(string id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {

                    SqlParameter[] parameters = new SqlParameter[] {
                        new SqlParameter("@tpid",id),
                    };
                    var result =
                        SqlHelper.ExecuteReader
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_CheckIfTPIdExists",
                            parameters
                            );
                    if (null != result && result.HasRows)
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

        public string ForgetPassword(string emailId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                        new SqlParameter("@Email",emailId)
                    };
                    var user =
                        SqlHelper.ExecuteReader
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_ForgetPassword",
                            parameters
                            );
                    if (null != user && user.HasRows)
                    {
                        var dt = new DataTable();
                        dt.Load(user);
                        return Convert.ToString(dt.Rows[0]["Email"]);

                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new InvalidUserCredentialsException("Email Id is not valid");


        }

        public bool ResetPasswordData(UserModel user)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                        new SqlParameter("@Email",user.Email),
                        new SqlParameter("@Password",user.Password)

                    };
                    var result =
                        SqlHelper.ExecuteNonQuery
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_UpdatePassword",
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
            throw new UserNotCreatedException("Unable to register, please contact your teck deck with your details.");
        }

        public bool CreateNewPassword(CreateNewPasswordModel user)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                        new SqlParameter("@Email",user.Email),
                        new SqlParameter("@Password",user.Password),
                        new SqlParameter("@OldPassword",user.OldPassword)

                    };
                    var result =
                        SqlHelper.ExecuteNonQuery
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_CreateNewPassword",
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
            throw new UserNotCreatedException("Unable to change password, either email id or password is not vailid");
        }



        public bool CheckIfUserExists(string email, string company)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                        new SqlParameter("@Email",email),
                        new SqlParameter("@Company",company)
                    };
                    var result =
                        SqlHelper.ExecuteReader
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_CheckIfUserExists",
                            parameters
                            );
                    if (null != result && result.HasRows)
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

        public bool CheckIfEmployerExists(string company, bool allEmployer=false)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                        new SqlParameter("@Company",company),
                        new SqlParameter("@allEmployer",allEmployer)
                    };
                    var result =
                        SqlHelper.ExecuteReader
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_CheckIfEmployerExists",
                            parameters
                            );
                    if (null != result && result.HasRows)
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

        public DataTable Role()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    var roles =
                        SqlHelper.ExecuteReader
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetRoles"
                            );
                    if (null != roles && roles.HasRows)
                    {
                        var dt = new DataTable();
                        dt.Load(roles);
                        //return dt.Rows[0];
                        return dt;
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("Can't fetch roles");

        }
        public bool GenerateOtp(string otp, string email)
        {
            if (CheckIfUserExists(email, ""))
            {
                return false;
            }
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                        new SqlParameter("@Otp",otp),
                        new SqlParameter("@Email",email),
                     };
                    var result =
                        SqlHelper.ExecuteNonQuery
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_InsertIntOptDate",
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
            throw new UserNotCreatedException("Unable to send OTP");
        }

        public bool SubmitOTP(string otp, string email)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                        new SqlParameter("@OTP",otp),
                        new SqlParameter("@Email",email),
                     };
                    var result =
                        SqlHelper.ExecuteReader
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_VerifyOTP",
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
            throw new UserNotCreatedException("OTP did not match");
        }

        public bool VerifyEmail(int userId, string aKey)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {

                    SqlParameter[] parameters = new SqlParameter[] {
                        new SqlParameter("@UserId",userId),
                        new SqlParameter("@ActivationKey",aKey),
                     };
                    int result =
                        SqlHelper.ExecuteNonQuery
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_VerifyEmailUsingActivationKey",
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

        public bool UserActivity(int userid)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {

                    SqlParameter[] parameters = new SqlParameter[]
                    {
                        new SqlParameter("@userid",userid)
                    };
                    int status = SqlHelper.ExecuteNonQuery(
                        connection,
                        CommandType.StoredProcedure,
                        "usp_UserActivity",
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
            }
            return false;
        }

    }
}
