using LMS.Business.Handlers.DataProcessorFactory;
using LMS.Business.Interfaces.Admin;
using LMS.Business.Shared;
using LMS.Data.Interfaces.Admin;
using LMS.Data.DataModel.Admin.ManageUsers;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using LMS.Model.DataViewModel.Admin.ManageUsers;
using LMS.Model.DataViewModel.Employer.JobPost;
using LMS.Model.DataViewModel.JobSeeker;
using Newtonsoft.Json;
using LMS.Model.DataViewModel.Shared;
using LMS.Business.Interfaces.Shared;
using LMS.Utility.Helpers;
using LMS.Utility.Exceptions;
using Microsoft.AspNetCore.Http;
using LMS.Data.Interfaces.Shared;

namespace LMS.Business.Handlers.Admin
{
    public class ManageUserHandler : IManageUsersHandler
    {
        private readonly IManageUserRepository _userProcessor;
        private readonly IEMailHandler emailHandler;
        private readonly IConfiguration config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMasterDataRepository masterRepo;
        private readonly string URLprotocol;

        public ManageUserHandler(IConfiguration configuration, IEMailHandler _emailHandler, IHttpContextAccessor httpContextAccessor)
        {
            var factory = new ProcessorFactoryResolver<IManageUserRepository>(configuration);
            _userProcessor = factory.CreateProcessor();
            var mFactory = new ProcessorFactoryResolver<IMasterDataRepository>(configuration);
            masterRepo = mFactory.CreateProcessor();
            emailHandler = _emailHandler;
            config = configuration;
            _httpContextAccessor = httpContextAccessor;
            URLprotocol = configuration["URLprotocol"];
        }
        public List<ManageUsersViewModel> GetAllUsers(int userId)
        {
            DataTable dt = _userProcessor.GetAllUsersList(userId);
            List<ManageUsersViewModel> users = new List<ManageUsersViewModel>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ManageUsersViewModel user = new ManageUsersViewModel
                {
                    Userid = Convert.ToInt32(dt.Rows[i]["Userid"]),
                    FirstName = Convert.ToString(dt.Rows[i]["FirstName"]),
                    LastName = Convert.ToString(dt.Rows[i]["LastName"]),
                    Email = Convert.ToString(dt.Rows[i]["Email"]),
                    Password = Convert.ToString(dt.Rows[i]["Password"]),
                    IsApproved = Convert.IsDBNull(dt.Rows[i]["IsApproved"]) ? 0 : Convert.ToInt32(dt.Rows[i]["IsApproved"]),
                    RoleName = Convert.ToString(dt.Rows[i]["RoleName"]),
                    CreatedDate = Convert.ToDateTime(dt.Rows[i]["CreatedOn"]),
                    IsViewed = Convert.ToBoolean(dt.Rows[i]["IsViewed"]),
                    RoleId = Convert.IsDBNull(dt.Rows[i]["RoleId"]) ? 0 : Convert.ToInt32(dt.Rows[i]["RoleId"]),
                    Address = Convert.IsDBNull(dt.Rows[i]["Address1"]) ? "" : Convert.ToString(dt.Rows[i]["Address1"]),
                    MobileNo = Convert.IsDBNull(dt.Rows[i]["MobileNo"]) ? "" : Convert.ToString(dt.Rows[i]["MobileNo"]),
                    City = Convert.IsDBNull(dt.Rows[i]["City"]) ? "" : Convert.ToString(dt.Rows[i]["City"]),
                    State = Convert.IsDBNull(dt.Rows[i]["State"]) ? "" : Convert.ToString(dt.Rows[i]["State"]),
                    Country = Convert.IsDBNull(dt.Rows[i]["Country"]) ? "" : Convert.ToString(dt.Rows[i]["Country"]),
                    Gender = Convert.IsDBNull(dt.Rows[i]["Gender"]) ? "" : Convert.ToString(dt.Rows[i]["Gender"]),
                    MaritalStatus = Convert.IsDBNull(dt.Rows[i]["MaritalStatus"]) ? "" : Convert.ToString(dt.Rows[i]["MaritalStatus"]),
                };
                users.Add(user);
            }
            //List<ManageUsersModel> users = ConvertDatatableToModelList.ConvertDataTable<ManageUsersModel>(dt);
            return users;
        }

        public List<AppliedJobsViewModel> GetAppliedJobsInRange(string startDate, string endDate)
        {
            DataTable dt = _userProcessor.GetAppliedJobsInRange(startDate, endDate);
            List<AppliedJobsViewModel> lstAppliedJobs = new List<AppliedJobsViewModel>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //var Skill = JsonConvert.DeserializeObject<Skills>(dt.Rows[i]["Skills"].ToString());
                AppliedJobsViewModel aJob = new AppliedJobsViewModel
                {
                    JobDetail = new JobPostViewModel(),
                    UserDetail=new UserViewModel(),
                    AppliedOn= Convert.ToDateTime(dt.Rows[i]["AppliedDate"]).Date
            };

                aJob.UserDetail.UserId = Convert.ToInt32(dt.Rows[i]["UserId"]);
                aJob.UserDetail.FirstName = Convert.ToString(dt.Rows[i]["FirstName"]);
                aJob.UserDetail.LastName = Convert.ToString(dt.Rows[i]["LastName"]);
                aJob.UserDetail.Email = Convert.ToString(dt.Rows[i]["Email"]);
                aJob.UserDetail.MobileNo = Convert.ToString(dt.Rows[i]["MobileNo"]);
                aJob.JobDetail.JobTitleByEmployer= Convert.ToString(dt.Rows[i]["JobTitleByEmployer"]);
                aJob.JobDetail.CTC = Convert.ToString(dt.Rows[i]["CTC"]);
                aJob.JobDetail.JobTypeSummary = Convert.ToString(dt.Rows[i]["JobTypeDesc"]);
                aJob.JobDetail.CompanyName= Convert.ToString(dt.Rows[i]["CompanyName"]);
                aJob.JobDetail.City= Convert.ToString(dt.Rows[i]["City"]);
                aJob.JobDetail.State = Convert.ToString(dt.Rows[i]["State"]);

                lstAppliedJobs.Add(aJob);
            }
            return lstAppliedJobs;
        }

        public List<ManageUsersViewModel> GetAllUserRegistrations(string registrationType, string sDate, string eDate)
        {
            DataTable dt = _userProcessor.GetAllUserRegistrations(registrationType,sDate,eDate);
            List<ManageUsersViewModel> users = new List<ManageUsersViewModel>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ManageUsersViewModel user = new ManageUsersViewModel
                {
                    Userid = Convert.ToInt32(dt.Rows[i]["Userid"]),
                    FirstName = Convert.ToString(dt.Rows[i]["FirstName"]),
                    LastName = Convert.ToString(dt.Rows[i]["LastName"]),
                    Email = Convert.ToString(dt.Rows[i]["Email"]),
                    RoleName = Convert.ToString(dt.Rows[i]["RoleName"]),
                    MobileNo = Convert.IsDBNull(dt.Rows[i]["MobileNo"]) ? "" : Convert.ToString(dt.Rows[i]["MobileNo"]),
                    City = Convert.IsDBNull(dt.Rows[i]["City"]) ? "" : Convert.ToString(dt.Rows[i]["City"]),
                    State = Convert.IsDBNull(dt.Rows[i]["State"]) ? "" : Convert.ToString(dt.Rows[i]["State"]),
                    Gender = Convert.IsDBNull(dt.Rows[i]["Gender"]) ? "" : Convert.ToString(dt.Rows[i]["Gender"]),
                    MaritalStatus = Convert.IsDBNull(dt.Rows[i]["MaritalStatus"]) ? "" : Convert.ToString(dt.Rows[i]["MaritalStatus"]),
                    CreatedDate = Convert.ToDateTime(dt.Rows[i]["CreatedOn"]),
                    TPName = Convert.ToString(dt.Rows[i]["TPName"]),
                    IsActive = Convert.ToBoolean(dt.Rows[i]["IsActive"]),
                };
                users.Add(user);
            }
            return users;
        }
        public List<JobPostViewModel> GetJobsInDateRange(string startDay, string endDay)
        {
            DataTable dt = _userProcessor.GetJobsInDateRange(startDay, endDay);
            List<JobPostViewModel> JobPostDetails = new List<JobPostViewModel>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                JobPostViewModel jobPost = new JobPostViewModel
                {
                    JobPostId= Convert.ToInt32(dt.Rows[i]["JobPostId"]),
                    Country = Convert.ToString(dt.Rows[i]["Country"]),
                    City = Convert.ToString(dt.Rows[i]["City"]),
                    State = Convert.ToString(dt.Rows[i]["State"]),
                    JobTypeSummary = Convert.ToString(dt.Rows[i]["JobTypeSummary"]),
                    JobDetails = Convert.ToString(dt.Rows[i]["JobDetails"]),
                    CTC = Convert.ToString(dt.Rows[i]["CTC"]),
                    CompanyName = Convert.ToString(dt.Rows[i]["CompanyName"]),
                    PostedOn = Convert.ToDateTime(dt.Rows[i]["PostedOn"]),
                    JobTitleByEmployer = Convert.ToString(dt.Rows[i]["JobTitleByEmployer"])
                };
                JobPostDetails.Add(jobPost);
            }
            return JobPostDetails;
        }

        public List<JobPostViewModel> JobPostMonthlyStateWiseRecord(string month, string year, string state)
        {
            DataTable dt = _userProcessor.JobPostMonthlyStateWiseRecord(month, year, state);
            List<JobPostViewModel> JobPostDetails = new List<JobPostViewModel>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                JobPostViewModel jobPost = new JobPostViewModel
                {
                    UserName = Convert.ToString(dt.Rows[i]["Email"]),
                    CityCode = Convert.ToString(dt.Rows[i]["City"]),
                    StateCode = Convert.ToString(dt.Rows[i]["State"]),
                    CountryCode = Convert.ToString(dt.Rows[i]["Country"]),
                    EmploymentStatusName = Convert.ToString(dt.Rows[i]["EmploymentStatusName"]),
                    //NoPosition = Convert.ToInt16(dt.Rows[i]["NoPosition"]),
                    Gender = Convert.ToString(dt.Rows[i]["Gender"]),
                    EmploymentTypeName = Convert.ToString(dt.Rows[i]["JobIndustryAreaName"]),
                    Mobile = Convert.ToString(dt.Rows[i]["MobileNo"]),
                    ContactPerson = Convert.ToString(dt.Rows[i]["ContactPerson"]),
                };
                JobPostDetails.Add(jobPost);
            }
            return JobPostDetails;
        }

        public IList<UserViewModel> MonthlyRegisteredUsers(int month, int year, string state, string gender)
        {
            DataTable dt = _userProcessor.MonthlyRegisteredUsers(month,year,state,gender);
            IList<UserViewModel> users = new List<UserViewModel>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                UserViewModel user = new UserViewModel
                {
                    UserId = Convert.ToInt32(dt.Rows[i]["Userid"]),
                    FirstName = Convert.ToString(dt.Rows[i]["FirstName"]),
                    LastName = Convert.ToString(dt.Rows[i]["LastName"]),
                    Email = Convert.ToString(dt.Rows[i]["Email"]),
                    RoleName = Convert.ToString(dt.Rows[i]["RoleName"]),
                    MobileNo = Convert.IsDBNull(dt.Rows[i]["MobileNo"]) ? "" : Convert.ToString(dt.Rows[i]["MobileNo"]),
                    City = Convert.IsDBNull(dt.Rows[i]["City"]) ? "" : Convert.ToString(dt.Rows[i]["City"]),
                    State = Convert.IsDBNull(dt.Rows[i]["State"]) ? "" : Convert.ToString(dt.Rows[i]["State"]),
                    Gender = Convert.IsDBNull(dt.Rows[i]["Gender"]) ? "" : Convert.ToString(dt.Rows[i]["Gender"]),
                    MaritalStatus = Convert.IsDBNull(dt.Rows[i]["MaritalStatus"]) ? "" : Convert.ToString(dt.Rows[i]["MaritalStatus"]),
                    CreatedDate = Convert.ToDateTime(dt.Rows[i]["CreatedOn"]),
                };
                users.Add(user);
            }
            return users;
        }

        public List<AppliedJobsViewModel> MonthlyAppliedJobs(int month, int year, string gender, string state)
        {
            DataTable dt = _userProcessor.MonthlyAppliedJobs(month, year, gender,state);
            List<AppliedJobsViewModel> lstAppliedJobs = new List<AppliedJobsViewModel>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                AppliedJobsViewModel aJob = new AppliedJobsViewModel
                {
                    JobDetail = new JobPostViewModel(),
                    UserDetail = new UserViewModel(),
                    AppliedOn = Convert.ToDateTime(dt.Rows[i]["AppliedDate"]).Date
                };

                aJob.UserDetail.UserId = Convert.ToInt32(dt.Rows[i]["UserId"]);
                aJob.UserDetail.FirstName = Convert.ToString(dt.Rows[i]["FirstName"]);
                aJob.UserDetail.LastName = Convert.ToString(dt.Rows[i]["LastName"]);
                aJob.UserDetail.Email = Convert.ToString(dt.Rows[i]["Email"]);
                aJob.UserDetail.MobileNo = Convert.ToString(dt.Rows[i]["MobileNo"]);
                aJob.JobDetail.JobTitleByEmployer = Convert.ToString(dt.Rows[i]["JobTitleByEmployer"]);
                aJob.JobDetail.CTC = Convert.ToString(dt.Rows[i]["CTC"]);
                aJob.JobDetail.JobTypeSummary = Convert.ToString(dt.Rows[i]["JobTypeDesc"]);
                aJob.JobDetail.CompanyName = Convert.ToString(dt.Rows[i]["CompanyName"]);
                aJob.JobDetail.City = Convert.ToString(dt.Rows[i]["City"]);
                aJob.JobDetail.State = Convert.ToString(dt.Rows[i]["State"]);

                lstAppliedJobs.Add(aJob);
            }
            return lstAppliedJobs;
        }

        public DataSet DashboardTilesRecordCount(string date, string endDate)
        {
            DataSet dataSet = _userProcessor.DashboardTilesRecordCount(date, endDate);
            return dataSet;
        }

        public IList<JobPostViewModel> MonthlyJobs(int month, int year, string state, bool activeJobs)
        {
            var jobs = _userProcessor.MonthlyJobs(month, year, state,activeJobs);
            IList<JobPostViewModel> jModel = new List<JobPostViewModel>();
            if (null != jobs && jobs.Rows.Count > 0)
            {
                foreach (DataRow row in jobs.Rows)
                {
                    jModel.Add(new JobPostViewModel
                    {
                        JobPostId = Convert.ToInt32(row["JobPostId"]),
                        Country = Convert.ToString(row["Country"]),
                        State = Convert.ToString(row["State"]),
                        City = Convert.ToString(row["City"]),
                        JobTypeSummary = Convert.ToString(row["JobTypeSummary"]),
                        JobDetails = Convert.ToString(row["JobDetails"]),
                        CTC = Convert.ToString(row["CTC"]),
                        PostedOn = Convert.ToDateTime(row["PostedOn"]),
                        JobTitleByEmployer = Convert.ToString(row["JobTitleByEmployer"]),
                        JobTitle = Convert.ToString(row["JobTitleByEmployer"]),
                        CompanyName = Convert.ToString(row["CompanyName"]),
                    });
                }
            }
            return jModel;
        }

        public DataSet GetGraphData(int year, string gender, string state)
        {
            DataSet dataSet = _userProcessor.GetGraphData(year, gender,state);
            return dataSet;
        }

        public bool DeleteUsersById(string userid)
        {
            return _userProcessor.DeleteUsersById(userid);            
        }

        public List<StateViewModel> GetStates(string country)
        {
            DataTable state = masterRepo.GetStates(country);
            List<StateViewModel> lstState = new List<StateViewModel>();
            if (state.Rows.Count > 0)
            {
                lstState = ConvertDatatableToModelList.ConvertDataTable<StateViewModel>(state);
            }
            return lstState;            
        }

        public List<GenderViewModel> GetGenders()
        {
            DataTable genderdata = masterRepo.GetAllGender(true);
            List<GenderViewModel> lstGender = new List<GenderViewModel>();
            if (genderdata.Rows.Count > 0)
            {
                lstGender = ConvertDatatableToModelList.ConvertDataTable<GenderViewModel>(genderdata);
            }
            return lstGender;
        }

        public bool UpdateUsersData(ManageUsersViewModel manageuserViewModel)
        {
            ManageUsersModel user = new ManageUsersModel
            {
                Userid = manageuserViewModel.Userid,
                FirstName = manageuserViewModel.FirstName,
                LastName = manageuserViewModel.LastName,
                RoleId = manageuserViewModel.RoleId,
                Email = manageuserViewModel.Email,
                Password = manageuserViewModel.Password
            };
            return _userProcessor.UpdateUsersData(user);
            
        }

        public bool ApproveUser(ManageUsersViewModel user)
        {
            var result = _userProcessor.ApproveUser(user.Userid);
            if (result)
            {
                string loginUrl =
                $"{URLprotocol}://" +
                $"{_httpContextAccessor.HttpContext.Request.Host.Value}" +
                $"/Auth/Index";

                var eModel = new EmailViewModel
                {
                    Subject = "Account Approval",
                    Body = "Dear " + user.FirstName + "," + "<br/>You have successfully registered with us.You are one step away to explore our application.<br/><br/>Please <a href=" + loginUrl + ">click here</a> to proceed." +
                    "<br/><br/><br/>Thank You<br/>Placement Portal Team",
                    To = new string[] { user.Email },
                    From = config["EmailCredential:Fromemail"],
                    IsHtml = true,
                    MailType = (int)MailType.UserApproval
                };
                emailHandler.SendMail(eModel, user.Userid);
                return true;
            }
            return false;
        }

        public List<JobPostViewModel> GetBulkJobSearchList(int CompanyId, string FY, string statecode, string citycode)
        {
            DataTable dt = _userProcessor.GetBulkJobSearchList(CompanyId, FY, statecode, citycode);
            List<JobPostViewModel> JobPostDetails = new List<JobPostViewModel>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                JobPostViewModel jobPost = new JobPostViewModel
                {
                    JobPostId = Convert.ToInt32(dt.Rows[i]["JobPostId"]),
                    Country = Convert.ToString(dt.Rows[i]["Country"]),
                    City = Convert.ToString(dt.Rows[i]["City"]),
                    State = Convert.ToString(dt.Rows[i]["State"]),
                    JobTypeSummary = Convert.ToString(dt.Rows[i]["JobTypeSummary"]),
                    JobDetails = Convert.ToString(dt.Rows[i]["JobDetails"]),
                    CTC = Convert.ToString(dt.Rows[i]["CTC"]),
                    CompanyName = Convert.ToString(dt.Rows[i]["CompanyName"]),
                    PostedOn = Convert.ToDateTime(dt.Rows[i]["PostedOn"]),
                    JobTitleByEmployer = Convert.ToString(dt.Rows[i]["JobTitleByEmployer"])
                };
                JobPostDetails.Add(jobPost);
            }
            return JobPostDetails;
        }
        public bool DeleteBulkJobs(string JobPostId)
        {
            return _userProcessor.DeleteBulkJobPost(JobPostId);
        }
    }
}
