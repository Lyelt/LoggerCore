using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LyeltLogger.Enums;

namespace LyeltLogger
{
    /// <summary>
    /// Class for managing a global logger as well as getting loggers and setting common log options
    /// </summary>
    public static class LogManager
    { 
        private static LogOptions _defaults;
        private static Logger _globalLogger;

        // Allow static logging to occur without ever providing options, by using the default options
        static LogManager()
        {
            _defaults = LogOptions.Default;
            _globalLogger = GetLogger<Logger>(_defaults);
            _globalLogger.AddLogWriter(LogFileWriter.Default);
        }

        /// <summary>
        /// Get the global logger instance for use with static Log methods
        /// </summary>
        /// <returns>The global logger instance</returns>
        public static Logger GetGlobalLogger()
        {
            return _globalLogger;
        }

        /// <summary>
        /// Set the default options for the global logger and all future logger instances
        /// </summary>
        /// <param name="options">Common log options</param>
        public static void SetDefaults(LogOptions options)
        {
            _defaults = options;
            _globalLogger.SetAllOptions(_defaults);
        }

        /// <summary>
        /// Get a logger for the given type with default writers and options
        /// </summary>
        /// <typeparam name="T">The type of the class this logger is for</typeparam>
        /// <returns>Logger for the given type with default writers and options</returns>
        public static Logger GetLogger<T>()
        {
            return GetLogger<T>(_defaults);
        }

        /// <summary>
        /// Get a logger for the given type
        /// </summary>
        /// <typeparam name="T">The type of the class this logger is for</typeparam>
        /// <param name="commonOptions">The options common to all log writers</param>
        /// <param name="options">Log options detailing the types of log writers to include. Default is just LogFileWriter</param>
        /// <returns>Logger for the given class with the specified writers and options</returns>
        public static Logger GetLogger<T>(LogOptions commonOptions)
        {
            return new Logger(typeof(T), commonOptions);
        }

        internal static void LogDeferred(LogLevel level, Func<string> deferred)
        {
            _globalLogger.LogDeferred(level, deferred);
        }

        internal static void LogMessage(LogLevel level, string message)
        {
            _globalLogger.LogMessage(level, message);
        }
    }
}
