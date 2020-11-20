using LMS.Data.DataModel.Shared;
using LMS.Data.Helper;
using LMS.Data.Interfaces.Employer.Profile;
using LMS.Model.DataViewModel.Shared;
using LMS.Utility.Exceptions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace LMS.Data.Repositories.Employer.Profile
{
    public class EmpProfileRepository : IEmpProfileRepository
    {
        private readonly string connectionString;

        public EmpProfileRepository(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionStrings:NassComLMS.B"];
        }
        public DataTable GetEmpUserDetails(int userId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {

                    SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@EmpId",userId)
            };
                    var country =
                        SqlHelper.ExecuteDataset
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_GetEmployers",
                             parameters
                            );
                    if (null != country && country.Tables.Count>0)
                    {
                        return country.Tables[0]; ;
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("Unable to find data.");
        }
        public bool InsertUpdateEmpDetails(UserModel model)
        {
            using (var connection = new SqlConnection(connectionString))
            {

                try
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@companyName",model.CompanyName),
                new SqlParameter("@contactPerson",model.FirstName),
                //new SqlParameter("@email",model.Email),
                new SqlParameter("@phone",model.MobileNo),
                new SqlParameter("@address",model.Address1),
                new SqlParameter("@profile",model.ProfilePic),
                new SqlParameter("@userId",model.UserId),
                new SqlParameter("@Gender",model.Gender),
                new SqlParameter("@MaritalStatus",model.MaritalStatus)
            };
                    var result =
                        SqlHelper.ExecuteNonQuery
                        (
                            connection,
                            CommandType.StoredProcedure,
                            "usp_InsertEmpDetails",
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
            throw new UserNotCreatedException("Unable to create profile summary, please contact your teck deck with your details.");
        }
    }
}
