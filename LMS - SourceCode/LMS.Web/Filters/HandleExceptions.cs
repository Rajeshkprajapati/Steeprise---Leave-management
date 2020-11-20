using LMS.Utility.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Web.Filters
{
    public class HandleExceptions:IExceptionFilter
    {
        private readonly IModelMetadataProvider _modelMetadataProvider;
        public HandleExceptions(IModelMetadataProvider modelMetadataProvider)
        {
            _modelMetadataProvider = modelMetadataProvider;
        }
        public void OnException(ExceptionContext context)
        {
            //  Approach to log unhandled exception into our tables.
            Logger.Logger.WriteLog(Logger.Logtype.Error, context.Exception.Message, 0, typeof(HandleExceptions),context.Exception);
            context.ExceptionHandled = true;
        }
    }

    //  Let's create attribute to use.

    public class HandleExceptionsAttribute : TypeFilterAttribute
    {
        public HandleExceptionsAttribute() : base(typeof(HandleExceptions))
        {
            //Logger.Logger.WriteLog(Logger.Logtype.Error,"Handle Exception Attribute" , 0, typeof(HandleExceptions) );
            Arguments = new object[0];
        }
    }
}
