using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KamerConnect.Extensions;
using KamerConnect.Models;
using KamerConnect.Services;
using KamerConnect.View.MAUI.Views;


namespace KamerConnect.View.MAUI.Pages;

public partial class ProfilePage : ContentPage
{
    private readonly FileService _fileService;
    private readonly PersonService _personService;
    private readonly AuthenticationService _authenticationService;
    private const string _bucketName = "profilepictures";
    private IServiceProvider _serviceProvider;
    private Person _person;
    
    // private readonly FileService _fileService;

    public ProfilePage(FileService fileService, AuthenticationService authenticationService, PersonService personService, IServiceProvider serviceProvider)
    {
        // _fileService = fileService;
        _serviceProvider = serviceProvider;
        NavigationPage.SetHasNavigationBar(this, false);


        InitializeComponent();
        _fileService = fileService;
        _authenticationService = authenticationService;
        _personService = personService;
        GetCurrentPerson().GetAwaiter().GetResult();
        LoadProfileData();
        var navbar = serviceProvider.GetRequiredService<Navbar>();
        NavbarContainer.Content = navbar;
    }

    private async Task GetCurrentPerson()
    {
        var session = await _authenticationService.GetSession();
        if (session != null)
        {
            _person = _personService.GetPersonById(session.personId);
        }
    }

    private void LoadProfileData()
    {
        string profileImageUrl = _person.ProfilePicturePath != null
            ? _fileService.GetFilePath(_bucketName, _person.ProfilePicturePath)
            : "camera.jpg";
        string name = $"{_person.FirstName} {_person.MiddleName} {_person.Surname}";
        string phone = _person.PhoneNumber;
        string email = _person.Email;
        string gender = _person.Gender.GetEnumDescription();
        string school = _person.Personality.School;
        string course = _person.Personality.Study;
        string description = _person.Personality.Description;
        string motivation = "Deze prachtige instapklare woning biedt alles wat u zoekt: ...";
        
        

        // Assign Data
        ProfileImage.Source = profileImageUrl;
        NameLabel.Text = name;
        PhoneLabel.Text = $"{phone}";
        EmailLabel.Text = $"{email}";
        GenderLabel.Text = $"{gender}";
        SchoolLabel.Text = $"{school}";
        CourseLabel.Text = $"{course}";
        DescriptionLabel.Text = description;
        MotivationLabel.Text = motivation;
    }
}