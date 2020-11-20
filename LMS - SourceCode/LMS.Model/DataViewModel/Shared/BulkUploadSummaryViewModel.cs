using System.Collections.Generic;

namespace LMS.Model.DataViewModel.Shared
{
    public class BulkUploadSummaryViewModel<T>
    {
        public string FileName { get; set; }
        public IList<T> Summary { get; set; }
    }
}
