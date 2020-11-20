using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Utility.Exceptions
{
    public class UnableToParseResumeTemplate:ApplicationException
    {
        public UnableToParseResumeTemplate(string message) : base(message)
        {

        }

        public UnableToParseResumeTemplate(string message, Exception exception) : base(message, exception)
        {

        }
    }
}
