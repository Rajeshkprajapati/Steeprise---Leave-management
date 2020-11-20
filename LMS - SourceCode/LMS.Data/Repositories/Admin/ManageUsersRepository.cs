using LMS.Data.DataModel.Admin.ManageUsers;
using LMS.Data.Helper;
using LMS.Data.Interfaces.Admin;
using LMS.Utility.Exceptions;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;

namespace LMS.Data.Repositories.Admin
{
    public class ManageUsersRepository : IManageUserRepository
    {
        private readonly string connectionString;
        public ManageUsersRepository(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionStrings:NassComLMS.B"];
        }


        public DataTable GetAllUsersList(int userId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[]
                    {
                        new SqlParameter("@userId",userId)
                    };
                    var allusers =
                        SqlHelper.ExecuteReader
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetAllUsers",
                            parameters
                            );
                    if (null != allusers && allusers.HasRows)
                    {
                        var dt = new DataTable();
                        dt.Load(allusers);
                        return dt;
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("Users not found");
        }

        public DataTable GetAppliedJobsInRange(string startDate, string endDate)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@StartDate",startDate),
                    new SqlParameter("@EndDate",endDate),
                    };
                    var jobs =
                        SqlHelper.ExecuteDataset
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetAppliedJobsInDateRange",
                            parameters
                            );
                    if (null != jobs && jobs.Tables.Count>0)
                    {
                        return jobs.Tables[0];
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("Data not found");
        }

        public DataTable GetAllUserRegistrations(string registrationType, string sDate, string eDate)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@registrationType",registrationType),
                    new SqlParameter("@sDate",sDate),
                    new SqlParameter("@eDate",eDate)
                    };
                    var allusers =
                        SqlHelper.ExecuteDataset
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetAllUsersRegistrations",
                            parameters
                            );
                    if (null != allusers && allusers.Tables.Count>0)
                    {
                        return allusers.Tables[0];
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("Data not found");
        }

        public DataTable GetJobsInDateRange(string startDay, string endDay)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@StartDate",startDay),
                    new SqlParameter("@EndDate",endDay)
                    };
                    var jobs =
                        SqlHelper.ExecuteDataset
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetJobsInDateRange",
                            parameters
                            );
                    if (null != jobs && jobs.Tables.Count>0)
                    {
                        return jobs.Tables[0];
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("Data not found");
        }

        public DataTable JobPostMonthlyStateWiseRecord(string month, string year, string state)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@Month",month),
                    new SqlParameter("@Year",year),
                    new SqlParameter("@State",state),
                    };
                    var allusers =
                        SqlHelper.ExecuteReader
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetJobPostMonthlyStateWise",
                            parameters
                            );
                    if (null != allusers && allusers.HasRows)
                    {
                        var dt = new DataTable();
                        dt.Load(allusers);
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

        public DataTable MonthlyAppliedJobs(int month, int year, string gender, string state)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                        new SqlParameter("@Month", month),
                        new SqlParameter("@Year", year),
                        new SqlParameter("@Gender", gender),
                        new SqlParameter("@State", state)
                    };
                    var jobs =
                        SqlHelper.ExecuteDataset
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetMonthlyAppliedJobs",
                            parameters
                            );
                    if (null != jobs && jobs.Tables.Count>0)
                    {
                        return jobs.Tables[0];
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("Data not found");
        }

        public DataTable MonthlyRegisteredUsers(int month, int year, string state, string gender)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                        new SqlParameter("@Month", month),
                        new SqlParameter("@Year", year),
                        new SqlParameter("@State", state),
                        new SqlParameter("@Gender", gender)
                    };
                    var jobs =
                        SqlHelper.ExecuteDataset
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetMonthlyRegisteredUsers",
                            parameters
                            );
                    if (null != jobs && jobs.Tables.Count > 0)
                    {
                        return jobs.Tables[0];
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("Data not found");
        }

        public bool DeleteUsersById(string userid)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@userid",userid)
                    };
                    var users =
                        SqlHelper.ExecuteNonQuery
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_DeleteUserById",
                            parameters
                            );
                    if (users > 0)
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

        public DataSet DashboardTilesRecordCount(string date, string endDate)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[]
                    {
                        new SqlParameter("@Date", date),
                        new SqlParameter("@EndDate", endDate)
                    };
                    var ds = SqlHelper.ExecuteDataset(
                        connection,
                        CommandType.StoredProcedure,
                        "usp_AdminDashboardStats",
                        parameters
                        );
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        return ds;
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("Data not found");
        }

        public DataTable MonthlyJobs(int month, int year, string state, bool activeJobs)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[]
                    {
                        new SqlParameter("@Month", month),
                        new SqlParameter("@Year", year),
                        new SqlParameter("@State", state),
                        new SqlParameter("@ActiveJobs",activeJobs)
                    };

                    var ds = SqlHelper.ExecuteDataset(
                        connection,
                        CommandType.StoredProcedure,
                        "usp_GetMonthlyJobs",
                        parameters
                        );
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        return ds.Tables[0];
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("Data not found");
        }

        public DataSet GetGraphData(int year, string gender, string state)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[]
                    {
                        new SqlParameter("@Year", year),
                        new SqlParameter("@Gender", gender),
                        new SqlParameter("@State", state)
                    };

                    var ds = SqlHelper.ExecuteDataset(
                        connection,
                        CommandType.StoredProcedure,
                        "usp_GetAdminGraphData",
                        parameters
                        );
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        return ds;
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("Data not found");
        }

        public bool UpdateUsersData(ManageUsersModel user)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@id",user.Userid),
                    new SqlParameter("@firstname",user.FirstName),
                    new SqlParameter("@lastname",user.LastName),
                    //new SqlParameter("@email",user.Email),
                    //new SqlParameter("@Roleid",user.RoleId),
                    //new SqlParameter("@psd",user.Password)
                    };
                    var users =
                        SqlHelper.ExecuteNonQuery
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_UpdateUserData",
                            parameters
                            );
                    if (users > 0)
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

        public bool ApproveUser(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@userid",id),
                    };
                    var users =
                        SqlHelper.ExecuteNonQuery
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_ApproveUser",
                            parameters
                            );
                    if (users > 0)
                    {
                        return true;
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("Unable to Approve user");
        }

        public DataTable GetBulkJobSearchList(int CompanyId, string FY, string statecode, string citycode)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@CompanyId",CompanyId),
                    new SqlParameter("@FY",FY),
                    new SqlParameter("@statecode",statecode),
                    new SqlParameter("@citycode",citycode),
                    };
                    var jobs =
                        SqlHelper.ExecuteDataset
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_SearchBulkJobList",
                            parameters
                            );
                    if (null != jobs && jobs.Tables.Count > 0)
                    {
                        return jobs.Tables[0];
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("Data not found");
        }

        public bool DeleteBulkJobPost(string JobPostId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@JobPostId",JobPostId)
                    };
                    var data =
                       SqlHelper.ExecuteNonQuery
                       (
                           connection,
                           CommandType.StoredProcedure,
                           "DeleteBulkJob",
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
    }
}
