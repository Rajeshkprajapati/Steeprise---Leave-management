using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMS.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using LMS.Utility.Exceptions;
using LMS.Business.Interfaces.Admin;
using Microsoft.AspNetCore.Http;
using LMS.Model.DataViewModel.Shared;
using LMS.Utility.Helpers;
using LMS.Utility.ExtendedMethods;

namespace LMS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("[controller]")]
    [HandleExceptionsAttribute]
    [UserAuthentication("Admin")]
    public class JobTitleController : Controller
    {
        private readonly IJobTitleHandler _jobTitleHandler;
        public JobTitleController(IJobTitleHandler jobTitleHandler)
        {
            _jobTitleHandler = jobTitleHandler;
        }
        public IActionResult Index()
        {
            return View();
        }

        //[HttpGet]
        //[Route("[action]")]
        //public IActionResult GetJobTitle()
        //{
        //    List<JobTitleViewModel> list = new List<JobTitleViewModel>();
        //    try
        //    {                
        //            list = _jobTitleHandler.GetJobTitle();
        //    }
        //    catch (DataNotFound ex)
        //    {
        //        Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, 0, typeof(JobTitleController), ex);
        //        ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
        //    }
        //    return View(list);
        //}

        [HttpPost]
        [Route("[action]")]
        public IActionResult InsertUpdateJobTitle([FromBody]JobTitleViewModel jobTitleModel)
        {
            var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
            
            var result = _jobTitleHandler.InsertUpdateJobTile(jobTitleModel);
            if (result && jobTitleModel.JobTitleId != 0)
            {
                //return View();
                return Json("Record Updated");
            }
            else if (result && jobTitleModel.JobTitleId == 0)
            {
                return Json("Record Added");
            }

            return Json("Unable to do this action");
        }
        [HttpGet]
        [Route("[action]")]
        public IActionResult DeleteJobTitle(string jobTitleId)
        {
            var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
            var result = _jobTitleHandler.DeleteJobTitle(jobTitleId, Convert.ToString(user.UserId));
            if (result)
            {
                //return View();
                return Json("Record Deleted");
            }
            return Json("Record Can't be Deleted");
            //return View();
        }

        [HttpGet]
        [Route("[action]")]
        public PartialViewResult GetJobTitle()
        {
            List<JobTitleViewModel> list = new List<JobTitleViewModel>();
            try
            {
                list = _jobTitleHandler.GetJobTitle();
            }
            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, 0, typeof(JobTitleController), ex);
                ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
            }
            return PartialView("GetJobTitle", list);
        }
    }
}