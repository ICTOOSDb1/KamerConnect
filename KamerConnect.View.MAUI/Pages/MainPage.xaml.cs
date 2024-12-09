
using KamerConnect.View.MAUI.Views;
using KamerConnect.Models;
using KamerConnect.Services;
using System.Collections.ObjectModel;

namespace KamerConnect.View.MAUI.Pages;

public partial class MainPage : ContentPage
{
    private readonly IServiceProvider _serviceProvider;
    private readonly AuthenticationService _authenticationService;
    private readonly HouseService _houseService;
    private readonly HousePreferenceService _housePreferenceService;
    private readonly PersonService _personService;
    private ObservableCollection<House> _houses = new ObservableCollection<House>();
    public ObservableCollection<House> Houses
    {
        get => _houses;
        set
        {
            if (_houses != value)
            {
                _houses = value;
                OnPropertyChanged(nameof(Houses));
            }
        }
    }

    private Person _person;

    public MainPage(IServiceProvider serviceProvider,
        AuthenticationService authenticationService,
        HouseService houseService,
        PersonService personService,
        HousePreferenceService housePreferenceService)
    {
        _serviceProvider = serviceProvider;
        _authenticationService = authenticationService;
        _houseService = houseService;
        _housePreferenceService = housePreferenceService;
        _personService = personService;

        GetCurrentPerson().GetAwaiter().GetResult();
        LoadHouses();

        InitializeComponent();

        NavigationPage.SetHasNavigationBar(this, false);
        var navbar = _serviceProvider.GetRequiredService<Navbar>();
        NavbarContainer.Content = navbar;

        BindingContext = this;
    }

    private void LoadHouses()
    {
        var housePreferences = _housePreferenceService.GetHousePreferences(_person.Id);
        var houses = _houseService.GetByPreferences(housePreferences);
        Houses = new ObservableCollection<House>(houses);
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

