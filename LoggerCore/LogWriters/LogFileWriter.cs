using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LyeltLogger
{
    public class LogFileWriter : LogWriter
    {

        private static string DEFAULT_LOG_DIR = "logs";
        private static LogFileComparer LOG_COMPARER = new LogFileComparer();
        private static string NEW_LOG = ".1.log";
        private static string LOG_EXT = ".log";
        private static string LOG_MATCH = ".*.log";
        private string _logDir;
        private string _baseLogName;

        /// <summary>
        /// The default log file writer. Uses default name, directory, and log options.
        /// </summary>
        public static LogFileWriter Default
        {
            get
            {
                return new LogFileWriter(Logger.DefaultLogFileWriterName);
            }
        }

        /// <summary>
        /// Create a new log file writer with the specified name and the default LogOptions
        /// </summary>
        /// <param name="name">Internal, unique name of the LogWriter</param>
        public LogFileWriter(string name) : this(name, DEFAULT_LOG_DIR, LogOptions.Default) { }

        /// <summary>
        /// Create a new log file writer with the specified name and log directory
        /// </summary>
        /// <remarks>
        /// This constructor is used to initialize the log directory, as the log file to use is set at construction time
        /// </remarks>
        /// <param name="name">Internal, unique name of the LogWriter</param>
        /// <param name="logDirectory">Directory to create log files in</param>
        public LogFileWriter(string name, string logDirectory) : this(name, logDirectory, LogOptions.Default) { }

        /// <summary>
        /// Create a new log file writer with the specified name and log options
        /// </summary>
        /// <param name="name">Internal, unique name of the LogWriter</param>
        /// <param name="common">Log options commmon to all writers</param>
        public LogFileWriter(string name, LogOptions common) : this(name, DEFAULT_LOG_DIR, common) { }

        /// <summary>
        /// Create a new log file writer with the specified name, log directory, and log options.
        /// </summary>
        /// <param name="name">Internal, unique name of the LogWriter</param>
        /// <param name="logDirectory">Directory to create log files in</param>
        /// <param name="common">Log options commmon to all writers</param>
        public LogFileWriter(string name, string logDirectory, LogOptions common) : base(name, common)
        {
            LogDirectory = logDirectory;

            DirectoryInfo dirInfo = Directory.CreateDirectory(LogDirectory);
            _logDir = dirInfo.FullName;
            _baseLogName = _logDir + "\\" + _commonOptions.AppName;
            LogFile = _baseLogName + LOG_EXT;
        }

        /// <summary>
        /// Set the common options for this writer as well as the default options for the logger
        /// </summary>
        /// <param name="options">Common log options</param>
        public override void SetCommonOptions(LogOptions options)
        {
            base.SetCommonOptions(options);
            
            _baseLogName = _logDir + "\\" + _commonOptions.AppName;
            LogFile = _baseLogName + LOG_EXT;
        }

        public override void LogMessage(LogMessage message)
        {
            try
            {
                Console.WriteLine(message.ToString());
                File.AppendAllText(LogFile, message.ToString());
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error writing log message to file. Message: {message}");
                Console.Error.WriteLine(ex);
            }
            finally
            {
                if (RotateLogs)
                    RotateLogFiles();
            }
        }

        private void RotateLogFiles()
        {
            try
            {
                FileInfo file = new FileInfo(LogFile);
                if (file.Length >= MaxLogFileSize * 1024 || DateTime.Now - file.LastWriteTime > MaxLogFileAge)
                {
                    string newPath = _baseLogName + NEW_LOG;
                    if (File.Exists(newPath))
                    {
                        RotateOldFiles();
                    }

                    File.Move(LogFile, newPath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error rotating log files...");
                Console.Error.WriteLine(ex);
            }
        }

        private void RotateOldFiles()
        {
            string path = Path.GetDirectoryName(LogFile);
            List<string> logs = Directory.GetFiles(path, _commonOptions.AppName + LOG_MATCH).ToList();
            logs.Sort(LOG_COMPARER);
            logs.Reverse();

            foreach (string log in logs)
            {
                int logNum = GetLogNumber(log);

                if (logNum > MaxNumberLogFiles)
                {
                    if (CompressArchivedFiles)
                        CompressFile(log);
                    else
                        File.Delete(log);
                }
                else
                {
                    File.Move(log, $"{_baseLogName}.{++logNum}{LOG_EXT}");
                }
            }
        }

        private void CompressFile(string filePath)
        {

        }

        internal static int GetLogNumber(string log)
        {
            string[] split = log.Split('.');
            int logNum = 0;

            if (split.Length > 2)
                int.TryParse(split[split.Length - 2], out logNum);

            return logNum;
        }

        #region Properties
        /// <summary>
        /// Directory to log to. Default current directory
        /// </summary>
        public string LogDirectory { get; set; } = Directory.GetCurrentDirectory();

        /// <summary>
        /// Name of the log file.
        /// </summary>
        public string LogFile { get; set; }

        /// <summary>
        /// Whether to rotate logs when the max file size is reached. Default true
        /// </summary>
        public bool RotateLogs { get; set; } = true;

        /// <summary>
        /// Maximum log file size in KB. Default 1024
        /// </summary>
        public float MaxLogFileSize { get; set; } = 1024;

        /// <summary>
        /// Maximum log file age. Default 7 days
        /// </summary>
        public TimeSpan MaxLogFileAge { get; set; } = TimeSpan.FromDays(7);

        /// <summary>
        /// Maximum number of log files to keep. Default 100
        /// </summary>
        public int MaxNumberLogFiles { get; set; } = 100;

        /// <summary>
        /// Whether to compress log files past the number to keep. Default false
        /// </summary>
        public bool CompressArchivedFiles { get; set; } = false;
        #endregion

        // Compare two log files based on their log number
        private class LogFileComparer : IComparer<string>
        {
            public int Compare(string log1, string log2)
            {
                int logNum1 = GetLogNumber(log1);
                int logNum2 = GetLogNumber(log2);

                return logNum1.CompareTo(logNum2);
            }
        }
    }
}
