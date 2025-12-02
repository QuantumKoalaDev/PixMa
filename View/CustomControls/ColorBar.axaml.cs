using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using System;

namespace PixelPaintApp.View.CustomControls;

public partial class ColorBar : UserControl
{
    public Action<Color>?  ColorSelected { get; set; }

    public ColorBar()
    {
        InitializeComponent();

        GradientBorder.PointerPressed += OnPick_Click;
    }

    private void OnPick_Click(object? sender, PointerEventArgs e)
    {
        if (sender is not Border border)
            return;

        Point pos = e.GetPosition(border);

        double t = Math.Clamp(pos.Y / border.Bounds.Height, 0, 1);

        var stops = new[]
        {
            (Offset: 0, Color: Colors.Magenta),
            (Offset: 0.17, Color: Colors.Blue),
            (Offset: 0.34, Color: Colors.Cyan),
            (Offset: 0.51, Color: Colors.Green),
            (Offset: 0.68, Color: Colors.Yellow),
            (Offset: 0.85, Color: Colors.Orange),
            (Offset: 1.00, Color: Colors.Red)
        };

        (double Offset, Color Color) left = stops[0], right = stops[^1];

        for (int i = 0; i < stops.Length - 1; i++)
        {
            if (t >= stops[i].Offset && t <= stops[i + 1].Offset)
            {
                left = stops[i];
                right = stops[i + 1];
                break;
            }
        }

        double localT = (t - left.Offset) / (right.Offset - left.Offset);

        byte r = (byte)(left.Color.R + (right.Color.R - left.Color.R) * localT);
        byte g = (byte)(left.Color.G + (right.Color.G - left.Color.G) * localT);
        byte b = (byte)(left.Color.B + (right.Color.B - left.Color.B) * localT);

        Color color = Color.FromRgb(r, g, b);

        ColorSelected?.Invoke(color);
    }    
}
