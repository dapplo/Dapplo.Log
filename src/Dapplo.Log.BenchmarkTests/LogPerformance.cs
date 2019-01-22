using System;
using BenchmarkDotNet.Attributes;

namespace Dapplo.Log.BenchmarkTests
{
    /// <summary>
    /// This defines the benchmarks which can be done
    /// </summary>
    [MinColumn, MaxColumn, MemoryDiagnoser]
    public class LogPerformance
    {
        private readonly Exception _exampleException = new Exception("Example");

        private readonly ILogger _loggerUnderTest = new NullLogger
        {
            LogLevel = LogLevels.Debug
        };
        private readonly LogSource _logSource = LogSource.ForCustomSource(Guid.NewGuid().ToString());

        [GlobalSetup]
        public void Setup()
        {
            _logSource.LogTo(_loggerUnderTest);
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            LoggerMapper.DeRegisterLoggerFor(_logSource, _loggerUnderTest);
        }

        [Benchmark]
        public void IsLevelEnabled()
        {
            _logSource.IsFatalEnabled();
        }

        [Benchmark]
        public void WriteLine()
        {
            _logSource.Debug().WriteLine("Test");
        }

        [Benchmark]
        public void WriteLineFormat()
        {
            const string testString = "Dapplo";
            _logSource.Debug().WriteLine("Formatting test {0}", testString);
        }

        [Benchmark]
        public void WriteLineFormatException()
        {
            const string testString = "Dapplo";
            _logSource.Error().WriteLine(_exampleException, "Formatting test {0}", testString);
        }
    }
}
