using LMS.Business.Interfaces.Jobseeker;
using LMS.Business.Interfaces.Shared;
using LMS.Model.DataViewModel.JobSeeker;
using LMS.Model.DataViewModel.Shared;
using LMS.Utility.Exceptions;
using LMS.Utility.ExtendedMethods;
using LMS.Utility.FilesUtility;
using LMS.Utility.Helpers;
using LMS.Web.Filters;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace LMS.Web.Areas.Jobseeker.Controllers
{
    [Area("Jobseeker")]
    [Route("[controller]")]
    [UserAuthentication("Student,Training Partner")]
    [HandleExceptionsAttribute]
    public class ResumeBuilderController : Controller
    {
        private readonly IResumeBuilderHandler _rBuilderHandler;
        private readonly IFileHandler fileHandler;
        private readonly IHostingEnvironment environment;

        public ResumeBuilderController(IResumeBuilderHandler handler, IFileHandler _fileHandler, IHostingEnvironment env)
        {
            environment = env;
            _rBuilderHandler = handler;
            fileHandler = _fileHandler;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("[action]")]
        public JsonResult GetUserDetails()
        {
            bool isSuccess = true;
            dynamic data = null;
            var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
            user = user ?? new UserViewModel();
            try
            {
                data = _rBuilderHandler.GetUserDetails(user.UserId);

            }

            catch (DataNotUpdatedException ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(ResumeBuilderController), ex);
                isSuccess = false;
            }
            return new JsonResult(
                new { isSuccess = isSuccess, data = data },
                ContractSerializer.JsonInPascalCase()
                );
        }

        [HttpPost]
        [Route("[action]")]
        public JsonResult SavePersonalDetails([FromBody]UserViewModel[] users)
        {
            bool isSuccess = true;
            var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
            user = user ?? new UserViewModel();
            try
            {
                _rBuilderHandler.InsertPersonalDetails(user.UserId, users[0]);
            }

            catch (DataNotUpdatedException ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(ResumeBuilderController), ex);
                isSuccess = false;
            }
            return new JsonResult(new { isSuccess = isSuccess });
        }

        [HttpPost]
        [Route("[action]")]
        public JsonResult SaveExperienceDetails([FromBody]UserDetail expDetails)
        {
            bool isSuccess = true;
            var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
            user = user ?? new UserViewModel();
            try
            {
                _rBuilderHandler.InsertExperienceDetails(user.UserId, expDetails.ExperienceDetails, expDetails.Skills);
            }

            catch (DataNotUpdatedException ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(ResumeBuilderController), ex);
                isSuccess = false;
            }
            return new JsonResult(new { isSuccess = isSuccess });
        }

        [HttpPost]
        [Route("[action]")]
        public JsonResult SaveEducationDetails([FromBody]EducationalDetails[] eduDetails)
        {
            bool isSuccess = true;
            var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
            try
            {
                _rBuilderHandler.InsertEducationDetails(user.UserId, eduDetails);
            }

            catch (DataNotUpdatedException ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(ResumeBuilderController), ex);
                isSuccess = false;
            }
            return new JsonResult(new { isSuccess = isSuccess });
        }

        [Route("[action]")]
        public IActionResult ResumePreview()
        {
            bool isSuccess = true;
            string errMsg = string.Empty;
            var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
            try
            {
                ViewBag.ResumeTemplate = "ResumeTemplates/BasicResumePartial";
                ResumeViewModel details = _rBuilderHandler.GetUserInfoToCreateResume(user.UserId);
                return View("ResumePreview", details);
            }

            catch (FileNotDownloadedException ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(ResumeBuilderController), ex);
                isSuccess = false;
                errMsg = ex.Message;
            }
            return new JsonResult(new { isSuccess = isSuccess, msg = errMsg });
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult CreateResume([FromBody]string htmlContent)
        {
            bool isSuccess = true;
            string errMsg = string.Empty;
            var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
            try
            {
                _rBuilderHandler.CreateResume(user.UserId, htmlContent);
            }

            catch (FileNotDownloadedException ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(ResumeBuilderController), ex);
                isSuccess = false;
                errMsg = ex.Message;
            }
            return new JsonResult(new { isSuccess = isSuccess, msg = errMsg });
        }

        [Route("[action]")]
        public async Task<IActionResult> DownloadResume()
        {
            bool isSuccess = true;
            string errMsg = string.Empty;
            var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
            try
            {
                var filePath = _rBuilderHandler.GetResume(Convert.ToInt32(user.UserId));
                return File(await fileHandler.FileToStream(filePath), FileTypes.MimeTypes[Path.GetExtension(filePath).ToLowerInvariant()], Path.GetFileName(filePath));
            }

            catch (FileNotDownloadedException ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(ResumeBuilderController), ex);
                isSuccess = false;
                errMsg = ex.Message;
            }
            return new JsonResult(new { isSuccess = isSuccess, msg = errMsg });
        }

        [Route("[action]")]
        public JsonResult GetStates(string countryCode)
        {
            IEnumerable<StateViewModel> states = null;
            try
            {
                states = _rBuilderHandler.GetStates(countryCode);
                return new JsonResult(new { isSuccess = true, States = states }, ContractSerializer.JsonInPascalCase());
            }

            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, 0, typeof(ResumeBuilderController), ex);
                return new JsonResult(new { isSuccess = false });
            }
        }

        [Route("[action]")]
        public JsonResult GetCities(string stateCode)
        {
            IEnumerable<CityViewModel> cities = null;
            try
            {
                cities = _rBuilderHandler.GetCities(stateCode);
                return new JsonResult(new { isSuccess = true, Cities = cities }, ContractSerializer.JsonInPascalCase());
            }

            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, 0, typeof(ResumeBuilderController), ex);
                return new JsonResult(new { isSuccess = false });
            }
        }

        [Route("[action]")]
        public JsonResult GetCourses(int cCategory)
        {
            IEnumerable<CourseViewModel> courses = null;
            try
            {
                courses = _rBuilderHandler.GetCourses(cCategory);
                return new JsonResult(new { isSuccess = true, Courses = courses }, ContractSerializer.JsonInPascalCase());
            }

            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, 0, typeof(ResumeBuilderController), ex);
                return new JsonResult(new { isSuccess = false });
            }
        }

    }
}