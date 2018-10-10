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
        public void Warning(string message)
        {
            LogMessage(LogLevel.Warning, message);
        }

        public void Warning(string message, params object[] args)
        {
            LogMessage(LogLevel.Warning, string.Format(message, args));
        }

        public void Warning(Func<string> msgFunc)
        {
            LogDeferred(LogLevel.Warning, msgFunc);
        }
    }

    public static partial class Log
    {
        public static void Warning(string message)
        {
            LogManager.LogMessage(LogLevel.Warning, message);
        }

        public static void Warning(string message, params object[] args)
        {
            LogManager.LogMessage(LogLevel.Warning, string.Format(message, args));
        }

        public static void Warning(Func<string> msgFunc)
        {
            LogManager.LogDeferred(LogLevel.Warning, msgFunc);
        }
    }
}
