using LMS.Business.Handlers.DataProcessorFactory;
using LMS.Business.Interfaces.Home;
using LMS.Business.Interfaces.Jobseeker;
using LMS.Business.Shared;
using LMS.Data.DataModel.Shared;
using LMS.Data.Interfaces.Jobseeker;
using LMS.Model.DataViewModel.JobSeeker;
using LMS.Model.DataViewModel.Shared;
using LMS.Utility.Exceptions;
using LMS.Utility.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace LMS.Business.Handlers.Jobseeker
{
    public class SearchJobHandler : ISearchJobHandler
    {
        private readonly ISearchJobRepository _searchJobRepository;
        private readonly IHostingEnvironment _hostingEnviroment;
        private readonly IHomeHandler homeHandler;
        public SearchJobHandler(IConfiguration configuration, IHostingEnvironment hostingEnvironment, IHomeHandler _homeHandler)
        {
            var factory = new ProcessorFactoryResolver<ISearchJobRepository>(configuration);
            _searchJobRepository = factory.CreateProcessor();
            _hostingEnviroment = hostingEnvironment;
            homeHandler = _homeHandler;
        }

        public List<SearchJobListViewModel> SearchJobList(SearchJobViewModel searches, int UserId)
        {

            var sModel = new JobSearchModel
            {
                Skills = searches.Skills,
                JobRole = searches.JobTitle,
                City = string.Join(Constants.CommaSeparator, searches.City),
                Experiance = searches.Experiance,
                JobCategory = string.Join(Constants.CommaSeparator, searches.JobCategory),
                CompanyUserId = string.Join(Constants.CommaSeparator, searches.CompanyUserId)
            };
            int quarterStartMonth = Convert.ToInt32(ConfigurationHelper.Config.GetSection(Constants.JobPostingQuarterStartingMonthKey).Value);
            DataTable jobList = _searchJobRepository.GetSearchJobList(sModel, UserId,quarterStartMonth);
            List<SearchJobListViewModel> lstJobList = new List<SearchJobListViewModel>();
            if (jobList.Rows.Count > 0)
            {
                lstJobList = ConvertDatatableToModelList.ConvertDataTable<SearchJobListViewModel>(jobList);
                var appliedJobs =homeHandler.GetAplliedJobs(UserId);
                for (int i = 0; i < lstJobList.Count; i++)
                {
                    //getting the all the jobs applied by user only if the user logged in
                    if (UserId != 0 && appliedJobs.Count > 0)
                    {
                        lstJobList[i].IsApplied = appliedJobs.Any(aj => aj == lstJobList[i].JobPostId);
                    }

                    //Handled if image url exist in db but not available physically
                    string picpath = System.IO.Path.GetFullPath(_hostingEnviroment.WebRootPath + lstJobList[i].CompanyLogo);
                    if (!System.IO.File.Exists(picpath))
                    {
                        string fName = $@"\ProfilePic\" + "Avatar_company.jpg";
                        lstJobList[i].CompanyLogo = fName;
                    }
                }
            }
            return lstJobList;
        }

    }
}
