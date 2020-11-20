using LMS.Model.DataViewModel.JobSeeker;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Business.Interfaces.Admin
{
    public interface IManageJobsHandler
    {
        bool UpdateFeaturedJobDisplayOrder(int jobpostid,int displayoder);
        bool DeleteFeaturedJob(int jobpostid);
    }
}
