using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Data.DataModel.Admin.Designation
{
    public class DesignationModel
    {
        public int DesignationId { get; set; }
        public string Designation { get; set; }
        public string Abbrivation { get; set; }
        public int IsActive { get; set; }
    }
}
