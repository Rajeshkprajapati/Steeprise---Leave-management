//using LMS.Model.DataModel.Employer.JobPost;
//using LMS.Model.DataModel.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Model.DataViewModel.JobSeeker
{
    public class SearchJobListViewModel
    {
        public string JobTitle { get; set; }
        public string City { get; set; }
        public string EmploymentStatus { get; set; }
        public string HiringCriteria { get; set; }
        public string CompanyName { get; set; }
        public int JobPostId { get; set; }
        public string CompanyLogo { get; set; }
        public string Skills { get; set; }
        public bool IsApplied { get; set; }
        public string JobTitleByEmployer { get; set; }
        public int FeaturedJobDisplayOrder { get; set; }
        public string JobDetails { get; set; }
        public string CTC { get; set; }
        public string NumberOfDays { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
