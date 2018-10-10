using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LyeltLogger.Enums;

namespace LyeltLogger
{
    /// <summary>
    /// Represents a single log message to be logged by any log writer
    /// </summary>
    public class LogMessage
    {
        private const string DATE_FORMAT = @"yyyy-MM-dd hh:mm:ss.ffff";
        /// <summary>
        /// The text content of the log message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The severity level with which this message is being logged
        /// </summary>
        public LogLevel Level { get; set; }

        /// <summary>
        /// The name of the application logging the message
        /// </summary>
        public string AppName { get; set; }

        /// <summary>
        /// The datetime of the log message
        /// </summary>
        public DateTime MessageTime { get; set; }

        /// <summary>
        /// The class type from which the log message originated
        /// </summary>
        public Type Class { get; set; }

        /// <summary>
        /// Create a log message
        /// </summary>
        /// <param name="level">Level of the log message</param>
        /// <param name="message">Text of the message</param>
        /// <param name="appName">Sending application name</param>
        /// <param name="dt">DateTime of the message</param>
        /// <param name="type">Sending class type</param>
        public LogMessage(LogLevel level, string message, string appName, DateTime dt, Type type)
        {
            Level = level;
            Message = message;
            AppName = appName;
            MessageTime = dt;
            Class = type;
        }

        /// <summary>
        /// Format the log message in the default way to be printed to a log writer
        /// </summary>
        public override string ToString()
        {
            return $"[{MessageTime.ToString(DATE_FORMAT)}] <{Level}> in {AppName}.{Class.Name}: {Message}{Environment.NewLine}";
        }
    }
}
