using System;
using System.Security.Claims;
using LMS.Business.Interfaces.Auth;
using LMS.Business.Interfaces.Jobseeker;
using LMS.Business.Interfaces.Shared;
using LMS.Model.DataViewModel.JobSeeker;
using LMS.Model.DataViewModel.Shared;
using LMS.Utility.EncryptDecrypt;
using LMS.Utility.Exceptions;
using LMS.Utility.ExtendedMethods;
using LMS.Utility.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace LMS.Web.Controllers
{

    //[HandleExceptionsAttribute]
    public class AuthController : Controller
    {
        private readonly IAuthHandler authHandler;
        private readonly IHostingEnvironment hostingEnviroment;
        private readonly IEMailHandler emailHandler;
        private readonly IUserProfileHandler userProfileHandler;
        private readonly IConfiguration config;
        private readonly string URLprotocol;
        public AuthController(IEMailHandler _emailHandler, IConfiguration _config, IAuthHandler _authHandler,
            IHostingEnvironment _hostingEnvironment, IUserProfileHandler _userProfileHandler)
        {
            authHandler = _authHandler;
            hostingEnviroment = _hostingEnvironment;
            emailHandler = _emailHandler;
            userProfileHandler = _userProfileHandler;
            config = _config;
            URLprotocol = config["URLprotocol"];
        }
        public IActionResult Index(string returnUrl)
        {
            TempData[Constants.SessionRedirectUrl] = returnUrl;
            return View();
        }

        [HttpPost]
        public IActionResult Login(UserViewModel user)
        {
            try
            {
                var result = authHandler.Login(user.Email.Trim(), user.Password);
                //if (result != null && result.IsApproved == "False")
                //{
                //    throw new NotApprovedByAdminException("Sorry!!! Your account is not activated. Contact your tech deck.");
                //}
                if (null != result)
                {
                    var identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Email,result.Email),
                    new Claim(ClaimTypes.Role,result.RoleName)
                    }, CookieAuthenticationDefaults.AuthenticationScheme);

                    var principal = new ClaimsPrincipal(identity);
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    if (!string.IsNullOrEmpty(result.PasswordExpirayDate) && DateTime.Now.Date <= Convert.ToDateTime(result.PasswordExpirayDate))
                    {
                        //Handled if image url exist in db but not available physically
                        string picpath = hostingEnviroment.WebRootPath + result.ProfilePic;
                        if (!System.IO.File.Exists(picpath))
                        {
                            string fName = $@"\ProfilePic\" + "Avatar.jpg";
                            result.ProfilePic = fName;
                        }
                        HttpContext.Session.Set<UserViewModel>(Constants.SessionKeyUserInfo, result);
                        authHandler.UserActivity(result.UserId);
                        return GoAhead(result.RoleName, result.UserId);
                        //return View("Index");
                    }
                    else
                    {
                        return View("CreateNewPassword");
                    }
                }
            }
            catch (InvalidUserCredentialsException ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(AuthController), ex);
                ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
            }
            catch (UserNotFoundException ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(AuthController), ex);
                ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
            }
            catch (NotApprovedByAdminException ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(AuthController), ex);
                ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
            }
            return View("JobSeekerLogin");
        }

        [NonAction]
        private IActionResult GoAhead(string role, int userid)
        {
            string rUrl = Convert.ToString(TempData[Constants.SessionRedirectUrl]);
            if (!string.IsNullOrWhiteSpace(rUrl))
            {
                return new RedirectResult(rUrl);
            }

            if (role == Constants.AdminRole)
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }
            else if (role == Constants.CorporateRole || role == Constants.StaffingPartnerRole)
            {
                return RedirectToAction("EmpDashboard", "Dashboard", new { area = "Employer" });
            }
            else if (role == Constants.TrainingPartnerRole)
            {
                return RedirectToAction("TPDashboard", "Dashboard", new { area = "TrainingPartner" });
            }
            else if (role == Constants.DemandAggregationRole)
            {
                return RedirectToAction("DemandAggregation", "Dashboard", new { area = "Admin" });
            }
            else
            {
                var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
                UserDetail userDetail = userProfileHandler.GetJobseekerDetail(user.UserId);
                if (userDetail.PersonalDetails.DOB == "" || userDetail.PersonalDetails.Gender == "" || userDetail.PersonalDetails.MobileNo == "" || userDetail.Skills.SkillSets == null || userDetail.EducationalDetails == null || userDetail.PersonalDetails.Resume == "")
                {
                    return RedirectToAction("Profile", "JobSeekerManagement", new { area = "Jobseeker" });
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
        }

        public IActionResult JobseekerRegistration(string data)
        {
            //var lstRoles = authHandler.RolesList();
            //ViewBag.RoleListEmp = lstRoles.FindAll(x => x.IsEmp == true);
            //ViewBag.RoleListCandidate = lstRoles.FindAll(x => x.IsEmp == false);
            return View();
        }
        public IActionResult EmployerRegistration(string data)
        {
            //var lstRoles = authHandler.RolesList();
            //ViewBag.RoleListEmp = lstRoles.FindAll(x => x.IsEmp == true);
            //ViewBag.RoleListCandidate = lstRoles.FindAll(x => x.IsEmp == false);
            return View();
        }

        //[HttpPost]
        //public JsonResult SubmitRegistration([FromForm]UserViewModel user)
        //{
        //    var isregistered = true;
        //    var message = "User registered successfully, please login to proceed.";
        //    try
        //    {
        //        authHandler.RegisterUser(user, user.ImageFile);
        //        var eModel = new EmailViewModel
        //        {
        //            Subject = "Resgistration completed successfully",
        //            Body = "Dear " + user.FirstName + "," + "<br/>You have successfully registered with us." + "<br>Your login details are below:<br>" + "User Name: " + user.Email + "<br>Password: " + user.Password + "<br><br>Thank You<br>Placement Portal Team",
        //            To = new string[] { user.Email },
        //            From = config["EmailCredential:Fromemail"],
        //            IsHtml = true,
        //            MailType = (int)MailType.ForgotPassword
        //        };
        //        emailHandler.SendMail(eModel, -1);
        //    }
        //    catch (UserNotCreatedException ex)
        //    {
        //        Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(AuthController), ex);
        //        isregistered = false;
        //        message = ex.Message;
        //        //ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
        //    }
        //    catch (UserAlreadyExists ex)
        //    {
        //        Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(AuthController), ex);
        //        isregistered = false;
        //        message = ex.Message;
        //        //ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
        //    }
        //    return Json(new { isRegistered = isregistered, msg = message });
        //}

        //[HttpPost]
        //public JsonResult SubmitEmpRegistration([FromForm]UserViewModel user)
        //{
        //    var isregistered = true;
        //    //var message = "Employer registered successfully, Please wait for the Account Activation";
        //    var message = "Account created successfully, You will get a mail after account activation.";
        //    try
        //    {
        //        authHandler.RegisterEmployer(user, user.ImageFile);

        //        var eModel = new EmailViewModel
        //        {
        //            Subject = "Account Activation",
        //            Body = "Dear " + user.CompanyName + "," + "<br/>Please wait for the account activation." + "<br><br>Thank You<br>Placement Portal Team",
        //            To = new string[] { user.Email },
        //            From = config["EmailCredential:Fromemail"],
        //            IsHtml = true,
        //            MailType = (int)MailType.ForgotPassword
        //        };
        //        emailHandler.SendMail(eModel, -1);

        //        //ViewData["SuccessMessage"] = "Employer registered successfully, please login to proceed";
        //    }
        //    catch (UserNotCreatedException ex)
        //    {
        //        Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(AuthController), ex);
        //        isregistered = false;
        //        message = ex.Message;
        //        //ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
        //    }
        //    catch (UserAlreadyExists ex)
        //    {
        //        Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(AuthController), ex);
        //        isregistered = false;
        //        message = ex.Message;
        //        //ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
        //    }
        //    return Json(new { isRegistered = isregistered, msg = message });
        //}

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string email)
        {
            var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);//for loggging
            try
            {
                var emailID = authHandler.ForgetPassword(email);
                /* Mail Send */
                string emailEncr = EncryptDecrypt.Encrypt(emailID, "sblw-3hn8-sqoy19");
                var basePath = string.Format("{0}://{1}", /*Request.Scheme*/ URLprotocol, Request.Host);
                var link = basePath + "/Auth/ResetPassword/?id=" + emailEncr;

                var eModel = new EmailViewModel
                {
                    Subject = "Reset Password",
                    Body = "Dear Sir/Madam,<br/>You initiated a request to help with your account password. Click the link below to set a new password for Placement Portal" +
                    "<br/><a href=" + link + ">Reset Password link</a><br><br>" + "Thank You<br>Placement Portal Team",
                    To = new string[] { emailID },
                    From = config["EmailCredential:Fromemail"],
                    IsHtml = true,
                    MailType = (int)MailType.ForgotPassword
                };
                emailHandler.SendMail(eModel, -1);

                //SendVerificationLinkEmail(emailID, "ResetPassword");
                ViewData["SuccessMessage"] = "Password Reset link send to your Email";
            }
            catch (UserNotFoundException ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, 0, typeof(AuthController), ex);
                ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
                ViewData["SuccessMessage"] = ex.Message;
            }
            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, 0, typeof(AuthController), ex);
                ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
                ViewData["SuccessMessage"] = ex.Message;
            }
            return View();
        }

        [HttpGet]
        //[UserAuthenticationAttribute(Constants.AllRoles)]
        public ActionResult ResetPassword(string id)
        {

            int mod4 = id.Length % 4;
            if (mod4 > 0)
            {
                id += new string('=', 4 - mod4);
            }
            string email = EncryptDecrypt.Decrypt(id.Replace(" ", "+"), "sblw-3hn8-sqoy19");
            UserViewModel userModel = new UserViewModel
            {
                Email = email
            };
            return View(userModel);
        }

        [HttpPost]
        public ActionResult ResetPassword(UserViewModel user)
        {
            try
            {
                authHandler.ResetPasswordData(user);
                ViewData["SuccessMessage"] = "Password change successfully, please login to proceed";
            }
            catch (UserNotCreatedException ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(AuthController), ex);
                ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
            }
            return View("Index");
        }

        public IActionResult Logout(string returnUrl = "")
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            //return RedirectToAction("Index", new { returnUrl = returnUrl });
            return RedirectToAction("Index", "Home", new { returnUrl = returnUrl });
        }

        [HttpGet]
        //[UserAuthenticationAttribute(Constants.AllRoles)]
        public IActionResult CreateNewPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword([FromBody]ResetPasswordViewModel user)
        {
            var status = false;
            try
            {
                var model = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
                user.Email = model.Email;
                status = authHandler.ChangePassword(user);
            }
            catch (UserNotCreatedException ex)
            {
                status = false;
            }
            ModelState.Clear();
            return Json(status);
        }

        [HttpGet]
        public IActionResult GenrateOtp(string Email, int RoleId, string Name)
        {
            bool isOtp = false;
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            string Gotp = _rdm.Next(_min, _max).ToString();

            string errMsg = string.Empty;
            try
            {
                var user = authHandler.GenerateOtp(Gotp, Email);
                if (!user)
                {
                    return Json(new { isOtp = false, msg = "Email already exist" });
                }
                if (RoleId == 3 || RoleId == 4)
                {
                    if (RoleId == 3 && Name == null)
                    {
                        Name = "Employer";
                    }
                    else if (RoleId == 4 && Name == null)
                    {
                        Name = "Stuffing Partner";
                    }
                    var eModel = new EmailViewModel
                    {
                        Subject = "Login OTP",
                        Body = "Dear " + Name + "," + "<br/>" + Gotp + " is OTP to complete the registration."
                                                + "<br/>Do not disclose the OTP to anyone.<br/>Happy job post and search candidate with IT-ITeS Sector Skills Council NASSCOM." + "<br><br> Thank You" + "<br>Placement Portal Team",
                        To = new string[] { Email },
                        From = config["EmailCredential:Fromemail"],
                        IsHtml = true,
                        MailType = (int)MailType.OTP
                    };
                    emailHandler.SendMail(eModel, -1);
                }

                else
                {
                    if (RoleId == 2 && Name == null)
                    {
                        Name = "Candidate";
                    }
                    else if (RoleId == 5 && Name == null)
                    {
                        Name = "Praining Partner";
                    }

                    var eModel = new EmailViewModel
                    {
                        Subject = "Login OTP",
                        Body = "Dear " + Name + "," + "<br/>" + Gotp + " is OTP to complete the registration."
                                                + "<br/>Do not disclose the OTP to anyone.<br/>Happy job post and search candidate with IT-ITeS Sector Skills Council NASSCOM." + "<br><br> Thank You" + "<br>Placement Portal Team",
                        To = new string[] { Email },
                        From = config["EmailCredential:Fromemail"],
                        IsHtml = true,
                        MailType = (int)MailType.OTP
                    };
                    emailHandler.SendMail(eModel, -1);
                }

                return Json(new { isOtp = true });
            }
            catch (UserNotCreatedException ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, 0, typeof(AuthController), ex);
                isOtp = false;
                errMsg = ex.Message;
            }
            catch (Exception ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, 0, typeof(AuthController), ex);
                isOtp = false;
                errMsg = ex.Message;
            }
            return Json(new { isOtp, errMsg });

        }

        [HttpPost]
        public IActionResult SubmitOtp([FromBody] OtpDataViewModel otpModel)
        {
            bool matchOtp = false;
            string errMsg = string.Empty;
            try
            {
                var user = authHandler.SubmitOTP(otpModel.Otp, otpModel.Email);
                return Json(new { matchOtp = true });
            }

            catch (UserNotCreatedException ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, 0, typeof(AuthController), ex);
                matchOtp = false;
                errMsg = ex.Message;
            }
            catch (Exception ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, 0, typeof(AuthController), ex);
                matchOtp = false;
                errMsg = ex.Message;
            }
            return Json(new { matchOtp, errMsg });

        }

        [HttpPost]
        //[UserAuthenticationAttribute(Constants.AllRoles)]
        public IActionResult CreateNewPassword(ResetPasswordViewModel users)
        {
            try
            {
                authHandler.CreateNewPassword(users);
                Logout();
                ViewData["SuccessMessage"] = "Password changed successfully, kindly login with new password";
            }
            catch (UserNotCreatedException ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, 0, typeof(AuthController), ex);
                ViewData["NotFoundMessage"] = ex.Message;
                ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
            }
            return View("CreateNewPassword");
        }

        [HttpGet]
        public IActionResult UnauthorizedUser()
        {
            var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
            if (user.RoleName == Constants.AdminRole)
            {
                ViewBag.Message = "This features is not related to Admin";
            }
            else if (user.RoleName == Constants.StaffingPartnerRole || user.RoleName == Constants.CorporateRole)
            {
                ViewBag.Message = "This features is not related to Employer";
            }
            else if (user.RoleName == Constants.TrainingPartnerRole || user.RoleName == Constants.StudentRole)
            {
                ViewBag.Message = "This features is not related to Job Seeker";
            }
            return View();
        }

        [HttpGet]
        public IActionResult EmailVerification(int uId, string aKey)
        {
            ViewBag.Uid = uId;
            ViewBag.ActivationKey = aKey;
            return View();
        }

        [HttpGet]
        public IActionResult VerifyEmail(int userId, string activationKey)
        {
            ViewBag.Uid = userId;
            ViewBag.ActivationKey = activationKey;
            bool isVerified = authHandler.VerifyEmail(userId, activationKey);
            if (!isVerified)
            {
                ViewBag.EmailVerificationFailed = "Oops! Unable to verify your email, Please contact your teck deck.";
                return View("EmailVerification");
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult JobSeekerLogin(string returnUrl)
        {

            TempData[Constants.SessionRedirectUrl] = returnUrl;
            return View();
        }

        [HttpGet]
        public IActionResult EmployerLogin(string returnUrl)
        {
            TempData[Constants.SessionRedirectUrl] = returnUrl;
            return View();
        }

        #region Googlelogin
        //public IActionResult GoogleLogin()
        //{
        //    string redirectUrl = Url.Action("GoogleResponse", "Auth");
        //    var properties = signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
        //    return new ChallengeResult("Google", properties);
        //}

        //public async Task<IActionResult> GoogleResponse()
        //{
        //    ExternalLoginInfo info = await signInManager.GetExternalLoginInfoAsync();
        //    if (info == null)
        //    {
        //        return RedirectToAction(nameof(Login));
        //    }

        //    var result = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
        //    string[] userInfo = { info.Principal.FindFirst(ClaimTypes.Name).Value, info.Principal.FindFirst(ClaimTypes.Email).Value };
        //    if (result.Succeeded)
        //    {
        //        return View(userInfo);
        //    }
        //    else
        //    {
        //        var user = new IdentityUser
        //        {
        //            Email = info.Principal.FindFirst(ClaimTypes.Email).Value,
        //            UserName = info.Principal.FindFirst(ClaimTypes.Email).Value
        //        };

        //        IdentityResult identResult = await userManager.CreateAsync(user);
        //        if (identResult.Succeeded)
        //        {
        //            identResult = await userManager.AddLoginAsync(user, info);
        //            if (identResult.Succeeded)
        //            {
        //                await signInManager.SignInAsync(user, false);
        //                return View(userInfo);
        //            }
        //        }
        //        return UnauthorizedUser();
        //    }
        //}
        #endregion


        [HttpPost]
        public IActionResult EmployerRegistration(EmployeeViewModel user)
        {
            var message = "User registered successfully, please login to proceed.";
            try
            {
                if (ModelState.IsValid)
                {                    
                    user.RoleId = 3;//For Employer
                    authHandler.RegisterEmployer(user);
                    SendRegistrationMailToEmployer(user);                   
                    TempData["successMsg"] = "Registration Successful Please wait for admin approval!";
                    ModelState.Clear();                   
                }
            }
            catch (UserNotCreatedException ex)
            {
                TempData["errorMsg"] = "Unable to register user Please try again later!";
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, 0, typeof(AuthController), ex);
                message = ex.Message;
                //ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
            }
            catch (UserAlreadyExists ex)
            {
                TempData["errorMsg"] = "User already exist please login!";
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, 0, typeof(AuthController), ex);
                message = ex.Message;
                //ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
            }
            catch (Exception ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, 0, typeof(AuthController), ex);
                ViewData["SuccessMessage"] = "Unable to send Mail";
            }
            return View();
        }

        private void SendRegistrationMailToEmployer(EmployeeViewModel user)
        {
            try
            {
                var basePath = string.Format("{0}://{1}{2}", Request.Scheme, URLprotocol, Request.Host);
                var link = basePath + "/Auth/EmployerLogin";                
                var eModel = new EmailViewModel
                {
                    Subject = "Welcome aboard!",
                    Body = "<b>Hi " + user.CompanyName + "</b>," + "<br/><br/>Thank You for signing up with SRLMS.com. " +
                    "We are delighted to have you on board." +
                    "<br/><br/>Your login details are below:<br/><br/>" + "User Name: " + user.Email + "<br>Password: " + user.Password +
                    "<br/><br/>You can update your contact and registration details at any time by logging on to http://srtechjob.com/" +
                    "<br/><br/>See you on board!<br/><a href=" + link + "> SRTechJob</a> Team",
                    To = new string[] { user.Email },
                    From = config["EmailCredential:Fromemail"],
                    IsHtml = true,
                    MailType = (int)MailType.ForgotPassword
                };
                                
                emailHandler.SendMail(eModel, -1);
            }
            catch(Exception ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, 0, typeof(AuthController), ex);
            }
        }

        [HttpPost]
        public IActionResult JobseekerRegistration(JobSeekerViewModel user)
        {
            var message = "User registered successfully, please login to proceed.";
            try
            {
                if (ModelState.IsValid)
                {
                    var basePath = string.Format("{0}://{1}{2}", Request.Scheme, URLprotocol, Request.Host);
                    var link = basePath + "/Auth/JobSeekerLogin";

                    user.RoleId = 2;//For Student
                    authHandler.RegisterUser(user);
                    var eModel = new EmailViewModel
                    {
                        Subject = "Welcome to SRLMS.com",
                        Body = "<b>Dear " + user.FirstName + "</b>," + "<br/><br/>Congratulations! You have successfully registered with" +
                        " SRLMS.com<br/>" +
                        "<br/>Please note that your username and password are both case sensitive.<br/><br/>Your login details are below:<br/><br/>" +
                        "User Name: " + user.Email +
                        "<br>Password: " + user.Password +
                        "<br/><br/>You can update your contact and registration details at any time by logging on to SRLMS.com" +
                        "<br/><br/>Wish you all the best!<br/><a href=" + link + "> SrLMS.com</a> Team",
                        To = new string[] { user.Email },
                        From = config["EmailCredential:Fromemail"],
                        IsHtml = true,
                        MailType = (int)MailType.ForgotPassword
                    };
                    TempData["successMsg"] = "Registration Successful Please wait for admin approval!";
                    ModelState.Clear();
                    emailHandler.SendMail(eModel, -1);
                }
            }
            catch (UserNotCreatedException ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(AuthController), ex);
                message = ex.Message;
                TempData["errorMsg"] = "Unable to register user Please try again later!";
                //ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
            }
            catch (UserAlreadyExists ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(AuthController), ex);
                message = ex.Message;
                TempData["errorMsg"] = "User already exist please login!";
                //ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
            }
            catch (Exception ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, 0, typeof(AuthController), ex);
                ViewData["SuccessMessage"] = "Unable to send Mail";
            }
            return View();
        }

        [HttpPost]
        public void EmployeePassword(string email)
        {
            var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);//for loggging
            try
            {
                var emailID = authHandler.ForgetPassword(email);
                /* Mail Send */
                string emailEncr = EncryptDecrypt.Encrypt(emailID, "sblw-3hn8-sqoy19");
                var basePath = string.Format("{0}://{1}", /*Request.Scheme*/ URLprotocol, Request.Host);
                var link = basePath + "/Auth/EmployerPassword/?id=" + emailEncr;

                var eModel = new EmailViewModel
                {
                    Subject = "Welcome to LMS.| Activate your Profile",
                    Body = "Dear " + email + ",<br/>Please varify email to activate your account instantly.<br/>Click the link below " +
                    "<br/><a href=" + link + ">Verify Email now !</a><br><br>" + "Thank You<br>Placement Portal Team",
                    To = new string[] { emailID },
                    From = config["EmailCredential:Fromemail"],
                    IsHtml = true,
                    MailType = (int)MailType.ForgotPassword
                };
                emailHandler.SendMail(eModel, -1);
                ViewData["SuccessMessage"] = "Password Reset link send to your Email";
            }
            catch (UserNotFoundException ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, 0, typeof(AuthController), ex);
                //ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
                ViewData["SuccessMessage"] = ex.Message;
            }
            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, 0, typeof(AuthController), ex);
                //ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
                ViewData["SuccessMessage"] = ex.Message;
            }
            catch (Exception ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, 0, typeof(AuthController), ex);
                ViewData["SuccessMessage"] = "Unable to send Mail";
            }
        }
        [HttpPost]
        public JsonResult FBRegistration([FromBody]string accesstoken)
        {
            var isSuccess = true;
            try
            {
                var resp = authHandler.GetFBUserInfo(accesstoken);

                if (resp == null)
                {
                    throw new UserNotCreatedException("Invalid access token");
                }
                var randomPassword = RandomGenerator.GetRandom(5);
                var user = new EmployeeViewModel
                {
                    FirstName = resp.FirstName,
                    LastName = resp.LastName,
                    Email = resp.Email,
                    Password = randomPassword,
                    CompanyName = resp.FirstName,
                };

                user.RoleId = 3;//For Employer
                authHandler.RegisterEmployer(user);
                SendRegistrationMailToEmployer(user);
            }
            catch (UserNotCreatedException ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, 0, typeof(AuthController), ex);
                isSuccess = false;
            }catch(UserAlreadyExists ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, 0, typeof(AuthController), ex);
                isSuccess = false;
            }
            catch (Exception ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, 0, typeof(AuthController), ex);
                isSuccess = false;
            }
            return Json(new { isSuccess });

        }
    }
}