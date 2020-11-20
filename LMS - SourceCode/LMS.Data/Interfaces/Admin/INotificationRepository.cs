using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LMS.Data.Interfaces.Admin
{
    public interface INotificationRepository
    {
        DataSet GetNotificationsCounter();
    }
}
