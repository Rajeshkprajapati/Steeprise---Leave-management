using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Data.DataModel.Admin.ManageUsers
{
    public class ManageUsersModel
    {
        public int Userid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string MobileNo { get; set; }
        public string Country { get; set; }
        public string MaritalStatus { get; set; }
        public string ProfilePic { get; set; }
        public int CreatedBy { get; set; }
        public int IsApproved { get; set; }
        public string RoleName { get; set; }
        public int RoleId { get; set; }
    }
}
