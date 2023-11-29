using System.Collections.Generic;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Xunit;

namespace Zplify.Tests;
public class GeneralTests
{
    [Fact]
    public void CompressHex_CorrectlyCompressesHexString()
    {
        // Arrange
        var input = "FFFFFFF00000000FFFFFFFFF0000000000FFFFFF";
        int widthBytes = 10;
        var expected = "MFN0KFJFP0KF";

        var result = ZplImageConverter.CompressHex(input, widthBytes);

        Assert.Equal(expected, result);
    }
}