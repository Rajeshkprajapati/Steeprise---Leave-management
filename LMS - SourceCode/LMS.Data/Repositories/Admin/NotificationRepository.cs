using LMS.Data.Helper;
using LMS.Data.Interfaces.Admin;
using LMS.Utility.Exceptions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace LMS.Data.Repositories.Admin
{
    public class NotificationRepository: INotificationRepository
    {
        private readonly string connectionString;
        public NotificationRepository(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionStrings:NassComLMS.B"];
        }

        public DataSet GetNotificationsCounter()
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
                            "usp_GetNotificationsCounter"
                            );
                    if (null != result)
                    {
                        return result;
                    }
                }
                finally
                {
                    SqlHelper.CloseConnection(connection);
                }
            }
            throw new DataNotFound("Notifications not found to display.");
        }
    }
}
