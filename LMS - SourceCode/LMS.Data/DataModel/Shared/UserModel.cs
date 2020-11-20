using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Data.DataModel.Shared
{
    public class UserModel
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int RoleId { get; set; }
        public string BatchNumber { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string MaritalStatus { get; set; }
        public string ProfilePic { get; set; }
        public string Gender { get; set; }
        public string DOB { get; set; }
        public string CandidateId { get; set; }
        public string CompanyName { get; set; }
        public string ContactPerson { get; set; }
        public string AboutMe { get; set; }
        public string JobIndustryArea { get; set; }
        public string EmploymentStatus { get; set; }
        public string CTC { get; set; }
        public string ECTC { get; set; }
        public int JobTitleId { get; set; }
        public int CreatedBy { get; set; }
        public double TotalExperience { get; set; }
        public bool IsActive { get; set; } = true;
        public string ActivationKey { get; set; }
        public string LinkedinProfile { get; set; }
        public bool IsApproved { get; set; }
    }
    public class RolesModel
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsEmp { get; set; }
    }
    public class CreateNewPasswordModel
    {

        public string Email { get; set; }
        public string OldPassword { get; set; }
        public string Password { get; set; }

    }
}
