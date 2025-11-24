using PixelPaintApp.Domain;
using Avalonia.Media;


namespace PixelPaintApp.UIAdapter
{
    public class AvaloniaAdapter
    {
        public static Color ConvertToAvaloniaColor(PixelColor color)
        {
            return new Color(color.A, color.R, color.G, color.B);
        }

        public static Color[,] ConvertToAvaloniaColor(PixelColor[,] colors)
        {
            int height = colors.GetLength(0);
            int width = colors.GetLength(1);

            Color[,] pixels = new Color[height, width];

            for (int colId = 0; colId < width; colId++)
            {
                for (int rowId = 0; rowId < height; rowId++)
                {
                    pixels[colId, rowId] = ConvertToAvaloniaColor(colors[colId, rowId]);
                }
            }

            return pixels;
        }
        public static PixelColor ConvertToInternColor(Color color)
        {
            return new PixelColor(color.R, color.G, color.B, color.A);
        }
    }


}
