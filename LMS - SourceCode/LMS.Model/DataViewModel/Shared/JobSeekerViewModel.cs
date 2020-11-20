using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LMS.Model.DataViewModel.Shared
{
    public class JobSeekerViewModel
    {
        public int UserId { get; set; }

        public int RoleId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string FullName { get { return $"{FirstName} {LastName}"; } }

        [Required]
        public string MobileNo { get; set; }

        public string UserName { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

       
        public string Industry { get; set; }
                
        public string Address { get; set; }
                
        public string PasswordExpirayDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ActiveFrom { get; set; }
        
        
        public string PreferredLocation { get; set; }

        
        public string Skills { get; set; }

        
        public double TotalExperience { get; set; }        

    }
}
