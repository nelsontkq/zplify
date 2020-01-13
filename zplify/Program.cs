using System;
using System.Drawing;
using System.IO;
using System.Text;

namespace zplify
{
    class Program
    {
        internal static void Main(string[] args)
        {
            var arguments = ArgumentParser.Parse(args, out var error);
            if (error != null)
            {
                Console.WriteLine(error);
                return;
            }
            if (args.Length == 0)
            {
                var ms = new MemoryStream();
                using (Stream stdin = Console.OpenStandardInput())
                {
                    var buffer = new byte[2048];
                    int bytes;
                    while ((bytes = stdin.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, bytes);
                    }
                }
                if (ms.Length > 0)
                {
                    CreateLabel(new Bitmap(ms), 0, 0, null);
                }
                else
                {
                    Console.WriteLine("Please provide argument. --help for more details");
                }
            }
            else
            {
                CreateLabel(new Bitmap(arguments.InPath), arguments.Width, arguments.Length, arguments.OutPath);
            }
        }
        private static void CreateLabel(Bitmap file, int width, int length, string outPath)
        {

            var converter = width > 0 && length > 0
                ? new ZplImageConverter(length, width)
                : new ZplImageConverter();
            if (outPath != null)
            {
                File.WriteAllText(outPath, converter.BuildLabel(file));
            }
            else
            {
                Console.Write(converter.BuildLabel(file));
            }
        }
    }

}
