using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using LMS.Business.Interfaces.Employer;
using LMS.Business.Interfaces.Employer.JobPost;
using LMS.Business.Interfaces.Home;
using LMS.Model.DataViewModel.Employer.Dashboard;
using LMS.Model.DataViewModel.Employer.JobPost;
using LMS.Model.DataViewModel.Shared;
using LMS.Utility.Exceptions;
using LMS.Utility.ExtendedMethods;
using LMS.Utility.Helpers;
using LMS.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Web.Areas.Employer.Controllers
{
    [Area("Employer")]
    [Route("[controller]")]
    [UserAuthentication(Constants.CorporateRole + "," + Constants.StaffingPartnerRole + "," + Constants.AdminRole)]
    public class DashboardController : Controller
    {

        private readonly IDashboardHandler dashboardHandler;
        private readonly IJobPostHandler _jobpastHandler;
        private readonly IHomeHandler _homeHandler;
        public DashboardController(IDashboardHandler _dashboardHandler, IJobPostHandler jobpastHandler,IHomeHandler homeHandler)
        {
            dashboardHandler = _dashboardHandler;
            _jobpastHandler = jobpastHandler;
            _homeHandler = homeHandler;
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult EmpDashboard()
        {
            //var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
            //var dBoard = dashboardHandler.GetDashboard(user.UserId);
            //ViewBag.JobTitle = _jobpastHandler.GetJobTitleDetails() ?? new List<JobTitleViewModel>();
            //ViewBag.CityList = dashboardHandler.GetCityListWithoutState() ?? new List<CityViewModel>();
            //return View(dBoard);
            return View();
        }

        [HttpGet]
        [Route("[action]")]
        public PartialViewResult EmpDashboardData()
        {
            var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
            var dBoard = dashboardHandler.GetDashboard(user.UserId);
            ViewBag.JobTitle = _jobpastHandler.GetJobTitleDetails() ?? new List<JobTitleViewModel>();
            ViewBag.CityList = dashboardHandler.GetCityListWithoutState() ?? new List<CityViewModel>();
            return PartialView("_EmpDashboardDataPartial",dBoard);
        }

        [HttpGet]
        [Route("[action]")]
        public PartialViewResult GetEmployerDetail()
        {
            UserViewModel empDetail = null;
            var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
            user = user ?? new UserViewModel();
            try
            {
                empDetail = dashboardHandler.GetProfileData(user.UserId);
            }
            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(DashboardController), ex);
                empDetail = new UserViewModel();
            }
            return PartialView("EmployerDetailPartial", empDetail);
        }

        [HttpGet]
        [Route("[action]")]
        public PartialViewResult GetJobSeekersBasedOnEmployerHiringCriteria(string year,string city,string role)
        {

            IEnumerable<UserViewModel> jobseekers = null;
            var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
            user = user ?? new UserViewModel();
            try
            {
                ViewBag.AllJobRoles = _homeHandler.GetAllJobRoles();
                ViewBag.AllCities = dashboardHandler.GetCityListWithoutState();
                jobseekers = dashboardHandler.GetJobSeekersBasedOnEmployerHiringCriteria(user.UserId,year,city,role);              
            }
            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(DashboardController), ex);
                jobseekers = new List<UserViewModel>();
            }
            return PartialView("JobsSeekersPartial", jobseekers);
        }

        [HttpGet]
        [Route("[action]")]
        public PartialViewResult GetJobs(int year, int employer = 0)
        {
            IEnumerable<JobPostViewModel> jobs = null;
            var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
            user = user ?? new UserViewModel();
            try
            {
                if (user.RoleName == Constants.AdminRole)
                {
                    ViewBag.Employers = dashboardHandler.GetEmployers();
                    ViewBag.SelectedEmployer = employer;
                }
                else
                {
                    employer = user.UserId;
                }
                jobs = dashboardHandler.GetJobs(employer, year);
            }
            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(DashboardController), ex);
                jobs = new List<JobPostViewModel>();
            }
            return PartialView("JobsPartial", jobs);
        }

        [HttpGet]
        [Route("[action]")]
        public PartialViewResult GetJobSeekers()
        {
            IEnumerable<Model.DataViewModel.Employer.Dashboard.JobSeekerViewModel> employees = null;
            var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
            user = user ?? new UserViewModel();
            try
            {
                employees = dashboardHandler.GetJobSeekers(user.UserId);
            }
            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(DashboardController), ex);
                employees = new List<Model.DataViewModel.Employer.Dashboard.JobSeekerViewModel>();
            }
            return PartialView("JobsSeekersWithJobTitlePartial", employees);
        }

        [HttpGet]
        [Route("[action]")]
        public PartialViewResult AddJobsPartial()
        {
            try
            {
                string msg = Convert.ToString(TempData["msg"]);
                if (msg != "" || msg != null)
                {
                    ViewData["SuccessMessage"] = msg;
                }
                ViewBag.JobTitle = _jobpastHandler.GetJobTitleDetails();
                ViewBag.JobIndustryArea = _jobpastHandler.GetJobIndustryAreaDetails();
                //ViewBag.Gender = jobpastHandler.GetGenderListDetail();
                ViewBag.EmploymentStatus = _jobpastHandler.GetJobJobEmploymentStatusDetails();
                ViewBag.EmploymentType = _jobpastHandler.GetJobJobEmploTypeDetails();
                ViewBag.Country = _jobpastHandler.GetCountryDetails();
                ViewBag.JobTypes = _jobpastHandler.GetJobTypes();                
            }
            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, 0, typeof(DashboardController), ex);
                ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
            }
            return PartialView("_AddJobPartial");
        }
        [HttpGet]
        [Route("[action]")]
        public PartialViewResult MyProfilePartial()
        {
            var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
            ViewBag.Emailid = user.Email;
            return PartialView("_MyProfilePartial",user);
        }

        [HttpGet]
        [Route("[action]")]
        public PartialViewResult GetViewedProfiles()
        {
            IEnumerable<UserViewModel> jSeekers = null;
          
            var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
            user = user ?? new UserViewModel();
            try
            {
               jSeekers = dashboardHandler.GetViewedProfiles(user.UserId);
                ViewData["isViewdProfile"] = 1;
            }
            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(DashboardController), ex);
                jSeekers = new List<UserViewModel>();
            }

            return PartialView("JobsSeekersPartial", jSeekers);
        }

        [HttpGet]
        [Route("[action]")]
        public PartialViewResult GetJobScreenById(int jobId)
        {
            JobPostViewModel job = null;
            var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
            user = user ?? new UserViewModel();
            try
            {
                ViewData["RoleName"] = user.RoleName;
                ViewBag.Countries = dashboardHandler.GetCountries();
                ViewBag.JobRoles = dashboardHandler.GetJobRoles();
                job = dashboardHandler.GetJob(jobId, user.UserId);
                ViewBag.States = dashboardHandler.GetStates(job.CountryCode);
                ViewBag.Cities = dashboardHandler.GetCities(job.StateCode);
            }
            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(DashboardController), ex);
                job = new JobPostViewModel();
            }
            return PartialView("EditJobPartial", job);
        }

        [HttpGet]
        [Route("[action]")]
        public PartialViewResult GetMessages(string date)
        {
            IEnumerable<MessageViewModel> messages = null;
            var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
            user = user ?? new UserViewModel();
            try
            {
                DateTime _date = DateTime.Parse(date);
                //DateTime _date = DateTime.ParseExact(date,"MM/dd/yyyy", CultureInfo.InvariantCulture);
                messages = dashboardHandler.GetMessages(_date, user.UserId);
            }            
            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(DashboardController), ex);
                messages = new List<MessageViewModel>();
            }            
            return PartialView("MessagesPartial", messages);
        }

        [HttpPost]
        [Route("[action]")]
        public JsonResult UpdateJobDetails([FromBody]JobPostViewModel model)
        {
            bool isUpdated = true;
            var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
            user = user ?? new UserViewModel();
            try
            {
                dashboardHandler.UpdateJob(model, user.UserId);
            }

            catch (DataNotUpdatedException ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(DashboardController), ex);
                isUpdated = false;
            }
            return new JsonResult(new { isUpdated = isUpdated });
        }

        [HttpPost]
        [Route("[action]")]
        public PartialViewResult GetReplyPrompt([FromBody]MessageViewModel message)
        {
            ViewBag.MessageId = message.MessageId;
            ViewBag.SenderId = message.ReceiverId;
            ViewBag.ReceiverId = message.SenderId;
            EmailViewModel email = new EmailViewModel
            {
                From = message.ReceiverEmail,
                To = new string[] { message.SenderEmail },
            };
            return PartialView("EmailPromptPartial", email);
        }

        [HttpPost]
        [Route("[action]")]
        public JsonResult ReplyToJobSeeker([FromBody]MessageViewModel model)
        {
            bool isSuccess = true;
            var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
            user = user ?? new UserViewModel();
            try
            {
                dashboardHandler.ReplyToJobSeeker(model, user.UserId);
            }
            catch (DataNotUpdatedException ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(DashboardController), ex);
                isSuccess = false;
            }
            return new JsonResult(new { isSuccess = isSuccess });
        }
        [HttpGet]
        [Route("[action]")]
        public IActionResult CityDetails(string stateCode)
        {
            IEnumerable<CityViewModel> cityList = new List<CityViewModel>();
            try
            {
                cityList = dashboardHandler.GetCities(stateCode);

            }
            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, 0, typeof(JobManagementController), ex);
                ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
            }
            return Json(cityList);
        }
    }
}
