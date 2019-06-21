using System;
using System.Drawing;
using System.IO;

namespace zplify
{
    class Program
    {
        internal static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please provide argument. --help for more details");
            }
            else
            {
                var arguments = ArgumentParser.Parse(args, out var error);
                if (error != null)
                {
                    Console.WriteLine(error);
                    return;
                }
                CreateLabel(arguments);
            }
        }
        private static void CreateLabel(Arguments args)
        {
            var file = new Bitmap(args.InPath);

            var converter = args.Width > 0 && args.Length > 0
                ? new ZplImageConverter(args.Length, args.Width)
                : new ZplImageConverter();
            if (args.OutPath != null)
            {
                File.WriteAllText(args.OutPath, converter.BuildLabel(file));
            }
            else
            {
                Console.Write(converter.BuildLabel(file));
            }
        }
    }

}
