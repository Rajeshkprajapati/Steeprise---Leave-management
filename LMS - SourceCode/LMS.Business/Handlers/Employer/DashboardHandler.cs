using LMS.Business.Handlers.DataProcessorFactory;
using LMS.Business.Interfaces.Employer;
using LMS.Business.Interfaces.Shared;
using LMS.Business.Shared;
using LMS.Data.DataModel.Employer.JobPost;
using LMS.Data.Interfaces.Employer;
using LMS.Data.Interfaces.Shared;
using LMS.Model.DataViewModel.Employer.Dashboard;
using LMS.Model.DataViewModel.Employer.JobPost;
using LMS.Model.DataViewModel.Shared;
using LMS.Utility.Exceptions;
using LMS.Utility.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace LMS.Business.Handlers.Employer
{
    public class DashboardHandler : IDashboardHandler
    {
        private readonly IDashboardRepository dashboardRepository;
        private readonly IMasterDataRepository masterDataRepository;
        private readonly IEMailHandler emailHandler;
        private readonly IHostingEnvironment environment;

        public DashboardHandler(IConfiguration configuration, IEMailHandler _emailHandler, IHostingEnvironment env)
        {
            var factory = new ProcessorFactoryResolver<IDashboardRepository>(configuration);
            dashboardRepository = factory.CreateProcessor();
            var mfactory = new ProcessorFactoryResolver<IMasterDataRepository>(configuration);
            masterDataRepository = mfactory.CreateProcessor();
            emailHandler = _emailHandler;
            environment = env;
        }

        public UserViewModel GetProfileData(int empId)
        {
            var empDetail = dashboardRepository.GetProfileData(empId);
            if (null != empDetail && empDetail.Rows.Count > 0)
            {
                return new UserViewModel
                {
                    UserId = Convert.ToInt32(empDetail.Rows[0]["UserId"]),
                    FirstName = Convert.ToString(empDetail.Rows[0]["FirstName"]),
                    LastName = Convert.ToString(empDetail.Rows[0]["Lastname"]),
                    MobileNo = Convert.ToString(empDetail.Rows[0]["MobileNo"]),
                    Email = Convert.ToString(empDetail.Rows[0]["Email"]),
                    Address1 = Convert.ToString(empDetail.Rows[0]["Address1"]),
                    Address2 = Convert.ToString(empDetail.Rows[0]["Address2"]),
                    Address3 = Convert.ToString(empDetail.Rows[0]["Address3"]),
                    CityName = Convert.ToString(empDetail.Rows[0]["City"]),
                    StateName = Convert.ToString(empDetail.Rows[0]["State"]),
                    CountryName = Convert.ToString(empDetail.Rows[0]["Country"]),
                    MaritalStatusName = Convert.ToString(empDetail.Rows[0]["MaritalStatus"]),
                    ProfilePic = Convert.ToString(empDetail.Rows[0]["ProfilePic"]),
                    ActiveFrom = Convert.ToDateTime(empDetail.Rows[0]["ActiveFrom"]),
                    GenderName = Convert.ToString(empDetail.Rows[0]["Gender"]),
                    CompanyName = Convert.ToString(empDetail.Rows[0]["CompanyName"]),
                    HiringFor = Convert.ToString(empDetail.Rows[0]["HiringFor"])
                };
            }
            throw new DataNotFound("Employer details not found");
        }

        public IEnumerable<JobPostViewModel> GetJobs(int empId, int year)
        {
            var jobs = dashboardRepository.GetJobs(empId, year);
            if (null != jobs && jobs.Rows.Count > 0)
            {
                IList<JobPostViewModel> jModel = new List<JobPostViewModel>();
                foreach (DataRow row in jobs.Rows)
                {
                    jModel.Add(new JobPostViewModel
                    {
                        JobPostId = Convert.ToInt32(row["JobPostId"]),
                        Country = Convert.ToString(row["Country"]),
                        State = Convert.ToString(row["State"]),
                        City = Convert.ToString(row["City"]),
                        JobTitleByEmployer = Convert.ToString(row["JobTitleByEmployer"]),
                        HiringCriteria = Convert.ToString(row["HiringCriteria"]),
                        JobType = Convert.ToInt32(row["JobType"]),
                        JobTypeSummary = Convert.ToString(row["JobTypeSummary"]),
                        JobDetails = Convert.ToString(row["JobDetails"]),
                        CTC = Convert.ToString(row["CTC"]),
                        TotalApplications = Convert.ToInt32(row["TotalApplications"]),
                        PostedOn = Convert.ToDateTime(row["PostedOn"]),
                        Quarter1 = Convert.ToString(row["Quarter1"]),
                        Quarter2 = Convert.ToString(row["Quarter2"]),
                        Quarter3 = Convert.ToString(row["Quarter3"]),
                        Quarter4 = Convert.ToString(row["Quarter4"]),
                        Featured = Convert.ToString(row["Featured"]),
                        DisplayOrder = row["FeaturedJobDisplayOrder"] as int? ?? 0
                    });
                }
                return jModel;
            }
            throw new DataNotFound("Jobs not found");
        }

        public DashboardSummary GetDashboard(int empId)
        {
            var dashboardSummary = dashboardRepository.GetDashboard(empId);
            if (null != dashboardSummary && dashboardSummary.Tables.Count > 0)
            {
                DashboardSummary dashboard = new DashboardSummary();
                int i = -1;
                foreach (DataTable table in dashboardSummary.Tables)
                {
                    i++;
                    switch (i)
                    {
                        case 0:
                            dashboard.TotalProfileViewes = Convert.ToInt32(table.Rows[0]["TotalViewed"]);
                            break;
                        case 1:
                            dashboard.TotalResumeList = Convert.ToInt32(table.Rows[0]["TotalApplications"]);
                            break;
                        case 2:
                            dashboard.TotalMessages = Convert.ToInt32(table.Rows[0]["TotalMessages"]);
                            break;
                        case 3:
                            dashboard.RespondTime = Convert.ToInt32(table.Rows[0]["RespondTime"]);
                            break;
                        default:
                            break;
                    }
                }
                return dashboard;
            }
            throw new DataNotFound("Dashboard Summary not found");
        }

        public IEnumerable<Model.DataViewModel.Employer.Dashboard.JobSeekerViewModel> GetJobSeekers(int empId)
        {
            var jobSeekers = dashboardRepository.GetJobSeekers(empId);
            if (null != jobSeekers && jobSeekers.Rows.Count > 0)
            {
                IList<Model.DataViewModel.Employer.Dashboard.JobSeekerViewModel> jSeekers = new List<Model.DataViewModel.Employer.Dashboard.JobSeekerViewModel>();
                foreach (DataRow row in jobSeekers.Rows)
                {
                    string jtitle = Convert.ToString(row["JobTitleByEmployer"]);
                    Model.DataViewModel.JobSeeker.Skills skills =
                        JsonConvert.DeserializeObject<Model.DataViewModel.JobSeeker.Skills>(Convert.ToString(row["Skills"]));
                    if (null == skills)
                    {
                        skills = new Model.DataViewModel.JobSeeker.Skills();
                    }
                    string resumePath = Convert.ToString(row["Resume"]);
                    if (!string.IsNullOrWhiteSpace(resumePath))
                    {
                        if (!File.Exists($"{environment.WebRootPath}{resumePath}"))
                        {
                            resumePath = string.Empty;
                        }
                    }

                    var employeers = jSeekers.Where(j => j.JobTitleByEmployer == jtitle).FirstOrDefault();
                    if (null == employeers)
                    {
                        employeers = new Model.DataViewModel.Employer.Dashboard.JobSeekerViewModel
                        {
                            JobRoles = Convert.ToString(row["JobRole"]),
                            JobTitleByEmployer = jtitle,
                            jobSeekers = new List<UserViewModel>()
                        };

                        employeers.jobSeekers.Add(
                            new UserViewModel
                            {
                                UserId = Convert.ToInt32(row["UserId"]),
                                CandidateId = Convert.ToString(row["Candidateid"]),
                                FirstName = Convert.ToString(row["FirstName"]),
                                LastName = Convert.ToString(row["LastName"]),
                                MobileNo = Convert.ToString(row["MobileNo"]),
                                Email = Convert.ToString(row["Email"]),
                                GenderName = Convert.ToString(row["Gender"]),
                                Skills = skills,
                                CTC = Convert.ToString(row["CurrentSalary"]),
                                ECTC = Convert.ToString(row["ExpectedSalary"]),
                                Resume = resumePath
                            });
                        jSeekers.Add(employeers);
                    }
                    else
                    {
                        employeers.jobSeekers.Add(
                            new UserViewModel
                            {
                                UserId = Convert.ToInt32(row["UserId"]),
                                CandidateId = Convert.ToString(row["Candidateid"]),
                                FirstName = Convert.ToString(row["FirstName"]),
                                LastName = Convert.ToString(row["LastName"]),
                                MobileNo = Convert.ToString(row["MobileNo"]),
                                Email = Convert.ToString(row["Email"]),
                                GenderName = Convert.ToString(row["Gender"]),
                                Skills = skills,
                                CTC = Convert.ToString(row["CurrentSalary"]),
                                ECTC = Convert.ToString(row["ExpectedSalary"]),
                                Resume = resumePath
                            });
                    }
                }
                return jSeekers;
            }
            throw new DataNotFound("Jobseekers not found");
        }

        public IEnumerable<UserViewModel> GetViewedProfiles(int empId)
        {
            var jobSeekers = dashboardRepository.GetViewedProfiles(empId);
            IList<UserViewModel> jSeekers = new List<UserViewModel>();
            if (null != jobSeekers && jobSeekers.Rows.Count > 0)
            {
                foreach (DataRow row in jobSeekers.Rows)
                {
                    Model.DataViewModel.JobSeeker.Skills skills =
                        JsonConvert.DeserializeObject<Model.DataViewModel.JobSeeker.Skills>(Convert.ToString(row["Skills"]));
                    if (null == skills)
                    {
                        skills = new Model.DataViewModel.JobSeeker.Skills();
                    }
                    string resumePath = Convert.ToString(row["Resume"]);
                    if (!string.IsNullOrWhiteSpace(resumePath))
                    {
                        if (!File.Exists($"{environment.WebRootPath}{resumePath}"))
                        {
                            resumePath = string.Empty;
                        }
                    }
                    jSeekers.Add(
                        new UserViewModel
                        {
                            UserId = Convert.ToInt32(row["UserId"]),
                            CandidateId = Convert.ToString(row["Candidateid"]),
                            FirstName = Convert.ToString(row["FirstName"]),
                            LastName = Convert.ToString(row["LastName"]),
                            MobileNo = Convert.ToString(row["MobileNo"]),
                            Email = Convert.ToString(row["Email"]),
                            GenderName = Convert.ToString(row["Gender"]),
                            Skills = skills,
                            CTC = Convert.ToString(row["CurrentSalary"]),
                            ECTC = Convert.ToString(row["ExpectedSalary"]),
                            Resume = resumePath,
                            RoleName = Convert.ToString(row["JobTitleName"]),
                            CityName = Convert.ToString(row["Name"])
                        });
                }
                return jSeekers;
            }
            return jSeekers;
        }

        public IEnumerable<UserViewModel> GetJobSeekersBasedOnEmployerHiringCriteria(int empId, string year, string city, string role)
        {
            var jobSeekers = dashboardRepository.GetJobSeekersBasedOnEmployerHiringCriteria(empId,year,city,role);
            IList<UserViewModel> jSeekers = new List<UserViewModel>();
            if (null != jobSeekers && jobSeekers.Rows.Count > 0)
            {
                foreach (DataRow row in jobSeekers.Rows)
                {
                    Model.DataViewModel.JobSeeker.Skills skills =
                        JsonConvert.DeserializeObject<Model.DataViewModel.JobSeeker.Skills>(Convert.ToString(row["Skills"]));
                    if (null == skills)
                    {
                        skills = new Model.DataViewModel.JobSeeker.Skills();
                    }
                    string resumePath = Convert.ToString(row["Resume"]);
                    if (!string.IsNullOrWhiteSpace(resumePath))
                    {
                        if (!File.Exists($"{environment.WebRootPath}{resumePath}"))
                        {
                            resumePath = string.Empty;
                        }
                    }
                    jSeekers.Add(
                        new UserViewModel
                        {
                            UserId = Convert.ToInt32(row["UserId"]),
                            CandidateId = Convert.ToString(row["Candidateid"]),
                            FirstName = Convert.ToString(row["FirstName"]),
                            LastName = Convert.ToString(row["LastName"]),
                            MobileNo = Convert.ToString(row["MobileNo"]),
                            Email = Convert.ToString(row["Email"]),
                            GenderName = Convert.ToString(row["Gender"]),
                            Skills = skills,
                            CTC = Convert.ToString(row["CurrentSalary"]),
                            ECTC = Convert.ToString(row["ExpectedSalary"]),
                            Resume = resumePath,
                            ProfileSummary = Convert.ToString(row["ProfileSummary"]),
                            AboutMe = Convert.ToString(row["AboutMe"]),
                            RoleName= Convert.ToString(row["JobTitleName"]),
                            CityName = Convert.ToString(row["Name"])

                        });
                }
                return jSeekers;
            }
            throw new DataNotFound("Jobseekers information not found");
        }

        public JobPostViewModel GetJob(int jobId, int empId)
        {
            var jobs = dashboardRepository.GetJob(jobId);
            if (null != jobs && jobs.Rows.Count > 0)
            {
                JobPostViewModel job = new JobPostViewModel();
                foreach (DataRow row in jobs.Rows)
                {
                    job.JobPostId = Convert.ToInt32(row["JobPostId"]);
                    job.CompanyName = Convert.ToString(row["CompanyName"]);
                    job.JobTitleId = Convert.ToString(row["JobRoleId"]);
                    job.JobTitle = Convert.ToString(row["JobRole"]);
                    job.CountryCode = Convert.ToString(row["CountryCode"]);
                    job.Country = Convert.ToString(row["Country"]);
                    job.StateCode = Convert.ToString(row["StateCode"]);
                    job.State = Convert.ToString(row["State"]);
                    job.City = Convert.ToString(row["City"]);
                    job.CityCode = Convert.ToString(row["CityCode"]);
                    job.ContactPerson = Convert.ToString(row["SPOC"]);
                    job.SPOCEmail = Convert.ToString(row["SPOCEmail"]);
                    job.Mobile = Convert.ToString(row["SPOCContact"]);
                    job.MonthlySalary = Convert.ToString(row["MonthlySalary"]);
                    job.CTC = Convert.ToString(row["CTC"]);
                    job.HiringCriteria = Convert.ToString(row["HiringCriteria"]);
                    job.JobType = Convert.ToInt32(row["JobType"]);
                    job.JobTypeSummary = Convert.ToString(row["JobTypeSummary"]);
                    job.JobDetails = Convert.ToString(row["JobDetails"]);
                    //job.Quarter1 = Convert.ToString(row["Quarter1"]);
                    //job.Quarter2 = Convert.ToString(row["Quarter2"]);
                    //job.Quarter3 = Convert.ToString(row["Quarter3"]);
                    //job.Quarter4 = Convert.ToString(row["Quarter4"]);
                    job.NoPosition = Convert.ToInt32(row["NoPosition"]);
                    job.PostedOn = Convert.ToDateTime(row["CreatedDate"]);
                    job.Featured = Convert.ToString(row["Featured"]);
                    job.DisplayOrder = row["FeaturedJobDisplayOrder"] as int? ?? 0;
                    job.JobTitleByEmployer = Convert.ToString(row["JobTitleByEmployer"]);
                    job.FinancialYear = Convert.ToInt32(row["FinancialYear"]);
                    if (!Convert.IsDBNull(row["PostingDate"]))
                    {
                        job.PositionStartDate = Convert.ToDateTime(row["PostingDate"]).Date.ToString("yyyy-MM-dd");
                    }
                    if (!Convert.IsDBNull(row["ExpiryDate"]))
                    {
                        job.PositionEndDate = Convert.ToDateTime(row["ExpiryDate"]).Date.ToString("yyyy-MM-dd");
                    }
                }

                //  Validating Quarter basis resource requirements.....
                var currDate = DateTime.Now.Date;
                int quarterStartMonth = Convert.ToInt32(ConfigurationHelper.Config.GetSection(Constants.JobPostingQuarterStartingMonthKey).Value);
                DateTime[] quarterEnds = new DateTime[4];
                //quarterEnds[0] = new DateTime(job.PostedOn.Year, quarterStartMonth, 1).AddMonths(3).Date;
                quarterEnds[0] = new DateTime(job.FinancialYear, quarterStartMonth, 1).AddMonths(3).Date;
                quarterEnds[1] = quarterEnds[0].AddMonths(3);
                quarterEnds[2] = quarterEnds[1].AddMonths(3);
                quarterEnds[3] = quarterEnds[2].AddMonths(3);

                if (quarterEnds[0] < currDate)
                {
                    job.IsQuarter1ReadOnly = true;
                }
                if (quarterEnds[1] < currDate)
                {
                    job.IsQuarter2ReadOnly = true;
                }
                if (quarterEnds[2] < currDate)
                {
                    job.IsQuarter3ReadOnly = true;
                }
                if (quarterEnds[3] < currDate)
                {
                    job.IsQuarter4ReadOnly = true;
                }
                //.....................................................

                return job;
            }
            throw new DataNotFound("Job not found");
        }

        public IEnumerable<MessageViewModel> GetMessages(DateTime msgsOnDate, int empId)
        {
            var messages = dashboardRepository.GetMessages(msgsOnDate, empId);
            if (null != messages && messages.Rows.Count > 0)
            {
                IList<MessageViewModel> msgs = new List<MessageViewModel>();
                foreach (DataRow row in messages.Rows)
                {
                    msgs.Add(
                            new MessageViewModel
                            {
                                MessageId = Convert.ToInt32(row["MessageId"]),
                                IsReplied = Convert.ToBoolean(row["IsReplied"]),
                                SenderId = Convert.ToInt32(row["SenderId"]),
                                ReceiverId = Convert.ToInt32(row["ReceiverId"]),
                                SenderFName = Convert.ToString(row["SenderFName"]),
                                SenderLName = Convert.ToString(row["SenderLName"]),
                                SenderMobile = Convert.ToString(row["SenderMobile"]),
                                SenderEmail = Convert.ToString(row["FromEmail"]),
                                ReceiverEmail = Convert.ToString(row["ToEmail"])
                            });
                }
                return msgs;
            }
            throw new DataNotFound("Messages not found");
        }

        public IEnumerable<StateViewModel> GetStates(string cCode)
        {
            IList<StateViewModel> states = null;
            var dTable = masterDataRepository.GetStates(cCode);
            if (null != dTable && dTable.Rows.Count > 0)
            {
                states = new List<StateViewModel>();
                foreach (DataRow row in dTable.Rows)
                {
                    states.Add(new StateViewModel
                    {
                        StateCode = Convert.ToString(row["StateCode"]),
                        State = Convert.ToString(row["State"])
                    });
                }
            }
            return states;
        }

        public IEnumerable<CityViewModel> GetCities(string sCode)
        {
            IList<CityViewModel> cities = null;
            var dTable = masterDataRepository.GetCities(sCode);
            if (null != dTable && dTable.Rows.Count > 0)
            {
                cities = new List<CityViewModel>();
                foreach (DataRow row in dTable.Rows)
                {
                    cities.Add(new CityViewModel
                    {
                        CityCode = Convert.ToString(row["CityCode"]),
                        City = Convert.ToString(row["City"])
                    });
                }
            }
            return cities;
        }

        public IEnumerable<UserViewModel> GetEmployers()
        {
            DataTable dt = masterDataRepository.GetEmployers();
            var employers = new List<UserViewModel>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var emp = new UserViewModel()
                {
                    UserId = Convert.ToInt32(dt.Rows[i]["Userid"]),
                    FirstName = Convert.ToString(dt.Rows[i]["FirstName"]),
                    LastName = Convert.ToString(dt.Rows[i]["LastName"]),
                    CompanyName = Convert.ToString(dt.Rows[i]["CompanyName"]),
                    CityName = Convert.ToString(dt.Rows[i]["City"])

                };
                if (!employers.Any(e => e.CompanyName == emp.CompanyName))
                {
                    employers.Add(emp);
                }
            }
            return employers;
        }

        public IEnumerable<CountryViewModel> GetCountries()
        {
            IList<CountryViewModel> countries = null;
            var dTable = masterDataRepository.GetCountries();
            if (null != dTable && dTable.Rows.Count > 0)
            {
                countries = new List<CountryViewModel>();
                foreach (DataRow row in dTable.Rows)
                {
                    countries.Add(new CountryViewModel
                    {
                        CountryCode = Convert.ToString(row["CountryCode"]),
                        Country = Convert.ToString(row["Country"])
                    });
                }
            }
            return countries;
        }

        public IEnumerable<JobTitleViewModel> GetJobRoles()
        {
            IList<JobTitleViewModel> jobRoles = null;
            var dTable = masterDataRepository.GetJobRoles();
            if (null != dTable && dTable.Rows.Count > 0)
            {
                jobRoles = new List<JobTitleViewModel>();
                foreach (DataRow row in dTable.Rows)
                {
                    jobRoles.Add(new JobTitleViewModel
                    {
                        JobTitleId = Convert.ToInt32(row["JobTitleId"]),
                        JobTitleName = Convert.ToString(row["JobTitleName"])
                    });
                }
            }
            return jobRoles;
        }

        public bool UpdateJob(JobPostViewModel job, int userId)
        {
            var jModel = new JobPostModel
            {
                CityCode = job.CityCode,
                CountryCode = job.CountryCode,
                CreatedBy = Convert.ToString(userId),
                CTC = job.CTC,
                HiringCriteria = job.HiringCriteria,
                JobDetails = job.JobDetails,
                JobTitleId = Convert.ToString(job.JobTitleId),
                JobType = job.JobType,
                MonthlySalary = job.MonthlySalary,
                Quarter1 = job.Quarter1,
                Quarter2 = job.Quarter2,
                Quarter3 = job.Quarter3,
                Quarter4 = job.Quarter4,
                SPOC = job.ContactPerson,
                SPOCContact = job.Mobile,
                SPOCEmail = job.SPOCEmail,
                StateCode = job.StateCode,
                Featured = job.Featured,
                DisplayOrder = job.DisplayOrder,
                JobTitleByEmployer = job.JobTitleByEmployer,
                PositionStartDate = job.PositionStartDate,
                PositionEndDate = job.PositionEndDate,
                FinancialYear =job.FinancialYear
            };
            return dashboardRepository.UpdateJob(userId, job.JobPostId, jModel);
        }

        public bool ReplyToJobSeeker(MessageViewModel msg, int userId)
        {
            var tos = msg.ReceiverEmail.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string to in tos)
            {
                if (dashboardRepository.UpdateJobSeekerMailStatus(msg.MessageId, userId))
                {
                    var mModel = new EmailViewModel
                    {
                        From = msg.SenderEmail,
                        To = new string[] { to },
                        Body = msg.Body,
                        MailType = (int)MailType.JobApplicationResponse,
                        Subject = msg.Subject,
                        IsHtml = true,
                    };
                    emailHandler.SendMail(mModel, userId);
                }
            }
            return true;
        }

        public List<CityViewModel> GetCityListWithoutState()
        {
            DataTable city = masterDataRepository.GetAllCitiesWithoutState();
            if (city.Rows.Count > 0)
            {
                List<CityViewModel> lstCity = new List<CityViewModel>();
                lstCity = ConvertDatatableToModelList.ConvertDataTable<CityViewModel>(city);
                return lstCity;
            }
            return null;
            //throw new UserNotFoundException("User not found");
        }
    }
}

