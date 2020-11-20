using LMS.Model.DataViewModel.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Model.DataViewModel.Employer.Dashboard
{
    public class JobSeekerViewModel
    {
        public string JobTitleByEmployer { get; set; }
        public string JobRoles { get; set; }
        public IList<UserViewModel> jobSeekers { get; set; }
    }
}
