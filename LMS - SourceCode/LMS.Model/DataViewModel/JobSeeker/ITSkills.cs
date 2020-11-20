using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Model.DataViewModel.JobSeeker
{
   public class ITSkills
    {
        public int Id { get; set; }
        public string Skill { get; set; }
        public string SkillVersion { get; set; }
        public string LastUsed { get; set; }
        public string ExperienceYear { get; set; }
        public string ExperienceMonth { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
