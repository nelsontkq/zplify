﻿using BenchmarkDotNet.Running;

namespace Zplify.Benchmark
{
    /// <summary>
    ///     Benchmark
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<CompressMappingMethodVsArray>();
            BenchmarkRunner.Run<ArgumentParserBench>();
        }
    }
}
