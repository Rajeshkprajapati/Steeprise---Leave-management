using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using LMS.Business.Interfaces.Employer.JobPost;
using LMS.Business.Interfaces.Employer.SearchResume;
using LMS.Business.Interfaces.Home;
using LMS.Business.Interfaces.Jobseeker;
using LMS.Business.Interfaces.Shared;
using LMS.Model.DataViewModel.Employer.SearchResume;
using LMS.Model.DataViewModel.Shared;
using LMS.Utility.Exceptions;
using LMS.Utility.ExtendedMethods;
using LMS.Utility.Helpers;
using LMS.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace LMS.Web.Areas.Employer.Controllers
{
    [Area("Employer")]
    [Route("[controller]")]
    [HandleExceptionsAttribute]
    [UserAuthentication(Constants.CorporateRole + "," + Constants.StaffingPartnerRole)]
    public class SearchResumeController : Controller
    {

        private readonly IJobPostHandler jobpastHandler;
        private readonly IHomeHandler homeHandler;
        private readonly ISearchResumeHandler searchresumehandler;
        private readonly IEMailHandler emailHandler;
        private readonly IConfiguration config;
        public SearchResumeController(IEMailHandler _emailHandler,IConfiguration _config, IJobPostHandler _jobpastHandler, IHomeHandler _homeHandler, ISearchResumeHandler _searchResumeHandler)
        {
            jobpastHandler = _jobpastHandler;
            homeHandler = _homeHandler;
            searchresumehandler = _searchResumeHandler;
            emailHandler = _emailHandler;
            config = _config;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [HttpGet]
        [Route("[action]")]
        public IActionResult SearchResumeList(SearchResumeViewModel searches)
        {
            List<SearchResumeListViewModel> lstResumeList = new List<SearchResumeListViewModel>();
            try
            {
                var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
                var props = searches.GetType().GetProperties();
                foreach (PropertyInfo prop in props)
                {
                    if (prop.PropertyType.IsArray)
                    {
                        var values = prop.GetValue(searches) as string[];
                        if (null != values)
                        {
                            List<string> finalValues = new List<string>();
                            foreach (var value in values)
                            {
                                if (null != value)
                                {
                                    finalValues.AddRange(value.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries));
                                }
                            }
                            prop.SetValue(searches, finalValues.ToArray());
                        }
                    }
                }
                ViewBag.JobIndustryArea = jobpastHandler.GetJobIndustryAreaWithStudentData();
                ViewBag.City = homeHandler.GetCitiesWithJobSeekerInfo();
                ViewBag.Searches = searches;
                lstResumeList = searchresumehandler.GetSearchResumeList(searches);
            }

            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, 0, typeof(SearchResumeController), ex);
            }
            return View(lstResumeList);
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult ShowCandidateDetail(int userId)
        {
            SearchResumeListViewModel listresume = new SearchResumeListViewModel();
            var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
            user = user ?? new UserViewModel();
            try
            {
                listresume = searchresumehandler.ShowCandidateDetails(user.UserId, userId);
            }
            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(SearchResumeController), ex);
                //ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
            }
            return View(listresume);
        }


        [HttpGet]
        [Route("[action]")]
        public IActionResult SendMessage(string userEmail,string JobSeekerName)
        {
            bool isSend = true;
            string errorMessage;
            var user  = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo) ?? new UserViewModel();
            JobSeekerName = string.IsNullOrWhiteSpace(JobSeekerName) ? "Candidate" : JobSeekerName;
            try
            {
                var eModel = new EmailViewModel
                {
                    Subject = "New Job from Placement Portal",
                    Body = "Dear " + JobSeekerName + ",<br/>Your resume has been shortlisted by " + user.CompanyName + ".<br/>The employer will connect with you for further processing.<br/><br/>Thank You<br/>Placement Portal Team",
                    To = new string[] { userEmail },
                    From = config["EmailCredential:Fromemail"],
                    IsHtml = true,
                    MailType = (int)MailType.NotAllowed
                };
                emailHandler.SendMail(eModel,-1);

                errorMessage = "Your mail has been successfully send to the Jobseeker";
                return Json(new { isSend, errorMessage });
            }
            catch
            {
                ViewData["Error"] = "your mail has not been send";
                isSend = false;
                errorMessage = "mail not send";

            }
            return Json(new { isSend, errorMessage });
        }

    }
}