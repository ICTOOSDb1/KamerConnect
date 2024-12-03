using KamerConnect.Services;
using KamerConnect.Utils;
using KamerConnect.View.MAUI.Utils;
using LukeMauiFilePicker;
using KamerConnect.Models;

namespace KamerConnect.View.MAUI.Views;

public partial class UpdateAccountsForm : ContentView
{
    private const string _bucketName = "profilepictures";

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

            await _fileService.UploadFileAsync(_bucketName, fileName, filePath, contentType);

            _currentPerson.ProfilePicturePath = fileName;
            profile_picture.Source = _fileService.GetFilePath(_bucketName, fileName);
        }
    }

    public async void Button_Update_Account()
    {
        if (!ValidateForm()) return;
        _personService.UpdatePerson(_currentPerson);
        await Application.Current?.MainPage?.DisplayAlert("Opgeslagen", "Succesvol opgeslagen!", "Ga verder");
    }

    public bool ValidateForm()
    {
        firstNameEntry.Validate();
        surNameEntry.Validate();
        emailEntry.Validate();
        phoneNumberEntry.Validate();

        return firstNameEntry.IsValid &&
               surNameEntry.IsValid &&
               emailEntry.IsValid &&
               phoneNumberEntry.IsValid;
    }
}
