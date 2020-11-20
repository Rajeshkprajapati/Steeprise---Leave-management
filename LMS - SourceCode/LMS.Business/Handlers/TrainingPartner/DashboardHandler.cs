using LMS.Business.Handlers.DataProcessorFactory;
using LMS.Business.Interfaces.TrainingPartner;
using LMS.Data.DataModel.Shared;
using LMS.Data.Interfaces.TrainingPartner;
using LMS.Model.DataViewModel.TrainingPartner;
using LMS.Utility.Exceptions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LMS.Business.Handlers.TrainingPartner
{
    public class DashboardHandler: IDashboardHandler
    {
        private readonly IDashboardRepository dashboardRepository;
        public DashboardHandler(IConfiguration configuration)
        {
            var factory = new ProcessorFactoryResolver<IDashboardRepository>(configuration);
            dashboardRepository = factory.CreateProcessor();
        }

        public IList<CandidatesViewModel> GetCandidates(int tpId)
        {
            var candidates = dashboardRepository.GetCandidates(tpId);
            IList<CandidatesViewModel> candidatesVM = null;
            if (null != candidates && candidates.Rows.Count > 0)
            {
                candidatesVM = new List<CandidatesViewModel>();
                foreach (DataRow row in candidates.Rows)
                {
                    candidatesVM.Add(new CandidatesViewModel
                    {
                        Id=Convert.ToInt32(row["UserId"]),
                        CandidateId = Convert.ToString(row["Candidateid"]),
                        FirstName = Convert.ToString(row["FirstName"]),
                        LastName= Convert.ToString(row["LastName"]),
                        Email = Convert.ToString(row["Email"]),
                        Password= Convert.ToString(row["Password"]),
                        IsActive = Convert.ToBoolean(row["IsActive"])
                    });
                }
            }
            return candidatesVM;
        }
        public CandidatesViewModel GetCandidateDetails(int userid)
        {
            var details = dashboardRepository.GetCandidateDetails(userid);
            CandidatesViewModel candidatedetails = new CandidatesViewModel();
            if(details != null && details.Rows.Count > 0)
            {
                candidatedetails.Id = userid;
                candidatedetails.CandidateId = details.Rows[0]["Candidateid"] as string ?? "";
                candidatedetails.FirstName = details.Rows[0]["FirstName"] as string ?? "";
                candidatedetails.LastName = details.Rows[0]["LastName"] as string ?? "";
                candidatedetails.Email = details.Rows[0]["Email"] as string ?? "";
                candidatedetails.Password = details.Rows[0]["Password"] as string ?? "";
            }
            return candidatedetails;
        }
        public bool DeleteCandidate(int userid)
        {
            bool result = dashboardRepository.DeleteCandidate(userid);
            return result;
        }

        public bool UpdateCandidateDetails(CandidatesViewModel model)
        {
            UserModel candidate = new UserModel() {
                UserId = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                CandidateId = model.CandidateId,
                Password = model.Password
            };
            bool result = dashboardRepository.UpdateCandidateDetails(candidate);
            return result;
        }

    }
}
