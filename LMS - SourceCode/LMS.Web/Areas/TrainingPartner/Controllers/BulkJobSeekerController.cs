using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LMS.Business.Interfaces.TrainingPartner;
using LMS.Model.DataViewModel.Shared;
using LMS.Model.DataViewModel.TrainingPartner;
using LMS.Utility.Exceptions;
using LMS.Utility.ExtendedMethods;
using LMS.Utility.Helpers;
using LMS.Web.Filters;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace LMS.Web.Areas.TrainingPartner.Controllers
{
    [Area("TrainingPartner")]
    [Route("[controller]")]
    [UserAuthentication(Constants.TrainingPartnerRole)]
    public class BulkJobSeekerController : Controller
    {
        private readonly IBulkJobSeekerUploadHandler bulkJobSeekerHandler;
        private readonly IHostingEnvironment hostingEnvironment;

        public BulkJobSeekerController(IBulkJobSeekerUploadHandler _bulkJobSeekerHandler, IHostingEnvironment _hostingEnvironment)
        {
            bulkJobSeekerHandler = _bulkJobSeekerHandler;
            hostingEnvironment = _hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UploadJobSeekers(List<IFormFile> files)
        {
            IEnumerable<BulkUploadSummaryViewModel<BulkJobSeekerUploadSummaryViewModel>> summary = null;
            var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
            user = user ?? new UserViewModel();
            try
            {
                summary = bulkJobSeekerHandler.RegisterJobSeekers(user, files);
            }
            catch (XmlFileMapperException ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(BulkJobSeekerController), ex);
                ModelState.AddModelError("FileUploadProcessError", ex.Message);
                return View("Index");
            }
            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(BulkJobSeekerController), ex);
                ModelState.AddModelError("FileUploadProcessError", ex.Message);
                return View("Index");
            }
            return View("BulkJobSeekerSummary", summary);
        }

        [HttpGet]
        [Route("[action]")]
        public void DownloadTemplate()
        {
            string filePath =
                Path.Combine(hostingEnvironment.WebRootPath, "Templates/BulkJobSeekerTemplates/BulkJobSeekers.xlsx");

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                HttpContext.Response.ContentType = "application/vnd.ms-excel";

                ContentDispositionHeaderValue contentDisposition =
                    new ContentDispositionHeaderValue("attachment");

                contentDisposition.SetHttpFileName(
                    string.Format("{0}.xlsx", DateTime.Now.Ticks));

                HttpContext.Response.Headers[HeaderNames.ContentDisposition]
                    = contentDisposition.ToString();
                stream.CopyTo(HttpContext.Response.Body);
            }
        }
    }
}