
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
    private readonly PersonService _personService;
    private ObservableCollection<House> _houses;
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
        PersonService personService)
    {
        _serviceProvider = serviceProvider;
        _authenticationService = authenticationService;
        _houseService = houseService;
        _personService = personService;
        NavigationPage.SetHasNavigationBar(this, false);

        GetCurrentPerson().GetAwaiter().GetResult();

        Houses = new ObservableCollection<House>();
        LoadHouses();
        LoadHouses();
        LoadHouses();
        LoadHouses();


        InitializeComponent();

        var navbar = serviceProvider.GetRequiredService<Navbar>();
        NavbarContainer.Content = navbar;

        BindingContext = this;
    }

    private void LoadHouses()
    {
        Houses.Add(GetCurrentHouse());
    }

    private async Task GetCurrentPerson()
    {
        var session = await _authenticationService.GetSession();
        if (session != null)
        {
            _person = _personService.GetPersonById(session.personId);
        }
    }

    private House GetCurrentHouse()
    {
        return _houseService.GetByPersonId(_person.Id);
    }
}

