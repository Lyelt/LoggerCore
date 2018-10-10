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
        public void Debug(string message)
        {
            LogMessage(LogLevel.Debug, message);
        }

        public void Debug(string message, params object[] args)
        {
            LogMessage(LogLevel.Debug, string.Format(message, args));
        }

        public void Debug(Func<string> msgFunc)
        {
            LogDeferred(LogLevel.Debug, msgFunc);
        }
    }

    public static partial class Log
    {
        public static void Debug(string message)
        {
            LogManager.LogMessage(LogLevel.Debug, message);
        }

        public static void Debug(string message, params object[] args)
        {
            LogManager.LogMessage(LogLevel.Debug, string.Format(message, args));
        }

        public static void Debug(Func<string> msgFunc)
        {
            LogManager.LogDeferred(LogLevel.Debug, msgFunc);
        }
    }
}
