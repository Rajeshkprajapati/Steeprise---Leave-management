using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Model.DataViewModel.Admin.Dashboard
{
    public class DemandAggregationDataOnQuarterViewModel
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public DemandAggregationDataViewModel DemandAggregations { get; set; }
    }
}
