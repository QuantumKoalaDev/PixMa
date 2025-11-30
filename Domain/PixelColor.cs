namespace PixelPaintApp.Domain
{
    public struct PixelColor
    {
        public byte R;
        public byte G;
        public byte B;
        public byte A;

        public PixelColor(byte red, byte green, byte blue, byte alpha)
        {
            R = red;
            G = green;
            B = blue;
            A = alpha;
        }

        public PixelColor(byte red, byte green, byte blue)
        {
            R = red;
            G = green;
            B = blue;
            A = 255;
        }

        public static readonly PixelColor Transparent = new(0, 0, 0, 0);
        public static readonly PixelColor Red = new(255, 0, 0);
        public static readonly PixelColor Green = new(0, 255, 0);
        public static readonly PixelColor Blue = new(0, 0, 255);
        public static readonly PixelColor Black = new(0, 0, 0);
        public static readonly PixelColor White = new(255, 255, 255);
    }
}
