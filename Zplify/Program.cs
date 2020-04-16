using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Zplify
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var redirectedInput = false;
            Bitmap file = null;
            if (Console.IsInputRedirected)
            {
                await using var ms = new MemoryStream();
                redirectedInput = true;
                await Console.OpenStandardInput().CopyToAsync(ms);
                if (ms.Length == 0)
                {
                    Console.WriteLine("Timed out trying to read from stdin.");
                }
                ms.Seek(0, SeekOrigin.Begin);
                file = new Bitmap(ms);
            }
            if (!redirectedInput && args.Length == 0)
            {
                Console.WriteLine("Please provide argument. --help for more details");
            }
            else
            {
                var arguments = ArgumentParser.Parse(args, out var error);
                if (file is null && arguments.InPath is null)
                {
                    Console.WriteLine("please provide file path argument or pipe file to stdin.");
                    return;
                }
                if (error is null)
                {
                    using var bitmap = file ??= new Bitmap(arguments.InPath);
                    CreateLabel(arguments, file);
                }
                else
                {
                    Console.WriteLine(error);
                }
            }
        }

        private static void CreateLabel(Arguments args, Bitmap file)
        {
            var converter = args.Width > 0 && args.Length > 0
                ? new ZplImageConverter(args.Length, args.Width)
                : new ZplImageConverter();
            if (args.OutPath != null)
                File.WriteAllText(args.OutPath, converter.BuildLabel(file));
            else
                Console.Write(converter.BuildLabel(file));
        }
    }
}