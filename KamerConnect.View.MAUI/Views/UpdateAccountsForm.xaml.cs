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
    private Person? currentPerson;
    public Person? CurrentPerson
    {
        get => currentPerson;
        set
        {
            currentPerson = value;
        }
    }
    private string? _minioURL;

    public UpdateAccountsForm(FileService fileService, PersonService personService, Person person)
    {
        _fileService = fileService;
        _personService = personService;
        CurrentPerson = person;
        CheckIfPersonFieldsAreNull(person);
        _minioURL = person.ProfilePicturePath;
        BindingContext = CurrentPerson;
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
            _minioURL = $"http://localhost:9000/{BucketName}/{fileName}";
            profile_picture.Source = _minioURL;
        }
    }
    private void Button_Update_Account(object sender, EventArgs e)
    {
        ValidateIfAccountDetailsChanged(CurrentPerson);
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

    public void ValidateIfAccountDetailsChanged(Person person)
    {
        
        List<string> fieldsToUpdate = new List<string>();
        List<NpgsqlParameter> parametersToUpdate = new List<NpgsqlParameter>(); 
        
        if (person.FirstName != firstNameEntry.DefaultText)
        {
            person.FirstName = firstNameEntry.DefaultText;
            fieldsToUpdate.Add("first_name = @FirstName");
            parametersToUpdate.Add(new NpgsqlParameter("@FirstName", person.FirstName));
        }
        if (person.MiddleName != middleNameEntry.DefaultText)
        {
            person.MiddleName = middleNameEntry.DefaultText;
            fieldsToUpdate.Add("middle_name = @MiddleName");
            parametersToUpdate.Add(new NpgsqlParameter("@MiddleName", person.MiddleName));
        }
        if (person.Surname != surNameEntry.DefaultText)
        {
            person.Surname = surNameEntry.DefaultText;
            fieldsToUpdate.Add("surname = @Surname");
            parametersToUpdate.Add(new NpgsqlParameter("@Surname", person.Surname));
        }
        if (person.Email != emailEntry.DefaultText)
        {
            person.Email = emailEntry.DefaultText;
            fieldsToUpdate.Add("email = @Email");
            parametersToUpdate.Add(new NpgsqlParameter("@Email", person.Email));
        }

        if (person.PhoneNumber != phoneNumberEntry.DefaultText)
        {
            person.PhoneNumber = phoneNumberEntry.DefaultText;
            fieldsToUpdate.Add("phone_number = @PhoneNumber");
            parametersToUpdate.Add(new NpgsqlParameter("@PhoneNumber", person.PhoneNumber));
        }
        
        if (person.ProfilePicturePath != _minioURL)
        {
            person.ProfilePicturePath = _minioURL;
            fieldsToUpdate.Add("profile_picture_path = @ProfilePicturePath");
            parametersToUpdate.Add(new NpgsqlParameter("@ProfilePicturePath", person.ProfilePicturePath));
        }
        Guid personId = Guid.Parse(person.Id);
        
        _personService.UpdatePerson(personId, "id", fieldsToUpdate, parametersToUpdate, "person");
    }

    private void CheckIfPersonFieldsAreNull(Person person)
    {
        if (person.PhoneNumber == null)
        {
            person.PhoneNumber = "";
        }
        if (person.MiddleName == null)
        {
            person.MiddleName = "";
        }
        if (person.ProfilePicturePath == null)
        {
            person.ProfilePicturePath = "";
        }
    }
}
