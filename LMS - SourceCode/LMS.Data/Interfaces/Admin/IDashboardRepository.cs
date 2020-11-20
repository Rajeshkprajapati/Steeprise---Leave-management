using LMS.Data.DataModel.Admin.Dashboard;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LMS.Data.Interfaces.Admin
{
    public interface IDashboardRepository
    {
        DataTable GetDemandAggregationDataOnQuarter(DemandAggregationSearchFilters filters);
        DataTable GetDemandAggregationDataOnJobRole(DemandAggregationSearchFilters filters);
        DataTable GetDemandAggregationOnState(DemandAggregationSearchFilters filters);
        DataTable ViewDemandAggregationDetails(string onBasis, string value, DemandAggregationSearchFilters filters);
        DataTable GetDemandAggregationOnEmployer(DemandAggregationSearchFilters filters);
        DataTable GetDemandAggregationReportData(DemandAggregationSearchFilters filters);
    }
}
