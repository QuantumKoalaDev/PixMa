using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace PixelPaintApp.View;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        MyColorBar.ColorSelected = color => MyCanvas.SetColor(color);
    }

    private void Minimize_Click(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void Maximize_Click(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
    }

    private void Close_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void TitleBar_OnPointerPressed(object sender, PointerPressedEventArgs e)
    {
        if (!e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            return;

        if (e.ClickCount == 2)
        {
            Maximize_Click(sender, e);
            return;
        }
            
        
        BeginMoveDrag(e);
    }
    
    private void Resize_Right(object? sender, PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            BeginResizeDrag(WindowEdge.East, e);
    }
    
    private void Resize_Left(object? sender, PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            BeginResizeDrag(WindowEdge.West, e);
    }
    
    private void Resize_Bottom(object? sender, PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            BeginResizeDrag(WindowEdge.South, e);
    }
    
    // private void Resize_Top(object? sender, PointerPressedEventArgs e)
    // {
    //     if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
    //         BeginResizeDrag(WindowEdge.North, e);
    // }
    
    private void Eraser_Click(object sender, RoutedEventArgs e) => MyCanvas.SetColor(Colors.Transparent);
    private void PixelSize1_Click(object sender, RoutedEventArgs e) => MyCanvas.SetBrushSize(1);
    private void PixelSize4_Click(object sender, RoutedEventArgs e) => MyCanvas.SetBrushSize(4);
    private void PixelSize9_Click(object sender, RoutedEventArgs e) => MyCanvas.SetBrushSize(9);
    private void ToggleGridLines_Click(object sender, RoutedEventArgs e) => MyCanvas.ToggleGridLines();
    private void Save_Click(object sender, RoutedEventArgs e) => MyCanvas.Save();
    //private void OnPick_Click(object? sender, PointerPressedEventArgs e)
    //{
    //    Point pos = e.GetPosition(ColorBar);

    //    double t = Math.Clamp(pos.Y / ColorBar.Bounds.Height, 0, 1);

    //    var stops = new[]
    //    {
    //        (Offset: 0, Color: Colors.Magenta),
    //        (Offset: 0.17, Color: Colors.Blue),
    //        (Offset: 0.34, Color: Colors.Cyan),
    //        (Offset: 0.51, Color: Colors.Green),
    //        (Offset: 0.68, Color: Colors.Yellow),
    //        (Offset: 0.85, Color: Colors.Orange),
    //        (Offset: 1.00, Color: Colors.Red)
    //    };

    //    (double Offset, Color Color) left = stops[0], right = stops[^1];
    //    for (int i = 0; i < stops.Length - 1; i++)
    //    {
    //        if (t >= stops[i].Offset && t <= stops[i + 1].Offset)
    //        {
    //            left = stops[i];
    //            right = stops[i + 1];
    //            break;
    //        }
    //    }

    //    double localT = (t - left.Offset) / (right.Offset - left.Offset);

    //    byte r = (byte)(left.Color.R + (right.Color.R - left.Color.R) * localT);
    //    byte g = (byte)(left.Color.G + (right.Color.G - left.Color.G) * localT);
    //    byte b = (byte)(left.Color.B + (right.Color.B - left.Color.B) * localT);

    //    Color color = Color.FromRgb(r, g, b);
    //    MyCanvas.SetColor(color);
    //}
}