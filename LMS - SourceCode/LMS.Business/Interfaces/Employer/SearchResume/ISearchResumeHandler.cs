using System;
using System.Collections.Generic;
using System.Text;
using LMS.Model.DataViewModel.Employer.SearchResume;
using LMS.Model.DataViewModel.Shared;

namespace LMS.Business.Interfaces.Employer.SearchResume
{
    public interface ISearchResumeHandler
    {
        List<SearchResumeListViewModel> GetSearchResumeList(SearchResumeViewModel searches);
        SearchResumeListViewModel ShowCandidateDetails(int employerId, int jobSeekerId);
    }
}
