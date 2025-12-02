using Avalonia.Controls;

namespace PixelPaintApp.View.CustomControls
{
    public partial class ColorInputDialog : Window
    {
        public ColorInputDialog()
        {
            InitializeComponent();
        }

        private void Ok_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Close(ColorTextBox.Text);
        }
    }
}
