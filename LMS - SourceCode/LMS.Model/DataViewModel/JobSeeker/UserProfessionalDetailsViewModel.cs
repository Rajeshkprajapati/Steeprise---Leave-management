using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Model.DataViewModel.JobSeeker
{
    public class UserProfessionalDetailsViewModel
    {
        public string UserId { get; set; }
        public ExperienceDetails[] ExperienceDetails { get; set; }
        public EducationalDetails[] EducationalDetails { get; set; }
        public Skills Skills { get; set; }
        public string CurrentSalary { get; set; }
        public string ExpectedSalary { get; set; }
        public string DateOfBirth { get; set; }
        public string Resume { get; set; }
        public string AboutMe { get; set; }
        public string ProfileSummary { get; set; }
        public string Status { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }        
        public string MobileNo { get; set; }
        public string Gender { get; set; }
        public string MaritalStatus { get; set; }
        public string Address { get; set; }
        public string City
        {
            get; set;
        }
        public string State
        {
            get; set;
        }
        public string Country
        {
            get; set;
        }
        public int EmploymentStatus { get; set; }
        public int JobIndustryArea { get; set; }
        public int ID { get; set; }
        public string TotalExperiance { get; set; }
        public DateTime AppliedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime JobPostedDate { get; set; }
        public string JobRole { get; set; }
        public string jobPostCity { get; set; }
        public string JobPostContactPerson { get; set; }
        public string JobPostedCompany { get; set; }
    }
}
