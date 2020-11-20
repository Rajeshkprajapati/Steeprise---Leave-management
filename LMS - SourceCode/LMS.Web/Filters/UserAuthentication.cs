using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LMS.Utility.ExtendedMethods;
using LMS.Model.DataViewModel.Shared;
using LMS.Utility.Helpers;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace LMS.Web.Filters
{
    public class UserAuthentication : IAuthorizationFilter
    {
        private readonly string[] role;
        public UserAuthentication(string[] _role)
        {
            role = _role;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var retUrl = $"/Auth/Logout?returnUrl={context.HttpContext.Request.Path.Value}";

            if (!context.HttpContext.User.Identity.IsAuthenticated || context.HttpContext.Session.IsSessionExpired())
            {

                if (context.HttpContext.Request.IsAjaxRequest())
                {
                    context.HttpContext.Response.StatusCode = StatusCodes.Status511NetworkAuthenticationRequired;
                    context.Result = new JsonResult(new { returnUrl = retUrl });
                }
                else
                {
                    context.Result = new RedirectResult($"~{retUrl}");
                }

            }
            else
            {
                if (!context.HttpContext.User.Claims
                    .Any(c => c.Type == ClaimTypes.Role
                    &&
                    role.Any(r => r.Trim().ToLower() == c.Value.Trim().ToLower())))
                {
                    if (context.HttpContext.Request.IsAjaxRequest())
                    {
                        context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Result = new JsonResult(new { returnUrl = "/Auth/UnauthorizedUser" });
                    }
                    else
                    {
                        context.Result = new RedirectResult("~/Auth/UnauthorizedUser");
                    }
                }
            }
        }
    }

    //  Let's create attribute to use.

    public class UserAuthenticationAttribute : TypeFilterAttribute
    {
        public UserAuthenticationAttribute(string role) : base(typeof(UserAuthentication))
        {
            var roles = role.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            Arguments = new object[] { roles.Select(r => r.Trim()).ToArray() };
        }
    }
}
