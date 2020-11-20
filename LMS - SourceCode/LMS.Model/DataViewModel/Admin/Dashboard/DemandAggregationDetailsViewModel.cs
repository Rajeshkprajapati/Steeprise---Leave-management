using System;

namespace LMS.Model.DataViewModel.Admin.Dashboard
{
    public class DemandAggregationDetailsViewModel
    {
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string JobRole { get; set; }
        public string CreatedByFName { get; set; }
        public string CreatedByLName { get; set; }
        public string CreatedBy { get { return $"{CreatedByFName} {CreatedByLName}"; } }
        public string Company { get; set; }
        public DateTime CreatedDate { get; set; }
        public string FinancialYear { get; set; }
        public int Quarter1 { get; set; }
        public int Quarter2 { get; set; }
        public int Quarter3 { get; set; }
        public int Quarter4 { get; set; }
        public int Total { get { return Quarter1 + Quarter2 + Quarter3 + Quarter4; } }
    }
}
