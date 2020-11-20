using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Utility.Exceptions
{
    public class XmlFileMapperException:ApplicationException
    {
        public XmlFileMapperException(string message) : base(message)
        {

        }

        public XmlFileMapperException(string message, Exception exception) : base(message, exception)
        {

        }
    }
}
