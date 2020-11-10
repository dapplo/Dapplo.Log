using System;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
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
            var jobCore50 = Job.Default.WithRuntime(CoreRuntime.Core50).WithPlatform(Platform.X64);
            var jobNet472 = Job.Default.WithRuntime(ClrRuntime.Net472).WithPlatform(Platform.X64);
            var config = DefaultConfig.Instance.AddJob(jobCore50).AddJob(jobNet472);
            BenchmarkRunner.Run<LogPerformance>(config);
            Console.ReadLine();
        }
    }
}
