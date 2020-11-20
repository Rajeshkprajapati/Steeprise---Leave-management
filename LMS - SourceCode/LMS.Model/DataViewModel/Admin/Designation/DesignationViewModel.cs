using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Model.DataViewModel.Admin.Designation
{
    public class DesignationViewModel
    {
        public int DesignationId { get; set; }
        public string Designation { get; set; }
        public string Abbrivation { get; set; }
        public int IsActive { get; set; }
    }
}
