using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using LyeltLogger;
using static LyeltLogger.Enums;
using System.IO;

namespace LoggerTests
{
    [TestFixture]
    public class FileLogTests
    {
        private const string WRITER_NAME = "Log File Writer";
        private const string APP_NAME = "LogTestAppName";

        private const string TEST_DEBUG = "test debug message";
        private const string TEST_INFO = "test info message";
        private const string TEST_WARN = "test warn message";
        private const string TEST_ERROR = "test error message";
        private const string TEST_FATAL = "test fatal message";

        private Logger _log;

        [OneTimeSetUp]
        public void Setup()
        {
            Directory.SetCurrentDirectory("C:\\Workspace\\Logger\\");
            LogOptions opts = new LogOptions(APP_NAME, LogLevel.Debug, true, true);
            _log = LogManager.GetLogger<FileLogTests>(opts);
            var writer = new LogFileWriter(WRITER_NAME, "testLogFiles", opts);
            writer.CompressArchivedFiles = false;
            writer.MaxLogFileSize = 50;
            writer.RotateLogs = true;
            _log.AddLogWriter(writer);
        }

        [Test]
        public void TestLogWriterAdded()
        {
            if (_log.GetLogWriter(WRITER_NAME) is LogFileWriter writer)
            {
                DirectoryAssert.Exists(writer.LogDirectory);
            }
            else
            {
                Assert.Fail($"LogWriter {WRITER_NAME} is not a LogFileWriter type.");
            }
        }

        [Test]
        public void TestLogWriter()
        {
            var writer = _log.GetLogWriter(WRITER_NAME) as LogFileWriter;

            _log.Debug(TEST_DEBUG);
            _log.Information(TEST_INFO);
            _log.Warning(TEST_WARN);
            _log.Error(TEST_ERROR);
            _log.Fatal(TEST_FATAL);

            System.Threading.Thread.Sleep(1000);

            FileAssert.Exists(writer.LogFile);

            List<string> logFileLines = File.ReadAllLines(writer.LogFile).ToList();

            // All 5 lines were logged
            Assert.That(logFileLines.Count >= 5);

            // All 5 messages were properly logged
            Assert.That(logFileLines.Any(line => line.Contains(TEST_DEBUG)));
            Assert.That(logFileLines.Any(line => line.Contains(TEST_INFO)));
            Assert.That(logFileLines.Any(line => line.Contains(TEST_WARN)));
            Assert.That(logFileLines.Any(line => line.Contains(TEST_ERROR)));
            Assert.That(logFileLines.Any(line => line.Contains(TEST_FATAL)));

            // All 5 log levels are properly represented
            Assert.That(logFileLines.Any(line => line.Contains("<" + "Debug" + ">")));
            Assert.That(logFileLines.Any(line => line.Contains("<" + "Information" + ">")));
            Assert.That(logFileLines.Any(line => line.Contains("<" + "Warning" + ">")));
            Assert.That(logFileLines.Any(line => line.Contains("<" + "Error" + ">")));
            Assert.That(logFileLines.Any(line => line.Contains("<" + "Fatal" + ">")));
        }

        [Test]
        public void TestLogRotate()
        {
            for (int i = 0; i < 10000; ++i)
            {
                _log.Information($"{i} trying to fill the log files so logging this long message");
            }
        }
    }
}
