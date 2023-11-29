using System;
using System.IO;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Zplify;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var redirectedInput = false;
        Image<Rgba32> image = null;
        if (Console.IsInputRedirected)
        {
            await using var ms = new MemoryStream();
            redirectedInput = true;
            await Console.OpenStandardInput().CopyToAsync(ms);
            if (ms.Length == 0)
            {
                Console.WriteLine("Timed out trying to read from stdin.");
                return;
            }
            ms.Seek(0, SeekOrigin.Begin);
            image = Image.Load<Rgba32>(ms);
        }

        if (!redirectedInput && args.Length == 0)
        {
            Console.WriteLine("Please provide argument. --help for more details");
            return;
        }

        var arguments = ArgumentParser.Parse(args, out var error);
        if (image is null && arguments.InPath is null)
        {
            Console.WriteLine("Please provide file path argument or pipe file to stdin.");
            return;
        }

        if (error is null)
        {
            image ??= Image.Load<Rgba32>(arguments.InPath);
            await CreateLabel(arguments, image);
        }
        else
        {
            Console.WriteLine(error);
        }
    }

    private static async Task CreateLabel(Arguments args, Image<Rgba32> image)
    {
        var converter = args.Width > 0 && args.Length > 0
            ? new ZplImageConverter(args.Length, args.Width)
            : new ZplImageConverter();

        var label = converter.BuildLabel(image);
        if (args.OutPath != null)
        {
            await File.WriteAllTextAsync(args.OutPath, label);
        }
        else
        {
            Console.Write(label);
        }
    }
}
