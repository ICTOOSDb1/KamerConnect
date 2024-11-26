using System.Diagnostics;
using System.Security.Cryptography;
using DotNetEnv;
using KamerConnect.EnvironmentVariables;
using Microsoft.Maui.Storage;
using KamerConnect.Services;
using KamerConnect.Utils;
using KamerConnect.View.MAUI.Utils;
using LukeMauiFilePicker;
using KamerConnect.Models;
using Npgsql;

namespace KamerConnect.View.MAUI.Views;

public partial class UpdateAccountsForm : ContentView
{

    private const string BucketName = "profilepictures";

    private readonly FileService _fileService;
    private readonly PersonService _personService;
    private Person? _currentPerson;

    public UpdateAccountsForm(FileService fileService, PersonService personService, Person person)
    {
        _fileService = fileService;
        _personService = personService;
        _currentPerson = person;
        InitializeComponent();
        BindingContext = _currentPerson;
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

            await _fileService.UploadFileAsync(BucketName, fileName, filePath, contentType);
            string localhost = Env.GetString("MINIO_ENDPOINT");
            _currentPerson.ProfilePicturePath = $"{localhost}/{BucketName}/{fileName}";
            profile_picture.Source = _currentPerson.ProfilePicturePath;
        }
    }
    private void Button_Update_Account(object sender, EventArgs e)
    {
        if (!ValidateForm()) return;
        _personService.UpdatePerson(_currentPerson);
    }

    public bool ValidateForm()
    {
        firstNameEntry.Validate();
        surNameEntry.Validate();
        middleNameEntry.Validate();
        emailEntry.Validate();
        phoneNumberEntry.Validate();

        return firstNameEntry.IsValid &&
               surNameEntry.IsValid &&
               middleNameEntry.IsValid &&
               emailEntry.IsValid &&
               phoneNumberEntry.IsValid;
    }
}
