using System;
using System.Linq;

using BenchmarkDotNet.Running;

using HeavyEngine.Linq;

namespace HeavyBenchmarking {
    public class Program {
        public static void Main() {
            var summary = BenchmarkRunner.Run<LinqBenchmark>();

            Console.WriteLine(summary);

            Console.ReadKey();
        }
    }
}
