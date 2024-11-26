using KamerConnect.Services;
using KamerConnect.Utils;
using KamerConnect.View.MAUI.Utils;
using LukeMauiFilePicker;

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

    private async void Image_tapped(object sender, EventArgs e)
    {
        IFilePickerService picker = new FilePickerService();

        var result = await picker.PickFileAsync(
            "Selecteer foto's",
            MauiFileUtils.ImageFileTypes
        );

        if (result?.FileResult != null)
        {
            string filePath = result.FileResult.FullPath;
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(filePath);
            string contentType = FileUtils.GetContentType(fileName);

            await _fileService.UploadFileAsync(bucketName, fileName, filePath, contentType);
        }
    }
    private void Button_Clicked(object sender, EventArgs e)
    {

    }
}

