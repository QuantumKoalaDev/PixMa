using Avalonia.Controls.Documents;
using PixelPaintApp.Domain;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;

namespace PixelPaintApp.UIModel
{
    public class PixelCanvasModel
    {
        private PixelColor[,] _pixels;
        
        public CanvasShape Shape { get; init; }

        // rows => height aussen, cols => width innen
        public PixelCanvasModel(uint height, uint width)
        {
            Shape = new CanvasShape(width, height);
            _pixels = new PixelColor[height, width];
        }
        
        public PixelColor[,] GetPixels() { return this._pixels; }

        public PixelColor this[uint x, uint y]
        {
            get => _pixels[y, x];
            set => _pixels[y, x] = value;
        }
    }
}