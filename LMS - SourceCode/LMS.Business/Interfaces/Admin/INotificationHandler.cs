using LMS.Model.DataViewModel.Admin.Notifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Business.Interfaces.Admin
{
    public interface INotificationHandler
    {
        NotificationsViewModel GetNotificationsCounter();
    }
}
