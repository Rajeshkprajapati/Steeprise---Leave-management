using LMS.Model.DataViewModel.JobSeeker;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Data.Interfaces.Admin
{
    public interface IManageJobsRepository
    {
        bool UpdateFeaturedJobDisplayOrder(int jobpostid,int displayorder);
        bool DeleteFeaturedJob(int jobpostid);
    }
}
