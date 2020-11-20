using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Utility.Exceptions
{
    public class FileEmptyException: ApplicationException
    {
        public FileEmptyException(string message) : base(message)
        {

        }

        public FileEmptyException(string message, Exception exception) : base(message, exception)
        {

        }
    }
}
