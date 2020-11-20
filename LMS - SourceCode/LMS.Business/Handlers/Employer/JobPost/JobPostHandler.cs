using LMS.Business.Handlers.DataProcessorFactory;
using LMS.Business.Interfaces.Employer.JobPost;
using LMS.Business.Shared;
using LMS.Data.DataModel.Employer.JobPost;
using LMS.Data.Interfaces.Employer.JobPost;
using LMS.Data.Interfaces.Shared;
using LMS.Model.DataViewModel.Employer.JobPost;
using LMS.Model.DataViewModel.JobSeeker;
using LMS.Model.DataViewModel.Shared;
using LMS.Utility.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;

namespace LMS.Business.Handlers.Employer.JobPost
{
    public class JobPostHandler : IJobPostHandler
    {
        private readonly IJobPostRepository _jobPostProcessor;
        private IHostingEnvironment hostingEnviroment;
        private readonly IMasterDataRepository masterDataRepository;
        public JobPostHandler(IConfiguration configuration, IHostingEnvironment _hostingEnvironment)
        {
            var factory = new ProcessorFactoryResolver<IJobPostRepository>(configuration);
            _jobPostProcessor = factory.CreateProcessor();
            var mfactory = new ProcessorFactoryResolver<IMasterDataRepository>(configuration);
            masterDataRepository = mfactory.CreateProcessor();
            hostingEnviroment = _hostingEnvironment;
        }

        public List<JobTitleViewModel> GetJobTitleDetails()
        {
            DataTable jobTitle = masterDataRepository.GetJobRoles();
            List<JobTitleViewModel> lstJobTitle = new List<JobTitleViewModel>();
            if (jobTitle.Rows.Count > 0)
            {
                lstJobTitle = ConvertDatatableToModelList.ConvertDataTable<JobTitleViewModel>(jobTitle);
                return lstJobTitle;
            }
            return lstJobTitle;
        }

        public List<JobIndustryAreaModel> GetJobIndustryAreaDetails()
        {
            DataTable jobIndustryArea = _jobPostProcessor.GetJobIndustryAreaDetail();
            List<JobIndustryAreaModel> lstJobIndustryArea = new List<JobIndustryAreaModel>();
            if (jobIndustryArea.Rows.Count > 0)
            {
                lstJobIndustryArea = ConvertDatatableToModelList.ConvertDataTable<JobIndustryAreaModel>(jobIndustryArea);
            }
            return lstJobIndustryArea;
        }

        public bool AddPreferredLocation(string[] location,int userid)
        {
            try
            {
                var status = true;
                for (int i = 0; i < location.Length; i++)
                {
                    if (!_jobPostProcessor.AddPreferredLocation(location[i], i+1,userid))
                    {
                        return false;//break;
                    }
                }
                return status;
            }catch(Exception ex)
            {
                throw new Exception("Unable to add location");
            }
        }

        public List<JobIndustryAreaModel> GetJobIndustryAreaWithJobPost()
        {
            DataTable jobIndustryArea = _jobPostProcessor.GetJobIndustryAreaWithJobPost();
            List<JobIndustryAreaModel> lstJobIndustryArea = new List<JobIndustryAreaModel>();
            if (jobIndustryArea.Rows.Count > 0)
            {
                lstJobIndustryArea = ConvertDatatableToModelList.ConvertDataTable<JobIndustryAreaModel>(jobIndustryArea);
            }
            return lstJobIndustryArea;
        }

        public List<JobIndustryAreaModel> GetJobIndustryAreaWithStudentData()
        {
            DataTable jobIndustryArea = _jobPostProcessor.GetJobIndustryAreaWithStudentData();
            if (jobIndustryArea.Rows.Count > 0)
            {
                List<JobIndustryAreaModel> lstJobIndustryArea = new List<JobIndustryAreaModel>();
                lstJobIndustryArea = ConvertDatatableToModelList.ConvertDataTable<JobIndustryAreaModel>(jobIndustryArea);
                return lstJobIndustryArea;
            }
            throw new DataNotFound("Data not found");
        }


        public List<EmploymentStatusModel> GetJobJobEmploymentStatusDetails()
        {
            DataTable JobEmploymentStatus = _jobPostProcessor.GetJobJobEmploymentStatusDetail();
            List<EmploymentStatusModel> lstJobEmploymentStatus = new List<EmploymentStatusModel>();
            if (JobEmploymentStatus.Rows.Count > 0)
            {
                lstJobEmploymentStatus = ConvertDatatableToModelList.ConvertDataTable<EmploymentStatusModel>(JobEmploymentStatus);
            }
            return lstJobEmploymentStatus;
        }

        public List<EmploymentTypeModel> GetJobJobEmploTypeDetails()
        {
            DataTable JobEmploymentType = _jobPostProcessor.GetJobJobEmploTypeDetail();
            List<EmploymentTypeModel> lstJobEmploymentType = new List<EmploymentTypeModel>();
            if (JobEmploymentType.Rows.Count > 0)
            {
                lstJobEmploymentType = ConvertDatatableToModelList.ConvertDataTable<EmploymentTypeModel>(JobEmploymentType);
            }
            return lstJobEmploymentType;
        }

        public List<CountryViewModel> GetCountryDetails()
        {
            DataTable country = masterDataRepository.GetCountries();
            List<CountryViewModel> lstCountry = new List<CountryViewModel>();
            if (country.Rows.Count > 0)
            {
                lstCountry = ConvertDatatableToModelList.ConvertDataTable<CountryViewModel>(country);
            }
            return lstCountry;
        }

        public List<JobTypeViewModel> GetJobTypes()
        {
            DataTable jTypes = masterDataRepository.GetJobTypes();
            List<JobTypeViewModel> jobTypes = new List<JobTypeViewModel>();
            if (null!= jTypes && jTypes.Rows.Count > 0)
            {
                foreach (DataRow row in jTypes.Rows)
                {
                    jobTypes.Add(new JobTypeViewModel
                    {
                        Id=Convert.ToInt32(row["Id"]),
                        Type = Convert.ToString(row["Type"])
                    });
                }
            }
            return jobTypes;
        }

        public List<CourseViewModel> GetCourseCategory()
        {
            var dTable = masterDataRepository.GetCoursesCategory();
            List<CourseViewModel> courses = new List<CourseViewModel>();
            if (null != dTable && dTable.Rows.Count > 0)
            {
                foreach (DataRow row in dTable.Rows)
                {
                    courses.Add(new CourseViewModel
                    {
                        Id = Convert.ToInt32(row["CategoryId"]),
                        Name = Convert.ToString(row["Name"])
                    });
                }
            }
            return courses;
        }

        public List<CourseViewModel> GetCourses(int categoryid)
        {
            List<CourseViewModel> course = null;
            var dTable = masterDataRepository.GetCourses(categoryid);
            if (null != dTable && dTable.Rows.Count > 0)
            {
                course = new List<CourseViewModel>();
                foreach (DataRow row in dTable.Rows)
                {
                    course.Add(new CourseViewModel
                    {
                        Id = Convert.ToInt32(row["CourseId"]),
                        Name = Convert.ToString(row["Name"])
                    });
                }
            }
            return course;
        }


        public List<StateViewModel> GetStateList(string CountryCode)
        {
            DataTable state = _jobPostProcessor.GetStateListDetail(CountryCode);
            if (state.Rows.Count > 0)
            {
                List<StateViewModel> lstState = new List<StateViewModel>();
                lstState = ConvertDatatableToModelList.ConvertDataTable<StateViewModel>(state);
                return lstState;
            }
            throw new UserNotFoundException("User not found");
        }

        public List<CityViewModel> GetCityList(string StateCode)
        {
            DataTable city = _jobPostProcessor.GetCityListDetail(StateCode);
            if (city.Rows.Count > 0)
            {
                List<CityViewModel> lstCity = new List<CityViewModel>();
                lstCity = ConvertDatatableToModelList.ConvertDataTable<CityViewModel>(city);
                return lstCity;
            }
            throw new UserNotFoundException("User not found");
        }

        public List<GenderViewModel> GetGenderListDetail()
        {
            DataTable gender = _jobPostProcessor.GetGenderListDetail();
            if (gender.Rows.Count > 0)
            {
                List<GenderViewModel> genderList = new List<GenderViewModel>();
                genderList = ConvertDatatableToModelList.ConvertDataTable<GenderViewModel>(gender);
                return genderList;
            }
            throw new UserNotFoundException("Data not found");
        }

        public bool AddJobPost(JobPostViewModel jobpostviewmodel, int userId)
        {
            JobPostModel model = new JobPostModel
            {
                JobIndustryAreaId = jobpostviewmodel.JobIndustryAreaId,
                OtherJobIndustryArea = jobpostviewmodel.OtherJobIndustryArea,
                CountryCode = jobpostviewmodel.CountryCode,
                StateCode = jobpostviewmodel.StateCode,
                CityCode = jobpostviewmodel.CityCode,
                EmploymentStatusId = jobpostviewmodel.EmploymentStatusId,
                JobTitleId = jobpostviewmodel.JobTitleId,
                EmploymentTypeId = jobpostviewmodel.EmploymentTypeId,
                MonthlySalary = jobpostviewmodel.MonthlySalary,
                NoPosition = jobpostviewmodel.NoPosition,
                Nationality = jobpostviewmodel.Nationality,
                PositionStartDate = jobpostviewmodel.PositionStartDate,
                PositionEndDate = jobpostviewmodel.PositionEndDate,
                HiringCriteria = jobpostviewmodel.HiringCriteria,
                JobDetails = jobpostviewmodel.JobDetails,
                Gender = jobpostviewmodel.Gender,
                CreatedBy = Convert.ToString(userId),
                JobType = jobpostviewmodel.JobType,
                Userid = userId,
                CTC = jobpostviewmodel.CTC,
                SPOC = jobpostviewmodel.ContactPerson,
                SPOCContact = jobpostviewmodel.Mobile,
                SPOCEmail = jobpostviewmodel.SPOCEmail,
                IsWalkin = jobpostviewmodel.IsWalkIn,
                //Quarter1 = jobpostviewmodel.Quarter1,
                //Quarter2 = jobpostviewmodel.Quarter2,
                //Quarter3 = jobpostviewmodel.Quarter3,
                //Quarter4 = jobpostviewmodel.Quarter4,
                Skills = jobpostviewmodel.Skills,
                JobTitleByEmployer=jobpostviewmodel.JobTitleByEmployer,
                MinExp=(int)jobpostviewmodel.MinExp,
                MaxExp=(int)jobpostviewmodel.MaxExp,
                FinancialYear=jobpostviewmodel.FinancialYear
            };
            bool result = _jobPostProcessor.AddJobPostData(model);
            if (result == true)
            {
                return result;
            }
            throw new UserNotCreatedException("Job Post is not create.");
        }
        public JobPostViewModel GetJobDetails(int jobid)
        {
            var dt = _jobPostProcessor.GetJobDetails(jobid);
            if (dt != null)
            {
                string picpath = System.IO.Path.GetFullPath(hostingEnviroment.WebRootPath + dt.Rows[0]["CompanyLogo"]);
                if (!System.IO.File.Exists(picpath))
                {
                    string fName = $@"\ProfilePic\" + "Avatar_company.jpg";
                    dt.Rows[0]["CompanyLogo"] = fName;
                }
                JobPostViewModel jobdetails = new JobPostViewModel
                {
                    JobPostId = Convert.ToInt32(dt.Rows[0]["JobPostId"]),
                    JobIndustryAreaId = Convert.ToInt32(dt.Rows[0]["JobIndustryAreaId"]),
                    CountryCode = Convert.ToString(dt.Rows[0]["Country"]),
                    StateCode = Convert.ToString(dt.Rows[0]["StateName"]),
                    CityCode = Convert.ToString(dt.Rows[0]["City"]),
                    MonthlySalary = Convert.ToString(dt.Rows[0]["MonthlySalary"]),
                    //NoPosition = Convert.ToInt32(dt.Rows[0]["NoPosition"]),
                    EmploymentStatusName = Convert.ToString(dt.Rows[0]["EmploymentStatusName"]),
                    EmploymentTypeName = Convert.ToString(dt.Rows[0]["EmploymentTypeName"]),
                    JobTitle = Convert.ToString(dt.Rows[0]["JobTitleName"]),
                    Nationality = Convert.ToString(dt.Rows[0]["Nationality"]),
                    PositionStartDate = Convert.ToString(dt.Rows[0]["PositionStartDate"]),
                    PositionEndDate = Convert.ToString(dt.Rows[0]["PositionEndDate"]),
                    HiringCriteria = Convert.ToString(dt.Rows[0]["HiringCriteria"]),
                    Gender = Convert.ToString(dt.Rows[0]["Gender"]),
                    CTC = (dt.Rows[0]["CTC"]) as string ?? "",
                    JobDetails = Convert.ToString(dt.Rows[0]["JobDetails"]),
                    CompanyLogo = Convert.ToString(dt.Rows[0]["CompanyLogo"]),
                    CompanyName = Convert.ToString(dt.Rows[0]["CompanyName"]),
                    JobTitleByEmployer = Convert.ToString(dt.Rows[0]["JobTitleByEmployer"]),
                    JobTypeSummary = Convert.ToString(dt.Rows[0]["JobTypeSummary"]),
                };
                //jobdetails.HiringCriteria = jobdetails.HiringCriteria.Substring(jobdetails.HiringCriteria.IndexOf(':') + 2);

                //int openings = DateTime.Now.Month;
                //if (openings <= 3)
                //{
                //    jobdetails.NoPosition = dt.Rows[0]["Quarter4"] as int? ?? 0;
                //}
                //else if (openings > 3 && openings <= 4)
                //{
                //    jobdetails.NoPosition = dt.Rows[0]["Quarter1"] as int? ?? 0;
                //}
                //else if (openings > 4 && openings <= 7)
                //{
                //    jobdetails.NoPosition = dt.Rows[0]["Quarter2"] as int? ?? 0;
                //}
                //else if (openings > 7 && openings <= 12)
                //{
                //    jobdetails.NoPosition = dt.Rows[0]["Quarter3"] as int? ?? 0;
                //}
                return jobdetails;
            }
            throw new DataNotFound("Job details not found");
        }

        public List<SearchJobListViewModel> RecommendedJobs(int roleid)
        {
            DataTable dt = _jobPostProcessor.RecommendedJobs(roleid);
            if (dt.Rows.Count > 0)
            {
                List<SearchJobListViewModel> lstRecommendedJobs = new List<SearchJobListViewModel>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SearchJobListViewModel RecommendedJob = new SearchJobListViewModel
                    {
                        JobPostId = Convert.ToInt32(dt.Rows[i]["JobPostId"]),
                        CompanyLogo = Convert.ToString(dt.Rows[i]["CompanyLogo"]),
                        JobTitle = Convert.ToString(dt.Rows[i]["JobTitle"]),
                        EmploymentStatus = Convert.ToString(dt.Rows[i]["EmploymentStatus"]),
                        City = Convert.ToString(dt.Rows[i]["City"]),
                        HiringCriteria = Convert.ToString(dt.Rows[i]["HiringCriteria"]),
                        CompanyName = Convert.ToString(dt.Rows[i]["CompanyName"]),
                        JobTitleByEmployer = Convert.ToString(dt.Rows[i]["JobTitleByEmployer"]),

                    };
                    lstRecommendedJobs.Add(RecommendedJob);
                }
                return lstRecommendedJobs;
            }
            throw new DataNotFound("Data not found");
        }
    }
}
