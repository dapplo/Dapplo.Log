using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Columns;

namespace Dapplo.Log.BenchmarkTests
{
    /// <summary>
    /// This defines the benchmarks which can be done
    /// </summary>
    [MinColumn, MaxColumn, MemoryDiagnoser]
    public class LogPerformance
    {
        private static readonly LogSource Log = new LogSource();

        [Benchmark]
        public void IsLevelEnabled()
        {
            Log.IsFatalEnabled();
        }

        [Benchmark]
        public void WriteLine()
        {
            Log.Debug().WriteLine("Test");
        }
    }
}
