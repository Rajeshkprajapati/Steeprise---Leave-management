using LMS.Model.DataViewModel.Shared;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Business.Interfaces.Shared
{
    public interface IBulkJobPostHandler
    {
        IEnumerable<BulkUploadSummaryViewModel<BulkJobPostSummaryDetailViewModel>> UploadJobs(UserViewModel user,IList<IFormFile> files);
        Task UploadJobsInBackground(UserViewModel user, IList<IFormFile> files);
    }
}
