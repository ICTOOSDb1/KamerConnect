using System.Security.Cryptography;
using Microsoft.Maui.Storage;
using Microsoft.UI.Xaml.Input;
using KamerConnect.DataAccess.Postgres;
using KamerConnect.DataAccess.Minio;
using KamerConnect.Repositories;
using KamerConnect.Services;
using KamerConnect.Models;
using Npgsql;

namespace KamerConnect.View.MAUI.Views;

public partial class UpdateAccountsForm : ContentView
{

    private const string BucketName = "profilepictures";

    private readonly FileService _fileService;
    private readonly PersonService _personService;
    private Person? _currentPerson;
    private string? _minioURL;

    public UpdateAccountsForm(FileService fileService, PersonService personService, Person person)
    {
        _fileService = fileService;
        _personService = personService;
        _currentPerson = person;
        _minioURL = person.ProfilePicturePath;
        BindingContext = _currentPerson;
        InitializeComponent();
    }

    private async void Image_tapped(object sender, TappedEventArgs e)
    {
        var result = await FilePicker.Default.PickAsync(new PickOptions
        {
            PickerTitle = "Selecteer een foto",
            FileTypes = FilePickerFileType.Images
        });

        if (result != null)
        {
            string filePath = result.FullPath;
            var fileBytes = await File.ReadAllBytesAsync(filePath);
            string fileName = Path.GetFileName(filePath)+DateTime.Now.ToString("yyyyMMddHHmmss");
            string contentType = GetContentType(fileName);

            await _fileService.UploadFileAsync(BucketName, fileName, fileBytes, contentType);
            string localhost = LoadLocalEnv();
            _minioURL = $"{localhost}/{BucketName}/{fileName}";
            profile_picture.Source = _minioURL;
        }
    }
    private void Button_Update_Account(object sender, EventArgs e)
    {
        _personService.UpdatePerson(_currentPerson);
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
    
    public string LoadLocalEnv()
    {
        try
        {
            string envFilePath = "KamerConnect.EnvironmentVariables/local.env";
            DotNetEnv.Env.Load(envFilePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Local.env doesn't load: {ex.Message}");
        }
        string localhost = Environment.GetEnvironmentVariable("MINIO_ENDPOINT") ?? "http://localhost:9000";
        return localhost;
    }
}
