using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Model.DataViewModel.Admin.Dashboard
{
    public class DemandAggregationOnEmployersViewModel
    {
        public int EmployerId { get; set; }
        public string EmployerFName { get; set; }
        public string EmployerLName { get; set; }
        public string EmployerFullName { get { return $"{EmployerFName} {EmployerLName}"; } }
        public string Company { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public DemandAggregationDataViewModel DemandAggregations { get; set; }
    }
}
