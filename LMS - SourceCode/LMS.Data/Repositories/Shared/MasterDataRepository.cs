using LMS.Data.Helper;
using LMS.Data.Interfaces.Shared;
using LMS.Utility.Exceptions;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LMS.Data.Repositories.Shared
{
    public class MasterDataRepository : IMasterDataRepository
    {
        private readonly string connectionString;
        public MasterDataRepository(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionStrings:NassComLMS.B"];
        }

        public DataTable GetStates(string country)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {

                    SqlParameter[] parameters = new SqlParameter[] {
                        new SqlParameter("@countryCode",country)
                    };
                    var result =
                        SqlHelper.ExecuteDataset
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetStates",
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
            throw new DataNotFound("States not found, please contact your tech deck.");
        }

        public DataTable GetEmployers(int? empId, bool isAll)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {

                    SqlParameter[] parameters = new SqlParameter[] {
                       new SqlParameter("@EmpId",empId),
                       new SqlParameter("@IsAll",isAll)

                    };
                    var result =
                        SqlHelper.ExecuteDataset
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetEmployers",
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
            throw new DataNotFound("Employers not found, please contact your tech deck.");
        }

        public DataTable GetCities(string state)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {

                    SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@stateCode",state)
            };
                    var result =
                        SqlHelper.ExecuteDataset
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetCities",
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
            throw new DataNotFound("Cities not found, please contact your tech deck.");
        }

        public DataRow GetCityByCode(string cityCode)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {

                    SqlParameter[] parameters = new SqlParameter[] {
                        new SqlParameter("@cityCode",cityCode)
                    };
                    var result =
                        SqlHelper.ExecuteDataset
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetCityByCityCode",
                            parameters
                            );
                    if (null != result && result.Tables.Count > 0)
                    {
                        return result.Tables[0].Rows[0];
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            return null;
        }

        public DataTable GetCoursesById(int courseid)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                        new SqlParameter("@courseid",courseid)
                    };
                    var result =
                        SqlHelper.ExecuteDataset
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetCourseNameBycourseId",
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
            throw new DataNotFound("Course not found, please contact your tech deck.");
        }

        public DataTable GetJobRoles(int roleId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {

                    SqlParameter[] parameters = new SqlParameter[] {
                        new SqlParameter("@roleId",roleId)
                    };
                    var result =
                        SqlHelper.ExecuteDataset
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetJobRoles",
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
            throw new DataNotFound("Job roles not found, please contact your tech deck.");
        }

        public DataTable GetCourses(int cCategory)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                        new SqlParameter("@CategoryId",cCategory)
                    };
                    var result =
                        SqlHelper.ExecuteDataset
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetCourses",
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

            throw new DataNotFound("Courses not found, please contact your tech deck.");
        }

        public DataTable GetCourseType()
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
                            "usp_GetCourseTypeMaster"
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
            throw new DataNotFound("Courses types not found, please contact your tech deck.");
        }

        public DataTable GetCoursesCategory()
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
                            "usp_GetCourseCategories"
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
            throw new DataNotFound("Courses Category not found, please contact your tech deck.");
        }

        public DataTable GetCountries()
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
                            "usp_GetCountries"
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
            throw new DataNotFound("Countries not found, please contact your tech deck.");
        }

        public DataTable GetJobTypes()
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
                            "usp_GetJobTypes"
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
            throw new DataNotFound("Job types not found, please contact your tech deck.");
        }

        public DataTable GetAllCitiesWithoutState()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    //SqlParameter[] parameters = new SqlParameter[] {
                    //   new SqlParameter("@cityFirstChar", cityFirstChar)
                    //};
                    var city =
                        SqlHelper.ExecuteDataset
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetAllCitiesWithoutState"
                            );
                    if (null != city && city.Tables.Count > 0)
                    {
                        return city.Tables[0];
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("No City Found");
        }

        public DataTable GetAllGender(bool withAll)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[]
                    {
                        new SqlParameter("@IsAll",withAll)
                    };
                    var genders =
                        SqlHelper.ExecuteDataset
                        (
                            connection,
                        CommandType.StoredProcedure,
                        "usp_GetGenderMaster",
                        parameters
                           );
                    if (null != genders && genders.Tables.Count > 0)
                    {
                        return genders.Tables[0];
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("No data found");
        }

        public DataTable GetMaritalStatusMaster()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {

                    var maritalStatus =
                        SqlHelper.ExecuteDataset
                        (
                            connection,
                        CommandType.StoredProcedure,
                        "usp_GetMaritalStatusMaster"
                           );
                    if (null != maritalStatus && maritalStatus.Tables.Count > 0)
                    {
                        return maritalStatus.Tables[0];
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("No data found");
        }
    }
}
