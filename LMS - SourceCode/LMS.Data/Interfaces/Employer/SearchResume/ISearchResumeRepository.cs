using LMS.Data.DataModel.Shared;
using LMS.Model.DataViewModel.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LMS.Data.Interfaces.Employer.SearchResume
{
    public interface ISearchResumeRepository
    {
        DataTable GetSearchResumeList(SearchResumeModel searches);
        DataTable ShowCandidateDetails(int employerId, int jobSeekerId);
    }
}

