using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Model.DataViewModel.Admin.Dashboard
{
    public class DemandAggregationOnStatesViewModel
    {
        public string StateCode { get; set; }
        public string State { get; set; }
        public int Year { get; set; }
        public int Month{ get; set; }
        public DemandAggregationDataViewModel DemandAggregations { get; set; }
    }
}
