using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyeltLogger
{
    public abstract class LogWriter
    {
        /// <summary>
        /// The name of the log writer
        /// </summary>
        public string Name { get; set; }

        protected LogOptions _commonOptions;

        /// <summary>
        /// Create a new log writer with the given name and common options
        /// </summary>
        /// <param name="name">Unique name of the log writer</param>
        /// <param name="options">Options for this log writer</param>
        public LogWriter(string name, LogOptions options)
        {
            Name = name;
            SetCommonOptions(options);
        }

        /// <summary>
        /// Set the options for this log writer which are common to all log writers
        /// </summary>
        /// <param name="options">Log options to set</param>
        public virtual void SetCommonOptions(LogOptions options)
        {
            _commonOptions = options;
        }
        

        /// <summary>
        /// Log the current log message using the given log writer
        /// </summary>
        /// <param name="message">The log message to write</param>
        public abstract void LogMessage(LogMessage message);
    }
}
