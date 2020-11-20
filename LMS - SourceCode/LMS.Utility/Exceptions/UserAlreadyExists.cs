using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Utility.Exceptions
{
    public class UserAlreadyExists:ApplicationException
    {
        public UserAlreadyExists(string message) : base(message)
        {

        }

        public UserAlreadyExists(string message, Exception exception) : base(message, exception)
        {

        }
    }
}
