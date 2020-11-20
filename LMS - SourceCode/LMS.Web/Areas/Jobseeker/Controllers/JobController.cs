using LMS.Business.Interfaces.Employer.JobPost;
using LMS.Business.Interfaces.Home;
using LMS.Business.Interfaces.Jobseeker;
using LMS.Model.DataViewModel.Employer.JobPost;
using LMS.Model.DataViewModel.JobSeeker;
using LMS.Model.DataViewModel.Shared;
using LMS.Utility.Exceptions;
using LMS.Utility.ExtendedMethods;
using LMS.Utility.Helpers;
using LMS.Web.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LMS.Web.Areas.Jobseeker.Controllers
{
    [Area("Jobseeker")]
    [Route("[controller]")]

    [HandleExceptionsAttribute]
    public class JobController : Controller
    {
        private readonly IUserProfileHandler userProfileHandler;
        private readonly IJobPostHandler jobpastHandler;
        private readonly IHomeHandler homeHandler;
        private readonly ISearchJobHandler searchJobHandler;
        public JobController(IJobPostHandler _jobpastHandler, IHomeHandler _homeHandler, IUserProfileHandler _userProfileHandler, ISearchJobHandler _searchJobHandler)
        {
            jobpastHandler = _jobpastHandler;
            homeHandler = _homeHandler;
            searchJobHandler = _searchJobHandler;
            userProfileHandler = _userProfileHandler;
        }

        public IActionResult Index()
        {
            return View();
        }

        //[HttpGet]
        //[Route("[action]")]
        //public IActionResult SearchJobList([FromQuery]SearchJobViewModel searches)
        //{

        //    return View();
        //}

        [HttpPost]
        [HttpGet]
        [Route("[action]")]
        public IActionResult SearchJobList(SearchJobViewModel searches)
        {
            List<SearchJobListViewModel> lstjobList = new List<SearchJobListViewModel>();
            var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
            user = user ?? new UserViewModel();
            try
            {
                if (null == user)
                {
                    user = new UserViewModel();
                }
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

                ViewBag.JobIndustryArea = jobpastHandler.GetJobIndustryAreaWithJobPost();
                ViewBag.City = homeHandler.GetCityHasJobPostId();
                ViewBag.Company = homeHandler.GetCompanyHasJobPostId();
                ViewBag.Searches = searches;
                lstjobList = searchJobHandler.SearchJobList(searches, user.UserId);
            }

            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(JobController), ex);
                ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
            }
            catch(Exception ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(JobController), ex);
            }
            return View(lstjobList);
        }


        [HttpGet]
        [Route("[action]")]
        //[UserAuthentication(Constants.JobSeekers)]
        public IActionResult ApplyJob(int jobPostId, string currentUrl)
        {
            string applyJobURL = currentUrl;
            HttpContext.Session.Set<string>(Constants.SessionRedirectUrl, applyJobURL);

            var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
            user = user ?? new UserViewModel();
            string result = "";
            try
            {
                if (user.UserId == 0)
                {
                    result = "Please login to apply this job";
                }

                else if (user.RoleName == "Student")
                {
                    if (userProfileHandler.ApplyJobDetails(user, jobPostId))
                    {
                        result = "Job applied";
                    }
                }
                else
                {
                    result = "Oops! Applicable For Job Seeker Only.";
                }
            }

            catch (FaildToApplyJob ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(JobController), ex);
                result = ex.Message;
                ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
            }
            catch (DataNotUpdatedException ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(JobController), ex);
                result = ex.Message;
                ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
            }

            catch (AllReadyExistJob ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(JobController), ex);
                result = ex.Message;
                ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
            }
            catch (System.Net.Mail.SmtpFailedRecipientException ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(JobController), ex);
                result = "Job applied";
                ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
            }
            return Json(result);
        }


        [HttpGet]
        [Route("[action]")]
        //[UserAuthentication(Constants.AllRoles)]
        public IActionResult JobDetails(int jobid)
        {
            JobPostViewModel jobdetail = new JobPostViewModel();
            var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
            user = user ?? new UserViewModel();
            try
            {
                jobdetail = jobpastHandler.GetJobDetails(jobid);
                //string imgname = Path.GetFileName(jobdetail.CompanyLogo);
                //jobdetail.CompanyLogo = $@"/ProfilePic/" + imgname;
                if (user != null)
                {
                    List<int> appliedjobs = homeHandler.GetAplliedJobs(user.UserId);
                    //if(jobid == appliedjobs.)
                    for (int i = 0; i < appliedjobs.Count; i++)
                    {
                        //getting the all the jobs applied by user only if the user logged in
                        if (appliedjobs[i] == jobid)
                        {
                            jobdetail.IsApplied = true;
                            break;
                        }
                    }
                }

            }
            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(JobController), ex);
                //result = ex.Message;
                ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
            }
            return View(jobdetail);
        }

        [HttpGet]
        [Route("[action]")]
        [UserAuthentication(Constants.JobSeekers)]
        public IActionResult RecommendedJobs()
        {
            List<SearchJobListViewModel> list = new List<SearchJobListViewModel>();
            var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
            user = user ?? new UserViewModel();
            try
            {
                //list = jobpastHandler.RecommendedJobs(user.SSCJobRoleId);
                if (user != null)
                {
                    List<int> appliedjobs = homeHandler.GetAplliedJobs(user.UserId);
                    for (int i = 0; i < appliedjobs.Count; i++)
                    {
                        list[i].IsApplied = appliedjobs.Any(aj => aj == list[i].JobPostId);
                    }                    
                }
            }
            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(JobController), ex);
                ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
            }
            return View(list);
        }
    }
}