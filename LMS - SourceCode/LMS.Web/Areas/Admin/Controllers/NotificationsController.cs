using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMS.Business.Interfaces.Admin;
using LMS.Model.DataViewModel.Admin.Notifications;
using LMS.Model.DataViewModel.Shared;
using LMS.Utility.Exceptions;
using LMS.Utility.ExtendedMethods;
using LMS.Utility.Helpers;
using LMS.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("[controller]")]
    [HandleExceptionsAttribute]
    [UserAuthentication(Constants.AdminRole + "," + Constants.DemandAggregationRole)]
    public class NotificationsController : Controller
    {
        private readonly INotificationHandler notificationHandler;

        public NotificationsController(INotificationHandler _notificationHandler)
        {
            notificationHandler = _notificationHandler;
        }

        [Route("[action]")]
        public JsonResult GetNotificationsCounter()
        {
            var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
            user = user ?? new UserViewModel();
            NotificationsViewModel counts = null;
            try
            {
                counts=notificationHandler.GetNotificationsCounter();
                return new JsonResult(new { counts });
            }

            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(NotificationsController), ex);
                counts = new NotificationsViewModel();
            }
            return new JsonResult(new { counts });
        }
    }
}