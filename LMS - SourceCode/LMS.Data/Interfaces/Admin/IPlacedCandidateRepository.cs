using LMS.Data.DataModel.Admin.PlacedCandidate;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LMS.Data.Interfaces.Admin
{
    public interface IPlacedCandidateRepository
    {
        bool UploadFileData(PlacedCandidateModel user,int userid);
        DataTable GetAllCandidate();
    }
}
