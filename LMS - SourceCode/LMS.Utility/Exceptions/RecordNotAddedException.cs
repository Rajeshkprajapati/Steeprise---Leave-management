using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Utility.Exceptions
{
    public class RecordNotAddedException:ApplicationException
    {
        public RecordNotAddedException(string message) : base(message)
        {

        }
        public RecordNotAddedException(string message, Exception exception) : base(message)
        {

        }
    }
}
