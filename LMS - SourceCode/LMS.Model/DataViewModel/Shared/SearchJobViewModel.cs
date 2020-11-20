using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Model.DataViewModel.Shared
{
    public class SearchJobViewModel
    {
        public string Skills { get; set; } = string.Empty;
        public int JobTitle { get; set; } = 0;
        public string[] JobCategory { get; set; } = new string[0];
        public int Experiance { get; set; } = -1;
        public string[] City { get; set; } = new string[0];
        public string[] CompanyUserId { get; set; } = new string[0];
    }
}
