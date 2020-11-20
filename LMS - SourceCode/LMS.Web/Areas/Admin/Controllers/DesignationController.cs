using LMS.Business.Interfaces.Admin;
using LMS.Model.DataViewModel.Admin.Designation;
using LMS.Utility.Exceptions;
using LMS.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace LMS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("[controller]")]
    [HandleExceptionsAttribute]
    [UserAuthentication("Admin")]
    public class DesignationController : Controller
    {
        private readonly IDesignationHandler _designationHandler;
        public DesignationController(IDesignationHandler designationHandler)
        {
            _designationHandler = designationHandler;
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Route("[action]")]
        public ActionResult GetDesignation()
        {
            List<DesignationViewModel> list = new List<DesignationViewModel>();
            try
            {
                list = _designationHandler.GetDesignationList();
            }
            catch (DataNotFound ex)
            {
                Logger.Logger.WriteLog(Logger.Logtype.Error, ex.Message, 0, typeof(DesignationController), ex);
                ModelState.AddModelError("ErrorMessage", string.Format("{0}", ex.Message));
            }
            return View(list);
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult AddDesigantion([FromBody]DesignationViewModel designationModel)
        {
            var result = _designationHandler.AddDesignation(designationModel);
            if (result)
            {
                return Json(new { Msg = "Designation Added" });
            }
            return Json(new { Msg = "Designation/Abbr Already Exist" });
        }


        [HttpPost]
        [Route("[action]")]
        public IActionResult UpdateDesignation([FromBody]DesignationViewModel designationModel)
        {
            
            var result = _designationHandler.UpdateDesignation(designationModel);
            if (result)
            {
                return Json(new { Msg = "Record Updated" });
            }
            return Json(new { Msg = "Designation/Abbr Already Exist" });
        }


        [HttpGet]
        [Route("[action]")]
        public IActionResult DeleteDesignation(int DesignationId)
        {
            var result = _designationHandler.DeleteDesignation(DesignationId);
            if (result)
            {
                return Json("Record Deleted");
            }
            return Json("Record Can't be Deleted");
        }
    }
}