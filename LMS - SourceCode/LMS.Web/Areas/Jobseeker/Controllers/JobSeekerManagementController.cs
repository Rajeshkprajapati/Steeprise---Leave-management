using LMS.Business.Interfaces.Employer.JobPost;
using LMS.Business.Interfaces.Home;
using LMS.Business.Interfaces.Jobseeker;
using LMS.Model.DataViewModel.JobSeeker;
using LMS.Model.DataViewModel.Shared;
using LMS.Utility.Exceptions;
using LMS.Utility.ExtendedMethods;
using LMS.Utility.Helpers;
using LMS.Web.Filters;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace LMS.Web.Areas.Jobseeker.Controllers
{
    [Area("Jobseeker")]
    [Route("[controller]")]
    [UserAuthentication("Student,Training Partner")]
    [HandleExceptionsAttribute]
    public class JobSeekerManagementController : Controller
    {
        private readonly IUserProfileHandler userProfileHandler;
        private readonly IJobPostHandler jobpastHandler;
        private readonly IHomeHandler homeHandler;
        private IHostingEnvironment hostingEnviroment;
        public JobSeekerManagementController(IJobPostHandler _jobpastHandler, IHomeHandler _homeHandler, IUserProfileHandler _userProfileHandler, IHostingEnvironment _hostingEnvironment)
        {
            userProfileHandler = _userProfileHandler;
            jobpastHandler = _jobpastHandler;
            homeHandler = _homeHandler;
            hostingEnviroment = _hostingEnvironment;
        }
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult Profile()
        {
            try
            {
                ViewBag.JobIndustryArea = jobpastHandler.GetJobIndustryAreaDetails();
                ViewBag.EmploymentStatus = jobpastHandler.GetJobJobEmploymentStatusDetails();
                ViewBag.Country = jobpastHandler.GetCountryDetails();
                ViewBag.Coursecategory = jobpastHandler.GetCourseCategory();
                ViewBag.JobTypes = userProfileHandler.GetJobTypes();
                ViewBag.AllJobTitle = jobpastHandler.GetJobTitleDetails();
            }
            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, 0, typeof(JobSeekerManagementController), ex);
                ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
            }
            catch (InvalidUserCredentialsException ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, 0, typeof(JobSeekerManagementController), ex);
                ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
            }
            return View();
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult ProfileData()
        {
            UserDetail model = new UserDetail();
            var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
            user = user ?? new UserViewModel();
            try
            {
                ViewBag.JobIndustryArea = jobpastHandler.GetJobIndustryAreaDetails();
                ViewBag.EmploymentStatus = jobpastHandler.GetJobJobEmploymentStatusDetails();
                ViewBag.Country = jobpastHandler.GetCountryDetails();

                model = userProfileHandler.GetJobseekerDetail(user.UserId);
            }
            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(JobSeekerManagementController), ex);
                ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
            }
            return new JsonResult(model, ContractSerializer.JsonInPascalCase());
        }

        [HttpPost]
        [Route("[action]")]
        public JsonResult AddPreferredLocation([FromBody]string[] location)
        {
            var isSuccess = true;
            var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
            try
            {
                isSuccess = jobpastHandler.AddPreferredLocation(location, user.UserId);

            }
            catch (Exception ex)
            {
                isSuccess = false;
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, 0, typeof(JobSeekerManagementController), ex);
            }
            return Json(new { isSuccess });
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult GetCourse(int categoryid)
        {
            List<CourseViewModel> courses = new List<CourseViewModel>();
            try
            {
                courses = jobpastHandler.GetCourses(categoryid);
            }
            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, 0, typeof(JobSeekerManagementController), ex);
                ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
            }
            return Json(courses);
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult AddProfileDetail([FromBody]UserViewModel model)
        {
            bool result = false;
            var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
            user = user ?? new UserViewModel();
            try
            {
                model.UserId = user.UserId;
                result = userProfileHandler.AddProfileDetails(model);
                return Json(result);
            }
            catch (InvalidUserCredentialsException ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(JobSeekerManagementController), ex);
                ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
            }
            catch (UserNotFoundException ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(JobSeekerManagementController), ex);
                ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
            }
            return Json(result);
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult AddExperienceDetail([FromBody]ExperienceDetails[] model)
        {
            var result = false;
            if (model != null)
            {
                var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
                user = user ?? new UserViewModel();
                try
                {
                    result = userProfileHandler.AddExperienceDetails(Convert.ToInt32(user.UserId), model);
                    return Json(result);
                }
                catch (InvalidUserCredentialsException ex)
                {
                    Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(JobSeekerManagementController), ex);
                    ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
                }
                catch (UserNotFoundException ex)
                {
                    Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(JobSeekerManagementController), ex);
                    ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
                }
                return Json(result);
            }
            else
            {
                return Json(result);
            }
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult AddEducationDetail([FromBody]EducationalDetails[] model)
        {
            var result = false;
            var redirectUrl = HttpContext.Session.Get<string>(Constants.SessionRedirectUrl);
            if (model != null)
            {
                var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
                user = user ?? new UserViewModel();
                try
                {
                    result = userProfileHandler.AddEducationalDetailsDetails(Convert.ToInt32(user.UserId), model);
                    return Json(new { isSuccess = result, msg = redirectUrl });
                }
                catch (InvalidUserCredentialsException ex)
                {
                    Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(JobSeekerManagementController), ex);
                    ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
                }
                catch (UserNotFoundException ex)
                {
                    Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(JobSeekerManagementController), ex);
                    ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
                }
                return Json(new { isSuccess = result, msg = redirectUrl });
            }
            else
            {
                return Json(result);
            }
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult AddSkillDetails([FromBody]Skills model)
        {
            var result = false;
            var redirectUrl = HttpContext.Session.Get<string>(Constants.SessionRedirectUrl);
            if (model != null)
            {
                var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
                user = user ?? new UserViewModel();
                try
                {
                    result = userProfileHandler.AddSkillDetails(user.UserId, model);
                }
                catch (InvalidUserCredentialsException ex)
                {
                    Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(JobSeekerManagementController), ex);
                    ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
                }
                catch (UserNotFoundException ex)
                {
                    Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(JobSeekerManagementController), ex);
                    ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
                }
                return Json(new { isSuccess = result, msg = redirectUrl });
            }
            else
            {
                return Json(result);
            }
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult AddProfileSummary(string profile, string userId)
        {
            var result = false;
            if (profile != null)
            {
                var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
                user = user ?? new UserViewModel();
                try
                {
                    result = userProfileHandler.AddProfileSummaryDetails(profile, user.UserId);
                }
                catch (InvalidUserCredentialsException ex)
                {
                    Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(JobSeekerManagementController), ex);
                    ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
                }
                catch (UserNotFoundException ex)
                {
                    Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(JobSeekerManagementController), ex);
                    ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
                }
                return Json(result);
            }
            else
            {
                return Json(result);
            }
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult UploadFileValue([FromForm]IFormFile resume)
        {
            bool result = false;
            if (resume != null)
            {
                //var file = Request.Form.Files[0];
                var file = resume;
                string filename = file.FileName;
                var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
                string fName = $@"\Resume\{user.UserId + "_" + filename}";
                filename = hostingEnviroment.WebRootPath + fName;
                using (FileStream fs = System.IO.File.Create(filename))
                {
                    file.CopyTo(fs);
                }
                result = userProfileHandler.UploadFileData(fName, user.UserId);
                return Json(result);
            }
            else
            {
                return Json(result);
            }
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult UploadProfilePicture(IFormFile profilepic)
        {
            bool result = false;
            if (profilepic != null)
            {
                string filename = profilepic.FileName;
                var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
                string fName = $@"\ProfilePic\{user.UserId + "_" + filename}";
                filename = hostingEnviroment.WebRootPath + fName;
                using (FileStream fs = System.IO.File.Create(filename))
                {
                    profilepic.CopyTo(fs);
                }
                result = userProfileHandler.UploadProfilePicture(fName, user.UserId);
                user.ProfilePic = fName;
                return Json(result);
            }
            else
            {
                return Json(result);
            }
        }


        [HttpGet]
        [Route("[action]")]
        public IActionResult StateDetails(string CountryCode)
        {
            var statelist = new List<StateViewModel>();
            try
            {
                statelist = jobpastHandler.GetStateList(CountryCode);

            }
            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, 0, typeof(JobSeekerManagementController), ex);
                ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
            }
            return Json(statelist);
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult CityDetails(string stateCode)
        {
            var cityList = new List<CityViewModel>();
            try
            {
                cityList = jobpastHandler.GetCityList(stateCode);

            }
            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, 0, typeof(JobSeekerManagementController), ex);
                ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
            }
            return Json(cityList);
        }
        [HttpPost]
        [Route("[action]")]
        public IActionResult UpdateITSkill([FromBody]ITSkills ITSkills)
        {
            var result = false;
            if (ITSkills != null)
            {
                var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
                user = user ?? new UserViewModel();
                try
                {
                    result = userProfileHandler.UpdateItSkills(ITSkills, user.UserId);
                }
                catch (InvalidUserCredentialsException ex)
                {
                    Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(JobSeekerManagementController), ex);
                    ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
                }
                catch (UserNotCreatedException ex)
                {
                    Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(JobSeekerManagementController), ex);
                    ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
                }
                return Json(result);
            }
            else
            {
                return Json(result);
            }
        }
        [HttpPost]
        [Route("[action]")]
        public IActionResult DeleteITSkill(int ITSkillId)
        {
            var result = false;
            if (ITSkillId != 0)
            {
                var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
                user = user ?? new UserViewModel();
                try
                {
                    result = userProfileHandler.DeleteITSkill(ITSkillId, user.UserId);
                }
                catch (InvalidUserCredentialsException ex)
                {
                    Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(JobSeekerManagementController), ex);
                    ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
                }
                catch (UserNotFoundException ex)
                {
                    Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(JobSeekerManagementController), ex);
                    ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
                }
                return Json(result);
            }
            else
            {
                return Json(result);
            }
        }
        [HttpGet]
        [Route("[action]")]
        [UserAuthentication(Constants.JobSeekers)]
        public IActionResult Dashboard()
        {
            return View();
        }
        [HttpGet]
        [Route("[action]")]
        public PartialViewResult GetJobseekerDashboard()
        {
            JobSeekerDashboardSummary dashboardsummary = null;
            Skills skill = null;
            var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
            user = user ?? new UserViewModel();
            try
            {
                dashboardsummary = userProfileHandler.GetJobSeekerDashboard(user.UserId);
                skill = userProfileHandler.JobSeekerSkills(user.UserId);
                List<SearchJobListViewModel> jobs = userProfileHandler.JobSeekerJobsOnSkills(skill.SkillSets, user.UserId);
                ViewBag.SkillsJobs = jobs;
            }
            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(JobSeekerManagementController), ex);

            }
            return PartialView("DashboardPartialPage", dashboardsummary);
        }

        [HttpGet]
        [Route("[action]")]
        public PartialViewResult GetJobseekerAppliedJobs()
        {
            List<SearchJobListViewModel> appliedJobs = null;
            var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
            user = user ?? new UserViewModel();
            try
            {
                appliedJobs = userProfileHandler.GetJobseekerAppliedJobs(user.UserId);
                ViewBag.Appliedjob = appliedJobs;
            }
            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(JobSeekerManagementController), ex);

            }
            return PartialView("JobseekerAppliedJobsPartial");
        }

        [HttpGet]
        [Route("[action]")]
        public PartialViewResult GetJobseekerViewedProfile()
        {
            List<JobSeekerViewedProfile> viwedProfile = null;
            var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
            user = user ?? new UserViewModel();
            try
            {
                viwedProfile = userProfileHandler.GetJobseekerViewedProfile(user.UserId);
                ViewBag.ViewedProfile = viwedProfile;
            }
            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(JobSeekerManagementController), ex);

            }
            return PartialView("JobseekerViewedProfilePartial");
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult DeleteAppliedJob(int JobPostId)
        {
            var result = false;
            if (JobPostId != 0)
            {
                var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
                user = user ?? new UserViewModel();
                try
                {
                    result = userProfileHandler.DeleteAppliedJob(JobPostId, user.UserId);
                }
                catch (InvalidUserCredentialsException ex)
                {
                    Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(JobSeekerManagementController), ex);
                    ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
                }
                catch (UserNotFoundException ex)
                {
                    Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(JobSeekerManagementController), ex);
                    ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
                }
                return Json(result);
            }
            else
            {
                return Json(result);
            }
        }

        [HttpGet]
        [Route("[action]")]
        public PartialViewResult EmployerFollowers()
        {
            List<EmployerFollowers> empFollowers = null;
            var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
            user = user ?? new UserViewModel();
            try
            {
                empFollowers = userProfileHandler.EmployerFollowers(user.UserId);
                ViewBag.EmployerFollowers = empFollowers;
            }
            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(JobSeekerManagementController), ex);

            }
            return PartialView("EmployerFollowersPartial");
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult UnfollowCompany(int EmployerId)
        {
            var result = false;
            if (EmployerId != 0)
            {
                var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
                user = user ?? new UserViewModel();
                try
                {
                    result = userProfileHandler.UnfollowEmployer(EmployerId, user.UserId);
                }
                catch (InvalidUserCredentialsException ex)
                {
                    Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(JobSeekerManagementController), ex);
                    ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
                }
                catch (UserNotFoundException ex)
                {
                    Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(JobSeekerManagementController), ex);
                    ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
                }
                return Json(result);
            }
            else
            {
                return Json(result);
            }
        }

        [HttpGet]
        [Route("[action]")]
        public PartialViewResult ManageResume()
        {
            return PartialView("ResumeBuilderPartial");
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult JobsAlert(int isAlert)
        {
            var result = false;
            var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
            user = user ?? new UserViewModel();
            try
            {
                result = userProfileHandler.JobsAlerts(isAlert, user.UserId);
            }
            catch (InvalidUserCredentialsException ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(JobSeekerManagementController), ex);
                ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
            }
            catch (UserNotFoundException ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(JobSeekerManagementController), ex);
                ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
            }
            return Json(result);

        }

        [HttpGet]
        [Route("[action]")]
        public PartialViewResult ContactedDetails()
        {
            List<MessageViewModel> JobSeekerContacted = null;
            var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
            user = user ?? new UserViewModel();
            try
            {
                JobSeekerContacted = userProfileHandler.JobSeekerContacted(user.UserId);
                ViewBag.JobSeekerContacted = JobSeekerContacted;
            }
            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(JobSeekerManagementController), ex);

            }
            return PartialView("JobSeekerContactedPartial");
        }
    }
}