using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static LyeltLogger.Enums;

namespace LyeltLogger
{
    /// <summary>
    /// Class representing an instance of a logger which maintains a queue of log messages written by all of the implemented log writers
    /// </summary>
    public partial class Logger
    {
        private LogOptions _commonOptions;
        private Type _type;
        private Dictionary<string, LogWriter> _logWriters = new Dictionary<string, LogWriter>();
        private BlockingCollection<LogMessage> _logQueue = new BlockingCollection<LogMessage>();
        private CancellationTokenSource _cts = new CancellationTokenSource();

        /// <summary>
        /// Create a logger of the specified type with the given options and writer options
        /// </summary>
        /// <param name="t">Logger type</param>
        /// <param name="commonOptions">Options common to all writers</param>
        /// <param name="options">Log writer options</param>
        public Logger(Type t, LogOptions commonOptions)
        {
            _commonOptions = commonOptions;
            _type = t;
            
            Task.Run(() => LogMessages(), _cts.Token);
        }

        /// <summary>
        /// Clear all log writers from this logger
        /// </summary>
        public void ClearWriters()
        {
            _logWriters.Clear();
        }

        /// <summary>
        /// Get the log writer with the given name
        /// </summary>
        /// <param name="writerName">The internal, unique name of the log writer</param>
        /// <returns>The log writer with the given name, or null if one does not exist</returns>
        public LogWriter GetLogWriter(string writerName)
        {
            if (_logWriters.ContainsKey(writerName))
                return _logWriters[writerName];
            else
                return null;
        }

        /// <summary>
        /// Set the log options for the given log writer
        /// </summary>
        /// <param name="writerName">Internal, unique name of the log writer</param>
        /// <param name="options">LogOptions to set for the given writer</param>
        public void SetLogOptions(string writerName, LogOptions options)
        {
            if (_logWriters.ContainsKey(writerName))
            {
                _logWriters[writerName].SetCommonOptions(options);
            }
        }

        /// <summary>
        /// Set options for all log writers maintained by this logger
        /// </summary>
        /// <param name="options">LogOptions to set for all writers</param>
        public void SetAllOptions(LogOptions options)
        {
            foreach (var writer in _logWriters.Values)
            {
                writer.SetCommonOptions(options);
            }
        }

        /// <summary>
        /// Add the given log writer to the logger's list of maintained log writers
        /// </summary>
        /// <param name="writer">The log writer to add</param>
        public void AddLogWriter(LogWriter writer)
        {
            _logWriters[writer.Name] = writer;
        }

        internal void LogMessage(LogLevel level, string message)
        {
            if (level < _commonOptions.Verbosity)
                return;

            var logMessage = new LogMessage(level, message, _commonOptions.AppName, DateTime.Now, _type);

            if (_commonOptions.SynchronousLogging)
            {
                foreach (var writer in _logWriters.Values)
                {
                    writer.LogMessage(logMessage);
                }
            }
            else
            {
                _logQueue.TryAdd(logMessage);
            }
        }

        internal void LogDeferred(LogLevel level, Func<string> msgFunc)
        {
            if (level >= _commonOptions.Verbosity)
                LogMessage(level, msgFunc());
        }

        private void LogMessages()
        {
            while (!_cts.IsCancellationRequested)
            {
                try
                {
                    if (_logQueue.TryTake(out var message))
                    {
                        foreach (var logger in _logWriters.Values)
                        {
                            logger.LogMessage(message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error logging messages.");
                    Console.Error.WriteLine(ex);
                }
            }
        }

        public static string DefaultLogFileWriterName
        {
            get { return "DefaultLogFileWriter"; }
        }
    }
}
