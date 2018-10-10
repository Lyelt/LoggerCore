using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyeltLogger
{
    public class LogDatabaseWriter : LogWriter
    {
        public LogDatabaseWriter(string name) : this(name, LogOptions.Default) { }

        public LogDatabaseWriter(string name, LogOptions common) : base(name, common)
        {
        }

        public override void LogMessage(LogMessage message)
        {

        }
    }
}
