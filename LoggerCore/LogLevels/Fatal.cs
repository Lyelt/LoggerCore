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
        public void Fatal(string message)
        {
            LogMessage(LogLevel.Fatal, message);
        }

        public void Fatal(string message, params object[] args)
        {
            LogMessage(LogLevel.Fatal, string.Format(message, args));
        }

        public void Fatal(Exception ex)
        {
            LogMessage(LogLevel.Fatal, ex.ToString());
        }

        public void Fatal(Exception ex, string message)
        {
            LogMessage(LogLevel.Fatal, string.Concat(message, Environment.NewLine, ex.ToString()));
        }

        public void Fatal(Exception ex, string message, params object[] args)
        {
            LogMessage(LogLevel.Fatal, string.Concat(string.Format(message, args), Environment.NewLine, ex.ToString()));
        }

        public void Fatal(Func<string> msgFunc)
        {
            LogDeferred(LogLevel.Fatal, msgFunc);
        }
    }

    public static partial class Log
    {
        public static void Fatal(string message)
        {
            LogManager.LogMessage(LogLevel.Fatal, message);
        }

        public static void Fatal(string message, params object[] args)
        {
            LogManager.LogMessage(LogLevel.Fatal, string.Format(message, args));
        }

        public static void Fatal(Exception ex)
        {
            LogManager.LogMessage(LogLevel.Fatal, ex.ToString());
        }

        public static void Fatal(Exception ex, string message)
        {
            LogManager.LogMessage(LogLevel.Fatal, string.Concat(message, Environment.NewLine, ex.ToString()));
        }

        public static void Fatal(Exception ex, string message, params object[] args)
        {
            LogManager.LogMessage(LogLevel.Fatal, string.Concat(string.Format(message, args), Environment.NewLine, ex.ToString()));
        }

        public static void Fatal(Func<string> msgFunc)
        {
            LogManager.LogDeferred(LogLevel.Fatal, msgFunc);
        }
    }
}
