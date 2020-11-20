using LMS.Model.DataViewModel.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Model.DataViewModel.JobSeeker
{
    public class UserDetail
    {
        public ExperienceDetails[] ExperienceDetails { get; set; }
        public EducationalDetails[] EducationalDetails { get; set; }
        public Skills Skills { get; set; }
        public UserViewModel PersonalDetails {get;set;}
        public List<CityViewModel> Cities { get; set; }
        public List<ITSkills> ITSkills { get; set; }
    }
}
