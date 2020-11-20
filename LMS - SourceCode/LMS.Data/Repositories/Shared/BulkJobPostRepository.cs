using LMS.Data.DataModel.Shared;
using LMS.Data.Helper;
using LMS.Data.Interfaces.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Data.SqlClient;

namespace LMS.Data.Repositories.Shared
{
    public class BulkJobPostRepository : IBulkJobPostRepository
    {
        private readonly string connectionString;
        public BulkJobPostRepository(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionStrings:NassComLMS.B"];
        }

        public DataTable GetIdFromValue(string value, string valueFor)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {

                    SqlParameter[] parameters = new SqlParameter[] {
                        new SqlParameter("@value",value),
                        new SqlParameter("@valueFor",valueFor)
                    };
                    var result =
                        SqlHelper.ExecuteDataset
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetIdForValue",
                            parameters
                            );
                    if (null != result && result.Tables.Count > 0)
                    {
                        return result.Tables[0];
                    }
                }
                catch(Exception ex)
                {
                    
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            return null;
        }

        public bool SaveDetailToAudit(BulkJobPostSummaryDetail detail)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {

                    SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@CompanyName",detail.CompanyName),
                new SqlParameter("@CreatedBy",detail.CreatedBy),
                new SqlParameter("@CTC",detail.CTC),
                new SqlParameter("@ErrorDetails",detail.ErrorDetails),
                new SqlParameter("@FileName",detail.FileName),
                //new SqlParameter("@FinancialYear",detail.FinancialYear),
                new SqlParameter("@HiringCriteria",detail.HiringCriteria),
                new SqlParameter("@JobDetails",detail.JobDetails),
                new SqlParameter("@JobLocation",detail.JobLocation),
                //new SqlParameter("@JobRole1",detail.JobRole1),
                //new SqlParameter("@JobRole2",detail.JobRole2),
                //new SqlParameter("@JobRole3",detail.JobRole3),
                new SqlParameter("@JobTitle",detail.JobTitle),
                new SqlParameter("@JobType",detail.JobType),
                new SqlParameter("@MaxExp",detail.MaxExp),
                new SqlParameter("@MinExp",detail.MinExp),
                new SqlParameter("@ProcessedBy",detail.ProcessedBy),
                new SqlParameter("@ProcessedOn",detail.ProcessedOn),
                //new SqlParameter("@Quarter1",detail.Quarter1),
                //new SqlParameter("@Quarter2",detail.Quarter2),
                //new SqlParameter("@Quarter3",detail.Quarter3),
                //new SqlParameter("@Quarter4",detail.Quarter4),
                new SqlParameter("@SerialNo",detail.SerialNo),
                new SqlParameter("@SPOC",detail.SPOC),
                new SqlParameter("@SPOCContact",detail.SPOCContact),
                new SqlParameter("@SPOCEmail",detail.SPOCEmail),
                new SqlParameter("@State",detail.State),
                new SqlParameter("@Status",detail.Status),
                new SqlParameter("@Total",detail.Total)
            };
                    var result =
                        SqlHelper.ExecuteNonQuery
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_InsertBulkJobPostSummaryDetail",
                            parameters
                            );
                    if (result > 0)
                    {
                        return true;
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            return false;
        }

        public bool InsertCity(ref CityModel city)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    var outParam = new SqlParameter
                    {
                        ParameterName = "@CityCode",
                        Direction = ParameterDirection.Output,
                        SqlDbType = SqlDbType.VarChar,
                        Size=15
                    };

                    SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@CityName",city.City),
                    new SqlParameter("@StateCode",city.StateCode),
                    outParam
                    };
                    var result =
                        SqlHelper.ExecuteNonQuery
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_InsertCity",
                            parameters
                            );
                    if (result > 0)
                    {
                        city.CityCode = Convert.ToString(outParam.Value);
                        return true;
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            return false;
        }

        public bool InsertState(ref StateModel state)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    var outParam = new SqlParameter
                    {
                        ParameterName = "@StateCode",
                        Direction = ParameterDirection.Output,
                        SqlDbType = SqlDbType.VarChar,
                        Size=5
                    };

                    SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@StateName",state.State),
                    new SqlParameter("@CountryCode",state.CountryCode),
                    outParam
                    };
                    var result =
                        SqlHelper.ExecuteNonQuery
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_InsertState",
                            parameters
                            );
                    if (result > 0)
                    {
                        state.StateCode= Convert.ToString(outParam.Value);
                        return true;
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            return false;
        }
    }
}
