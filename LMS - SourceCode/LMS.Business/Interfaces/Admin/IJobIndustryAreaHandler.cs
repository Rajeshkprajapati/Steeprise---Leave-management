using LMS.Model.DataViewModel.Admin.JobIndustryArea;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Business.Interfaces.Admin
{
    public interface IJobIndustryAreaHandler
    {
        List<JobIndustryAreaViewModel> GetJobIndustryAreaList();
        bool UpdateJobIndustryArea(JobIndustryAreaViewModel jobIndustryArea);
        bool DeleteJobIndustryArea(string jobIndustryAreaId, string deletedBy);
    }
}
