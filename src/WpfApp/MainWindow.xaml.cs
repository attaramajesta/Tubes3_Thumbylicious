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
using System.ComponentModel;
using System.Data;
using System.IO;
using PatternMatch;
using PatternMatching;
using System.Diagnostics;

namespace WpfApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window, INotifyPropertyChanged
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;
        SetSemiTransparentBackground();
    }

    public string _algorithm;

    public string Algorithm
    {
        get { return _algorithm; }
        set
        {
            _algorithm = value;
            OnPropertyChanged("Algorithm");
        }
    }

    private string _finalImageString;
    public string FinalImageString
    {
        get { return _finalImageString; }
        set
        {
            _finalImageString = value;
            OnPropertyChanged(nameof(FinalImageString));
        }
    }
    
    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public SolidColorBrush SemiTransparentBackground { get; set; }

    private void SetSemiTransparentBackground()
    {
        Color baseColor = Colors.White;
        byte alpha = 128;
        SemiTransparentBackground = GetSemiTransparentColor(baseColor, alpha);

    }

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

    private void HomeButton_Click(object sender, RoutedEventArgs e)
    {
        home.Height = Double.NaN; 
        about.Height = 0;
        information.Height = 0;
    }
    
    private void AboutButton_Click(object sender, RoutedEventArgs e)
    {
        home.Height = 0;
        about.Height = Double.NaN;
        information.Height = 0;
    }
    
    private void InformationButton_Click(object sender, RoutedEventArgs e)
    {
        home.Height = 0;
        about.Height = 0;
        information.Height = Double.NaN; 
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
                ImageOutput.Source = new BitmapImage(new Uri(filename));
    
                SelectImageButton.Visibility = Visibility.Collapsed;
    
                CancelButton.Visibility = Visibility.Visible;

                ResultsTextBlock.Text = ""; 

                ResultsTextBlocks.Text = "";

                ResultImage.Source = null;

                Algorithm = "";
            }
        }
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        ImageOutput.Source = null;

        CancelButton.Visibility = Visibility.Collapsed;
    
        SelectImageButton.Visibility = Visibility.Visible;

        ResultsTextBlock.Text = ""; 

        ResultsTextBlocks.Text = "";

        ResultImage.Source = null;

        Algorithm = "";
    }

    private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
        ScrollViewer scv = (ScrollViewer)sender;
        scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta / 5); 
        e.Handled = true;
    }

    private void KMPButton_Click(object sender, RoutedEventArgs e)
    {
        _algorithm = "KMP";
    }
    
    private void BMButton_Click(object sender, RoutedEventArgs e)
    {
        _algorithm = "BM";
    }

    private void SubmitButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var bitmapImage = ImageOutput.Source as BitmapImage;
            string sourcePath = bitmapImage.UriSource.LocalPath;

            string destinationPath = System.IO.Path.Combine("input", System.IO.Path.GetFileName(sourcePath));

            System.IO.File.Copy(sourcePath, destinationPath, true);
            
            Algorithm = _algorithm;
        
            int totalPixels = 60;
            string filePath = destinationPath;
            string algorithm = _algorithm;
        
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
        
            PatternMatcher matcher = new PatternMatcher();
            List<Tuple<DataTable, double>> results = matcher.Match(totalPixels, filePath,  algorithm);

            DatabaseManager dbManager = new DatabaseManager();
            string finalImage = dbManager.GetOutputImage(results);

            if (!string.IsNullOrEmpty(finalImage))
            {
                string basePath = AppDomain.CurrentDomain.BaseDirectory;

                string parentDirectory = basePath;
                for (int i = 0; i < 4; i++)
                {
                    parentDirectory = System.IO.Directory.GetParent(parentDirectory).FullName;
                }

                string relativePath = System.IO.Path.Combine(parentDirectory, finalImage);

                if (System.IO.File.Exists(relativePath))
                {
                    ResultImage.Source = new BitmapImage(new Uri(relativePath, UriKind.Absolute));
                }
                else
                {
                    MessageBox.Show($"Image not found or invalid path: {relativePath}");
                }
            }
            else
            {
                MessageBox.Show("Image path is empty.");
            }
           
            stopwatch.Stop();
            TimeSpan ts = stopwatch.Elapsed;
        
            if (results.Count == 0)
            {
                ResultsTextBlock.Text = "No results found.\n";
            }
            if (results.Any())
            {
                DataRow row = results[0].Item1.Rows[0]; 
                ResultsTextBlock.Text += $"NIK: {row["NIK"]}\n"
                                        + $"Nama: {row["nama"]}\n"
                                        + $"Tempat Lahir: {row["tempat_lahir"]}\n"
                                        + $"Tanggal Lahir: {row["tanggal_lahir"]}\n"
                                        + $"Jenis Kelamin: {row["jenis_kelamin"]}\n"
                                        + $"Golongan Darah: {row["golongan_darah"]}\n"
                                        + $"Alamat: {row["alamat"]}\n"
                                        + $"Agama: {row["agama"]}\n"
                                        + $"Status Perkawinan: {row["status_perkawinan"]}\n"
                                        + $"Pekerjaan: {row["pekerjaan"]}\n"
                                        + $"Kewarganegaraan: {row["kewarganegaraan"]}\n\n";
            }

            ResultsTextBlock.Text += $"Similarity: {results[0].Item2} %\n";
           
            ResultsTextBlock.Text += $"Execution Time: {ts.TotalMilliseconds} ms\n";

            for (int i = 1; i < Math.Min(5, results.Count); i++) 
            {
                DataRow row = results[i].Item1.Rows[0]; 
                ResultsTextBlocks.Text += $"RESULT {i+1}:\n\n"
                                        + $"NIK: {row["NIK"]}\n"
                                        + $"Nama: {row["nama"]}\n"
                                        + $"Tempat Lahir: {row["tempat_lahir"]}\n"
                                        + $"Tanggal Lahir: {row["tanggal_lahir"]}\n"
                                        + $"Jenis Kelamin: {row["jenis_kelamin"]}\n"
                                        + $"Golongan Darah: {row["golongan_darah"]}\n"
                                        + $"Alamat: {row["alamat"]}\n"
                                        + $"Agama: {row["agama"]}\n"
                                        + $"Status Perkawinan: {row["status_perkawinan"]}\n"
                                        + $"Pekerjaan: {row["pekerjaan"]}\n"
                                        + $"Kewarganegaraan: {row["kewarganegaraan"]}\n\n";
                ResultsTextBlocks.Text += $"Similarity: {results[i].Item2} %\n\n";
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An error occurred: {ex.Message}");
        }
    }
}