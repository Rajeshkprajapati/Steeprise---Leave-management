using LMS.Business.Handlers.DataProcessorFactory;
using LMS.Business.Interfaces.Home;
using LMS.Business.Shared;
using LMS.Data.Interfaces.Home;
using LMS.Data.Interfaces.Shared;
using LMS.Model.DataViewModel.Admin.JobIndustryArea;
using LMS.Model.DataViewModel.Admin.SuccessStory;
using LMS.Model.DataViewModel.Home;
using LMS.Model.DataViewModel.JobSeeker;
using LMS.Model.DataViewModel.Shared;
using LMS.Utility.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;

namespace LMS.Business.Handlers.Home
{
    public class HomeHandler : IHomeHandler
    {
        private readonly IHomeRepositories _homeRepositories;
        private IHostingEnvironment hostingEnviroment;
        private readonly IMasterDataRepository masterDataRepository;

        public HomeHandler(IConfiguration configuration, IHostingEnvironment _hostingEnvironment)
        {
            var factory = new ProcessorFactoryResolver<IHomeRepositories>(configuration);
            _homeRepositories = factory.CreateProcessor();
            var mfactory = new ProcessorFactoryResolver<IMasterDataRepository>(configuration);
            masterDataRepository = mfactory.CreateProcessor();
            hostingEnviroment = _hostingEnvironment;
        }

        public List<CityViewModel> GetAllCityList()
        {
            DataTable city = _homeRepositories.GetCityListDetail();
            if (city.Rows.Count > 0)
            {
                List<CityViewModel> lstCity = new List<CityViewModel>();
                lstCity = ConvertDatatableToModelList.ConvertDataTable<CityViewModel>(city);
                return lstCity;
            }
            throw new DataNotFound("City data not found");
        }

        public List<CityViewModel> GetCityListByChar(string cityFirstChar)
        {
            DataTable city = _homeRepositories.GetCityListByChar(cityFirstChar);
            if (city.Rows.Count > 0)
            {
                List<CityViewModel> lstCity = new List<CityViewModel>();
                lstCity = ConvertDatatableToModelList.ConvertDataTable<CityViewModel>(city);
                return lstCity;
            }
            throw new DataNotFound("City data not found");
        }

        public List<JobTitleViewModel> GetJobListByChar(string jobFirstChar)
        {
            List<JobTitleViewModel> jobslist = new List<JobTitleViewModel>();
            try
            {
                DataTable jobs = _homeRepositories.GetJobListByChar(jobFirstChar);
                if (jobs.Rows.Count > 0)
                {
                    jobslist = ConvertDatatableToModelList.ConvertDataTable<JobTitleViewModel>(jobs);
                }
                return jobslist;
            }
            catch (DataNotFound ex)
            {
                throw new DataNotFound(ex.Message);
            }
        }

        public List<CityViewModel> GetCityHasJobPostId()
        {
            DataTable city = _homeRepositories.GetCityHasJobPostId();
            List<CityViewModel> lstCity = new List<CityViewModel>();
            if (city.Rows.Count > 0)
            {
                lstCity = ConvertDatatableToModelList.ConvertDataTable<CityViewModel>(city);
            }
            return lstCity;
        }

        public List<CityViewModel> GetCitiesWithJobSeekerInfo()
        {
            DataTable city = _homeRepositories.GetCitiesWithJobSeekerInfo();
            if (city.Rows.Count > 0)
            {
                List<CityViewModel> lstCity = new List<CityViewModel>();
                lstCity = ConvertDatatableToModelList.ConvertDataTable<CityViewModel>(city);
                return lstCity;
            }
            throw new UserNotFoundException("User not found");
        }

        public List<int> GetAplliedJobs(int userid)
        {
            List<int> appliejobs = new List<int>();
            DataTable dt = _homeRepositories.GetAplliedJobs(userid);
            if(dt!=null && dt.Rows.Count > 0)
            {
                for (int i = 0; dt != null && i < dt.Rows.Count; i++)
                {
                    appliejobs.Add(Convert.ToInt32(dt.Rows[i]["JobPostId"]));
                }
            }
            return appliejobs;
        }

        public List<SuccessStoryViewModel> GetSuccussStory()
        {
            DataTable successStories = _homeRepositories.GetSuccessStory();
            List<SuccessStoryViewModel> lstsuccessStory = new List<SuccessStoryViewModel>();
            if (successStories.Rows.Count > 0)
            {
                DataTable dt = successStories;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SuccessStoryViewModel successStory = new SuccessStoryViewModel
                    {
                        Email = Convert.ToString(dt.Rows[i]["Email"]),
                        //city = Convert.ToString(dt.Rows[i]["City"]),
                        name = Convert.ToString(dt.Rows[i]["FirstName"]),
                        Tagline = Convert.ToString(dt.Rows[i]["TagLine"]),
                        Message = Convert.ToString(dt.Rows[i]["Message"]),
                        CreatedDate = Convert.ToDateTime(dt.Rows[i]["CreatedDate"]),
                        ImgUrl = Convert.ToString(dt.Rows[i]["ProfilePic"]),
                    };
                    lstsuccessStory.Add(successStory);
                }
            }
            return lstsuccessStory;
        }

        public bool PostSuccessStory(SuccessStoryViewModel model)
        {


            bool isSuccessStory = _homeRepositories.PostSuccsessStory(model);
            if (isSuccessStory)
            {
                return true;
            }
            throw new UserCanNotPostData("Unable to post data");
        }

        public List<SearchJobListViewModel> GetFeaturedJobs()
        {
            DataTable dt = _homeRepositories.GetFeaturedJobs();
            List<SearchJobListViewModel> lstfeautredJobs = new List<SearchJobListViewModel>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SearchJobListViewModel feautredJob = new SearchJobListViewModel
                    {
                        CompanyLogo = Convert.ToString(dt.Rows[i]["CompanyLogo"]) ?? "",
                        JobTitle = Convert.ToString(dt.Rows[i]["JobTitle"]) ?? "",
                        EmploymentStatus = Convert.ToString(dt.Rows[i]["EmploymentStatus"]) ?? "",
                        City = Convert.ToString(dt.Rows[i]["City"]) ?? "",
                        HiringCriteria = Convert.ToString(dt.Rows[i]["HiringCriteria"]) ?? "",
                        CompanyName = Convert.ToString(dt.Rows[i]["CompanyName"]) ?? "",
                        JobPostId = (dt.Rows[i]["JobPostId"] as int?) ?? 0,
                        JobTitleByEmployer = Convert.ToString(dt.Rows[i]["JobTitleByEmployer"]) ?? "",
                        FeaturedJobDisplayOrder = (dt.Rows[i]["FeaturedJobDisplayOrder"] as int?) ?? 0,
                        JobDetails = Convert.ToString(dt.Rows[i]["JobDetails"]) ?? "",
                        CTC = Convert.ToString(dt.Rows[i]["CTC"]) ?? "",
                        NumberOfDays = Convert.ToString(dt.Rows[i]["NumberOfDays"]) ?? ""
                    };
                    lstfeautredJobs.Add(feautredJob);
                }
            }
            return lstfeautredJobs;
        }

        public IList<JobTitleViewModel> GetAllJobRoles()
        {
            DataTable dt = masterDataRepository.GetJobRoles();
            List<JobTitleViewModel> jobRoles = new List<JobTitleViewModel>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    JobTitleViewModel jRole = new JobTitleViewModel
                    {
                        JobTitleId = Convert.ToInt32(dt.Rows[i]["JobTitleId"]),
                        JobTitleName = Convert.ToString(dt.Rows[i]["JobTitleName"])
                    };
                    jobRoles.Add(jRole);
                }
            }
            return jobRoles;
        }

        public List<PopulerSearchesViewModel> PopulerSearchesCategory()
        {
            DataTable dt = _homeRepositories.PopulerSearchesCategory();
            List<PopulerSearchesViewModel> lstCategory = new List<PopulerSearchesViewModel>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    PopulerSearchesViewModel category = new PopulerSearchesViewModel
                    {
                        JobIndustryAreaId = Convert.ToInt32(dt.Rows[i]["JobIndustryAreaId"]),
                        JobIndustryAreaName = Convert.ToString(dt.Rows[i]["JobIndustry"]),
                        TotalCount = Convert.ToString(dt.Rows[i]["Count"]),

                    };
                    lstCategory.Add(category);
                }
            }
            return lstCategory;
        }

        public List<PopulerSearchesViewModel> PopulerSearchesCity()
        {
            DataTable dt = _homeRepositories.PopulerSearchesCity();
            List<PopulerSearchesViewModel> lstCity = new List<PopulerSearchesViewModel>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    PopulerSearchesViewModel city = new PopulerSearchesViewModel
                    {
                        CityCode = Convert.ToString(dt.Rows[i]["CityCode"]),
                        City = Convert.ToString(dt.Rows[i]["City"]),
                        TotalCount = Convert.ToString(dt.Rows[i]["Count"]),

                    };
                    lstCity.Add(city);
                }
            }
            return lstCity;
        }

        public List<SearchJobListViewModel> ViewAllFeaturedJobs()
        {
            DataTable dt = _homeRepositories.ViewAllFeaturedJobs();
            if (dt.Rows.Count > 0)
            {
                List<SearchJobListViewModel> lstfeautredJobs = new List<SearchJobListViewModel>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SearchJobListViewModel feautredJob = new SearchJobListViewModel
                    {
                        JobPostId = Convert.ToInt32(dt.Rows[i]["JobPostId"]),
                        CompanyLogo = Convert.ToString(dt.Rows[i]["CompanyLogo"]),
                        JobTitle = Convert.ToString(dt.Rows[i]["JobTitle"]),
                        EmploymentStatus = Convert.ToString(dt.Rows[i]["EmploymentStatus"]),
                        City = Convert.ToString(dt.Rows[i]["City"]),
                        HiringCriteria = Convert.ToString(dt.Rows[i]["HiringCriteria"]),
                        CompanyName = Convert.ToString(dt.Rows[i]["CompanyName"]),
                        JobTitleByEmployer = Convert.ToString(dt.Rows[i]["JobTitleByEmployer"]),
                        FeaturedJobDisplayOrder = (dt.Rows[i]["FeaturedJobDisplayOrder"] as int?) ?? 0,

                    };
                    lstfeautredJobs.Add(feautredJob);
                }
                return lstfeautredJobs;
            }
            throw new DataNotFound("Data not found");
        }

        public List<SearchJobListViewModel> AllJobsByCategory(int categoryId)
        {
            DataTable dt = _homeRepositories.AllJobsByCategory(categoryId);
            if (dt.Rows.Count > 0)
            {
                List<SearchJobListViewModel> lstfeautredJobs = new List<SearchJobListViewModel>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string picpath = System.IO.Path.GetFullPath(hostingEnviroment.WebRootPath + dt.Rows[i]["CompanyLogo"]);
                    if (!System.IO.File.Exists(picpath))
                    {
                        string fName = $@"\ProfilePic\" + "Avatar_company.jpg";
                        dt.Rows[i]["CompanyLogo"] = fName;
                    }
                    SearchJobListViewModel feautredJob = new SearchJobListViewModel
                    {
                        JobPostId = Convert.ToInt32(dt.Rows[i]["JobPostId"]),
                        CompanyLogo = Convert.ToString(dt.Rows[i]["CompanyLogo"]),
                        JobTitle = Convert.ToString(dt.Rows[i]["JobTitle"]),
                        EmploymentStatus = Convert.ToString(dt.Rows[i]["EmploymentStatus"]),
                        City = Convert.ToString(dt.Rows[i]["City"]),
                        HiringCriteria = Convert.ToString(dt.Rows[i]["HiringCriteria"]),
                        CompanyName = Convert.ToString(dt.Rows[i]["CompanyName"]),
                        CTC = Convert.ToString(dt.Rows[i]["CTC"]),
                        NumberOfDays = Convert.ToString(dt.Rows[i]["NumberOfDays"]),

                    };
                    lstfeautredJobs.Add(feautredJob);
                }
                return lstfeautredJobs;
            }
            throw new DataNotFound("Data not found");
        }

        public List<SearchJobListViewModel> AllJobsByCity(string CityCode)
        {
            DataTable dt = _homeRepositories.AllJobsByCity(CityCode);
            if (dt.Rows.Count > 0)
            {
                List<SearchJobListViewModel> lstfeautredJobs = new List<SearchJobListViewModel>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string picpath = System.IO.Path.GetFullPath(hostingEnviroment.WebRootPath + dt.Rows[i]["CompanyLogo"]);
                    if (!System.IO.File.Exists(picpath))
                    {
                        string fName = $@"\ProfilePic\" + "Avatar_company.jpg";
                        dt.Rows[i]["CompanyLogo"] = fName;
                    }
                    SearchJobListViewModel feautredJob = new SearchJobListViewModel
                    {
                        JobPostId = Convert.ToInt32(dt.Rows[i]["JobPostId"]),
                        CompanyLogo = Convert.ToString(dt.Rows[i]["CompanyLogo"]),
                        JobTitle = Convert.ToString(dt.Rows[i]["JobTitle"]),
                        EmploymentStatus = Convert.ToString(dt.Rows[i]["EmploymentStatus"]),
                        City = Convert.ToString(dt.Rows[i]["City"]),
                        HiringCriteria = Convert.ToString(dt.Rows[i]["HiringCriteria"]),
                        CompanyName = Convert.ToString(dt.Rows[i]["CompanyName"]),
                        CTC = Convert.ToString(dt.Rows[i]["CTC"]),
                        NumberOfDays = Convert.ToString(dt.Rows[i]["NumberOfDays"]),

                    };
                    lstfeautredJobs.Add(feautredJob);
                }
                return lstfeautredJobs;
            }
            throw new DataNotFound("Data not found");
        }

        public List<JobIndustryAreaViewModel> GetCategory()
        {
            DataTable dt = _homeRepositories.GetCategory();

            if (dt.Rows.Count > 0)
            {
                List<JobIndustryAreaViewModel> lstJobCategory = new List<JobIndustryAreaViewModel>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    JobIndustryAreaViewModel jobCategory = new JobIndustryAreaViewModel
                    {
                        JobIndustryAreaId = Convert.ToInt32(dt.Rows[i]["JobIndustryAreaId"]),
                        JobIndustryAreaName = Convert.ToString(dt.Rows[i]["JobIndustryAreaName"])
                    };
                    lstJobCategory.Add(jobCategory);
                }
                return lstJobCategory;
            }
            throw new DataNotFound("Data not found");
        }

        public List<TopEmployerViewModel> TopEmployer()
        {
            DataTable dt = _homeRepositories.TopEmployer();
            List<TopEmployerViewModel> lstTopEmployer = new List<TopEmployerViewModel>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TopEmployerViewModel topEmployer = new TopEmployerViewModel
                    {
                        CompanyName = Convert.ToString(dt.Rows[i]["CompanyName"]),
                        UserId = Convert.ToInt32(dt.Rows[i]["UserId"]),
                        Logo = Convert.ToString(dt.Rows[i]["Logo"]),
                        JobseekerId = Convert.ToInt32(dt.Rows[i]["JobSeekerID"]),
                        Count = Convert.ToInt32(dt.Rows[i]["Count"]),
                        FollowIsActive = Convert.ToInt32(dt.Rows[i]["FollowIsActive"])
                    };
                    lstTopEmployer.Add(topEmployer);
                }
            }
            return lstTopEmployer;
        }

        public List<SearchJobListViewModel> GetAllCompanyList()
        {
            DataTable dt = _homeRepositories.GetAllCompanyList();
            List<SearchJobListViewModel> list = new List<SearchJobListViewModel>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SearchJobListViewModel model = new SearchJobListViewModel
                    {
                        CompanyLogo = Convert.ToString(dt.Rows[i]["CompanyLogo"]),
                        CompanyName = Convert.ToString(dt.Rows[i]["CompanyName"]),
                    };
                    list.Add(model);
                }
            }
            return list;
        }

        public List<SearchJobListViewModel> NasscomJobs()
        {
            DataTable dt = _homeRepositories.NasscomJobsList();
            List<SearchJobListViewModel> list = new List<SearchJobListViewModel>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SearchJobListViewModel model = new SearchJobListViewModel
                    {
                        CompanyLogo = Convert.ToString(dt.Rows[i]["CompanyLogo"]),
                        CompanyName = Convert.ToString(dt.Rows[i]["CompanyName"]),
                        JobPostId = Convert.ToInt32(dt.Rows[i]["JobPostId"]),
                        JobTitle = Convert.ToString(dt.Rows[i]["JobTitle"]),
                        City = Convert.ToString(dt.Rows[i]["City"]),
                        EmploymentStatus = Convert.ToString(dt.Rows[i]["EmploymentStatus"]),
                    };
                    list.Add(model);
                }
            }
            return list;
        }

        public List<SuccessStoryVideoViewModel> GetSuccussStoryVideos()
        {
            DataTable successStoriesVideo = _homeRepositories.GetSuccessStoryVideo();
            List<SuccessStoryVideoViewModel> lstsuccessStoryvideo = new List<SuccessStoryVideoViewModel>();
            if (successStoriesVideo.Rows.Count > 0)
            {
                DataTable dt = successStoriesVideo;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SuccessStoryVideoViewModel successStory = new SuccessStoryVideoViewModel
                    {
                        Title = Convert.ToString(dt.Rows[i]["title"]),
                        Video = Convert.ToString(dt.Rows[i]["filename"]),
                        Type = Convert.ToString(dt.Rows[i]["type"]),
                        UpdatedBy = Convert.ToString(dt.Rows[i]["createdby"]),
                        CreatedDate = Convert.ToDateTime(dt.Rows[i]["CreatedDate"])
                    };
                    lstsuccessStoryvideo.Add(successStory);
                }
            }
            return lstsuccessStoryvideo;
        }

        public List<CompanyViewModel> GetCompanyHasJobPostId()
        {
            DataTable company = _homeRepositories.GetCompanyHasJobPostId();
            List<CompanyViewModel> lstCompany = new List<CompanyViewModel>();
            if (company.Rows.Count > 0)
            {
                lstCompany = ConvertDatatableToModelList.ConvertDataTable<CompanyViewModel>(company);
            }
            return lstCompany;
        }
        public string GetContactUsEmail()
        {
            string ContactUs;
            try {
                ContactUs = _homeRepositories.GetContactUsEmail();
            }
            catch
            {
                throw new DataNotFound("Data not found");
            }
            return ContactUs;
        }

        public string TalentConnectLink()
        {
            string TalentConnectPdf;
            try
            {
                TalentConnectPdf = _homeRepositories.TalentConnectLink();
            }
            catch
            {
                throw new DataNotFound("Data not found");
            }
            return TalentConnectPdf;
        }
        public string CandidateBulkUpload()
        {
            string CandidateBulkUploadPdf;
            try
            {
                CandidateBulkUploadPdf = _homeRepositories.CandidateBulkUpload();
            }
            catch
            {
                throw new DataNotFound("Data not found");
            }
            return CandidateBulkUploadPdf;
        }
        public string TPRegistrationGuide()
        {
            string TPRegistrationGuidePdf;
            try
            {
                TPRegistrationGuidePdf = _homeRepositories.TPRegistrationGuide();
            }
            catch
            {
                throw new DataNotFound("Data not found");
            }
            return TPRegistrationGuidePdf;
        }
        public List<SearchJobListViewModel> GetRecentJobs()
        {
            DataTable dt = _homeRepositories.GetRecentJobs();
            List<SearchJobListViewModel> lstRecentJobs = new List<SearchJobListViewModel>();
            try
            {
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        SearchJobListViewModel RecentJobs = new SearchJobListViewModel
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
                            NumberOfDays = Convert.ToString(dt.Rows[i]["NumberOfDays"]) ?? ""
                        };
                        lstRecentJobs.Add(RecentJobs);
                    }
                }
            }
            catch {
                throw new DataNotFound("data not found!");
            }
            return lstRecentJobs;
        }
        public List<SearchJobListViewModel> GetWalkInsJobs()
        {
            DataTable dt = _homeRepositories.GetWalkInJobs();
            List<SearchJobListViewModel> lstWalkinJobs = new List<SearchJobListViewModel>();
            try
            {
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        SearchJobListViewModel WalkinJobs = new SearchJobListViewModel
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
                            NumberOfDays = Convert.ToString(dt.Rows[i]["NumberOfDays"]) ?? ""
                        };
                        lstWalkinJobs.Add(WalkinJobs);
                    }
                }
            }
            catch
            {
                throw new DataNotFound("data not found!");
            }
            return lstWalkinJobs;
        }

        public bool EmployerFollower(int EmployerId, int UserId)
        {
            bool isInserted = _homeRepositories.EmployerFollower(EmployerId, UserId);
            if (isInserted)
            {
                return true;
            }
            throw new UserNotCreatedException("Unable to follow company, please contact your teck deck.");
        }

        public List<PopulerSearchesViewModel> CategoryJobVacancies()
        {
            DataTable dt = _homeRepositories.CategoryJobVacancies();
            List<PopulerSearchesViewModel> lstCategory = new List<PopulerSearchesViewModel>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    PopulerSearchesViewModel category = new PopulerSearchesViewModel
                    {
                        JobIndustryAreaId = Convert.ToInt32(dt.Rows[i]["JobIndustryAreaId"]),
                        JobIndustryAreaName = Convert.ToString(dt.Rows[i]["JobIndustry"]),
                        TotalCount = Convert.ToString(dt.Rows[i]["Count"]),

                    };
                    lstCategory.Add(category);
                }
            }
            return lstCategory;
        }

        public List<PopulerSearchesViewModel> CityJobVacancies()
        {
            DataTable dt = _homeRepositories.CityJobVacancies();
            List<PopulerSearchesViewModel> lstCity = new List<PopulerSearchesViewModel>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    PopulerSearchesViewModel city = new PopulerSearchesViewModel
                    {
                        CityCode = Convert.ToString(dt.Rows[i]["CityCode"]),
                        City = Convert.ToString(dt.Rows[i]["City"]),
                        TotalCount = Convert.ToString(dt.Rows[i]["Count"]),

                    };
                    lstCity.Add(city);
                }
            }
            return lstCity;
        }

        public List<PopulerSearchesViewModel> CompanyJobVacancies()
        {
            DataTable dt = _homeRepositories.CompanyJobVacancies();
            List<PopulerSearchesViewModel> lstCompany = new List<PopulerSearchesViewModel>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    PopulerSearchesViewModel company = new PopulerSearchesViewModel
                    {
                        UserId = Convert.ToInt32(dt.Rows[i]["UserId"]),
                        CompanyName = Convert.ToString(dt.Rows[i]["CompanyName"]),
                        TotalCount = Convert.ToString(dt.Rows[i]["Count"]),

                    };
                    lstCompany.Add(company);
                }
            }
            return lstCompany;
        }

        public List<SearchJobListViewModel> AllJobsByCompany(int UserId)
        {
            DataTable dt = _homeRepositories.AllJobsByCompany(UserId);
            if (dt.Rows.Count > 0)
            {
                List<SearchJobListViewModel> lstcompanyJobs = new List<SearchJobListViewModel>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string picpath = System.IO.Path.GetFullPath(hostingEnviroment.WebRootPath + dt.Rows[i]["CompanyLogo"]);
                    if (!System.IO.File.Exists(picpath))
                    {
                        string fName = $@"\ProfilePic\" + "Avatar_company.jpg";
                        dt.Rows[i]["CompanyLogo"] = fName;
                    }
                        SearchJobListViewModel companyJob = new SearchJobListViewModel
                    {
                        JobPostId = Convert.ToInt32(dt.Rows[i]["JobPostId"]),
                        CompanyLogo = Convert.ToString(dt.Rows[i]["CompanyLogo"]),
                        JobTitle = Convert.ToString(dt.Rows[i]["JobTitle"]),
                        EmploymentStatus = Convert.ToString(dt.Rows[i]["EmploymentStatus"]),
                        City = Convert.ToString(dt.Rows[i]["City"]),
                        HiringCriteria = Convert.ToString(dt.Rows[i]["HiringCriteria"]),
                        CompanyName = Convert.ToString(dt.Rows[i]["CompanyName"]),
                        CTC = Convert.ToString(dt.Rows[i]["CTC"]),

                    };
                    lstcompanyJobs.Add(companyJob);
                }
                return lstcompanyJobs;
            }
            throw new DataNotFound("Data not found");
        }
    }
}
