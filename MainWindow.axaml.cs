using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace PixelPaintApp;

using PixelPaintApp.View;
using SkiaSharp;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
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
    
    private void Black_Click(object sender, RoutedEventArgs e) => MyCanvas.SetColor(Colors.Black);
    private void Red_Click(object sender, RoutedEventArgs e) => MyCanvas.SetColor(Colors.Red);
    private void Green_Click(object sender, RoutedEventArgs e) => MyCanvas.SetColor(Colors.Green);
    private void Blue_Click(object sender, RoutedEventArgs e) => MyCanvas.SetColor(Colors.Blue);
    private void Eraser_Click(object sender, RoutedEventArgs e) => MyCanvas.SetColor(Colors.Transparent);
    private void PixelSize1_Click(object sender, RoutedEventArgs e) => MyCanvas.SetBrushSize(1);
    private void PixelSize4_Click(object sender, RoutedEventArgs e) => MyCanvas.SetBrushSize(4);
    private void PixelSize9_Click(object sender, RoutedEventArgs e) => MyCanvas.SetBrushSize(9);
    private void ToggleGridLines_Click(object sender, RoutedEventArgs e) => MyCanvas.ToggleGridLines();
    private void Save_Click(object sender, RoutedEventArgs e) => MyCanvas.Save();
}