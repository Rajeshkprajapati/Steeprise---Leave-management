using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Data.DataModel.Shared
{
    public class SearchResumeModel
    {
        public string Skills { get; set; }
        public string JobCategory { get; set; }
        public int MinExp { get; set; } 
        public int MaxExp { get; set; }
        public string City { get; set; }
    }
}
