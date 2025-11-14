using System;
using Avalonia.Reactive;

namespace  PixelPaintApp;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;

public class PixelCanvas : Control
{
    private Color[,] _pixels;
    private Color _currentColor = Colors.Black;
    
    private static readonly StyledProperty<int> CanvasWidthProperty =
        AvaloniaProperty.Register<PixelCanvas, int>(nameof(CanvasWidth), 32);
    
    private static readonly StyledProperty<int> CanvasHeightProperty = 
        AvaloniaProperty.Register<PixelCanvas, int>(nameof(CanvasHeight), 32);
    
    private static readonly StyledProperty<int> PixelSizeProperty =
        AvaloniaProperty.Register<PixelCanvas, int>(nameof(Pixel), 32);

    private static readonly StyledProperty<Color> BackgroundColorProperty =
        AvaloniaProperty.Register<PixelCanvas, Color>(nameof(Background), Colors.White);

    public PixelCanvas()
    {
        _pixels = new Color[GetValue(CanvasWidthProperty), GetValue(CanvasHeightProperty)];
        Focusable = true;

        this.GetPropertyChangedObservable(CanvasWidthProperty)
            .Subscribe(new AnonymousObserver<AvaloniaPropertyChangedEventArgs>(_ => Resize()));

        this.GetPropertyChangedObservable(CanvasHeightProperty)
            .Subscribe(new AnonymousObserver<AvaloniaPropertyChangedEventArgs>(_ => Resize()));

        
        PointerPressed += OnPointerPressed;
        PointerMoved += OnPointerMoved;
    }
    
    public int CanvasWidth
    {
        get => GetValue(CanvasWidthProperty);
        set => SetValue(CanvasWidthProperty, value);
    }

    public int CanvasHeight
    {
        get => GetValue(CanvasHeightProperty);
        set => SetValue(CanvasHeightProperty, value);
    }

    public int Pixel
    {
        get => GetValue(PixelSizeProperty);
        set => SetValue(PixelSizeProperty, value);
    }

    public Color Background
    {
        get => GetValue(BackgroundColorProperty);
        set => SetValue(BackgroundColorProperty, value);
    }

    private void Resize()
    {
        _pixels = new Color[GetValue(CanvasWidthProperty), GetValue(CanvasHeightProperty)];
        InvalidateMeasure();
        InvalidateVisual();
    }

    public void SetColor(Color color) => _currentColor = color;

    private void DrawPixel(int x, int y)
    {
        if (x < 0 || x >= GetValue(CanvasWidthProperty) || y < 0 || y >= GetValue(CanvasHeightProperty))
            return;

        _pixels[x, y] = _currentColor;
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

    public override void Render(DrawingContext context)
    {
        // Wichtig: das erzeugt eine sichtbare Fläche für Pointer Events
        context.FillRectangle(Brushes.Transparent, new Rect(Bounds.Size));

        for (int x = 0; x < GetValue(CanvasWidthProperty); x++)
        {
            for (int y = 0; y < GetValue(CanvasHeightProperty); y++)
            {
                Color color = _pixels[x, y];
                if (color.A == 0)
                {
                    Rect rectInvis = new(x * GetValue(PixelSizeProperty), y * GetValue(PixelSizeProperty), GetValue(PixelSizeProperty) , GetValue(PixelSizeProperty));
                    context.FillRectangle(new SolidColorBrush(GetValue(BackgroundColorProperty)), rectInvis); 
                    continue;
                }

                Rect rect = new(x * GetValue(PixelSizeProperty), y * GetValue(PixelSizeProperty), GetValue(PixelSizeProperty) , GetValue(PixelSizeProperty));
                context.FillRectangle(new SolidColorBrush(color), rect);
            }
        }
        
        Pen pen = new(Brushes.Gray, 0.5);
        int pixelSize =  this.GetValue(PixelSizeProperty);
        int canvasWidth = this.GetValue(CanvasWidthProperty) * pixelSize;
        int canvasHeight = this.GetValue(CanvasHeightProperty) * pixelSize;
        
        for (int x = 0; x <= GetValue(CanvasWidthProperty); x++)
            context.DrawLine(pen, new Point(x * pixelSize, 0), new Point(x * pixelSize, canvasHeight));
        
        for (int y = 0; y <= GetValue(CanvasHeightProperty); y++)
            context.DrawLine(pen, new Point(0, y * pixelSize), new Point(canvasWidth, y * pixelSize));

    }

}

// Brushes:
// 1px, 4px, 9 px
// Rasterlinien zum ein und ausschalten