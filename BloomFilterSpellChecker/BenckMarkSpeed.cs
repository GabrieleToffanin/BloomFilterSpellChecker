using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloomFilterSpellChecker
{
    [MemoryDiagnoser]
    public class BenckMarkSpeed
    {
        private delegate int HashFunction(string word = "Cane", uint seed = (uint)21);
        private static HashFunction hashAlgorithm = Filter<string>.Hash;
        private static HashFunction betterHashAlgorithm = Filter<string>.NewerHash;

        [Benchmark]
        public void MurMur3HashingAlgorithmSpeed()
        {
            var something = hashAlgorithm("Cane", 21);
        }

        [Benchmark]
        public void MurMur3HashingAlgorithmWithoutUnnecessaryAllocAndBetterMemoryManagement()
        {
            var something = betterHashAlgorithm("Cane", 21);
        }

        [Benchmark(Baseline = true)]
        public void GetHashCodeMicrosoftSpeed()
        {
            var result = "Cane".GetHashCode();
        }

    }
}
