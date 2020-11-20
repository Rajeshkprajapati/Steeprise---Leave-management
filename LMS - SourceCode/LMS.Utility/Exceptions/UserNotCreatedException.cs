using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Utility.Exceptions
{
    public class UserNotCreatedException:ApplicationException
    {
        public UserNotCreatedException(string message) : base(message)
        {

        }

        public UserNotCreatedException(string message, Exception exception) : base(message, exception)
        {

        }
    }
}
