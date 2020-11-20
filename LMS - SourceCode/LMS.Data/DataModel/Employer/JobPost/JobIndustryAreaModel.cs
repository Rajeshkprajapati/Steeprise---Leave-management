using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Data.DataModel.Employer.JobPost
{
    public class JobIndustryAreaModel
    {
        public int JobIndustryAreaId { get; set; }
        public string JobIndustryAreaName { get; set; }
        public bool Status { get; set; }
        public int CountValue { get; set; }
    }
}
