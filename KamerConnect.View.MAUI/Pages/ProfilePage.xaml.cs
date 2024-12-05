using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KamerConnect.Models;
using KamerConnect.Services;
using KamerConnect.Utils;
using KamerConnect.View.MAUI.Views;


namespace KamerConnect.View.MAUI.Pages;

public partial class ProfilePage : ContentPage
{
    private readonly FileService _fileService;
    private readonly MatchService _matchService;
    private readonly PersonService _personService;
    private readonly AuthenticationService _authenticationService;
    private const string _bucketName = "profilepictures";
    private IServiceProvider _serviceProvider;
    private Person _person;
    public Person _selectedPerson;


    public ProfilePage(FileService fileService, AuthenticationService authenticationService,
        PersonService personService, IServiceProvider serviceProvider, MatchService matchService)
    {
        _serviceProvider = serviceProvider;
        NavigationPage.SetHasNavigationBar(this, false);

        InitializeComponent();
        _fileService = fileService;
        _matchService = matchService;
        _authenticationService = authenticationService;
        _personService = personService;
        GetCurrentPerson().GetAwaiter().GetResult();
        // LoadProfileData();
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
}