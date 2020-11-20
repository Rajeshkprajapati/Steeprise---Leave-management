using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Model.DataViewModel.JobSeeker
{
    public class EducationalDetails
    {
        public int Id { get; set; }
        public string Qualification { get; set; }
        public string QualificationName { get; set; }
        public string Course { get; set; }
        public string CourseName { get; set; }
        public string OtherCourseName { get; set; }
        public string Specialization { get; set; }
        public string University { get; set; }
        public string CourseType { get; set; }
        public string PassingYear { get; set; }
        public string Percentage { get; set; }
    }
}
