using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Model.DataViewModel.TrainingPartner
{
    public class BulkJobSeekerUploadSummaryViewModel
    {
        public string SequenceNo { get; set; }
        public string CandidateId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ProcessedBy { get; set; }
        public string ProcessedOn { get; set; }
        public string Status { get; set; }
        public string ErrorDetails { get; set; }
    }
}
