using LMS.Model.DataViewModel.JobSeeker;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace LMS.Model.DataViewModel.Shared
{
    [Serializable]
    public class UserViewModel
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get { return $"{FirstName} {LastName}"; } }
        public int RoleId { get; set; }
        public string BatchNumber { get; set; }
        public string RoleName { get; set; }
        public string MobileNo { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string CityName { get; set; }
        //public string ContactPerson { get; set; }
        public string State { get; set; }
        public string StateName { get; set; }
        public string Country { get; set; }
        public string CountryName { get; set; }
        public IFormFile ImageFile { get; set; }

        public string FullAddress
        {
            get
            {
                return $"{Address1}" +
                    (!string.IsNullOrEmpty(Address2) ? string.Format(" {0}{1}", ",", Address2) : string.Empty) +
                    (!string.IsNullOrEmpty(Address3) ? string.Format(" {0}{1}", ",", Address3) : string.Empty) +
                    $"{(!string.IsNullOrEmpty(CityName) ? string.Format(" {0}{1}",", ",CityName) : string.Empty)}" +
                    $"{(!string.IsNullOrEmpty(StateName) ? string.Format(" {0}{1}", ", ", StateName) : string.Empty)}" +
                    $"{(!string.IsNullOrEmpty(CountryName) ? string.Format(" {0}{1}", ", ", CountryName) : string.Empty)}";
            }
        }
        public string MaritalStatus { get; set; }
        public string MaritalStatusName { get; set; }
        public string ProfilePic { get; set; }
        public string Gender { get; set; }
        public string GenderName { get; set; }
        public string DOB { get; set; }
        public string CompanyName { get; set; }
        public string CandidateId { get; set; }
        //public string TrainingPartnerID { get; set; }
        public string IsApproved { get; set; }
        public string PasswordExpirayDate { get; set; }
        public string ProfileSummary { get; set; }
        public string Resume { get; set; }
        public string CTC { get; set; }
        public string ECTC { get; set; }
        public string AboutMe { get; set; }
        public string EmploymentStatus { get; set; }
        public string JobIndustryArea { get; set; }
        public string JobTitleName { get; set; }
        public int JobTitleId { get; set; }
        public string JobTitlebyEmployer { get; set; }
        public string Jobdetails { get; set; }
        public DateTime CreatedDate { get; set; }
        //public int SSCJobRoleId { get; set; }
        public DateTime ActiveFrom { get; set; }
        //public string EmployerWithCity {
        //    get {
        //        return 
        //            string.IsNullOrWhiteSpace(CityName)?$"{CompanyName}":$"{ CompanyName} - {CityName}"; }
        //}

        public string HiringFor { get; set; }
        public string PreferredLocation { get; set; }
        public string PreferredLocation1 { get; set; }
        public string PreferredLocation2 { get; set; }
        public string PreferredLocation3 { get; set; }
        public Skills Skills { get; set; }
        public double TotalExperience { get; set; } = 0.0;
        public string LinkedinProfile { get; set; }

        public string ReturnUrl { get; set; }
        public IList<AuthenticationScheme> SocialLogins { get; set; }
        public string EmploymentStatusName { get; set; }
        public bool IsJobAlert { get; set; }
        public int ProfileScore { get; set; }
    }
}
