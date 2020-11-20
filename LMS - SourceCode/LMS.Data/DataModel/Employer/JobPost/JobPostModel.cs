using LMS.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Data.DataModel.Employer.JobPost
{
    public class JobPostModel
    {
        
        public int JobIndustryAreaId { get; set; }
        public string OtherJobIndustryArea { get; set; }
        public string CountryCode { get; set; }
        public string StateCode { get; set; }
        public string CityCode { get; set; }
        public int EmploymentStatusId { get; set; }
        public string JobTitleId { get; set; }
        public string JobTitleByEmployer { get; set; }
        public int EmploymentTypeId { get; set; }
        public string MonthlySalary { get; set; }
        public int NoPosition { get; set; }
        public string IsWalkin { get; set; }
        public string Nationality { get; set; }
        public string PositionStartDate { get; set; }
        public string PositionEndDate { get; set; }
        public string HiringCriteria { get; set; }
        public string CreatedBy { get; set; }
        public int JobType { get; set; }
        public string Gender { get; set; }
        public string JobDetails { get; set; }

        //  Here User Id in the sense of source of job
        public int Userid { get; set; }


        //  Additional columns in case of bulk upload
        public string SPOC { get; set; } =Constants.NotAvailalbe;
        public string SPOCEmail { get; set; } = Constants.NotAvailalbe;
        public string SPOCContact { get; set; } = Constants.NotAvailalbe;
        public string CTC { get; set; } = Constants.NotAvailalbe;
        public string Quarter1 { get; set; } = "0";
        public string Quarter2 { get; set; } = "0";
        public string Quarter3 { get; set; } = "0";
        public string Quarter4 { get; set; } = "0";
        public String Featured { get; set; }
        public int DisplayOrder { get; set; }
        public string Skills { get; set; }
        public int MinExp { get; set; }
        public int MaxExp { get; set; }
        public int FinancialYear { get; set; }
        public bool IsFromBulkUpload { get; set; }
    }
}
