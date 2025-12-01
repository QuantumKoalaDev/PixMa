using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using PixelPaintApp.UIAdapter;
using PixelPaintApp.View.CustomControls;

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
    private void ColorInput_Click(object sender, RoutedEventArgs e)
    {
        ShowColorInputDialog();
    }


    private async void ShowColorInputDialog()
    {
        Window dialog = new Window
        {
            Title = "Farbcode eigeben",
            Width = 200,
            Height = 150,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            CanResize = false,
            SystemDecorations = SystemDecorations.None,
        };

        TextBox textbox = new TextBox { Width = 150, Watermark = "#RRGGBB" };
        Button okButton = new Button { Content = "ok" };
        okButton.Click += (_, _) => dialog.Close(textbox.Text);

        dialog.Content = new StackPanel
        {
            Orientation = Avalonia.Layout.Orientation.Vertical,
            Margin = new Thickness(10),
            Children = { textbox, okButton }
        };

        string? result = await dialog.ShowDialog<string>(this);

        if (!string.IsNullOrEmpty(result))
        {
            MyCanvas.SetColor(AvaloniaAdapter.ConvertToAvaloniaColor(Domain.PixelColor.FromHex(result)));
        }
    }
}