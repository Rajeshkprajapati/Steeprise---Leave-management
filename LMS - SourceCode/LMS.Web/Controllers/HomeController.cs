using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using LMS.Business.Handlers.Employer.JobPost;
using LMS.Business.Interfaces.Employer.JobPost;
using LMS.Business.Interfaces.Home;
using LMS.Business.Interfaces.Shared;
using LMS.Model.DataViewModel.Admin.JobIndustryArea;
using LMS.Model.DataViewModel.Home;
using LMS.Model.DataViewModel.JobSeeker;
using LMS.Model.DataViewModel.Shared;
using LMS.Utility.Exceptions;
using LMS.Utility.ExtendedMethods;
using LMS.Utility.Helpers;
using LMS.Web.Filters;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace LMS.Web.Controllers
{
    [HandleExceptionsAttribute]

    public class HomeController : Controller
    {
        private readonly IJobPostHandler _jobpastHandler;
        private readonly IHomeHandler _homeHandler;
        private readonly IEMailHandler _mailHandler;
        private readonly IConfiguration _configuration;
        public HomeController(IJobPostHandler jobpastHandler, IHomeHandler homeHandler, IEMailHandler mailhandler, IConfiguration configuration)
        {
            _jobpastHandler = jobpastHandler;
            _homeHandler = homeHandler;
            _mailHandler = mailhandler;
            _configuration = configuration;
        }

        [UserAuthenticationAttribute(Constants.AllRoles)]
        public IActionResult GoToIndex()
        {
            return RedirectToAction("Index"); ;
        }

        public IActionResult Index()
        {
            //try
            //{
            ViewBag.JobIndustryArea = _jobpastHandler.GetJobIndustryAreaDetails();
            ViewBag.AllJobRoles = _homeHandler.GetAllJobRoles();
            ViewBag.PopulerSearchesCategory = _homeHandler.PopulerSearchesCategory();
            ViewBag.PopulerSearchesCity = _homeHandler.PopulerSearchesCity();
            ViewBag.TopEmployer = _homeHandler.TopEmployer();
            List<SearchJobListViewModel> featurejobs = _homeHandler.GetFeaturedJobs();
            featurejobs = featurejobs.OrderBy(o => o.FeaturedJobDisplayOrder).ToList();
            var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
            if (user != null)
            {
                List<int> appliedjobs = _homeHandler.GetAplliedJobs(user.UserId);
                for (int i = 0; i < featurejobs.Count; i++)
                {
                    //getting the all the jobs applied by user only if the user logged in
                    if (user.UserId != 0 && appliedjobs.Count > 0)
                    {
                        featurejobs[i].IsApplied = appliedjobs.Any(aj => aj == featurejobs[i].JobPostId);
                    }
                }
                ViewBag.FeaturedJobs = featurejobs;
            }
            else
            {
                ViewBag.FeaturedJobs = featurejobs;
            }

            List<SearchJobListViewModel> recentJobs = _homeHandler.GetRecentJobs();
            recentJobs = recentJobs.OrderBy(o => o.FeaturedJobDisplayOrder).ToList();
            if (user != null)
            {
                List<int> appliedjobs = _homeHandler.GetAplliedJobs(user.UserId);
                for (int i = 0; i < recentJobs.Count; i++)
                {
                    //getting the all the jobs applied by user only if the user logged in
                    if (user.UserId != 0 && recentJobs.Count > 0)
                    {
                        recentJobs[i].IsApplied = appliedjobs.Any(aj => aj == recentJobs[i].JobPostId);
                    }
                }
                ViewBag.RecentJobs = recentJobs;
            }
            else
            {
                ViewBag.RecentJobs = recentJobs;
            }

            List<SearchJobListViewModel> walkinJobs = _homeHandler.GetWalkInsJobs();
            ViewBag.WalkinJobs = walkinJobs;

            //    //ViewBag.City = homeHandler.GetCityList();
            //}
            //catch (DataNotFound ex)
            //{
            //    Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, 0, typeof(HomeController), ex);
            //    ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
            //}
            return View();
        }

        public IActionResult SucessStoryAndReview()
        {
            //var result = new List<SuccessStoryViewModel>();
            try
            {
                ViewBag.Comment = _homeHandler.GetSuccussStory();
                ViewBag.SuccessStoryVideo = _homeHandler.GetSuccussStoryVideos();
            }
            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, 0, typeof(HomeController), ex);
                ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
                ViewData["CommentViewBag"] = "No review yet! be the first to review";
            }

            return View();
        }

        public IActionResult PostSucessStoryReview(SuccessStoryViewModel model)
        {
            bool isPosted = false;
            var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
            var successStory = new SuccessStoryViewModel
            {
                name = user.FirstName,
                Email = user.Email,
                city = user.City,
                Tagline = model.Tagline,
                Message = model.Message,
                UserId = user.UserId
            };
            try
            {
                isPosted = _homeHandler.PostSuccessStory(successStory);
                if (isPosted)
                {
                    TempData["Feedback"] = "Thank you for posting your feedback.";
                }
                else
                {
                    TempData["Feedback"] = "Review could not post. Please try again.";
                }
            }
            catch (UserCanNotPostData ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user == null ? 0 : user.UserId, typeof(HomeController), ex);
                ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
                isPosted = false;
            }

            return RedirectToAction("SucessStoryAndReview");
        }

        public IActionResult ViewAllFeaturedJobs()
        {
            var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
            try
            {
                List<SearchJobListViewModel> featurejobs = _homeHandler.ViewAllFeaturedJobs();
                featurejobs = featurejobs.OrderBy(o => o.FeaturedJobDisplayOrder).ToList();
                if (user != null)
                {
                    List<int> appliedjobs = _homeHandler.GetAplliedJobs(user.UserId);
                    for (int i = 0; i < featurejobs.Count; i++)
                    {
                        //getting the all the jobs applied by user only if the user logged in
                        if (user.UserId != 0 && appliedjobs.Count > 0)
                        {
                            featurejobs[i].IsApplied = appliedjobs.Any(aj => aj == featurejobs[i].JobPostId);
                        }
                    }
                    ViewBag.AllFeaturedJobs = featurejobs;
                }
                else
                {
                    ViewBag.AllFeaturedJobs = featurejobs;
                }
                //ViewBag.AllFeaturedJobs = _homeHandler.ViewAllFeaturedJobs();
            }
            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user == null ? 0 : user.UserId, typeof(HomeController), ex);
                ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
            }

            return View();
        }

        public IActionResult AllJobsByCategory(int id)
        {

            var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
            try
            {
                List<SearchJobListViewModel> alljobsbycategory = _homeHandler.AllJobsByCategory(id);
                if (user != null)
                {
                    List<int> appliedjobs = _homeHandler.GetAplliedJobs(user.UserId);
                    for (int i = 0; i < alljobsbycategory.Count; i++)
                    {
                        //getting the all the jobs applied by user only if the user logged in
                        if (user.UserId != 0 && appliedjobs.Count > 0)
                        {
                            alljobsbycategory[i].IsApplied = appliedjobs.Any(aj => aj == alljobsbycategory[i].JobPostId);
                        }
                        else
                        {
                            break;
                        }
                    }
                    ViewBag.AllJobsCategory = alljobsbycategory;
                }
                else
                {
                    ViewBag.AllJobsCategory = alljobsbycategory;
                }
            }
            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user == null ? 0 : user.UserId, typeof(HomeController), ex);
                ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
            }

            return View();
        }

        public IActionResult AllJobsByCity(string citycode)
        {
            var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
            try
            {
                List<SearchJobListViewModel> alljobsbycity = _homeHandler.AllJobsByCity(citycode);
                if (user != null)
                {
                    List<int> appliedjobs = _homeHandler.GetAplliedJobs(user.UserId);
                    for (int i = 0; i < alljobsbycity.Count; i++)
                    {
                        //getting the all the jobs applied by user only if the user logged in
                        if (user.UserId != 0 && appliedjobs.Count > 0)
                        {
                            alljobsbycity[i].IsApplied = appliedjobs.Any(aj => aj == alljobsbycity[i].JobPostId);
                        }
                    }
                    ViewBag.AllJobsCity = alljobsbycity;
                }
                else
                {
                    ViewBag.AllJobsCity = alljobsbycity;
                }
            }
            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user == null ? 0 : user.UserId, typeof(HomeController), ex);
                ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));

            }

            return View();
        }

        public IActionResult GetJobCategory()
        {
            List<JobIndustryAreaViewModel> list = new List<JobIndustryAreaViewModel>();
            try
            {
                list = _homeHandler.GetCategory();
            }
            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, 0, typeof(HomeController), ex);
                ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));

            }

            return Json(list);
        }

        public IActionResult TalentConnectLink()
        {
            string link = "";
            try
            {
                link = _homeHandler.TalentConnectLink();
            }
            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, 0, typeof(HomeController), ex);
                ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));

            }
            return Json(link);
        }
        public IActionResult CandidateBulkUpload()
        {
            string link = "";
            try
            {
                link = _homeHandler.CandidateBulkUpload();
            }
            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, 0, typeof(HomeController), ex);
                ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));

            }
            return Json(link);
        }
        public IActionResult TPRegistrationGuide()
        {
            string link = "";
            try
            {
                link = _homeHandler.TPRegistrationGuide();
            }
            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, 0, typeof(HomeController), ex);
                ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));

            }
            return Json(link);
        }
        public IActionResult ContactUs()
        {
            ViewBag.ContactUs = _homeHandler.GetContactUsEmail();
            return View();
        }

        public JsonResult GetCityListChar(string cityFirstChar)
        {
            var result = new List<CityViewModel>();
            try
            {
                result = _homeHandler.GetCityListByChar(cityFirstChar);
            }

            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, 0, typeof(HomeController), ex);
                ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
            }
            return Json(result);

        }

        public JsonResult GetJobTitleList(string jobFirstChar)
        {
            var result = new List<JobTitleViewModel>();
            try
            {
                result = _homeHandler.GetJobListByChar(jobFirstChar);
            }

            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, 0, typeof(HomeController), ex);
                ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
            }
            return Json(result);

        }

        public IActionResult CompanyListing()
        {
            var model = new List<SearchJobListViewModel>();
            try
            {
                model = _homeHandler.GetAllCompanyList();
            }
            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, 0, typeof(HomeController), ex);
                ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
            }
            return View(model);
        }

        public IActionResult Career()
        {
            var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
            try
            {
                List<SearchJobListViewModel> list = _homeHandler.NasscomJobs();
                if (user != null)
                {
                    List<int> appliedjobs = _homeHandler.GetAplliedJobs(user.UserId);
                    for (int i = 0; i < list.Count; i++)
                    {
                        //getting the all the jobs applied by user only if the user logged in
                        if (user.UserId != 0 && appliedjobs.Count > 0)
                        {
                            list[i].IsApplied = appliedjobs.Any(aj => aj == list[i].JobPostId);
                        }
                    }
                    ViewBag.NasscomJobs = list;
                }
                else
                {
                    ViewBag.NasscomJobs = list;
                }
            }
            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user == null ? 0 : user.UserId, typeof(HomeController), ex);
                ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
            }
            return View();
        }

        [HttpPost]
        public IActionResult ConatctUs(ContactUs model)
        {
            var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
            try
            {
                string[] to = new string[1];
                if (user != null)
                {
                    to[0] = user.Email;
                }
                else
                {
                    to[0] = model.Email;
                }
                EmailViewModel mailtouser = new EmailViewModel()
                {
                    To = to,
                    Subject = "Contact-US",
                    Body = "We have received your request,We will get back to you soon.",
                    IsHtml = false,
                    From = "amaggo@nasscom.com",
                };
                _mailHandler.SendMail(mailtouser, 0, false);
                string adminmail = _configuration["AdminMail:Email"];
                string[] toadmin = { adminmail };
                EmailViewModel mailtoadmin = new EmailViewModel()
                {
                    To = toadmin,
                    Subject = "Contact-US",
                    Body = "<p>Body:" + model.Details + " " + "<br/><br/>Name:" + model.Fullname + " <br/><br/>Email: " + model.Email + "</p>",
                    IsHtml = true,
                    From = "amaggo@nasscom.com",
                };
                _mailHandler.SendMail(mailtoadmin, 0, false);
                ViewBag.Contact = "We have received your request,We will get back to you soon.";
            }
            catch (Exception ex)
            {
                ViewBag.ContactError = "Unable to process";//ex.Message;
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user == null ? 0 : user.UserId, typeof(HomeController), ex);
                ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
            }
            return View("Contactus");

        }

        [HttpPost]
        public IActionResult EmployerFollower(int EmployerId)
        {
            var result = false;
            if (EmployerId != 0)
            {
                var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
                user = user ?? new UserViewModel();
                try
                {
                    result = _homeHandler.EmployerFollower(EmployerId, user.UserId);
                }
                catch (InvalidUserCredentialsException ex)
                {
                    Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(HomeController), ex);
                    ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
                }
                catch (UserNotFoundException ex)
                {
                    Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(HomeController), ex);
                    ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
                }
                return Json(result);
            }
            else
            {
                return Json(result);
            }
        }

        public IActionResult FindJobVacancies()
        {
            var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
            try
            {
                ViewBag.CityJobs = _homeHandler.CityJobVacancies();
                ViewBag.CategoryJobs = _homeHandler.CategoryJobVacancies();
                ViewBag.CompanyJobs = _homeHandler.CompanyJobVacancies();
            }
            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user == null ? 0 : user.UserId, typeof(HomeController), ex);
                ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));

            }
            return View();
        }

        public IActionResult AllJobsByCompany(int UserId)
        {
            var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
            try
            {
                List<SearchJobListViewModel> alljobsbycompany = _homeHandler.AllJobsByCompany(UserId);
                if (user != null)
                {
                    List<int> appliedjobs = _homeHandler.GetAplliedJobs(user.UserId);
                    for (int i = 0; i < alljobsbycompany.Count; i++)
                    {
                        //getting the all the jobs applied by user only if the user logged in
                        if (user.UserId != 0 && appliedjobs.Count > 0)
                        {
                            alljobsbycompany[i].IsApplied = appliedjobs.Any(aj => aj == alljobsbycompany[i].JobPostId);
                        }
                    }
                    ViewBag.AllJobsCompany = alljobsbycompany;
                }
                else
                {
                    ViewBag.AllJobsCompany = alljobsbycompany;
                }
            }
            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user == null ? 0 : user.UserId, typeof(HomeController), ex);
                ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));

            }

            return View();
        }
    }
}