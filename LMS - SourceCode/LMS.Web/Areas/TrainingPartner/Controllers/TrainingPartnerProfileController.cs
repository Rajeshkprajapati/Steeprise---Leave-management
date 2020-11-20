using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using LMS.Model.DataViewModel.Shared;
using LMS.Utility.Helpers;
using LMS.Web.Filters;
using LMS.Business.Interfaces.TrainingPartner;
using LMS.Utility.Exceptions;
using Microsoft.AspNetCore.Mvc;
using LMS.Utility.ExtendedMethods;
using Microsoft.AspNetCore.Http;

namespace LMS.Web.Areas.TrainingPartner.Controllers
{
    [Area("TrainingPartner")]
    [Route("[controller]")]
    [UserAuthentication(Constants.TrainingPartnerRole)]
    public class TrainingPartnerProfileController : Controller
    {
        private readonly ITrainingPartnerProfileHandler _trainPartnerHandler;
        public TrainingPartnerProfileController(ITrainingPartnerProfileHandler trainingPartnerProfileHandler)
        {
            _trainPartnerHandler = trainingPartnerProfileHandler;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Route("[action]")]
        public ViewResult EditProfile()
        {
            UserViewModel tpUser = null;
            var user = HttpContext.Session.Get<UserViewModel>(Constants.SessionKeyUserInfo);
            user = user ?? new UserViewModel();
            try
            {
                tpUser = _trainPartnerHandler.GetTPDetail(user.UserId);
                return View(tpUser);
            }
            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(TrainingPartnerProfileController), ex);
            }

            return View();
        }


        [HttpPost]
        [Route("[action]")]
        public IActionResult UpdateTPDetail(UserViewModel user)
        {
            string msg = "Profile has been updated successfully.";
            try
            {
                _trainPartnerHandler.UpdateTPDetail(user);
            }
            catch (DataNotUpdatedException ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, user.UserId, typeof(TrainingPartnerProfileController), ex);
                msg = "Some issue occurred while updating profile, please contact your tech deck.";
            }
            ModelState.AddModelError("ResponseMessage", msg);
            return View("EditProfile",user);
        }
    }
}