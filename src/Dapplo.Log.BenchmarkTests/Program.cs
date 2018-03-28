using System;
using BenchmarkDotNet.Running;

namespace Dapplo.Log.BenchmarkTests
{
    /// <summary>
    /// This initializes the benchmark tests
    /// </summary>
    public static class Program
    {
        // ReSharper disable once UnusedParameter.Local
        private static void Main(string[] args)
        {
            LogSettings.RegisterDefaultLogger<NullLogger>(LogLevels.Info);
            BenchmarkRunner.Run<LogPerformance>();
            Console.ReadLine();
        }
    }
}
