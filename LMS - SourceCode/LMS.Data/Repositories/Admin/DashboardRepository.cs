using LMS.Data.DataModel.Admin.Dashboard;
using LMS.Data.Helper;
using LMS.Data.Interfaces.Admin;
using LMS.Utility.Exceptions;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LMS.Data.Repositories.Admin
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly string connectionString;
        public DashboardRepository(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionStrings:NassComLMS.B"];
        }

        public DataTable GetDemandAggregationDataOnQuarter(DemandAggregationSearchFilters filters)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@Year",filters.FinancialYear),
                    new SqlParameter("@userRole",filters.UserRole),
                     new SqlParameter("@states",filters.JobStates),
                     new SqlParameter("@employers",filters.Employers),
                     new SqlParameter("@jobRoles",filters.JobRoles)

                    };
                    var result =
                        SqlHelper.ExecuteDataset
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetDemandAggregationDashboardDataOnQuarter",
                            parameters
                            );
                    if (null != result && result.Tables.Count > 0)
                    {
                        return result.Tables[0];
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }

            throw new DataNotFound("Demand aggregation data not found, please contact your tech deck.");
        }

        public DataTable GetDemandAggregationDataOnJobRole(DemandAggregationSearchFilters filters)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@jobRoles",filters.JobRoles),
                    new SqlParameter("@year",filters.FinancialYear),
                    new SqlParameter("@userRole",filters.UserRole),
                    new SqlParameter("@employers",filters.Employers),
                    new SqlParameter("@jobStates",filters.JobStates)
                    };
                    var result =
                        SqlHelper.ExecuteDataset
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetDemandAggregationDashboardDataOnJobRole",
                            parameters
                            );
                    if (null != result && result.Tables.Count > 0)
                    {
                        return result.Tables[0];
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("Demand aggregation data for this job role not found, please contact your tech deck.");
        }

        public DataTable GetDemandAggregationOnState(DemandAggregationSearchFilters filters)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                     new SqlParameter("@states",filters.JobStates),
                     new SqlParameter("@year",filters.FinancialYear),
                     new SqlParameter("@userRole",filters.UserRole),
                     new SqlParameter("@employers",filters.Employers),
                     new SqlParameter("@jobRoles",filters.JobRoles)
                    };
                    var result =
                        SqlHelper.ExecuteDataset
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetDemandAggregationDashboardDataOnState",
                            parameters
                            );
                    if (null != result && result.Tables.Count > 0)
                    {
                        return result.Tables[0];
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("Demand aggregation data for this state not found, please contact your tech deck.");
        }

        public DataTable GetDemandAggregationOnEmployer(DemandAggregationSearchFilters filters)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@employers",filters.Employers),
                    new SqlParameter("@year",filters.FinancialYear),
                    new SqlParameter("@userRole",filters.UserRole),
                    new SqlParameter("@jobRoles",filters.JobRoles),
                    new SqlParameter("@jobStates",filters.JobStates)
                    };
                    var result =
                        SqlHelper.ExecuteDataset
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetDemandAggregationDashboardDataOnEmployer",
                            parameters
                            );
                    if (null != result && result.Tables.Count > 0)
                    {
                        return result.Tables[0];
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("Demand aggregation data for this employer not found, please contact your tech deck.");
        }

        public DataTable ViewDemandAggregationDetails(string onBasis, string value, DemandAggregationSearchFilters filters)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@employers",filters.Employers),
                    new SqlParameter("@year",filters.FinancialYear),
                    new SqlParameter("@userRole",filters.UserRole),
                    new SqlParameter("@jobRoles",filters.JobRoles),
                    new SqlParameter("@states",filters.JobStates),
                    new SqlParameter("@onBasis",onBasis),
                    new SqlParameter("@value",value)
                    };
                    var result =
                        SqlHelper.ExecuteDataset
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetDemandAggregationDashboardDetails",
                            parameters
                            );
                    if (null != result && result.Tables.Count > 0)
                    {
                        return result.Tables[0];
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("Demand aggregation details not found, please contact your tech deck.");
        }

        public DataTable GetDemandAggregationReportData(DemandAggregationSearchFilters filters)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@employers",filters.Employers),
                    new SqlParameter("@year",filters.FinancialYear),
                    new SqlParameter("@userRole",filters.UserRole),
                    new SqlParameter("@jobRoles",filters.JobRoles),
                    new SqlParameter("@states",filters.JobStates),
                    };
                    var result =
                        SqlHelper.ExecuteDataset
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetDemandAggregationDataToExport",
                            parameters
                            );
                    if (null != result && result.Tables.Count > 0)
                    {
                        return result.Tables[0];
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("Demand aggregation report data not found, please contact your tech deck.");
        }
    }
}
