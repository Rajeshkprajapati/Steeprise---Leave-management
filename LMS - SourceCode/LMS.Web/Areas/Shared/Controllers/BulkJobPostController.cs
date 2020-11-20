using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LMS.Business.Interfaces.Shared;
using LMS.Model.DataViewModel.Shared;
using LMS.Utility.Exceptions;
using LMS.Utility.ExtendedMethods;
using LMS.Utility.Helpers;
using LMS.Web.Filters;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Microsoft.Net.Http.Headers;

namespace LMS.Web.Areas.Shared.Controllers
{
    [Area("Shared")]
    [Route("[controller]")]
    [UserAuthentication(Constants.AdminRole + "," + Constants.CorporateRole + "," + Constants.StaffingPartnerRole)]
    public class BulkJobPostController : Controller
    {
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly IBulkJobPostHandler bjpHandler;
        public BulkJobPostController(IBulkJobPostHandler _bjpHandler, IHostingEnvironment _hostingEnvironment)
        {
            bjpHandler = _bjpHandler;
            hostingEnvironment = _hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ViewResult UploadJobs(List<IFormFile> files,bool inBackground)
        {
            IEnumerable <BulkUploadSummaryViewModel<BulkJobPostSummaryDetailViewModel>> summary = null;
                var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
            user = user ?? new UserViewModel();
            try
            {
                if (inBackground)
                {
                    bjpHandler.UploadJobsInBackground(user,files);
                    Logger.Logger.WriteLog(Logger.Logtype.Error, "Test", user.UserId, typeof(BulkJobPostController), "Text that this method is called");
                    ModelState.AddModelError("Message", "We have processed your bulk job uploads in background.");
                    return View("Index", summary);
                }
                else
                {
                        summary = bjpHandler.UploadJobs(user, files);
                }
            }
            //catch (DataNotUpdatedException ex)
            catch (XmlFileMapperException ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(BulkJobPostController), ex);
                ModelState.AddModelError("FileUploadProcessError", ex.Message);
                return View("Index");
            }
            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(BulkJobPostController), ex);
                ModelState.AddModelError("FileUploadProcessError", ex.Message);
                return View("Index");
            }
            catch(Exception ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(BulkJobPostController), ex);
                ModelState.AddModelError("FileUploadProcessError", ex.Message);
                return View("Index");
            }
            return View("BulkJobPostSummary", summary);
        }

        [HttpGet]
        [Route("[action]")]
        public void DownloadTemplate()
        {
            string filePath = 
                Path.Combine(hostingEnvironment.WebRootPath, "Templates/BulkJobPostTemplates/BulkJobPosting.xlsx");

            using (var stream=new FileStream(filePath, FileMode.Open,FileAccess.Read))
            {
                HttpContext.Response.ContentType = "application/vnd.ms-excel";

                ContentDispositionHeaderValue contentDisposition =
                    new ContentDispositionHeaderValue("attachment");

                contentDisposition.SetHttpFileName(
                    string.Format("{0}.xlsx",DateTime.Now.Ticks));

                HttpContext.Response.Headers[HeaderNames.ContentDisposition] 
                    = contentDisposition.ToString();
                stream.CopyTo(HttpContext.Response.Body);
            }
        }
    }
}