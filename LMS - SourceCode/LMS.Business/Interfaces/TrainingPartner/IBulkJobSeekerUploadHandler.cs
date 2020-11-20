using LMS.Model.DataViewModel.Shared;
using LMS.Model.DataViewModel.TrainingPartner;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Business.Interfaces.TrainingPartner
{
    public interface IBulkJobSeekerUploadHandler
    {
        IEnumerable<BulkUploadSummaryViewModel<BulkJobSeekerUploadSummaryViewModel>> RegisterJobSeekers(UserViewModel user, IList<IFormFile> files);
    }
}
