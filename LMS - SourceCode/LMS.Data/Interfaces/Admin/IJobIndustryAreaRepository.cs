using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using LMS.Data.DataModel.Admin.JobIndustryArea;


namespace LMS.Data.Interfaces.Admin
{
   public interface IJobIndustryAreaRepository
    {
        DataTable GetJobIndustryArea();
        bool UpdateJobIndustryArea(JobIndustryAreaModel jobIndustry);
        bool DeleteJobIndustryArea(string jobIndustryAreaId,string deletedBy);
    }
}
