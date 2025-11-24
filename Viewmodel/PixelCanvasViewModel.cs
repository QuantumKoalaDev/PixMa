using PixelPaintApp.Commands;
using PixelPaintApp.Domain;
using PixelPaintApp.UIModel;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Collections.Generic;
using System.Windows.Input;

namespace PixelPaintApp.Viewmodel
{
    public class PixelCanvasViewModel
    {
        private readonly List<(int dx, int dy)> _pixelOffsetList = [];
        private int _brushSize;

        private PixelCanvasModel _model;

        public ICommand SaveCommand { get; }
        public PixelColor BrushColor { get; set; }
        public PixelColor BackgroundColor { get; set; }

        public bool GridLinesActive { get; set; }

        public CanvasShape Shape => _model.Shape;

        public int BrushSize 
        { 
            get => _brushSize; 
            set
            {
                if (_brushSize == value)
                    return;

                _brushSize = value;
                RebuildPixelOffsetList();
            }
        }

        public PixelColor this[int x, int y] => _model[(uint)x,  (uint)y];

        public PixelCanvasViewModel()
        {
            uint standardHeight = 32;
            uint standardWidth = 32;

            _model = new PixelCanvasModel(standardHeight, standardWidth);
            BrushColor = PixelColor.Black;
            BrushSize = 1;
            BackgroundColor = PixelColor.White;
            GridLinesActive = true;
            SaveCommand = new RelayCommand(Save);
        }

        private void RebuildPixelOffsetList()
        {
            _pixelOffsetList.Clear();

            int size = _brushSize;

            if (size == 1)
                _pixelOffsetList.Add((0, 0));

            if (size % 2 == 0)
            {
                int half = (int)(size / 2);

                for (int i = 0; i < half; i++)
                {
                    for (int j = 0; j < half; j++)
                    {
                        _pixelOffsetList.Add((i, j));
                    }
                }
            }


            if (size % 3 == 0)
            {
                int half = (size- 1) / 2;


                for (int i = -half; i <= half; i++)
                {
                    for (int j = -half; j <= half; j++)
                    {
                        _pixelOffsetList.Add((i, j));

                    }
                }
            }

        }
        private bool DrawSingelPixel(int x, int y)
        {
            if (x < 0 || x >= _model.Shape.Width || y < 0 || y >= _model.Shape.Height)
                return false;

            _model[(uint)x, (uint)y] = BrushColor;
            return true;
        }
        private void Save()
        {
            try
            {
                uint height = Shape.Height;
                uint width = Shape.Width;

                using Image<Rgba32> img = new((int)width, (int)height);

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        PixelColor p = _model[(uint)x, (uint)y];
                        img[x, y] = new Rgba32(p.R, p.G, p.B, p.A);
                    }
                }

                img.Save("image.png");
            }
            catch
            {
                return ;
            }
        }

        public void ToggleGridLines() => GridLinesActive = !GridLinesActive;

        public bool DrawPixel(int x, int y)
        {
            foreach ((int dx, int dy) in  _pixelOffsetList)
            {
                DrawSingelPixel(dx + x, dy + y);
            }

            return true;
        }

        public void Save(string path)
        {
            try
            {
                uint height = Shape.Height;
                uint width = Shape.Width;

                using Image<Rgba32> img = new((int)width, (int)height);

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        PixelColor p = _model[(uint)x, (uint)y];
                        img[x, y] = new Rgba32(p.R, p.G, p.B, p.A);
                    }
                }

                img.Save("image.png");
            }
            catch
            {
                return;
            }
        }
    }
}
