using LMS.Model.DataViewModel.JobSeeker;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Model.DataViewModel.Employer.SearchResume
{
    public class SearchResumeListViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Skills Skills { get; set; }
        public string Resume { get; set; }
        public string CityCode { get; set; }
        public string CityName { get; set; }
        public string JobIndustryAreaName { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Address{ get; set; }
        public string State { get; set; }
        public string StateName { get; set; }
        public string Country { get; set; }
       public string MobileNo { get; set; }
       public string ProfilePic { get; set; }
        public string AboutMe { get; set; }
        public double TotalExperience { get; set; } = 0.0;
        public string DateOfBirth { get; set; }
        public string CurrentSalary { get; set; }
        public string ExpectedSalary { get; set; }
        public ExperienceDetails[] ExperienceDetails { get; set; }
        public EducationalDetails[] EducationalDetails { get; set; }
        public string LinkedinProfile { get; set; }
        public string JobTitle { get; set; }
        public string CountryName { get; set; }
    } 

    //public class SearchResume
    //{
    //    public string LMS.{ get; set; }
    //    public string JobCategory { get; set; }
    //    public string Experiance { get; set; }
    //    public string City { get; set; }       
    //}
}
