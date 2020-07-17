using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using Xunit;

namespace Zplify.Tests
{
    public class GeneralTests
    {
        // Conversions should be identical so these should always remain the same.
        private static readonly List<string> LABEL_OUTPUTS = new List<string>
        {
            "should-rotate.png",
            "UPS-label.png",
            "USPS-label.png"
        };

        [Theory]
        [ClassData(typeof(LabelLoader))]
        public void Compression_IsLossless(string path)
        {
            var conv = new ZplImageConverter();
            var fileOutput = conv.BuildLabel(new Bitmap(path));
            var zplOutput = File.ReadAllText(Path.ChangeExtension(path, "zpl"));
            Assert.Equal(fileOutput, zplOutput);
        }
    }
}