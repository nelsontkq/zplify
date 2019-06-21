using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace zplify
{
    public class ZplImageConverter
    {
        private const int BLACK_LIMIT = 125;
        private static readonly uint[] _lookup32Unsafe = CreateHexLookup();
        private static unsafe readonly uint* _lookup32UnsafeP
            = (uint*)GCHandle.Alloc(_lookup32Unsafe, GCHandleType.Pinned).AddrOfPinnedObject();
        private readonly int _length;
        private readonly int _width;

        private static string[] MapHex => CreateHexCompressionMapping(); //new[] { "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "g", "gG", "gH", "gI", "gJ", "gK", "gL", "gM", "gN", "gO", "gP", "gQ", "gR", "gS", "gT", "gU", "gV", "gW", "gX", "gY", "h", "hG", "hH", "hI", "hJ", "hK", "hL", "hM", "hN", "hO", "hP", "hQ", "hR", "hS", "hT", "hU", "hV", "hW", "hX", "hY", "i", "iG", "iH", "iI", "iJ", "iK", "iL", "iM", "iN", "iO", "iP", "iQ", "iR", "iS", "iT", "iU", "iV", "iW", "iX", "iY", "j", "jG", "jH", "jI", "jJ", "jK", "jL", "jM", "jN", "jO", "jP", "jQ", "jR", "jS", "jT", "jU", "jV", "jW", "jX", "jY", "k", "kG", "kH", "kI", "kJ", "kK", "kL", "kM", "kN", "kO", "kP", "kQ", "kR", "kS", "kT", "kU", "kV", "kW", "kX", "kY", "l", "lG", "lH", "lI", "lJ", "lK", "lL", "lM", "lN", "lO", "lP", "lQ", "lR", "lS", "lT", "lU", "lV", "lW", "lX", "lY", "m", "mG", "mH", "mI", "mJ", "mK", "mL", "mM", "mN", "mO", "mP", "mQ", "mR", "mS", "mT", "mU", "mV", "mW", "mX", "mY", "n", "nG", "nH", "nI", "nJ", "nK", "nL", "nM", "nN", "nO", "nP", "nQ", "nR", "nS", "nT", "nU", "nV", "nW", "nX", "nY", "o", "oG", "oH", "oI", "oJ", "oK", "oL", "oM", "oN", "oO", "oP", "oQ", "oR", "oS", "oT", "oU", "oV", "oW", "oX", "oY", "p", "pG", "pH", "pI", "pJ", "pK", "pL", "pM", "pN", "pO", "pP", "pQ", "pR", "pS", "pT", "pU", "pV", "pW", "pX", "pY", "q", "qG", "qH", "qI", "qJ", "qK", "qL", "qM", "qN", "qO", "qP", "qQ", "qR", "qS", "qT", "qU", "qV", "qW", "qX", "qY", "r", "rG", "rH", "rI", "rJ", "rK", "rL", "rM", "rN", "rO", "rP", "rQ", "rR", "rS", "rT", "rU", "rV", "rW", "rX", "rY", "s", "sG", "sH", "sI", "sJ", "sK", "sL", "sM", "sN", "sO", "sP", "sQ", "sR", "sS", "sT", "sU", "sV", "sW", "sX", "sY", "t", "tG", "tH", "tI", "tJ", "tK", "tL", "tM", "tN", "tO", "tP", "tQ", "tR", "tS", "tT", "tU", "tV", "tW", "tX", "tY", "u", "uG", "uH", "uI", "uJ", "uK", "uL", "uM", "uN", "uO", "uP", "uQ", "uR", "uS", "uT", "uU", "uV", "uW", "uX", "uY", "v", "vG", "vH", "vI", "vJ", "vK", "vL", "vM", "vN", "vO", "vP", "vQ", "vR", "vS", "vT", "vU", "vV", "vW", "vX", "vY", "w", "wG", "wH", "wI", "wJ", "wK", "wL", "wM", "wN", "wO", "wP", "wQ", "wR", "wS", "wT", "wU", "wV", "wW", "wX", "wY", "x", "xG", "xH", "xI", "xJ", "xK", "xL", "xM", "xN", "xO", "xP", "xQ", "xR", "xS", "xT", "xU", "xV", "xW", "xX", "xY", "y", "yG", "yH", "yI", "yJ", "yK", "yL", "yM", "yN", "yO", "yP", "yQ", "yR", "yS", "yT", "yU", "yV", "yW", "yX", "yY", "z", "zG", "zH", "zI", "zJ", "zK", "zL", "zM", "zN", "zO", "zP", "zQ", "zR", "zS", "zT", "zU", "zV", "zW", "zX", "zY" };
        private static string[] CreateHexCompressionMapping()
        {
            var returnResult = new string[419];
            const string CAPITAL_LETTERS = "GHIJKLMNOPQRSTUVWXY";
            const string LOWERCASE_LETTERS = "ghijklmnopqrstuvwxyz";
            int returnIndex = 0;
            for (int i = 0; i < CAPITAL_LETTERS.Length; i++)
            {
                returnResult[returnIndex++] = new string(CAPITAL_LETTERS[i], 1);
            }
            for (int i = 0; i < LOWERCASE_LETTERS.Length; i++)
            {
                for (int j = 0; j < CAPITAL_LETTERS.Length; j++)
                {
                    if (j % 20 == 0)
                    {
                        returnResult[returnIndex++] = new string(LOWERCASE_LETTERS[i], 1);
                    }
                    returnResult[returnIndex++] = new string(new[] { LOWERCASE_LETTERS[i], CAPITAL_LETTERS[j] });
                }
            }
            return returnResult;
        }
        private static uint[] CreateHexLookup()
        {
            var result = new uint[256];
            for (int i = 0; i < 256; i++)
            {
                string s = i.ToString("X2");
                if (BitConverter.IsLittleEndian)
                    result[i] = ((uint)s[0]) + ((uint)s[1] << 16);
                else
                    result[i] = ((uint)s[1]) + ((uint)s[0] << 16);
            }
            return result;
        }
        public ZplImageConverter()
        {
            _length = 1200;
            _width = 800;
        }
        public ZplImageConverter(int length, int width)
        {
            _length = length;
            _width = width;
        }
        public unsafe string ByteArrayToHex(byte[] bytes)
        {
            var lookupP = _lookup32UnsafeP;
            var result = new char[bytes.Length * 2];
            fixed (byte* bytesP = bytes)
            fixed (char* resultP = result)
            {
                uint* resultP2 = (uint*)resultP;
                for (int i = 0; i < bytes.Length; i++)
                {
                    resultP2[i] = lookupP[bytesP[i]];
                }
            }
            return new string(result);
        }
        public string BuildLabel(Image image)
        {
            using (var bmp = ScaleBitmap(image))
            {
                var widthBytes = GetImageWidthInBytes(bmp);
                var total = widthBytes * bmp.Height;
                var body = ConvertBitmapToHex(bmp);
                return "^XA^FO0,0" // Start of header
                     + "^GFA," + total + "," + total + "," + widthBytes + "," // Graphic line declaration
                     + CompressHex(body, widthBytes) // Hex body compressed
                     + "^FS^XZ"; // closing
            }
        }
        public Bitmap ScaleBitmap(Image image)
        {
            var bitmap = new Bitmap(image, _width, _length);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            }
            return bitmap;
        }
        public int GetImageWidthInBytes(Bitmap originalImage)
        {
            int width = originalImage.Width;
            int widthBytes;
            if ((width % 8) > 0)
            {
                widthBytes = (width / 8) + 1;
            }
            else
            {
                widthBytes = width / 8;
            }
            return widthBytes;
        }
        public string ConvertBitmapToHex(Bitmap originalImage)
        {
            var graphics = Graphics.FromImage(originalImage);
            graphics.DrawImage(originalImage, 0, 0);
            int index = 7;
            int current = 0b0000_0000;
            var bytes = new byte[originalImage.Height * originalImage.Width / 8];
            int i = 0;
            for (int h = 0; h < originalImage.Height; h++)
            {
                for (int w = 0; w < originalImage.Width; w++)
                {
                    var rgb = originalImage.GetPixel(w, h);
                    int totalColor = (rgb.R + rgb.G + rgb.B) / 3;

                    if (totalColor < BLACK_LIMIT && rgb.A >= BLACK_LIMIT)
                    {
                        current |= 1 << index;
                    }
                    index--;

                    if (index == -1 || w == originalImage.Width - 1)
                    {
                        bytes[i++] = (byte)current;
                        index = 7;
                        current = 0b0000_0000;
                    }
                }
            }
            return ByteArrayToHex(bytes);
        }
        public string CompressHex(string code, int widthBytes)
        {
            int maxLineLength = widthBytes * 2;

            var sbCode = new StringBuilder();
            var sbLinea = new StringBuilder();
            string previousLine = null;
            var counter = 1;
            var firstChar = code[0];
            for (int i = 1; i < code.Length; i++)
            {
                if (i % maxLineLength == 0 || i == code.Length - 1)
                {
                    if (counter >= maxLineLength - 1 && firstChar == '0')
                    {
                        sbLinea.Append(",");
                    }
                    else if (counter >= maxLineLength - 1 && firstChar == 'F')
                    {
                        sbLinea.Append("!");
                    }
                    else
                    {
                        sbLinea.Append(MapHex[counter - 1] + firstChar);
                    }
                    if (sbLinea.ToString().Equals(previousLine))
                    {
                        sbCode.Append(":");
                    }
                    else
                    {
                        sbCode.Append(sbLinea.ToString());
                    }
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
}