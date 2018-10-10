using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LyeltLogger;
using static LyeltLogger.Enums;
using NUnit.Framework;
using System.IO;

namespace LoggerTests
{
    [TestFixture]
    public class StaticLogTests
    {
        private string APP_NAME = "TestStaticAppName";

        [OneTimeSetUp]
        public void LogTestSetup()
        {
            LogOptions options = new LogOptions(APP_NAME, LogLevel.Debug, false);
            LogManager.SetDefaults(options);
        }

        [Test]
        public void LogLevelTests()
        {
            Log.Debug("test debug message");
            Log.Information("test info message");
            Log.Warning("test warn message");
            Log.Error("test error message");
            Log.Fatal("test fatal message");

            System.Threading.Thread.Sleep(1000);

            var writer = LogManager.GetGlobalLogger().GetLogWriter(Logger.DefaultLogFileWriterName) as LogFileWriter;
            DirectoryAssert.Exists(writer.LogDirectory);
            FileAssert.Exists(writer.LogFile);
            var lines = File.ReadAllLines(writer.LogFile);

            Assert.That(lines.Count() > 0);
        }
    }
}
