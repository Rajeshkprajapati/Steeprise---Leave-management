using LMS.Model.DataViewModel.TrainingPartner;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Business.Interfaces.TrainingPartner
{
    public interface IDashboardHandler
    {
        IList<CandidatesViewModel> GetCandidates(int tpId);
        CandidatesViewModel GetCandidateDetails(int userid);
        bool DeleteCandidate(int userid);
        bool UpdateCandidateDetails(CandidatesViewModel model);
    }
}
