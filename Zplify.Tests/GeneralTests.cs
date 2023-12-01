using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.IO;
using Xunit;

namespace Zplify.Tests;
public class GeneralTests
{

    [Theory]
    [ClassData(typeof(LabelLoader))]
    public void Labels_CorrectlyCompress(string fileNameWithoutExtension)
    {

        var image = Image.Load<Rgba32>(fileNameWithoutExtension + ".png");
        ZplImageConverter.ScaleAndRotateImage(image, 800, 1200);
        var hex = Convert.ToHexString(ZplImageConverter.GetImageBytes(image));
        Assert.Equal(File.ReadAllText(fileNameWithoutExtension + ".hex"), hex);
        var widthBytes = ZplImageConverter.GetImageWidthInBytes(image);
        var compressed = ZplImageConverter.CompressHex(hex, widthBytes);
        Assert.Equal(File.ReadAllText(fileNameWithoutExtension + ".compressed"), compressed);
    }
}