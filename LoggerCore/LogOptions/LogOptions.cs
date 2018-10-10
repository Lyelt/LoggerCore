using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LyeltLogger.Enums;

namespace LyeltLogger
{
    public class LogOptions
    {
        /// <summary>
        /// Name of the current logging application
        /// </summary>
        public string AppName { get; set; }

        /// <summary>
        /// Minimum level of logging
        /// </summary>
        public LogLevel Verbosity { get; set; }

        /// <summary>
        /// Filter messages that are constantly logged
        /// </summary>
        public bool DuplicationFilter { get; set; }

        /// <summary>
        /// Log messages synchronously. When false, messages are queued to be logged ASAP instead of happening inline.
        /// </summary>
        public bool SynchronousLogging { get; set; }

        /// <summary>
        /// Create a new Log Options object with the given app name, verbosity, and duplication filter option.
        /// </summary>
        /// <remarks>
        /// Anything not specified will get a default value.
        /// </remarks>
        /// <param name="appName">App name to be used in the logs. Default is the current running application's name</param>
        /// <param name="verbosity">Minimum level of messages to log. Default Information</param>
        /// <param name="dupFilter">Whether to filter for duplicate files. Default is false</param>
        /// <param name="synchronousLogging">Whether to log messages synchronously. Default is false</param>
        public LogOptions(string appName = null, LogLevel verbosity = LogLevel.Information, bool dupFilter = false, bool synchronousLogging = false)
        {
            AppName = appName ?? Process.GetCurrentProcess()?.ProcessName ?? string.Empty;
            Verbosity = verbosity;
            DuplicationFilter = dupFilter;
            SynchronousLogging = synchronousLogging;
        }

        /// <summary>
        /// Default logger options 
        /// </summary>
        public static LogOptions Default { get { return new LogOptions(); } }
    }
}
