using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Model.DataViewModel.Shared
{
    public class SearchViewModel
    {
        public string Skill { get; set; }
        public string JobTitle { get; set; }
        public string[] JobCategory { get; set; }
        public string Experiance { get; set; }
        public string[] City { get; set; }
    }
}
