using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Model.DataViewModel.Shared
{
    public class BulkJobPostSummaryDetailViewModel
    {
        public string SequenceNo { get; set; }
        public string Status { get; set; }
        public string ErrorDetails { get; set; }
        public string CompanyName { get; set; }
        public string StateCode { get; set; }
        public string CityCode { get; set; }
        public string JobTitleByEmployer { get; set; }
        //public string JobRole1 { get; set; }
        //public string JobRole2 { get; set; }
        //public string JobRole3 { get; set; }
        public string SPOC { get; set; }
        public string SPOCEmail { get; set; }
        public string SPOCContact { get; set; }
        //public string MonthlySalary { get; set; }
        public string CTC { get; set; }
        public string HiringCriteria { get; set; }
        public string MinExp { get; set; }
        public string MaxExp { get; set; }
        public string JobType { get; set; }
        public string JobDetails { get; set; }
        //public string FinancialYear { get; set; }
        //public string Quarter1 { get; set; }
        //public string Quarter2 { get; set; }
        //public string Quarter3 { get; set; }
        //public string Quarter4 { get; set; }
        public string Total { get; set; }
        public string PositionStartDate { get; set; }
        public string PositionEndDate { get; set; }
        public string ProcessedBy { get; set; }
        public string ProcessedOn { get; set; }
    }
}
