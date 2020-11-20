using LMS.Data.DataModel.Admin.Designation;
using LMS.Data.Helper;
using LMS.Data.Interfaces.Admin;
using LMS.Utility.Exceptions;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LMS.Data.Repositories.Admin
{
    public class DesignationRepository : IDesignationRepository
    {
        private readonly string connectionString;

        public DesignationRepository(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionStrings:NassComLMS.B"];
        }

        public DataTable GetDesignationList()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    var Data =
                        SqlHelper.ExecuteReader
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetDesignationList"
                        );
                    if (null != Data && Data.HasRows)
                    {
                        var dt = new DataTable();
                        dt.Load(Data);
                        return dt;
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }

            throw new DataNotFound("Data not found");

        }

        private bool CheckDesignationExist(DesignationModel designationModel)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@designation",designationModel.Designation),
                    new SqlParameter("@abbr",designationModel.Abbrivation),
                    };
                    var data =
                       SqlHelper.ExecuteReader
                       (
                           connection,
                           CommandType.StoredProcedure,
                           "usp_CheckDesignationExist",
                           parameters
                           );
                    if (data != null && data.HasRows)
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

        public bool AddDesignation(DesignationModel designationModel)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@designation",designationModel.Designation),
                    new SqlParameter("@abbr",designationModel.Abbrivation),
                    };
                    var data =
                       SqlHelper.ExecuteNonQuery
                       (
                           connection,
                           CommandType.StoredProcedure,
                           "usp_AddDesignation",
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
            throw new DataNotFound("Unable to Add Designation");
        }

        public bool UpdateDesignation(DesignationModel designationModel)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@id",designationModel.DesignationId),
                    new SqlParameter("@designation",designationModel.Designation),
                    new SqlParameter("@abbr",designationModel.Abbrivation),
                    };
                    var data =
                       SqlHelper.ExecuteNonQuery
                       (
                           connection,
                           CommandType.StoredProcedure,
                           "usp_UpdateDesignation",
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
            throw new DataNotFound("Unable to Update Designation");
        }

        public bool DeleteDesignation(int designationId)
        {
            using(var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@id",designationId),
                    };
                    var data =
                       SqlHelper.ExecuteNonQuery
                       (
                           connection,
                           CommandType.StoredProcedure,
                           "usp_DeleteDesignation",
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
            throw new DataNotFound("Unable to Delete Designation");
        }
    }
}
