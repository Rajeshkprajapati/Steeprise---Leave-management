using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Model.DataViewModel.Shared
{
    //public class SearchViewModel
    //{
    //    public string Skill { get; set; }
    //    public string JobTitle { get; set; }
    //    public string[] JobCategory { get; set; }
    //    public string Experiance { get; set; }
    //    public string[] City { get; set; }
    //}

    public class SearchResumeViewModel
    {
        public string Skills { get; set; } = string.Empty;
        public string JobRoles { get; set; } = string.Empty;
        public string[] JobCategory { get; set; } = new string[0];
        public int MinExp { get; set; } = 0;
        public int MaxExp { get; set; } = 0;
        public string[] City { get; set; } = new string[0];
    }
}
