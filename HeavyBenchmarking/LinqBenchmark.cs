using System;
using System.Collections.Generic;
using System.Linq;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Jobs;

using HeavyEngine.Linq;

namespace HeavyBenchmarking {
    [SimpleJob(RuntimeMoniker.CoreRt31)]
    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    public class LinqBenchmark {
        private List<int> randomList;
        private IEnumerable<int> randomEnumerable;
        private readonly Consumer consumer = new Consumer();

        [Params(1000, 1000000)]
        public int N;

        public LinqBenchmark() {

        }

        [GlobalSetup]
        public void SetUp() {
            randomList = new List<int>();
            randomEnumerable = randomList;
            var random = new Random();

            for (int i = 0; i < N; i++)
                randomList.Add(random.Next());
        }

        [Benchmark]
        public int CountSystem() => randomEnumerable.Count(i => i % 2 == 0);

        [Benchmark]
        public int CountHeavy() => randomList.Count(i => i % 2 == 0);

        [Benchmark]
        public int CountFor() {
            int count = 0;

            for (int i = 0; i < randomList.Count; i++)
                if (randomList[i] % 2 == 0)
                    count++;

            return count;
        }

        [Benchmark]
        public int CountForEach() {
            int count = 0;

            foreach (var i in randomList)
                if (i % 2 == 0)
                    count++;

            return count;
        }

        [Benchmark]
        public void WhereSystem() => randomEnumerable.Where(i => i % 2 == 0).Consume(consumer);

        [Benchmark]
        public List<int> WhereHeavy() => randomList.Where(i => i % 2 == 0);

        [Benchmark]
        public List<int> WhereFor() {
            var result = new List<int>();

            for (int i = 0; i < randomList.Count; i++)
                if (randomList[i] % 2 == 0)
                    result.Add(randomList[i]);

            return result;
        }

        [Benchmark]
        public List<int> WhereForEach() {
            var result = new List<int>();

            foreach (var i in randomList)
                if (i % 2 == 0)
                    result.Add(i);

            return result;
        }

        [Benchmark]
        public void Where2System() => randomEnumerable.Where(i => i % 2 == 0).Where(i => i % 4 == 0).Consume(consumer);

        [Benchmark]
        public List<int> Where2Heavy() => randomList.Where(i => i % 2 == 0).Where(i => i % 4 == 0);

        [Benchmark]
        public List<int> Where2For() {
            var result = new List<int>();

            for (int i = 0; i < randomList.Count; i++)
                if (randomList[i] % 2 == 0 && randomList[i] % 4 == 0)
                    result.Add(randomList[i]);

            return result;
        }

        [Benchmark]
        public List<int> Where2ForEach() {
            var result = new List<int>();

            foreach (var i in randomList)
                if (i % 2 == 0 && i % 4 == 0)
                    result.Add(i);

            return result;
        }
    }
}
