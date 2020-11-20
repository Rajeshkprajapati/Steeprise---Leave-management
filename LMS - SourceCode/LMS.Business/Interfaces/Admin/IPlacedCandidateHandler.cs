using LMS.Model.DataViewModel.Admin.PlacedCandidate;
using LMS.Model.DataViewModel.Shared;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LMS.Business.Interfaces.Admin
{
    public interface IPlacedCandidateHandler
    {
        bool UploadFile(UserViewModel user,List<IFormFile> file);
        IList<PlacedCandidateViewModel> GetAllCandidate();
        DataTable GetDataInExcel();
    }
}
