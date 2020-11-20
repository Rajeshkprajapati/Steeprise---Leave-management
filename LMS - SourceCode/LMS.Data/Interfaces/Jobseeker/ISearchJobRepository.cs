using LMS.Data.DataModel.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LMS.Data.Interfaces.Jobseeker
{
    public interface ISearchJobRepository
    {
        DataTable GetSearchJobList(JobSearchModel searches,int UserId, int quarterStartMonth);

    }
}
