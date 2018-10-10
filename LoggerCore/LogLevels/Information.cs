using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LyeltLogger.Enums;

namespace LyeltLogger
{
    public partial class Logger
    {
        public void Information(string message)
        {
            LogMessage(LogLevel.Information, message);
        }

        public void Information(string message, params object[] args)
        {
            LogMessage(LogLevel.Information, string.Format(message, args));
        }

        public void Information(Func<string> msgFunc)
        {
            LogDeferred(LogLevel.Information, msgFunc);
        }
    }

    public static partial class Log
    {
        public static void Information(string message)
        {
            LogManager.LogMessage(LogLevel.Information, message);
        }

        public static void Information(string message, params object[] args)
        {
            LogManager.LogMessage(LogLevel.Information, string.Format(message, args));
        }

        public static void Information(Func<string> msgFunc)
        {
            LogManager.LogDeferred(LogLevel.Information, msgFunc);
        }
    }
}
