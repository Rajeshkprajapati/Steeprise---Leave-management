using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Data.DataModel.JobSeeker
{
    public class ExperienceDetails
    {
        public int Id { get; set; }
        public string Designation { get; set; }
        public string Organization { get; set; }
        public string AnnualSalary { get; set; }
        public string WorkingFrom { get; set; }
        public string WorkingTill { get; set; }
        public string WorkLocation { get; set; }
        public string NoticePeriod { get; set; }
        public bool ServingNoticePeriod { get; set; }
        public string Industry { get; set; }
        public string JobProfile { get; set; }
        public bool IsCurrentOrganization { get; set; }
        public Skills Skills { get; set; }
    }
}
