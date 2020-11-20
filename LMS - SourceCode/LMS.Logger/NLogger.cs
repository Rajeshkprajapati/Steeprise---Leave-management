using Microsoft.Extensions.Logging;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Logger
{
    public sealed class NLogger
    {
        private static NLogger _logger;
        private readonly NLog.Logger nLogger;

        private NLogger()
        {
            nLogger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
        }

        public static NLogger logger
        {
            get
            {
                if (null == _logger)
                {
                    _logger = new NLogger();
                }
                return _logger;
            }
        }

        public void FileLogger(string ex)
        {
            nLogger.Error(ex);
        }

    }
}
