namespace Zplify
{
    public class Arguments
    {
        public string InPath { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public string OutPath { get; set; }
        public System.Drawing.Drawing2D.InterpolationMode InterpolationMode { get; set; } = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
    }
}
