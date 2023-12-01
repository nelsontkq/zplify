using System;
using System.Runtime.InteropServices;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using System.Text;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Transforms;

namespace Zplify;

public class ZplImageConverter
{
    private const int BLACK_LIMIT = 125;

    private static readonly string[] MapHex =
    {
            "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "g", "gG",
            "gH", "gI", "gJ", "gK", "gL", "gM", "gN", "gO", "gP", "gQ", "gR", "gS", "gT", "gU", "gV", "gW", "gX", "gY",
            "h", "hG", "hH", "hI", "hJ", "hK", "hL", "hM", "hN", "hO", "hP", "hQ", "hR", "hS", "hT", "hU", "hV", "hW",
            "hX", "hY", "i", "iG", "iH", "iI", "iJ", "iK", "iL", "iM", "iN", "iO", "iP", "iQ", "iR", "iS", "iT", "iU",
            "iV", "iW", "iX", "iY", "j", "jG", "jH", "jI", "jJ", "jK", "jL", "jM", "jN", "jO", "jP", "jQ", "jR", "jS",
            "jT", "jU", "jV", "jW", "jX", "jY", "k", "kG", "kH", "kI", "kJ", "kK", "kL", "kM", "kN", "kO", "kP", "kQ",
            "kR", "kS", "kT", "kU", "kV", "kW", "kX", "kY", "l", "lG", "lH", "lI", "lJ", "lK", "lL", "lM", "lN", "lO",
            "lP", "lQ", "lR", "lS", "lT", "lU", "lV", "lW", "lX", "lY", "m", "mG", "mH", "mI", "mJ", "mK", "mL", "mM",
            "mN", "mO", "mP", "mQ", "mR", "mS", "mT", "mU", "mV", "mW", "mX", "mY", "n", "nG", "nH", "nI", "nJ", "nK",
            "nL", "nM", "nN", "nO", "nP", "nQ", "nR", "nS", "nT", "nU", "nV", "nW", "nX", "nY", "o", "oG", "oH", "oI",
            "oJ", "oK", "oL", "oM", "oN", "oO", "oP", "oQ", "oR", "oS", "oT", "oU", "oV", "oW", "oX", "oY", "p", "pG",
            "pH", "pI", "pJ", "pK", "pL", "pM", "pN", "pO", "pP", "pQ", "pR", "pS", "pT", "pU", "pV", "pW", "pX", "pY",
            "q", "qG", "qH", "qI", "qJ", "qK", "qL", "qM", "qN", "qO", "qP", "qQ", "qR", "qS", "qT", "qU", "qV", "qW",
            "qX", "qY", "r", "rG", "rH", "rI", "rJ", "rK", "rL", "rM", "rN", "rO", "rP", "rQ", "rR", "rS", "rT", "rU",
            "rV", "rW", "rX", "rY", "s", "sG", "sH", "sI", "sJ", "sK", "sL", "sM", "sN", "sO", "sP", "sQ", "sR", "sS",
            "sT", "sU", "sV", "sW", "sX", "sY", "t", "tG", "tH", "tI", "tJ", "tK", "tL", "tM", "tN", "tO", "tP", "tQ",
            "tR", "tS", "tT", "tU", "tV", "tW", "tX", "tY", "u", "uG", "uH", "uI", "uJ", "uK", "uL", "uM", "uN", "uO",
            "uP", "uQ", "uR", "uS", "uT", "uU", "uV", "uW", "uX", "uY", "v", "vG", "vH", "vI", "vJ", "vK", "vL", "vM",
            "vN", "vO", "vP", "vQ", "vR", "vS", "vT", "vU", "vV", "vW", "vX", "vY", "w", "wG", "wH", "wI", "wJ", "wK",
            "wL", "wM", "wN", "wO", "wP", "wQ", "wR", "wS", "wT", "wU", "wV", "wW", "wX", "wY", "x", "xG", "xH", "xI",
            "xJ", "xK", "xL", "xM", "xN", "xO", "xP", "xQ", "xR", "xS", "xT", "xU", "xV", "xW", "xX", "xY", "y", "yG",
            "yH", "yI", "yJ", "yK", "yL", "yM", "yN", "yO", "yP", "yQ", "yR", "yS", "yT", "yU", "yV", "yW", "yX", "yY",
            "z", "zG", "zH", "zI", "zJ", "zK", "zL", "zM", "zN", "zO", "zP", "zQ", "zR", "zS", "zT", "zU", "zV", "zW",
            "zX", "zY"
        };

    private readonly int _height;
    private readonly int _width;

    public ZplImageConverter()
    {
        _height = 1200;
        _width = 800;
    }

    public ZplImageConverter(int length, int width)
    {
        _height = length;
        _width = width;
    }

    public string BuildLabel(Image<Rgba32> image)
    {
        var img = ScaleAndRotateImage(image, _width, _height);
        var widthBytes = GetImageWidthInBytes(img);
        var total = widthBytes * img.Height;
        var body = GetImageBytes(img);
        return "^XA^FO0,0" // Start of header
               +
               string.Join(',', "^GFA", total, total, widthBytes) +
               "," // Graphic line declaration
               +
               CompressHex(Convert.ToHexString(body), widthBytes)
               +
               "^FS^XZ"; // closing
    }

    public static Image<Rgba32> ScaleAndRotateImage(Image<Rgba32> image, int width, int height)
    {
        bool shouldRotate = Math.Abs(image.Width - width) + Math.Abs(image.Height - height) >
                            Math.Abs(image.Height - width) + Math.Abs(image.Width - height);

        if (shouldRotate)
        {
            image.Mutate(x => x.Rotate(RotateMode.Rotate90));
        }
        image.Mutate(x => x.Resize(new ResizeOptions
        {
            Size = new Size(width, height),
            Sampler = new LanczosResampler(3),
        }));
        return image;
    }

    public static int GetImageWidthInBytes(Image<Rgba32> originalImage)
    {
        var width = originalImage.Width;
        int widthBytes;
        if (width % 8 > 0)
            widthBytes = width / 8 + 1;
        else
            widthBytes = width / 8;
        return widthBytes;
    }

    public static byte[] GetImageBytes(Image<Rgba32> image)
    {
        var index = 7;
        var currentByte = 0b0000_0000;
        var bytes = new byte[image.Height * image.Width / 8];
        var byteIndex = 0;

        for (int y = 0; y < image.Height; y++)
        {
            for (int x = 0; x < image.Width; x++)
            {
                Rgba32 pixel = image[x, y];
                int avgColor = (pixel.R + pixel.G + pixel.B) / 3;

                if (avgColor < BLACK_LIMIT && pixel.A >= BLACK_LIMIT)
                {
                    currentByte |= (1 << index);
                }

                index--;

                if (index == -1 || x == image.Width - 1)
                {
                    bytes[byteIndex++] = (byte)currentByte;
                    index = 7;
                    currentByte = 0b0000_0000;
                }
            }
        }
        return bytes;
    }

    public static string CompressHex(string code, int widthBytes)
    {
        var maxLineLength = widthBytes * 2;

        var sbCode = new StringBuilder();
        var sbLinea = new StringBuilder();
        string previousLine = null;
        var counter = 1;
        var firstChar = code[0];
        for (var i = 1; i < code.Length; i++)
        {
            if (i % maxLineLength == 0 || i == code.Length - 1)
            {
                if (counter >= maxLineLength - 1 && firstChar == '0')
                    sbLinea.Append(',');
                else if (counter >= maxLineLength - 1 && firstChar == 'F')
                    sbLinea.Append('!');
                else
                    sbLinea.Append(MapHex[counter - 1] + firstChar);
                if (sbLinea.ToString().Equals(previousLine))
                    sbCode.Append(':');
                else
                    sbCode.Append(sbLinea);
                previousLine = sbLinea.ToString();
                sbLinea.Clear();
                firstChar = code[i];
                counter = 0;
            }

            if (firstChar == code[i])
            {
                counter++;
            }
            else
            {
                sbLinea.Append(MapHex[counter - 1] + firstChar);
                counter = 1;
                firstChar = code[i];
            }
        }

        return sbCode.ToString();
    }
}
