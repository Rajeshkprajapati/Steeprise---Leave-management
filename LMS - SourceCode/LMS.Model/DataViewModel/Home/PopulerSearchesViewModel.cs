using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Model.DataViewModel.Home
{
  public class PopulerSearchesViewModel
    {
        public string JobIndustryAreaName { get; set; }
        public int JobIndustryAreaId { get; set; }
        public string City { get; set; }
        public string CityCode { get; set; }
        public string TotalCount { get; set; }
        public string CompanyName { get; set; }
        public int UserId { get; set; }
    }
}
