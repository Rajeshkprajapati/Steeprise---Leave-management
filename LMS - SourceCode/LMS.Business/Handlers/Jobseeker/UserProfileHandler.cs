using LMS.Business.Handlers.DataProcessorFactory;
using LMS.Business.Interfaces.Jobseeker;
using LMS.Business.Interfaces.Shared;
using LMS.Business.Shared;
using LMS.Data.DataModel.Shared;
using LMS.Data.Interfaces.Jobseeker;
using LMS.Data.Interfaces.Shared;
using LMS.Model.DataViewModel.JobSeeker;
using LMS.Model.DataViewModel.Shared;
using LMS.Utility.Exceptions;
using LMS.Utility.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
//using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.IO;
using System.Linq;

namespace LMS.Business.Handlers.Jobseeker
{
    public class UserProfileHandler : IUserProfileHandler
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private IHostingEnvironment _hostingEnviroment;
        private readonly IEMailHandler emailHandler;
        private readonly IConfiguration _configuration;
        private readonly IMasterDataRepository _masterRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string URLprotocol;

        public UserProfileHandler(IConfiguration configuration, IHostingEnvironment hostingEnvironment, IEMailHandler _emailHandler, IHttpContextAccessor httpContextAccessor)
        {
            var factory = new ProcessorFactoryResolver<IUserProfileRepository>(configuration);
            _userProfileRepository = factory.CreateProcessor();
            _hostingEnviroment = hostingEnvironment;
            emailHandler = _emailHandler;
            _configuration = configuration;
            var masterfactory = new ProcessorFactoryResolver<IMasterDataRepository>(configuration);
            _masterRepository = masterfactory.CreateProcessor();
            _httpContextAccessor = httpContextAccessor;
            URLprotocol = configuration["URLprotocol"];
        }

        public bool AddExperienceDetails(int userId, ExperienceDetails[] model)
        {
            bool isRegister = false;
            DataTable experienceDetails = _userProfileRepository.GetJobseekerData(userId);
            List<Data.DataModel.JobSeeker.ExperienceDetails> objExperience = null;
            if (experienceDetails.Rows.Count > 0)
            {
                objExperience = JsonConvert.DeserializeObject<List<Data.DataModel.JobSeeker.ExperienceDetails>>(experienceDetails.Rows[0]["ExperienceDetails"].ToString());
                if (null != objExperience)
                {
                    foreach (var m in model)
                    {
                        var ex = objExperience.Where(x => x.Id == m.Id).FirstOrDefault();
                        if (null != ex)
                        {
                            ex.AnnualSalary = m.AnnualSalary;
                            ex.Designation = m.Designation;
                            ex.IsCurrentOrganization = m.IsCurrentOrganization;
                            ex.JobProfile = m.JobProfile;
                            ex.NoticePeriod = m.NoticePeriod;
                            ex.ServingNoticePeriod = m.ServingNoticePeriod;
                            ex.WorkingFrom = m.WorkingFrom;
                            ex.WorkingTill = m.WorkingTill;
                            ex.Organization = m.Organization;
                        }
                        else
                        {
                            objExperience.Add(new Data.DataModel.JobSeeker.ExperienceDetails()
                            {
                                Id = objExperience.Count + 1,
                                Designation = m.Designation,
                                IsCurrentOrganization = m.IsCurrentOrganization,
                                JobProfile = m.JobProfile,
                                NoticePeriod = m.NoticePeriod,
                                ServingNoticePeriod = m.ServingNoticePeriod,
                                WorkingFrom = m.WorkingFrom,
                                WorkingTill = m.WorkingTill,
                                Organization = m.Organization,
                            });
                        }
                    }
                }
                else
                {
                    objExperience = new List<Data.DataModel.JobSeeker.ExperienceDetails>();
                    foreach (var m in model)
                    {
                        objExperience.Add(new Data.DataModel.JobSeeker.ExperienceDetails()
                        {
                            Id = objExperience.Count + 1,
                            Designation = m.Designation,
                            IsCurrentOrganization = m.IsCurrentOrganization,
                            JobProfile = m.JobProfile,
                            NoticePeriod = m.NoticePeriod,
                            ServingNoticePeriod = m.ServingNoticePeriod,
                            WorkingFrom = m.WorkingFrom,
                            WorkingTill = m.WorkingTill,
                            Organization = m.Organization,
                        });
                    }
                }
                isRegister = _userProfileRepository.AddNewExperience(userId, JsonConvert.SerializeObject(objExperience));
                if (isRegister)
                {
                    return true;
                }

            }
            throw new DataNotUpdatedException("Unable to create experience, please contact your teck deck.");
        }

        public bool AddEducationalDetailsDetails(int userId, EducationalDetails[] model)
        {
            bool isRegister = false;
            DataTable educationDetails = _userProfileRepository.GetJobseekerData(userId);
            List<Data.DataModel.JobSeeker.EducationalDetails> objEducational = null;
            if (educationDetails.Rows.Count > 0)
            {
                objEducational =
                    JsonConvert.DeserializeObject<List<Data.DataModel.JobSeeker.EducationalDetails>>(educationDetails.Rows[0]["EducationalDetails"]
                    .ToString());
                if (null != objEducational)
                {
                    foreach (var m in model)
                    {
                        var ex = objEducational.Where(x => x.Id == m.Id).FirstOrDefault();
                        if (null != ex)
                        {
                            ex.Course = m.Course;
                            ex.CourseType = m.CourseType;
                            ex.PassingYear = m.PassingYear;
                            ex.Percentage = m.Percentage;
                            ex.Qualification = m.Qualification;
                            ex.University = m.University;
                            ex.Specialization = m.Specialization;
                            ex.OtherCourseName = m.OtherCourseName;
                        }
                        else
                        {
                            objEducational.Add(new Data.DataModel.JobSeeker.EducationalDetails
                            {
                                Id = objEducational.Count + 1,
                                Course = m.Course,
                                CourseType = m.CourseType,
                                PassingYear = m.PassingYear,
                                Percentage = m.Percentage,
                                Qualification = m.Qualification,
                                University = m.University,
                                Specialization = m.Specialization,
                                OtherCourseName = m.OtherCourseName
                            });
                        }
                    }
                }
                else
                {
                    objEducational = new List<Data.DataModel.JobSeeker.EducationalDetails>();
                    foreach (var m in model)
                    {
                        objEducational.Add(new Data.DataModel.JobSeeker.EducationalDetails
                        {
                            Id = objEducational.Count + 1,
                            Course = m.Course,
                            CourseType = m.CourseType,
                            PassingYear = m.PassingYear,
                            Percentage = m.Percentage,
                            Qualification = m.Qualification,
                            University = m.University,
                            Specialization = m.Specialization,
                            OtherCourseName = m.OtherCourseName
                        });
                    }
                }
                isRegister = _userProfileRepository.AddNeweducational(userId, JsonConvert.SerializeObject(objEducational));
                if (isRegister)
                {
                    return true;
                }

            }
            throw new DataNotUpdatedException("Unable to create education , please contact your teck deck.");
        }

        public bool AddSkillDetails(int userId, Skills model)
        {
            bool isRegister = _userProfileRepository.AddNewSkills(userId, JsonConvert.SerializeObject(model));
            if (isRegister)
            {
                return true;
            }
            throw new DataNotUpdatedException("Unable to create user, please contact your teck deck.");
        }

        public bool AddProfileSummaryDetails(string profile, int userId)
        {
            bool isRegister = _userProfileRepository.AddNewProfileSummary(profile, userId);
            if (isRegister)
            {
                return true;
            }
            throw new UserNotCreatedException("Unable to create skills, please contact your teck deck.");
        }

        public UserDetail GetJobseekerDetail(int UserId)
        {
            DataTable jobSeekerDetail = _userProfileRepository.GetJobseekerData(UserId);
            DataTable allcities = _userProfileRepository.GetAllCities();
            DataTable preferredlocation = _userProfileRepository.GetUserPreferredlocation(UserId);
            DataTable ITSkills = _userProfileRepository.GetUserITSkills(UserId);
            DataTable ProfileScore = _userProfileRepository.ProfileScore(UserId);
            UserDetail model = new UserDetail();
            if (jobSeekerDetail.Rows.Count > 0)
            {
                model.PersonalDetails = new UserViewModel();
                ExperienceDetails[] objExperience = JsonConvert.DeserializeObject<ExperienceDetails[]>(jobSeekerDetail.Rows[0]["ExperienceDetails"].ToString());
                EducationalDetails[] objEducational = JsonConvert.DeserializeObject<EducationalDetails[]>(jobSeekerDetail.Rows[0]["EducationalDetails"].ToString());

                model.Cities = ConvertDatatableToModelList.ConvertDataTable<CityViewModel>(allcities);
                model.ITSkills = ConvertDatatableToModelList.ConvertDataTable<ITSkills>(ITSkills);
                
              //Adding preferred location
                if (preferredlocation != null && preferredlocation.Rows.Count > 0)
                {
                    //checking if locationid is null then assign otherlocation
                    model.PersonalDetails.PreferredLocation1 = (preferredlocation.Rows[0]["LocationId"] as string) ?? preferredlocation.Rows[0]["OtherLocation"] as string;
                    model.PersonalDetails.PreferredLocation2 = (preferredlocation.Rows[1]["LocationId"] as string) ?? (preferredlocation.Rows[1]["OtherLocation"]) as string;
                    model.PersonalDetails.PreferredLocation3 = (preferredlocation.Rows[2]["LocationId"] as string) ?? (preferredlocation.Rows[2]["OtherLocation"]) as string;
                }

                //model.PersonalDetails.Preferredlocation = ConvertDatatableToModelList.ConvertDataTable<string>(preferredlocation);

                model.ExperienceDetails = objExperience;
                model.EducationalDetails = objEducational;
                if (model.EducationalDetails != null)
                {

                    foreach (EducationalDetails edu in model.EducationalDetails)
                    {
                        if (edu.Course != null)
                        {
                            DataTable coursename = _masterRepository.GetCoursesById(Convert.ToInt32(edu.Course));
                            if (coursename != null)
                            {
                                edu.CourseName = Convert.ToString(coursename.Rows[0]["CourseName"]);
                            }
                        }
                    }
                }

                model.Skills = new Skills();
                if (!Convert.IsDBNull(jobSeekerDetail.Rows[0]["Skills"]))
                {
                    model.Skills = JsonConvert.DeserializeObject<Skills>(jobSeekerDetail.Rows[0]["Skills"].ToString());
                }
                model.PersonalDetails.DOB = Convert.ToString(jobSeekerDetail.Rows[0]["DateOfBirth"]);
                model.PersonalDetails.ProfileSummary = Convert.ToString(jobSeekerDetail.Rows[0]["ProfileSummary"]);
                model.PersonalDetails.ProfilePic = Convert.ToString(jobSeekerDetail.Rows[0]["ProfilePic"]);
                model.PersonalDetails.CandidateId = Convert.ToString(jobSeekerDetail.Rows[0]["CandidateId"]);
                model.PersonalDetails.JobTitleName = Convert.ToString(jobSeekerDetail.Rows[0]["JobTitleName"]);
                model.PersonalDetails.FirstName = Convert.ToString(jobSeekerDetail.Rows[0]["FirstName"]);
                model.PersonalDetails.LastName = Convert.ToString(jobSeekerDetail.Rows[0]["LastName"]);
                model.PersonalDetails.MobileNo = Convert.ToString(jobSeekerDetail.Rows[0]["MobileNo"]);
                model.PersonalDetails.Email = Convert.ToString(jobSeekerDetail.Rows[0]["Email"]);
                model.PersonalDetails.MaritalStatus = Convert.ToString(jobSeekerDetail.Rows[0]["MaritalStatus"]);
                model.PersonalDetails.MaritalStatusName = Convert.ToString(jobSeekerDetail.Rows[0]["MaritalStatusName"]);
                model.PersonalDetails.Address1 = Convert.ToString(jobSeekerDetail.Rows[0]["Address1"]);
                model.PersonalDetails.City = Convert.ToString(jobSeekerDetail.Rows[0]["City"]);
                model.PersonalDetails.JobTitleId = jobSeekerDetail.Rows[0]["JobTitleId"] as int? ?? 0;
                model.PersonalDetails.UserId = UserId;
                model.PersonalDetails.Gender = Convert.ToString(jobSeekerDetail.Rows[0]["Gender"]);
                model.PersonalDetails.Country = Convert.ToString(jobSeekerDetail.Rows[0]["Country"]);
                model.PersonalDetails.State = Convert.ToString(jobSeekerDetail.Rows[0]["State"]);
                model.PersonalDetails.Resume = Convert.ToString(jobSeekerDetail.Rows[0]["Resume"]);
                model.PersonalDetails.LinkedinProfile = Convert.ToString(jobSeekerDetail.Rows[0]["LinkedinProfile"] as string) ??"";
                model.PersonalDetails.IsJobAlert = Convert.ToBoolean(jobSeekerDetail.Rows[0]["IsJobAlert"]);
                //model.PersonalDetails.ProfileScore = ProfileScore;
                if (!Convert.IsDBNull(jobSeekerDetail.Rows[0]["TotalExperience"]))
                {
                    model.PersonalDetails.TotalExperience = Convert.ToDouble(jobSeekerDetail.Rows[0]["TotalExperience"]);
                }
                //Check if resume file exist on server
                string resumepath = Path.GetFullPath(_hostingEnviroment.WebRootPath + model.PersonalDetails.Resume);
                if (!File.Exists(resumepath))
                {
                    string fName = "";
                    model.PersonalDetails.Resume = fName;
                }

                if(ProfileScore!=null && ProfileScore.Rows.Count>0)
                {
                    model.PersonalDetails.ProfileScore = Convert.ToInt16(ProfileScore.Rows[0]["Total"]);
                }
                model.PersonalDetails.CTC = Convert.ToString(jobSeekerDetail.Rows[0]["CurrentSalary"]);
                model.PersonalDetails.ECTC = Convert.ToString(jobSeekerDetail.Rows[0]["ExpectedSalary"]);
                model.PersonalDetails.AboutMe = Convert.ToString(jobSeekerDetail.Rows[0]["AboutMe"]);
                model.PersonalDetails.JobIndustryArea = Convert.ToString(jobSeekerDetail.Rows[0]["JobIndustryAreaId"]);
                model.PersonalDetails.EmploymentStatus = Convert.ToString(jobSeekerDetail.Rows[0]["EmploymentStatusId"]);
                model.PersonalDetails.EmploymentStatusName = Convert.ToString(jobSeekerDetail.Rows[0]["EmploymentStatusName"]);
            }
            return model;
        }

        public bool AddProfileDetails(UserViewModel model)
        {
            var u = new UserModel
            {
                Address1 = model.Address1,
                Address2 = model.Address2,
                Address3 = model.Address3,
                AboutMe = model.AboutMe,
                EmploymentStatus = model.EmploymentStatus,
                MaritalStatus = model.MaritalStatus,
                CandidateId = model.CandidateId,
                Country = model.Country,
                City = model.City,
                State = model.State,
                CompanyName = model.CompanyName,
                DOB = model.DOB,
                Email = model.Email,
                FirstName = model.FirstName,
                Gender = model.Gender,
                MobileNo = model.MobileNo,
                JobIndustryArea = model.JobIndustryArea,
                UserId = model.UserId,
                JobTitleId = model.JobTitleId,
                CTC = model.CTC,
                ECTC = model.ECTC,
                TotalExperience = Math.Round(model.TotalExperience, 2),
                LinkedinProfile=model.LinkedinProfile

            };
            bool isRegister = _userProfileRepository.AddNewProfileDetail(u);
            if (isRegister)
            {
                return true;
            }
            throw new UserNotCreatedException("Unable to create skills, please contact your teck deck.");
        }

        public bool UploadFileData(string fName, int userId)
        {
            bool isRegister = _userProfileRepository.AddNewUploadFile(fName, userId);
            if (isRegister)
            {
                return true;
            }
            throw new UserNotCreatedException("Unable to create skills, please contact your teck deck.");
        }

        public bool UploadProfilePicture(string fName, int userId)
        {
            bool isSuccess = _userProfileRepository.UploadProfilePicture(fName, userId);
            if (isSuccess)
            {
                return true;
            }
            throw new UserNotCreatedException("Unable to create skills, please contact your teck deck.");
        }

        public bool ApplyJobDetails(UserViewModel user, int jobPostId)
        {
            int userId = user.UserId;
            if (usp_CheckIfUserExistInUserProfessionalDetails(Convert.ToString(userId)) == false)
            {
                throw new DataNotUpdatedException("To apply job please complete your profile");
            }
            if (CheckIfSkillEmpty(Convert.ToString(userId)) == false)
            {
                throw new DataNotUpdatedException("To apply job please complete your profile");
            }
            if (CheckIfResumeEmpty(Convert.ToString(userId)) == false)
            {
                throw new DataNotUpdatedException("To apply job please complete your profile");
            }
            if (CheckIfEducationDetailsEmpty(Convert.ToString(userId)) == false)
            {
                throw new DataNotUpdatedException("To apply job please complete your profile");
            }
            if (CheckIfJobExist(Convert.ToString(userId), Convert.ToString(jobPostId)))
            {
                throw new AllReadyExistJob("You have already applied this job");
            }

            if (_userProfileRepository.ApplyJob(userId, jobPostId))
            {
                UserViewModel e = GetEmployerDetailFromJobId(jobPostId);
                if (e != null)
                {
                    SendMailToJobSeeker(e, user.Email, user.FullName, user.UserId);
                    SendMailToEmployer(e, user.Email, e.FullName, user.UserId, jobPostId);
                    return true;
                }
                throw new DataNotFound("Employer details not found for this job");
            }

            throw new FaildToApplyJob("Unable to apply job");
        }

        public IList<JobTypeViewModel> GetJobTypes()
        {
            DataTable jTypes = _masterRepository.GetJobTypes();
            IList<JobTypeViewModel> jobTypes = new List<JobTypeViewModel>();
            if (null != jTypes && jTypes.Rows.Count > 0)
            {
                foreach (DataRow row in jTypes.Rows)
                {
                    if (Convert.ToInt32(row["Id"]) != 3)
                    {
                        jobTypes.Add(new JobTypeViewModel
                        {
                            Id = Convert.ToInt32(row["Id"]),
                            Type = Convert.ToString(row["Type"])
                        });
                    }
                }
            }
            return jobTypes;
        }

        private void SendMailToJobSeeker(UserViewModel e, string to, string name, int userId)
        {
            string jsProfile =
                $"{URLprotocol}://" +
                $"{_httpContextAccessor.HttpContext.Request.Host.Value}" +
                $"/JobSeekerManagement/Profile";

            var eModel = new EmailViewModel
            {
                Subject = "Applied Job from Placement Portal",
                Body = "Dear " + name + ",<br/><br/> We have successfully forwarded your application for the position of " + e.JobTitlebyEmployer
                        + "<br/><br/>Corporate Name  : " + e.CompanyName + "<br/>JobRole  : " + e.JobTitleName + "<br/>Job Description  : " + e.Jobdetails
                        + "<br/><br/>In case you don't find the above details appropriate, <a href="+jsProfile+">update profile</a> and apply again."
                        + "<br/><br/>Recruiters will be contacting you on this mobile number +91" + e.MobileNo + "<br/>To update it, please <a href="+jsProfile+">click here</a>"+ "<br><br>Thank You<br>Placement Portal Team",
                To = new string[] { to },
                From = e.Email,
                IsHtml = true,
                MailType = (int)MailType.JobApplication
            };
            emailHandler.SendMail(eModel, userId);
        }

        private void SendMailToEmployer(UserViewModel e, string from, string name, int userId, int jobpostid)
        {
            string jobDetail =
            $"{URLprotocol}://" +
            $"{_httpContextAccessor.HttpContext.Request.Host.Value}" +
            $"/Job/JobDetails/?jobid="+ jobpostid;

            var eModel = new EmailViewModel
            {
                Subject = "You have recevied one job application",
                Body = "Dear " + name + ",<br/><br/> You have received a new application for the job post " + e.JobTitlebyEmployer
                        + "<br/><br/>JobRole  : " + e.JobTitleName + "<br/>Job Description  : " + e.Jobdetails + "<br/>Job Posted On  : " + e.CreatedDate
                        + "<br/><br/>To Check Completed details please <a href="+ jobDetail + ">click here</a>" + "<br><br>Thank You<br>Placement Portal Team",
                To = new string[] { e.Email },
                From = from,
                IsHtml = true,
                MailType = (int)MailType.JobApplication
            };
            emailHandler.SendMail(eModel, userId);
        }

        private UserViewModel GetEmployerDetailFromJobId(int jobId)
        {
            DataTable dt = _userProfileRepository.GetEmployerDetailFromJobId(jobId);
            if (null != dt && dt.Rows.Count > 0)
            {
                UserViewModel employer = new UserViewModel
                {
                    UserId = Convert.ToInt32(dt.Rows[0]["UserId"]),
                    FirstName = Convert.ToString(dt.Rows[0]["FirstName"]),
                    LastName = Convert.ToString(dt.Rows[0]["LastName"]),
                    Email = Convert.ToString(dt.Rows[0]["Email"]),
                    CompanyName = Convert.ToString(dt.Rows[0]["CompanyName"]),
                    JobTitleName = Convert.ToString(dt.Rows[0]["JobTitleName"]),
                    JobTitlebyEmployer = Convert.ToString(dt.Rows[0]["JobTitleByEmployer"]),
                    Jobdetails = Convert.ToString(dt.Rows[0]["JobDetails"]),
                    MobileNo = Convert.ToString(dt.Rows[0]["MobileNo"]),
                    CreatedDate = Convert.ToDateTime(dt.Rows[0]["CreatedDate"]),
                };
                return employer;
            }
            return null;
        }

        private bool CheckIfJobExist(string UserId, string jobPostId)
        {
            return _userProfileRepository.CheckIfJobExist(UserId, jobPostId);
        }

        private bool CheckIfSkillEmpty(string UserId)
        {
            return _userProfileRepository.CheckIfskillEmpty(UserId);
        }

        private bool CheckIfResumeEmpty(string UserId)
        {
            return _userProfileRepository.CheckIfResumeEmpty(UserId);
        }

        private bool CheckIfEducationDetailsEmpty(string UserId)
        {
            return _userProfileRepository.CheckIfEducationDetailsEmpty(UserId);
        }

        private bool usp_CheckIfUserExistInUserProfessionalDetails(string UserId)
        {
            return _userProfileRepository.CheckIfUserAvailableInUserProfessionalDetails(UserId);
        }

        public bool UpdateItSkills(ITSkills ITSkills, int UserId)
        {
            var skill = new ITSkills
            {
                Id = ITSkills.Id,
                Skill = ITSkills.Skill,
                SkillVersion = ITSkills.SkillVersion,
                LastUsed = ITSkills.LastUsed,
                ExperienceYear = ITSkills.ExperienceYear,
                ExperienceMonth = ITSkills.ExperienceMonth
            };
            bool isAdded = _userProfileRepository.UpdateItSkills(skill, UserId);
            if (isAdded)
            {
                return true;
            }
            throw new UserNotCreatedException("Unable to create skills, please contact your teck deck.");
        }

        public bool DeleteITSkill(int ITSkillId, int UserId)
        {
            bool isDeleted = _userProfileRepository.DeleteITSkill(ITSkillId, UserId);
            if (isDeleted)
            {
                return true;
            }
            throw new UserNotCreatedException("Unable to delete skills, please contact your teck deck.");
        }
        public JobSeekerDashboardSummary GetJobSeekerDashboard(int UserId)
        {
            var dashboardSummary = _userProfileRepository.GetJobseekerDashboard(UserId);
            if (null != dashboardSummary && dashboardSummary.Tables.Count > 0)
            {
                JobSeekerDashboardSummary dashboard = new JobSeekerDashboardSummary();
                int i = -1;
                foreach (DataTable table in dashboardSummary.Tables)
                {
                    i++;
                    switch (i)
                    {
                        case 0:
                            dashboard.ViewedYourProfile = Convert.ToInt32(table.Rows[0]["ViewedYourProfile"]);
                            break;
                        case 1:
                            dashboard.TotalAppliedJobs = Convert.ToInt32(table.Rows[0]["TotalAppliedJobs"]);
                            break;
                        case 2:
                            dashboard.TotalContactedNo = Convert.ToInt32(table.Rows[0]["TotalContactedNo"]);
                            break;
                        case 3:
                            dashboard.TotalCompaniesFollowed = Convert.ToInt32(table.Rows[0]["TotalCompaniesFollowed"]);
                            break;
                        default:
                            break;
                    }
                }
                return dashboard;
            }
            throw new DataNotFound("Dashboard Summary not found");
        }
        public List<SearchJobListViewModel> GetJobseekerAppliedJobs(int userId)
        {
            DataTable dt = _userProfileRepository.GetJobseekerAppliedJobs(userId);
            List<SearchJobListViewModel> lstAppliedJobs = new List<SearchJobListViewModel>();
            try
            {
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        SearchJobListViewModel AppliedJobs = new SearchJobListViewModel
                        {
                            CompanyLogo = Convert.ToString(dt.Rows[i]["CompanyLogo"]) ?? "",
                            JobTitle = Convert.ToString(dt.Rows[i]["JobTitle"]) ?? "",
                            EmploymentStatus = Convert.ToString(dt.Rows[i]["EmploymentStatus"]) ?? "",
                            City = Convert.ToString(dt.Rows[i]["City"]) ?? "",
                            HiringCriteria = Convert.ToString(dt.Rows[i]["HiringCriteria"]) ?? "",
                            CompanyName = Convert.ToString(dt.Rows[i]["CompanyName"]) ?? "",
                            JobPostId = (dt.Rows[i]["JobPostId"] as int?) ?? 0,
                            JobTitleByEmployer = Convert.ToString(dt.Rows[i]["JobTitleByEmployer"]) ?? "",
                            CTC = Convert.ToString(dt.Rows[i]["CTC"]) ?? "",
                            NumberOfDays = Convert.ToString(dt.Rows[i]["NumberOfDays"]) ?? "",
                            CreatedDate = Convert.ToDateTime(dt.Rows[i]["CreatedDate"])
                        };
                        lstAppliedJobs.Add(AppliedJobs);
                    }
                }
            }
            catch
            {
                throw new DataNotFound("data not found!");
            }
            return lstAppliedJobs;
        }

        public List<JobSeekerViewedProfile> GetJobseekerViewedProfile(int userId)
        {
            DataTable dt = _userProfileRepository.GetJobseekerViewedProfile(userId);
            List<JobSeekerViewedProfile> lstViewedProfile = new List<JobSeekerViewedProfile>();
            try
            {
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        JobSeekerViewedProfile ViewedProfile = new JobSeekerViewedProfile
                        {
                            ModifiedViewedOn = Convert.ToString(dt.Rows[i]["ModifiedViewedOn"]) ?? "",
                            CompanyName = Convert.ToString(dt.Rows[i]["CompanyName"]) ?? ""
                        };
                        lstViewedProfile.Add(ViewedProfile);
                    }
                }
            }
            catch
            {
                throw new DataNotFound("data not found!");
            }
            return lstViewedProfile;
        }

        public bool DeleteAppliedJob(int JobPostId, int UserId)
        {
            bool isDeleted = _userProfileRepository.DeleteAppliedJob(JobPostId, UserId);
            if (isDeleted)
            {
                return true;
            }
            throw new UserNotCreatedException("Unable to delete applied jobs, please contact your teck deck.");
        }

        public List<EmployerFollowers> EmployerFollowers(int userId)
        {
            DataTable dt = _userProfileRepository.EmployerFollowers(userId);
            List<EmployerFollowers> lstEmpFollower = new List<EmployerFollowers>();
            try
            {
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        EmployerFollowers EmpFollowers = new EmployerFollowers
                        {
                            EmployerID = Convert.ToInt32(dt.Rows[i]["EmployerID"]),
                            CreatedDate = Convert.ToString(dt.Rows[i]["CreatedDate"]) ?? "",
                            CompanyName = Convert.ToString(dt.Rows[i]["CompanyName"]) ?? ""
                        };
                        lstEmpFollower.Add(EmpFollowers);
                    }
                }
            }
            catch
            {
                throw new DataNotFound("data not found!");
            }
            return lstEmpFollower;
        }

        public bool UnfollowEmployer(int EmployerId, int UserId)
        {
            bool isDeleted = _userProfileRepository.UnfollowEmployer(EmployerId, UserId);
            if (isDeleted)
            {
                return true;
            }
            throw new UserNotCreatedException("Unable to unfollow comapny, please contact your teck deck.");
        }

        public Skills JobSeekerSkills(int userId)
        {
            Skills model = new Skills();
            DataTable dt = _userProfileRepository.JobSeekerSkills(userId);
            try
            {
               if (!Convert.IsDBNull(dt.Rows[0]["Skills"]))
                {
                    model = JsonConvert.DeserializeObject<Skills>(dt.Rows[0]["Skills"].ToString());
                }
            }
            catch
            {
                throw new DataNotFound("data not found!");
            }
            return model;
        }

        public List<SearchJobListViewModel> JobSeekerJobsOnSkills(string skills,int UserId)
        {
            DataTable dt = _userProfileRepository.JobSeekerJobsOnSkills(skills, UserId);
            List<SearchJobListViewModel> lstJobs = new List<SearchJobListViewModel>();
            try
            {
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string picpath = System.IO.Path.GetFullPath(_hostingEnviroment.WebRootPath + dt.Rows[i]["CompanyLogo"]);
                        if (!System.IO.File.Exists(picpath))
                        {
                            string fName = $@"\ProfilePic\" + "Avatar_company.jpg";
                            dt.Rows[i]["CompanyLogo"] = fName;
                        }
                        SearchJobListViewModel Jobs = new SearchJobListViewModel
                        {
                            CompanyLogo = Convert.ToString(dt.Rows[i]["CompanyLogo"]) ?? "",
                            JobTitle = Convert.ToString(dt.Rows[i]["JobTitle"]) ?? "",
                            EmploymentStatus = Convert.ToString(dt.Rows[i]["EmploymentStatus"]) ?? "",
                            City = Convert.ToString(dt.Rows[i]["City"]) ?? "",
                            HiringCriteria = Convert.ToString(dt.Rows[i]["HiringCriteria"]) ?? "",
                            CompanyName = Convert.ToString(dt.Rows[i]["CompanyName"]) ?? "",
                            JobPostId = (dt.Rows[i]["JobPostId"] as int?) ?? 0,
                            JobTitleByEmployer = Convert.ToString(dt.Rows[i]["JobTitleByEmployer"]) ?? "",
                            Skills = Convert.ToString(dt.Rows[i]["Skills"]) ?? "",
                            CTC = Convert.ToString(dt.Rows[i]["CTC"]) ?? "",
                        };
                        lstJobs.Add(Jobs);
                    }
                }
            }
            catch
            {
                throw new DataNotFound("data not found!");
            }
            return lstJobs;
        }

        public bool JobsAlerts(int IsAlert,int UserId)
        {
            bool isTrue = _userProfileRepository.JobsAlert(IsAlert,UserId);
            if (isTrue)
            {
                return true;
            }
            throw new UserNotCreatedException("Unable to change alert, please contact your teck deck.");
        }

        public List<MessageViewModel> JobSeekerContacted(int userId)
        {
            DataTable dt = _userProfileRepository.JobSeekerContacted(userId);
            List<MessageViewModel> lstContacted = new List<MessageViewModel>();
            try
            {
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        MessageViewModel Contacted = new MessageViewModel
                        {
                            Subject = Convert.ToString(dt.Rows[i]["Subject"]),
                            SenderEmail = Convert.ToString(dt.Rows[i]["FromEmail"]) ?? "",
                            ReceiverEmail = Convert.ToString(dt.Rows[i]["ToEmail"]) ?? ""
                        };
                        lstContacted.Add(Contacted);
                    }
                }
            }
            catch
            {
                throw new DataNotFound("data not found!");
            }
            return lstContacted;
        }
    }
}
