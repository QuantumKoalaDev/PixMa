namespace PixelPaintApp.Domain
{
    public class CanvasShape
    {
        public uint Width { get; init; }
        public uint Height { get; init; }

        public CanvasShape(uint width, uint height)
        {
            Width = width;
            Height = height;
        }
    }
}
