using LMS.Model.DataViewModel.JobSeeker;
using LMS.Model.DataViewModel.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Business.Interfaces.Jobseeker
{
    public interface ISearchJobHandler
    {
        List<SearchJobListViewModel> SearchJobList(SearchJobViewModel searches, int UserId);
    }
}
