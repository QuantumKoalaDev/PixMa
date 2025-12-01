using System;

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


        public static PixelColor FromHex(string hex)
        {
            hex = hex.TrimStart('#');

            if (hex.Length == 6)
            {
                byte r = Convert.ToByte(hex[..2], 16);
                byte g = Convert.ToByte(hex.Substring(2, 2), 16);
                byte b = Convert.ToByte(hex.Substring(4, 2), 16);
                
                return new PixelColor(r, g, b);
            }
            else if (hex.Length == 8)
            {
                byte a = Convert.ToByte(hex[..2], 16);
                byte r = Convert.ToByte(hex.Substring(2, 2), 16);
                byte g = Convert.ToByte(hex.Substring(4,2), 16);
                byte b = Convert.ToByte(hex.Substring(6, 2), 16);

                return new PixelColor(r, g, b, a);
            }
            else
            {
                //throw new ArgumentException("Invaid hex color format");
                return PixelColor.White;
            }
        }
    }
}
