using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using LMS.Business.Handlers.DataProcessorFactory;
using LMS.Business.Interfaces.Auth;
using LMS.Business.Interfaces.Shared;
using LMS.Data.DataModel.Shared;
using LMS.Data.Interfaces.Auth;
using LMS.Model.DataViewModel.Shared;
using LMS.Model.DataViewModel.SocialLogin;
using LMS.Utility.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
//using System.IO;

namespace LMS.Business.Handlers.Auth
{
    public class AuthHandler : IAuthHandler
    {
        private readonly IAuthRepository _authProcessor;
        private IHostingEnvironment hostingEnviroment;
        private readonly IEMailHandler emailHandler;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string URLprotocol;

        public AuthHandler(IEMailHandler _emailHandler, IConfiguration configuration, IHostingEnvironment _hostingEnvironment,
            IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClientFactory)
        {
            var factory = new ProcessorFactoryResolver<IAuthRepository>(configuration);
            _authProcessor = factory.CreateProcessor();
            _configuration = configuration;
            emailHandler = _emailHandler;
            _httpContextAccessor = httpContextAccessor;
            hostingEnviroment = _hostingEnvironment;
            _httpClientFactory = httpClientFactory;
        }
        public UserViewModel Login(string userName, string password)
        {
            var user = _authProcessor.Login(userName, password);
            var u = new UserViewModel();
            if (null != user)
            {
                u.UserId = Convert.ToInt32(user["UserId"]);
                u.FirstName = Convert.ToString(user["FirstName"]);
                u.LastName = Convert.ToString(user["LastName"]);
                u.MobileNo = Convert.ToString(user["MobileNo"]);
                u.Email = Convert.ToString(user["Email"]);
                u.RoleName = Convert.ToString(user["RoleName"]);
                u.CompanyName = Convert.ToString(user["CompanyName"]);
                u.PasswordExpirayDate = Convert.IsDBNull(user["PasswordExpiryDate"]) ? "" : Convert.ToString(user["PasswordExpiryDate"]);
                u.IsApproved = Convert.ToString(user["IsApproved"]);
                u.ProfilePic = Convert.ToString(user["ProfilePic"]);
                u.JobTitleName = Convert.ToString(user["JobTitleName"]);
                u.Address1 = Convert.ToString(user["Address1"]);
                u.Address2 = Convert.ToString(user["Address2"]);
                u.Address3 = Convert.ToString(user["Address3"]);
            }
            return u;
        }


        public bool RegisterUser(JobSeekerViewModel user)
        {
            bool status = true;
            try
            {

                if (CheckIfUserExists(user.Email))
                {
                    throw new UserAlreadyExists("Seems this user already exists in our record, please login with previous credentials.");
                }


                var u = new UserModel
                {
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    MobileNo = user.MobileNo,
                    Password = user.Password,
                    RoleId = user.RoleId,
                    IsApproved = true,
                    IsActive = true
                };
                int isRegister = _authProcessor.RegisterUser(u);
                if (isRegister > 0)
                {
                    return status;
                }
            }
            catch (UserNotCreatedException ex)
            {
                status = false;
                throw new UserNotCreatedException("Unable to create user, please contact your teck deck.");
            }
            return status;
        }

        public bool RegisterEmployer(EmployeeViewModel user)
        {

            if (_authProcessor.CheckIfEmployerExists(user.CompanyName))
            {
                throw new UserAlreadyExists("Seems this company already exists in our record, please login with previous credentials.");
            }

            if (CheckIfUserExists(user.Email))
            {
                throw new UserAlreadyExists("Seems this user already exists in our record, please login with previous credentials.");
            }
            var u = new UserModel
            {
                CompanyName = user.CompanyName,
                Email = user.Email,
                Password = user.Password,
                RoleId = user.RoleId,
                IsActive = true,
                IsApproved = true
            };
            bool isRegister = _authProcessor.RegisterEmployer(u);
            if (isRegister)
            {
                return true;
            }
            throw new UserNotCreatedException("Unable to create user, please contact your teck deck.");
        }

        private bool CheckCandidateIdExist(string id)
        {
            return _authProcessor.CheckCandidateIdExist(id);
        }

        //private bool CheckTPIDExist(string id)
        //{
        //    return _authProcessor.CheckCandidateIdExist(id);
        //}
        private bool CheckIfUserExists(string email)
        {
            return _authProcessor.CheckIfUserExists(email);
        }
        //private void SendAccountActivationMail(UserModel user)
        //{
        //    string activationLink =
        //        $"{URLprotocol}://" +
        //        $"{_httpContextAccessor.HttpContext.Request.Host.Value}" +
        //        $"/Auth/EmailVerification?uId={user.UserId}&akey={user.ActivationKey}";

        //    var eModel = new EmailViewModel
        //    {
        //        Subject = "Account Activation Link",
        //        Body = $"Dear {user.FirstName},<br/>Congrat's you have successfully registered with us." +
        //        $"You are one step away to explore our application," +
        //        $"Please <a href={activationLink}>click here</a> to activate your account." +
        //        $"Your login details are below:<br/><br/>User Name:  {user.Email}<br/>Password: {user.Password} " +
        //        $"<br/><br/>Thank You <br/> Placement Portal Team",

        //        To = new string[] { user.Email },
        //        From = config["EmailCredential:Fromemail"],
        //        IsHtml = true,
        //        MailType = (int)MailType.UserRegistrationActivationLink
        //    };
        //    emailHandler.SendMail(eModel, -1);
        //}

        //public UserViewModel TPResult(string id)
        //{
        //    var user = _authProcessor.TPResult(id);
        //    if (null != user)
        //    {
        //        return new UserViewModel()
        //        {
        //            FirstName = Convert.ToString(user["FirstName"]),
        //            LastName = Convert.ToString(user["LastName"]),
        //        };
        //    }
        //    throw new DataNotFound("*Training Partner not found in Internal SDMS");
        //}

        public List<RoleViewModel> RolesList()
        {
            try
            {
                DataTable roles = _authProcessor.Role();
                List<RoleViewModel> list = new List<RoleViewModel>();
                for (int i = 0; i < roles.Rows.Count; i++)
                {
                    int roleId = Convert.ToInt32(roles.Rows[i]["ID"]);
                    if (roleId > 1 && roleId < 6)
                    {
                        RoleViewModel role = new RoleViewModel
                        {
                            RoleId = roleId,
                            RoleName = Convert.ToString(roles.Rows[i]["RoleName"]),
                            IsEmp = Convert.ToBoolean(roles.Rows[i]["IsEmployee"]),
                        };
                        list.Add(role);
                    }
                }
                return list;
            }
            catch (DataNotFound ex)
            {
                throw new DataNotFound("Can't fetch Roles");
            }

        }


        public string ForgetPassword(string emailId)
        {
            var status = _authProcessor.CheckIfUserExists(emailId);
            if (status)
            {
                var user = _authProcessor.ForgetPassword(emailId);
                if (null != user)
                {
                    return user;

                }
                throw new UserNotFoundException("User not found");
            }
            else
            {
                throw new DataNotFound("data not found");
            }
        }
        public bool ResetPasswordData(UserViewModel user)
        {
            var u = new UserModel
            {
                Password = user.Password,
                Email = user.Email

            };
            bool isRegister = _authProcessor.ResetPasswordData(u);
            if (isRegister)
            {
                return true;
            }
            throw new UserNotCreatedException("Unable to create user, please contact your teck deck.");
        }

        public bool CreateNewPassword(ResetPasswordViewModel user)
        {
            var u = new CreateNewPasswordModel
            {
                Password = user.Password,
                Email = user.Email,
                OldPassword = user.OldPassword
            };
            bool isRegister = _authProcessor.CreateNewPassword(u);
            if (isRegister)
            {
                return true;
            }
            throw new UserNotCreatedException("Unable to change password, Please insert vailid email and password");
        }

        public bool GenerateOtp(string otp, string email)
        {
            try
            {
                bool isOtp = _authProcessor.GenerateOtp(otp, email);
                if (isOtp)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Could not generate OTP");
            }
        }

        public bool SubmitOTP(string otp, string email)
        {
            bool isOtp = _authProcessor.SubmitOTP(otp, email);
            if (isOtp)
            {
                return true;
            }
            throw new Exception("Could not matched OTP");
        }

        public bool VerifyEmail(int userId, string aKey)
        {
            return _authProcessor.VerifyEmail(userId, aKey);
        }

        public bool UserActivity(int userid)
        {
            return _authProcessor.UserActivity(userid);
        }



        public bool ChangePassword(ResetPasswordViewModel user)
        {
            var status = false;
            try
            {
                if (CreateNewPassword(user))
                {
                    status = true;
                }
            }
            catch (UserNotCreatedException ex)
            {
                status = false;
            }
            return status;
        }

        

        private FBTokenValidatonViewModel ValidateFBAccessToken(string accessToken)
        {
            try
            {
                string tokenValidationUrl = _configuration["FBAPI:TokenValidationUrl"];
                string appId = _configuration["FBSettings:AppId"];
                string appSecret = _configuration["FBSettings:AppSecret"];

                var formattedUrl = string.Format(tokenValidationUrl, accessToken, appId, appSecret);                
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(formattedUrl);
                    using (var resp = client.GetAsync(client.BaseAddress))
                    {
                        resp.Wait();
                        var result = resp.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            var response = result.Content.ReadAsStringAsync();
                            response.Wait();
                            var tokenmodel = JsonConvert.DeserializeObject<FBTokenValidatonViewModel>(response.Result);
                            return tokenmodel;                            
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserNotCreatedException(ex.Message);
            }            
        }

        

        public FBUserInfoResultViewModel GetFBUserInfo(string accessToken)
        {
            var validateToken = ValidateFBAccessToken(accessToken);
            if (validateToken == null || !validateToken.Data.IsValid)
            {
                throw new UserNotCreatedException("Invalida access token");
            }

            string userInfoUrl = _configuration["FBAPI:GetUserInfo"];

            var formattedUrl = string.Format(userInfoUrl, accessToken);
            
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(formattedUrl);
                using (var resp = client.GetAsync(client.BaseAddress))
                {
                    resp.Wait();
                    var result = resp.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var response = result.Content.ReadAsStringAsync();
                        response.Wait();
                        return JsonConvert.DeserializeObject<FBUserInfoResultViewModel>(response.Result);
                    }
                    else
                    {
                        throw new UserNotCreatedException("Invalid access token");
                    }
                }
            }
            return null;
        }

    }
}
