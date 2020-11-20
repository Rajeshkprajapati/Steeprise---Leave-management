using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Model.DataViewModel.Home
{
   public class TopEmployerViewModel
    {
        public int UserId { get; set;}
        public int JobPostId { get; set;}
        public string CompanyName { get; set;}
        public string Logo { get; set;}
        public int JobseekerId { get; set;}
        public int Count { get; set;}
        public int FollowIsActive { get; set;}
    }
}
