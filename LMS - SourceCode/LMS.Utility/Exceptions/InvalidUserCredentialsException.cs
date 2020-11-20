using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Utility.Exceptions
{
    public class InvalidUserCredentialsException:ApplicationException
    {
        public InvalidUserCredentialsException(string message) : base(message)
        {

        }

        public InvalidUserCredentialsException(string message,Exception exception) : base(message,exception)
        {

        }
    }
}
