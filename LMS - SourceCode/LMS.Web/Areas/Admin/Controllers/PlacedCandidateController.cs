using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using ClosedXML.Excel;
using LMS.Business.Interfaces.Admin;
using LMS.Model.DataViewModel.Admin.PlacedCandidate;
using LMS.Model.DataViewModel.Shared;
using LMS.Utility.Exceptions;
using LMS.Utility.ExtendedMethods;
using LMS.Utility.Helpers;
using LMS.Web.Filters;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace LMS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("[controller]")]
    [UserAuthentication(Constants.AdminRole)]
    public class PlacedCandidateController : Controller
    {
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly IPlacedCandidateHandler placedCandidateHandler;

        public PlacedCandidateController(IHostingEnvironment _hostingEnvironment, IPlacedCandidateHandler _placedCandidateHandler)
        {
            hostingEnvironment = _hostingEnvironment;
            placedCandidateHandler = _placedCandidateHandler;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult UploadPlacedCandidate(List<IFormFile> files)
        {
            var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo) ?? new UserViewModel();
            var msg = string.Empty;
            try
            {
                placedCandidateHandler.UploadFile(user, files);
                ViewData["SuccessMessage"] = "Data Uploaded Successfully";
                msg = "Data Uploaded Successfully";
            }
            catch (XmlFileMapperException ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(PlacedCandidateController), ex);
                ModelState.AddModelError("FileUploadProcessError", ex.Message);
                return View("Index");
            }
            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(PlacedCandidateController), ex);
                ModelState.AddModelError("FileUploadProcessError", ex.Message);
                return View("Index");
            }
            //ModelState.AddModelError("FileUploadProcessError",msg);
            return View("Index");
        }


        [HttpGet]
        [Route("[action]")]
        public void DownloadTemplate()
        {
            string filePath =
                Path.Combine(hostingEnvironment.WebRootPath, "Templates/PlacedCandidate/PlacedCandidates.xlsx");

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

        [HttpGet]
        [Route("[action]")]
        public IActionResult AllPlacedCandidate()
        {
            var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo) ?? new UserViewModel();
            IList<PlacedCandidateViewModel> candidate = null;
            try
            {
                candidate = placedCandidateHandler.GetAllCandidate();
            }
            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(PlacedCandidateController), ex);
            }
            return View();
        }

        [HttpGet]
        [Route("[action]")]
        public JsonResult GetFilteredItems()
        {
            IList<PlacedCandidateViewModel> candidate = new List<PlacedCandidateViewModel>();
            try
            {
                candidate = placedCandidateHandler.GetAllCandidate();
            }
            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, 0, typeof(PlacedCandidateController), ex);
            }

            int draw = Convert.ToInt32(Request.Query["draw"]);

            int start = Convert.ToInt32(Request.Query["start"]);

            int length = Convert.ToInt32(Request.Query["length"]);

            int sortColumnIdx = Convert.ToInt32(Request.Query["order[0][column]"]);
            string sortColumnName = Request.Query["columns[" + sortColumnIdx + "][name]"];

            string sortColumnDirection = Request.Query["order[0][dir]"];

            string searchValue = Request.Query["search[value]"].FirstOrDefault()?.Trim();

            int recordsFilteredCount =
                    candidate
                    .Where(a => a.CandidateID.Contains(searchValue) || a.CandidateName.Contains(searchValue))
                    .Count();

            int recordsTotalCount = candidate.Count();

            IList<PlacedCandidateViewModel> filteredData = null;
            if (sortColumnDirection == "asc")
            {
                filteredData =
                    candidate
                    .Where(a => a.CandidateID.Contains(searchValue) || a.CandidateName.Contains(searchValue)
                    || a.CandidateEmail.Contains(searchValue) || a.EmployerspocEmail.Contains(searchValue)
                    || a.Castecategory.Contains(searchValue) || a.EducationAttained.Contains(searchValue))
                    .OrderBy(x => x.GetType().GetProperty(sortColumnName)?.GetValue(x))//Sort by sortColumn
                    .Skip(start)
                    .Take(length)
                    .ToList<PlacedCandidateViewModel>();
            }
            else
            {
                filteredData =
                   candidate
                   .Where(a => a.CandidateID.Contains(searchValue) || a.CandidateName.Contains(searchValue)
                    || a.CandidateEmail.Contains(searchValue) || a.EmployerspocEmail.Contains(searchValue)
                    || a.Castecategory.Contains(searchValue) || a.EducationAttained.Contains(searchValue))
                   .OrderByDescending(x => x.GetType().GetProperty(sortColumnName)?.GetValue(x))
                   .Skip(start)
                   .Take(length)
                   .ToList<PlacedCandidateViewModel>();
            }

            return Json(
                        new
                        {
                            data = filteredData,
                            draw = Request.Query["draw"],
                            recordsFiltered = recordsFilteredCount,
                            recordsTotal = recordsTotalCount
                        }
                    );
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult ExportToExcel()
        {
            var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo) ?? new UserViewModel();
            try
            {
                DataTable dt = placedCandidateHandler.GetDataInExcel();
                string fileName = "PlacedCandidate.xlsx";
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    }
                }
            }
            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(PlacedCandidateController), ex);
            }
            return View();
        }


    }
}