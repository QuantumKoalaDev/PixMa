namespace  PixelPaintApp.View;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using PixelPaintApp.UIAdapter;
using PixelPaintApp.Viewmodel;
using System;
using System.Runtime.CompilerServices;

public class PixelCanvas : Control
{
    private PixelCanvasViewModel _viewModel;
    
    private static readonly StyledProperty<int> PixelSizeProperty =
        AvaloniaProperty.Register<PixelCanvas, int>(nameof(Pixel), 32);

    public static readonly StyledProperty<bool> GridLinesActiveProperty =
        AvaloniaProperty.Register<PixelCanvas, bool>(nameof(GridLinesActive), true);


    public PixelCanvas()
    {
        _viewModel = new PixelCanvasViewModel();

        Focusable = true;
        
        PointerPressed += OnPointerPressed;
        PointerMoved += OnPointerMoved;
        AttachedToVisualTree += OnAttachToVisualTree;
    }

    public int Pixel
    {
        get => GetValue(PixelSizeProperty);
        set => SetValue(PixelSizeProperty, value);
    }

    public bool GridLinesActive
    {
        get => GetValue(GridLinesActiveProperty);
        set
        {
            SetValue(GridLinesActiveProperty, value);
            _viewModel.GridLinesActive = value;
        }
    }

    //private void Resize()
    //{
    //    _pixels = new Color[GetValue(CanvasWidthProperty), GetValue(CanvasHeightProperty)];
    //    InvalidateMeasure();
    //    InvalidateVisual();
    //}

    public void SetColor(Color color) => _viewModel.BrushColor = AvaloniaAdapter.ConvertToInternColor(color);

    public void SetBrushSize(int size) => _viewModel.BrushSize = size;

    public void ToggleGridLines()
    {
        _viewModel.ToggleGridLines();
        InvalidateVisual();
    }

    public void Save() => _viewModel.Save("image.png");

    private void DrawPixel(int x, int y)
    {
        bool result = this._viewModel.DrawPixel(x, y);

       if (result) 
            InvalidateVisual();
    }

    private void OnPointerPressed(object sender, PointerPressedEventArgs e)
    {
        Point pos = e.GetPosition(this);
        int x = (int)(pos.X / GetValue(PixelSizeProperty));
        int y = (int)(pos.Y / GetValue(PixelSizeProperty));
        DrawPixel(x, y);
    }

    private void OnPointerMoved(object sender, PointerEventArgs e)
    {
        if (!e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            return;
        
        Point pos = e.GetPosition(this);
        int x = (int)(pos.X / GetValue(PixelSizeProperty));
        int y = (int)(pos.Y / GetValue(PixelSizeProperty));
        DrawPixel(x, y);
    }

    private void OnAttachToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
    {
        if (this.Parent is Control parent)
        {
            parent.SizeChanged += OnParentSizeChanged;
        }
    }

    private void OnParentSizeChanged(object? sender, SizeChangedEventArgs e)
    {
        double parentWidth = e.NewSize.Width;
        double parentHeight = e.NewSize.Height;

        UpdateCanvasSize(parentWidth, parentHeight);
    }

    private void UpdateCanvasSize(double parentWidth, double parentHeight)
    {
        if (parentWidth < parentHeight)
        {
            double half = parentWidth / 2;
            uint halfPixels = _viewModel.Shape.Width / 2;
            SetValue(PixelSizeProperty, half / halfPixels);
            return;
        }

        if (parentHeight < parentWidth)
        {
            double half = parentHeight / 2;
            uint halfPixels = _viewModel.Shape.Height / 2;
            SetValue(PixelSizeProperty, (int)(half / halfPixels) - 2);
        }
    }

    private void RenderGridLines(DrawingContext context)
    {
        uint width = _viewModel.Shape.Width;
        uint height = _viewModel.Shape.Height;

        Pen pen = new(Brushes.Gray, 0.5);
        int pixelSize = GetValue(PixelSizeProperty);
        uint canvasWidth = width * (uint)pixelSize;
        uint canvasHeight = height * (uint)pixelSize;

        for (int x = 0; x <= width; x++)
            context.DrawLine(pen, new Point(x * pixelSize, 0), new Point(x * pixelSize, canvasHeight));

        for (int y = 0; y <= height; y++)
            context.DrawLine(pen, new Point(0, y * pixelSize), new Point(canvasWidth, y * pixelSize));
    }

    public override void Render(DrawingContext context)
    {
        // Wichtig: das erzeugt eine sichtbare Fläche für Pointer Events
        context.FillRectangle(Brushes.Transparent, new Rect(Bounds.Size));

        uint height = _viewModel.Shape.Height;
        uint width = _viewModel.Shape.Width;
        int pixelSize = GetValue(PixelSizeProperty);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color color = AvaloniaAdapter.ConvertToAvaloniaColor(_viewModel[x, y]);
                if (color.A == 0)
                {
                    Rect rectInvis = new(x * pixelSize, y * pixelSize, pixelSize, pixelSize);
                    context.FillRectangle(new SolidColorBrush(AvaloniaAdapter.ConvertToAvaloniaColor(_viewModel.BackgroundColor)), rectInvis);
                    continue;
                }

                Rect rect = new(x * pixelSize, y * pixelSize, pixelSize , pixelSize);
                context.FillRectangle(new SolidColorBrush(color), rect);
            }
        }
        
        if (_viewModel.GridLinesActive)
            RenderGridLines(context);
    }

}

// Brushes:
// 1px, 4px, 9 px
// Rasterlinien zum ein und ausschalten