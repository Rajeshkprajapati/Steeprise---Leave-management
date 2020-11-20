using LMS.Model.DataViewModel.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Model.DataViewModel.Employer.Dashboard
{
    public class DashboardSummary
    {
        public int TotalProfileViewes { get; set; }
        public int TotalResumeList { get; set; }
        public int TotalMessages{ get; set; }
        public int RespondTime { get; set; }
    }
}
