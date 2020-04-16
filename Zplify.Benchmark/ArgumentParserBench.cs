using BenchmarkDotNet.Attributes;

namespace Zplify.Benchmark
{
    public class ArgumentParserBench
    {
        [Params(new []{ "--help"}, new []{ "--length", "1200", "--width", "800", "-o", "."})]
        public string[] arguments;

        [Benchmark]
        public void ParseArgs()
        {
            ArgumentParser.Parse(arguments, out var error);
        }
    }
}