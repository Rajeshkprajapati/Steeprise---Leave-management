using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Utility.Exceptions
{
    public class UserCanNotPostData : ApplicationException
    {
        public UserCanNotPostData(string message) : base(message)
        {

        }

        public UserCanNotPostData(string message, Exception exception) : base(message, exception)
        {

        }
    }

}
