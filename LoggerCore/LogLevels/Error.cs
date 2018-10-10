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
        public void Error(string message)
        {
            LogMessage(LogLevel.Error, message);
        }

        public void Error(string message, params object[] args)
        {
            LogMessage(LogLevel.Error, string.Format(message, args));
        }

        public void Error(Exception ex)
        {
            LogMessage(LogLevel.Error, ex.ToString());
        }

        public void Error(Exception ex, string message)
        {
            LogMessage(LogLevel.Error, string.Concat(message, Environment.NewLine, ex.ToString()));
        }

        public void Error(Exception ex, string message, params object[] args)
        {
            LogMessage(LogLevel.Error, string.Concat(string.Format(message, args), Environment.NewLine, ex.ToString()));
        }

        public void Error(Func<string> msgFunc)
        {
            LogDeferred(LogLevel.Error, msgFunc);
        }
    }

    public static partial class Log
    {
        public static void Error(string message)
        {
            LogManager.LogMessage(LogLevel.Error, message);
        }

        public static void Error(string message, params object[] args)
        {
            LogManager.LogMessage(LogLevel.Error, string.Format(message, args));
        }

        public static void Error(Exception ex)
        {
            LogManager.LogMessage(LogLevel.Error, ex.ToString());
        }

        public static void Error(Exception ex, string message)
        {
            LogManager.LogMessage(LogLevel.Error, string.Concat(message, Environment.NewLine, ex.ToString()));
        }

        public static void Error(Exception ex, string message, params object[] args)
        {
            LogManager.LogMessage(LogLevel.Error, string.Concat(string.Format(message, args), Environment.NewLine, ex.ToString()));
        }

        public static void Error(Func<string> msgFunc)
        {
            LogManager.LogDeferred(LogLevel.Error, msgFunc);
        }
    }
}
