using System;

namespace LMS.Model.DataViewModel.Employer.JobPost
{
    public class JobPostViewModel
    {
        public int JobPostId { get; set; }
        public int JobIndustryAreaId { get; set; }
        public string OtherJobIndustryArea { get; set; }
        public string CountryCode { get; set; }
        public string StateCode { get; set; }
        public string CityCode { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public int EmploymentStatusId { get; set; }
        public string EmploymentStatusName { get; set; }
        public string JobTitleId { get; set; }
        public string JobTitle { get; set; }
        public string JobTitleByEmployer { get; set; }
        public int EmploymentTypeId { get; set; }
        public string EmploymentTypeName { get; set; }
        public string MonthlySalary { get; set; }
        public int NoPosition { get; set; }
        public string IsWalkIn { get; set; }
        public int JobType { get; set; }
        public string JobTypeSummary { get; set; }
        public bool IsApplied { get; set; }
        public string Nationality { get; set; }
        public string PositionStartDate { get; set; }
        public string PositionEndDate { get; set; }
        public string JobDetails { get; set; }
        public string HiringCriteria { get; set; }
        public string Gender { get; set; }
        public string CompanyLogo { get; set; }
        public string CompanyName { get; set; }
        public string Mobile { get; set; }
        public string ContactPerson { get; set; }
        public string UserName { get; set; }
        //public string CountryName { get; set; }
        //public string StateName { get; set; }
        //public string cityNAme { get; set; }


        //  Additional columns in case of bulk upload
        public string SPOCEmail { get; set; }
        public string CTC { get; set; }
        public string Quarter1 { get; set; } = "0";
        public string Quarter2 { get; set; } = "0";
        public string Quarter3 { get; set; } = "0";
        public string Quarter4 { get; set; } = "0";

        public bool IsQuarter1ReadOnly { get; set; }
        public bool IsQuarter2ReadOnly { get; set; }
        public bool IsQuarter3ReadOnly { get; set; }
        public bool IsQuarter4ReadOnly { get; set; }

        public int TotalApplications { get; set; }
        public DateTime PostedOn { get; set; }
        public string Featured { get; set; }
        public int DisplayOrder { get; set; }
        public string Skills { get; set; }

        public int? MinExp { get; set; } = -1;
        public int? MaxExp { get; set; } = -1;
        public int FinancialYear { get; set; } = DateTime.Now.Year;
        public DateTime CreatedDate { get; set; }
    }
}
