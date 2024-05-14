using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;
        SetSemiTransparentBackground();
    }
    public SolidColorBrush SemiTransparentBackground { get; set; }

    private void SetSemiTransparentBackground()
    {
        Color baseColor = Colors.White; // Base color
        byte alpha = 128;
        SemiTransparentBackground = GetSemiTransparentColor(baseColor, alpha);

    }

    // Helper method to create a semi-transparent color
    public static SolidColorBrush GetSemiTransparentColor(Color color, byte alpha)
    {
        return new SolidColorBrush(Color.FromArgb(alpha, color.R, color.G, color.B));
    }
    private void MinimizeButton_Click(object sender, RoutedEventArgs e)
    {
        this.WindowState = WindowState.Minimized;
    }

    private void MaximizeButton_Click(object sender, RoutedEventArgs e)
    {
        if (this.WindowState == WindowState.Maximized)
        {
            this.WindowState = WindowState.Normal;
            MainBorder.CornerRadius = new CornerRadius(20);
        }
        else
        {
            this.WindowState = WindowState.Maximized;
            MainBorder.CornerRadius = new CornerRadius(0);
        }
    }
    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private void SelectImageButton_Click(object sender, RoutedEventArgs e)
    {
        var openFileDialog = new Microsoft.Win32.OpenFileDialog
        {
            DefaultExt = ".png",
            Filter = "Image files (*.png;*.jpeg;*.jpg;*.bmp)|*.png;*.jpeg;*.jpg;*.bmp"
        };
    
        bool? result = openFileDialog.ShowDialog();
    
        if (result == true)
        {
            string filename = openFileDialog.FileName;
            ImageOutput.Source = new BitmapImage(new Uri(filename));
    
            (sender as Button).Visibility = Visibility.Collapsed;
    
            CancelButton.Visibility = Visibility.Visible;           
        }
    }

    private void ContentControl_DragEnter(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(DataFormats.FileDrop))
            e.Effects = DragDropEffects.Copy;
        else
            e.Effects = DragDropEffects.None;
    }
    
    private void ContentControl_Drop(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(DataFormats.FileDrop))
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files != null && files.Length > 0)
            {
                string filename = files[0];
                ImageContentControl.Content = new Image
                {
                    Source = new BitmapImage(new Uri(filename)),
                    Stretch = Stretch.UniformToFill
                };
            }
        }
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        ImageOutput.Source = null;

        CancelButton.Visibility = Visibility.Collapsed;
    
        SelectImageButton.Visibility = Visibility.Visible;
    }
}