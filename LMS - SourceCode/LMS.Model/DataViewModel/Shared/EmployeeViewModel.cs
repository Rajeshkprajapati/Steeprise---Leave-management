using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LMS.Model.DataViewModel.Shared
{
    public class EmployeeViewModel
    {
        [Required]
        public string CompanyName { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",ErrorMessage ="Invalid email format")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [StringLength(10)]
        public string Mobile { get; set; }
             
        public string Industry { get; set; }

        public int RoleId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

    }
}
