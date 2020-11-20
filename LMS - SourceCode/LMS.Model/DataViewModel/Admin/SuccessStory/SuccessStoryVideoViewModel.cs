using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Model.DataViewModel.Admin.SuccessStory
{
   public class SuccessStoryVideoViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string VideoFile { get; set; }
        public string UpdatedBy { get; set; }
        public string Type { get; set; }
        public string SerialNo { get; set; }
        public string Video { get; set; }
        public DateTime CreatedDate { get; set; }
        public int DisplayOrder { get; set; }
    }
}
