using System.Collections.Generic;

namespace LMS.Model.DataViewModel.Shared
{
    public class BulkJobPostSummaryViewModel
    {
        public string FileName { get; set; }
        public IList<BulkJobPostSummaryDetailViewModel> Summary { get; set; }
    }
}
