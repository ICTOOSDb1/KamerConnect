using Microsoft.Maui.Storage;
using Microsoft.UI.Xaml.Input;
using KamerConnect.DataAccess.Postgres;
using KamerConnect.DataAccess.Minio;
using KamerConnect.Repositories;
using KamerConnect.Services;

namespace KamerConnect.View.MAUI.Views;

public partial class UpdateAccountsForm : ContentView
{

    private const string bucketName = "profilepictures";

    private readonly FileService _fileService;

    public UpdateAccountsForm(FileService fileService)
    {
        _fileService = fileService;
        InitializeComponent();
    }

    private async void Image_tapped(object sender, TappedEventArgs e)
    {
        var result = await FilePicker.Default.PickAsync(new PickOptions
        {
            PickerTitle = "Select an image",
            FileTypes = FilePickerFileType.Images
        });

        if (result != null)
        {
            //datetime nog toevoegen bij filename
            string filePath = result.FullPath;
            var fileBytes = await File.ReadAllBytesAsync(filePath);
            string fileName = Path.GetFileName(filePath)+DateTime.Now.ToString("yyyyMMddHHmmss");
            string contentType = GetContentType(fileName);

            await _fileService.UploadFileAsync(bucketName, fileName, fileBytes, contentType);
        }
    }
    private void Button_Clicked(object sender, EventArgs e)
    {

    }


    private string GetContentType(string fileName)
    {
        var extension = Path.GetExtension(fileName)?.ToLowerInvariant();

        return extension switch
        {
            ".png" => "image/png",
            ".jpg" or ".jpeg" => "image/jpeg",
            ".gif" => "image/gif",
            ".bmp" => "image/bmp",
            ".tiff" => "image/tiff",
            _ => "application/octet-stream"
        };
    }
}
