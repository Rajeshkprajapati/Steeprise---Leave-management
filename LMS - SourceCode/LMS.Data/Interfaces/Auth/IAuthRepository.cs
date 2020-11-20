using LMS.Data.DataModel.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LMS.Data.Interfaces.Auth
{
    public interface IAuthRepository
    {
        DataRow Login(string userName, string password);
        bool CheckIfUserExists(string email, string company="");
        bool CheckCandidateIdExist(string id);
        bool CheckTPIdExist(string id);
        int RegisterUser(UserModel user);
        bool RegisterEmployer(UserModel user, bool isRegisterOnlyForDemandAggregationData=false);
        string ForgetPassword(string emailId);
        bool ResetPasswordData(UserModel user);
        bool CreateNewPassword(CreateNewPasswordModel user);
        DataRow CandidateResult(string id);
        DataRow TPResult(string id);
        DataTable Role();
        bool GenerateOtp(string otp, string email);
        bool SubmitOTP(string otp, string email);
        bool UserActivity(int userid);
        bool VerifyEmail(int userId, string aKey);
        bool CheckIfEmployerExists(string company, bool allEmployer = false);
    }
}
