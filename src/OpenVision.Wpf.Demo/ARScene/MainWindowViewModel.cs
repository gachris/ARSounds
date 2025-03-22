using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using OpenVision.Core.Reco.DataTypes;

namespace OpenVision.Wpf.Demo.ARScene;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly MainWindow _mainWindow;

    [ObservableProperty]
    private string? _targetId;

    public ObservableCollection<BitmapImage> LoadedImages { get; set; }

    public MainWindowViewModel(MainWindow mainWindow)
    {
        LoadedImages = new ObservableCollection<BitmapImage>();
        _mainWindow = mainWindow;
    }

    [RelayCommand]
    private void Load_Image_Button_Click()
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = "Image Files|*.jpg;*.jpeg;*.png;",
            Multiselect = true
        };

        if (openFileDialog.ShowDialog() != true)
        {
            return;
        }

        foreach (var filename in openFileDialog.FileNames)
        {
            try
            {
                var bitmap = new BitmapImage(new Uri(filename));
                LoadedImages.Add(bitmap);

                // Load image into EmguCV format
                var img = ImageData.Load(filename);
                _mainWindow._loadedImages.Add(img);
            }
            catch (Exception)
            {
            }
        }
    }
}