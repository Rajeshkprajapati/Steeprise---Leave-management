using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMS.Business.Handlers.Admin;
using LMS.Business.Interfaces.Admin;
using LMS.Business.Interfaces.Home;
using LMS.Model.DataViewModel.JobSeeker;
using LMS.Utility.Exceptions;
using LMS.Utility.Helpers;
using LMS.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("[controller]")]
    [HandleExceptionsAttribute]
    [UserAuthentication(Constants.AdminRole)]
    public class ManageJobsController : Controller
    {
        private readonly IHomeHandler _homeHandler;
        private readonly IManageJobsHandler _managejobshandler;
        public ManageJobsController(IHomeHandler homeHandler, IManageJobsHandler managejobshandler)
        {
            _homeHandler = homeHandler;
            _managejobshandler = managejobshandler;
        }
        public ViewResult Jobs()
        {
            return View();
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult FeaturedJobs()
        {
            List<SearchJobListViewModel> list = new List<SearchJobListViewModel>();
            try
            {
                list = _homeHandler.ViewAllFeaturedJobs();
            }
            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, 0, typeof(ManageJobsController), ex);
                ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
            }
            return View(list);
        }

        [HttpGet]
        [Route("[action]")]
        public JsonResult UpdateFeaturedJobDisplayOrder(int JobPostId,int FeaturedJobDisplayOrder)
        {
            string result = string.Empty;
            try
            {
                if (JobPostId != 0 && _managejobshandler.UpdateFeaturedJobDisplayOrder(JobPostId, FeaturedJobDisplayOrder))
                {
                    result = "Updated display Order";
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return Json(new { msg = result});
        }
        [HttpGet]
        [Route("[action]")]
        public JsonResult DeleteFeaturedJob(int jobpostid)
        {
            string result = string.Empty;
            try
            {
                if (_managejobshandler.DeleteFeaturedJob(jobpostid))
                {
                    result = "Removed Featured Job";
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return Json(new { msg = result });
        }
    }
}