using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Utility.Exceptions
{
    public class FileNotDownloadedException:ApplicationException
    {
        public FileNotDownloadedException(string message) : base(message)
        {

        }

        public FileNotDownloadedException(string message, Exception exception) : base(message, exception)
        {

        }
    }
}
