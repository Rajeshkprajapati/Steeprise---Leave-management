using LMS.Business.Interfaces.Admin;
using LMS.Model.DataViewModel.Admin.Dashboard;
using LMS.Model.DataViewModel.Admin.ManageUsers;
using LMS.Model.DataViewModel.Employer.JobPost;
using LMS.Model.DataViewModel.JobSeeker;
using LMS.Model.DataViewModel.Shared;
using LMS.Utility.Exceptions;
using LMS.Utility.ExtendedMethods;
using LMS.Utility.FilesUtility;
using LMS.Utility.Helpers;
using LMS.Web.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace LMS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("[controller]")]
    [HandleExceptionsAttribute]
    [UserAuthentication(Constants.AdminRole + "," + Constants.DemandAggregationRole)]
    public class DashboardController : Controller
    {
        private readonly IManageUsersHandler manageuserHandler;
        private readonly IDashboardHandler dashboardHandler;

        public DashboardController(IManageUsersHandler _manageuserHandler, IDashboardHandler _dashboardHandler)
        {
            manageuserHandler = _manageuserHandler;
            dashboardHandler = _dashboardHandler;
        }

        public ActionResult Index(string country = "IN")
        {
            try
            {
                var list = manageuserHandler.GetStates(country);
                var genders = manageuserHandler.GetGenders();
                ViewBag.Genders = genders;
                ViewBag.State = list;
            }
            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, 0, typeof(DashboardController), ex);
            }
            return View();
        }


        [HttpGet]
        [Route("[action]")]
        public ActionResult DashboardData(string date, string endDate)
        {
            DataSet list = new DataSet();
            try
            {
                list = manageuserHandler.DashboardTilesRecordCount(date, endDate);

            }
            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, 0, typeof(DashboardController), ex);
                ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
            }
            return Json(data: list);
        }

        [HttpGet]
        [Route("[action]")]
        public ActionResult GetGraphData(int year, string gender, string state)
        {
            DataSet list = new DataSet();
            try
            {
                list = manageuserHandler.GetGraphData(year, gender, state);
            }
            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, 0, typeof(DashboardController), ex);
                ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
            }
            return Json(data: list);
        }

        [HttpGet]
        [Route("[action]")]
        public ActionResult MonthlyJobs(int month, int year, string state, bool activeJobs = true)
        {
            IList<JobPostViewModel> lstJobs = new List<JobPostViewModel>();
            try
            {
                ViewData["Title"] = "Active Jobs";
                if (!activeJobs)
                {
                    ViewData["Title"] = "Closed Jobs";
                }
                lstJobs = manageuserHandler.MonthlyJobs(month, year, state, activeJobs);
            }
            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, 0, typeof(DashboardController), ex);
            }
            return View(lstJobs);
        }

        [HttpGet]
        [Route("[action]")]
        public PartialViewResult GetAllUsers()
        {
            List<ManageUsersViewModel> list = new List<ManageUsersViewModel>();

            var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
            user = user ?? new UserViewModel();
            try
            {
                list = manageuserHandler.GetAllUsers(user.UserId);
            }
            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(DashboardController), ex);

                ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
            }
            return PartialView("GetAllUsers", list);
        }

        [HttpGet]
        [Route("[action]")]
        public ActionResult GetAllUserRegistrations(string registrationType, string sDate, string eDate)
        {
            ViewData["Title"] = registrationType;
            List<ManageUsersViewModel> list = new List<ManageUsersViewModel>();
            try
            {
                list = manageuserHandler.GetAllUserRegistrations(registrationType, sDate, eDate);
            }
            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, 0, typeof(DashboardController), ex);
                ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
            }
            return View(list);
        }

        [HttpGet]
        [Route("[action]")]
        public ActionResult MonthlyRegisteredUsers(int month, int year,string state,string gender)
        {
            IList<UserViewModel> list = new List<UserViewModel>();
            try
            {
                list = manageuserHandler.MonthlyRegisteredUsers(month, year,state,gender);

            }
            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, 0, typeof(DashboardController), ex);
            }

            return View(list);
        }

        [HttpGet]
        [Route("[action]")]
        public ActionResult GetJobsInDateRange(string startDay, string endDay)
        {
            // bool result = false;
            List<JobPostViewModel> list = new List<JobPostViewModel>();
            try
            {
                list = manageuserHandler.GetJobsInDateRange(startDay, endDay);

            }
            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, 0, typeof(DashboardController), ex);
                ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
            }

            return View(list);
        }

        [HttpGet]
        [Route("[action]")]
        public ActionResult MonthlyAppliedJobs(int month, int year, string gender, string state)
        {
            List<AppliedJobsViewModel> list = new List<AppliedJobsViewModel>();
            try
            {
                list = manageuserHandler.MonthlyAppliedJobs(month, year, gender, state);

            }
            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, 0, typeof(DashboardController), ex);
            }

            return View(list);
        }


        [HttpGet]
        [Route("[action]")]
        public ActionResult GetAppliedJobsInRange(string startDate, string endDate)
        {
            List<AppliedJobsViewModel> list = new List<AppliedJobsViewModel>();
            try
            {
                list = manageuserHandler.GetAppliedJobsInRange(startDate, endDate);
            }
            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, 0, typeof(DashboardController), ex);
                ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
            }
            return View(list);
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult DeleteUsersById(string userid)
        {
            try
            {
                var result = manageuserHandler.DeleteUsersById(userid);
                return Json("Record Deleted");
            }
            catch (DataNotUpdatedException ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, 0, typeof(DashboardController), ex);
                ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
            }
            return Json("Record Can't be Deleted");

        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult Updatedata([FromBody]ManageUsersViewModel user)
        {
            try
            {
                var result = manageuserHandler.UpdateUsersData(user);
                return Json("Record Updated");
            }
            catch (DataNotUpdatedException ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, 0, typeof(DashboardController), ex);
                ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
            }
            return Json("Record Can't be Updated");
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult ApproveUser([FromBody]ManageUsersViewModel user)
        {
            var msg = string.Empty;
            var status = false;
            try
            {
                status = manageuserHandler.ApproveUser(user);
                if (status)
                {
                    msg = "User Approved";
                    return Json(new { status, msg });
                }
            }
            catch (DataNotFound ex)
            {
                msg = "Unable to approve user";
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, 0, typeof(DashboardController), ex);
                ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
            }
            return Json(new { status, msg });
        }

        [HttpGet]
        [Route("[action]")]
        public ActionResult ShoWGraphDetail()
        {
            List<ManageUsersViewModel> lstManagerUsers = new List<ManageUsersViewModel>();
            if (TempData["graphValue"] != null)
            {
                lstManagerUsers = JsonConvert.DeserializeObject<List<ManageUsersViewModel>>(TempData["graphValue"].ToString());
            }
            return View(lstManagerUsers);
        }

        [HttpGet]
        [Route("[action]")]
        public ActionResult JobPostMonthlyStateWise(string month, string year, string state)
        {
            // bool result = false;
            List<JobPostViewModel> list = new List<JobPostViewModel>();
            try
            {
                list = manageuserHandler.JobPostMonthlyStateWiseRecord(month, year, state);

            }
            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, 0, typeof(DashboardController), ex);
                ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
            }

            return View(list);
        }

        //private void SendVerificationLinkEmail(string emailID, string emailFor = "Approval")
        //{
        //    string email = EncryptDecrypt.Encrypt(emailID, "sblw-3hn8-sqoy19");
        //    var basePath = string.Format("{0}://{1}", Request.Scheme, Request.Host);

        //    var fromEmail = new MailAddress("nasscomtestmail@gmail.com", "Job Portal");
        //    var toEmail = new MailAddress(emailID);
        //    var fromEmailPassword = "steeprise@123"; // Replace with actual password

        //    string subject = "";
        //    string body = "";
        //    if (emailFor == "Approval")
        //    {
        //        subject = "Your account is successfully Approved!";
        //        body = "<br/><br/>We wanted to tell you that your account in Job Portal is approved by the Administrator </a>"+
        //            "<br><br><br>Thank You";
        //    }

        //    var smtp = new SmtpClient
        //    {
        //        Host = "smtp.gmail.com",
        //        Port = 587,
        //        EnableSsl = true,
        //        DeliveryMethod = SmtpDeliveryMethod.Network,
        //        UseDefaultCredentials = true,
        //        Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
        //    };

        //    using (var message = new MailMessage(fromEmail, toEmail)
        //    {
        //        Subject = subject,
        //        Body = body,
        //        IsBodyHtml = true
        //    })
        //        smtp.Send(message);
        //}



        //  Demand Aggregation Dashboard

        [Route("[action]")]
        public IActionResult DemandAggregation(string country = "IN")
        {
            ViewBag.Employers = dashboardHandler.GetEmployers(true);
            ViewBag.JobRoles = dashboardHandler.GetJobTitles();
            ViewBag.States = dashboardHandler.GetStates(country);
            return View();
        }

        [Route("[action]")]
        public JsonResult GetDemandAggregationDataOnQuarter(DemandAggregationSearchItems search)
        {
            bool isSuccess = true;
            IList<DemandAggregationDataOnQuarterViewModel> data = new List<DemandAggregationDataOnQuarterViewModel>();
            var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
            user = user ?? new UserViewModel();
            try
            {
                data = dashboardHandler.GetDemandAggregationDataOnQuarter(user.UserId, search);
            }

            catch (DataNotUpdatedException ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(DashboardController), ex);
                isSuccess = false;
            }
            return new JsonResult(
                new { isSuccess = isSuccess, data = data },
                ContractSerializer.JsonInPascalCase()
                );
        }


        [Route("[action]")]
        public JsonResult GetDemandAggregationDataOnJobRole(DemandAggregationSearchItems search)
        {
            bool isSuccess = true;
            IList<DemandAggregationOnJobRolesViewModel> data = new List<DemandAggregationOnJobRolesViewModel>();
            var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
            user = user ?? new UserViewModel();
            try
            {
                data = dashboardHandler.GetDemandAggregationDataOnJobRole(user.UserId, search);

            }

            catch (DataNotUpdatedException ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(DashboardController), ex);
                isSuccess = false;
            }
            return new JsonResult(
                new { isSuccess = isSuccess, data = data },
                ContractSerializer.JsonInPascalCase()
                );
        }

        [Route("[action]")]
        public JsonResult GetDemandAggregationDataOnState(DemandAggregationSearchItems search)
        {
            bool isSuccess = true;
            IList<DemandAggregationOnStatesViewModel> data = new List<DemandAggregationOnStatesViewModel>();
            var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
            user = user ?? new UserViewModel();
            try
            {
                data = dashboardHandler.GetDemandAggregationOnState(user.UserId, search);
            }

            catch (DataNotUpdatedException ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(DashboardController), ex);
                isSuccess = false;
            }
            return new JsonResult(
                new { isSuccess = isSuccess, data = data },
                ContractSerializer.JsonInPascalCase()
                );
        }

        [Route("[action]")]
        public JsonResult GetDemandAggregationDataOnEmployer(DemandAggregationSearchItems search)
        {
            bool isSuccess = true;
            IList<DemandAggregationOnEmployersViewModel> data = new List<DemandAggregationOnEmployersViewModel>();
            var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
            user = user ?? new UserViewModel();
            try
            {
                data = dashboardHandler.GetDemandAggregationDataOnEmployer(user.UserId, search);
            }

            catch (DataNotUpdatedException ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(DashboardController), ex);
                isSuccess = false;
            }
            return new JsonResult(
                new { isSuccess = isSuccess, data = data },
                ContractSerializer.JsonInPascalCase()
                );
        }

        [Route("[action]")]
        public ViewResult ViewDemandAggregationDetails(string onBasis, string value, DemandAggregationSearchItems search)
        {
            IList<DemandAggregationDetailsViewModel> data = new List<DemandAggregationDetailsViewModel>();
            var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
            user = user ?? new UserViewModel();
            try
            {
                data = dashboardHandler.ViewDemandAggregationDetails(onBasis, value, search);
            }

            catch (DataNotUpdatedException ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(DashboardController), ex);
            }
            return View(data);
        }

        [Route("[action]")]
        public void DownloadDemandAggregation(DemandAggregationSearchItems search)
        {
            FileExtensions ext = FileExtensions.xlsx;
            var workBook = dashboardHandler.GetDemandAggregationReportData(search, ext);

            string fileExtension = string.Empty;
            switch (ext)
            {
                case FileExtensions.xlsx:
                    fileExtension = $".{FileExtensions.xlsx.ToString()}";
                    break;
                case FileExtensions.xls:
                    fileExtension = $".{FileExtensions.xls.ToString()}";
                    break;
            }
            var response = HttpContext.Response;
            response.ContentType = FileTypes.MimeTypes[fileExtension];
            var contentDisposition =
                new ContentDispositionHeaderValue("attachment");
            contentDisposition.SetHttpFileName(
                string.Format("{0}{1}", DateTime.Now.Ticks, fileExtension));
            response.Headers[HeaderNames.ContentDisposition] = contentDisposition.ToString();
            workBook.Write(response.Body);
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult GetAllBulkJobs(string country = "IN")
        {
            ViewBag.EmployersData = dashboardHandler.GetEmployers(true);
            ViewBag.States = dashboardHandler.GetStates(country);
            return View();
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult SearchBulkJobList(int CompanyId, string FY, string statecode, string citycode)
        {
            var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
            IEnumerable<JobPostViewModel> bulkJobs = null;
            try
            {
                bulkJobs = manageuserHandler.GetBulkJobSearchList(CompanyId, FY, statecode, citycode);
           }
            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(DashboardController), ex);
                bulkJobs = null;
            }
            return PartialView("BulkJobResultPartial", bulkJobs);
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult DeleteBulkJobs(string JobPostId,string FY,string statecode,string CityId,int CompanyId)
        {
            var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
            IEnumerable<JobPostViewModel> bulkJobs = null;
            try
            {
               var result = manageuserHandler.DeleteBulkJobs(JobPostId);
                bulkJobs = manageuserHandler.GetBulkJobSearchList(CompanyId, FY, statecode, CityId);
                //return Json("Data deleted");
            }
            catch (DataNotUpdatedException ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, 0, typeof(ManageCityStateController), ex);
                ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
                bulkJobs = null;
            }
            return PartialView("BulkJobResultPartial", bulkJobs);
            //return Json("Unable to do this action");
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult CityListId(string StateCode)
        {
            var cityList = new List<CityViewModel>();
            try
            {
                cityList = dashboardHandler.GetCityList(StateCode);

            }
            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, 0, typeof(DashboardController), ex);
                ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
            }
            return Json(cityList);
        }

        [HttpGet]
        [Route("[action]")]
        public PartialViewResult GetAdminDashboard(string country = "IN")
        {


            try
            {
                var list = manageuserHandler.GetStates(country);
                var genders = manageuserHandler.GetGenders();
                ViewBag.Genders = genders;
                ViewBag.State = list;
            }
            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, 0, typeof(DashboardController), ex);
            }
           return PartialView("SummaryDashboardPartial");
        }
    }
}