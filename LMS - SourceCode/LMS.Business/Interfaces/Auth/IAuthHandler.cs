using LMS.Model.DataViewModel.Shared;
using LMS.Model.DataViewModel.SocialLogin;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LMS.Business.Interfaces.Auth
{
    public interface IAuthHandler
    {
        UserViewModel Login(string userName, string password);
        bool RegisterUser(JobSeekerViewModel user);
        bool RegisterEmployer(EmployeeViewModel user);
        string ForgetPassword(string emailId);
        bool ResetPasswordData(UserViewModel user);
        bool CreateNewPassword(ResetPasswordViewModel user);
        //UserViewModel CandidateResult(string id);
        List<RoleViewModel> RolesList();
        bool GenerateOtp(string otp, string email);
        bool SubmitOTP(string otp, string email);
        //UserViewModel TPResult(string id);
        bool UserActivity(int userid);
        bool VerifyEmail(int userId, string aKey);
        bool ChangePassword(ResetPasswordViewModel user);     
        
        FBUserInfoResultViewModel GetFBUserInfo(string accessToken);
    }
}
