using System;
using System.Collections.Generic;
using System.Text;
using LMS.Business.Interfaces.Admin;
using LMS.Business.Handlers.DataProcessorFactory;
using LMS.Data.Interfaces.Admin;
using System.Data;
using Microsoft.Extensions.Configuration;
using LMS.Data.DataModel.Admin.JobTitle;
using LMS.Model.DataViewModel.Shared;
using LMS.Data.Interfaces.Shared;

namespace LMS.Business.Handlers.Admin
{
    public class JobTitleHandler:IJobTitleHandler
    {
        private readonly IJobTitleRepositroy jobTitleRepositroy;
        private readonly IMasterDataRepository masterRepository;
        public JobTitleHandler(IConfiguration configuration)
        {
            var factory = new ProcessorFactoryResolver<IJobTitleRepositroy>(configuration);
            jobTitleRepositroy = factory.CreateProcessor();
            var masterFactory = new ProcessorFactoryResolver<IMasterDataRepository>(configuration);
            masterRepository = masterFactory.CreateProcessor();
        }
        public List<JobTitleViewModel> GetJobTitle()
        {
            DataTable dt = masterRepository.GetJobRoles();
            List<JobTitleViewModel> jobTitlesList = new List<JobTitleViewModel>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                JobTitleViewModel jobTitles = new JobTitleViewModel()
                {
                    JobTitleId = Convert.ToInt32(dt.Rows[i]["JobTitleId"]),
                    JobTitleName = Convert.ToString(dt.Rows[i]["JobTitleName"]),
                };
                jobTitlesList.Add(jobTitles);
            }
            return (jobTitlesList);
        }
        public bool InsertUpdateJobTile(JobTitleViewModel jobTitleViewModel)
        {
            JobTitleModel jobTitle = new JobTitleModel
            {
                JobTitleId = jobTitleViewModel.JobTitleId,
                JobTitleName = jobTitleViewModel.JobTitleName,
                UpdatedBy = jobTitleViewModel.UpdatedBy
            };
            var result = jobTitleRepositroy.InsertUpdateJobTile(jobTitle);
            if (result)
            {
                return true;
            }
            throw new Exception("Unable to update data");
        }
        public bool DeleteJobTitle(string jobTileId, string deletedBy)
        {
            var result = jobTitleRepositroy.DeleteJobTitle(jobTileId, deletedBy);
            if (result)
            {
                return true;
            }
            throw new Exception("Unable to delete data");
        }
    }
}
