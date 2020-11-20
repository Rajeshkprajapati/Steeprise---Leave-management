using LMS.Data.DataModel.Shared;
using LMS.Data.Helper;
using LMS.Data.Interfaces.Admin;
using LMS.Model.DataViewModel.Shared;
using LMS.Utility.Exceptions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace LMS.Data.Repositories.Admin
{
    public class ManageCityStateRepository : IManageCityStateRepository
    {
        private readonly string connectionString;

        public ManageCityStateRepository(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionStrings:NassComLMS.B"];

        }

        public DataTable GetAllState()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    var result =
                        SqlHelper.ExecuteDataset
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetAllStates"
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
            throw new DataNotFound("State data not found");
        }

        public bool AddCity(CityModel city)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                       new SqlParameter("@cityCode", city.CityCode),
                       new SqlParameter("@stateCode", city.StateCode),
                       new SqlParameter("@city", city.City),
                    };
                    var result =
                        SqlHelper.ExecuteNonQuery
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_AddCity",
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

        public bool DeleteCity(string citycode, string statecode)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                       new SqlParameter("@cityCode", citycode),
                       new SqlParameter("@stateCode", statecode),
                    };
                    var result =
                        SqlHelper.ExecuteNonQuery
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_DeleteCity",
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
            throw new DataNotFound("City data not found");
        }

        public bool UpdateCity(CityModel model)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                       new SqlParameter("@cityCode", model.CityCode),
                       new SqlParameter("@city", model.City),
                       new SqlParameter("@stateCode", model.StateCode),
                    };
                    var result =
                        SqlHelper.ExecuteNonQuery
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_UpdateCity",
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
            throw new DataNotFound("City data not found");
        }
        public bool InsertStateList(StateViewModel stateViewModel)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@countryCode",stateViewModel.CountryCode),
                    new SqlParameter("@stateCode",stateViewModel.StateCode),
                    new SqlParameter("@stateName",stateViewModel.State),
                    };
                    var data =
                        SqlHelper.ExecuteNonQuery
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_InsertStateDetails",
                            parameters
                            );
                    if (data > 0)
                    {
                        return true;
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotUpdatedException("Unable to insert data");
        }

        public bool UpdateStateList(StateViewModel stateViewModel)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@countryCode",stateViewModel.CountryCode),
                    new SqlParameter("@stateCode",stateViewModel.StateCode),
                    new SqlParameter("@stateName",stateViewModel.State),
                    };
                    var data =
                        SqlHelper.ExecuteNonQuery
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_UpdateState",
                            parameters
                            );
                    if (data > 0)
                    {
                        return true;
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotUpdatedException("Unable to update data");
        }

        public bool DeleteStateList(StateViewModel stateViewModel)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@countryCode",stateViewModel.CountryCode),
                    new SqlParameter("@stateCode",stateViewModel.StateCode),
                    };
                    var data =
                       SqlHelper.ExecuteNonQuery
                       (
                           connection,
                           CommandType.StoredProcedure,
                           "usp_DeleteState",
                           parameters
                           );
                    if (data > 0)
                    {
                        return true;
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotUpdatedException("Unable to delete data");
        }

        public bool CheckIfStateCodeExist(string stateCode)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                        new SqlParameter("@stateCode",stateCode)
                    };
                    var result =
                        SqlHelper.ExecuteReader
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_CheckIfStateCodeExist",
                            parameters
                            );
                    if (result != null && result.HasRows)
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
    }
}
