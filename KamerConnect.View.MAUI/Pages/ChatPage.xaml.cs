using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KamerConnect.Models;
using KamerConnect.Services;
using KamerConnect.View.MAUI.Views;

namespace KamerConnect.View.MAUI.Pages;

public partial class ChatPage : ContentPage
{
    private readonly AuthenticationService _authenticationService;
    private readonly PersonService _personService;
    private readonly IServiceProvider _serviceProvider;
    private Person _person;
    
    private ObservableCollection<Chat> _chats = new ObservableCollection<Chat>();
    public ObservableCollection<Chat> Chats
    {
        get => _chats;
        set
        {
            if (_chats != value)
            {
                _chats = value;
                OnPropertyChanged(nameof(Chats));
            }
        }
    }
    public ChatPage(AuthenticationService authenticationService, PersonService personService, IServiceProvider serviceProvider)
    {
        _personService = personService;
        _authenticationService = authenticationService;
        _serviceProvider = serviceProvider;
        GetCurrentPerson().GetAwaiter().GetResult();
        InitializeComponent();
        
        
        NavigationPage.SetHasNavigationBar(this, false);
        var navbar = _serviceProvider.GetRequiredService<Navbar>();
        NavbarContainer.Content = navbar;
        BindingContext = this;
    }
    
    /*private void LoadHouses()
    {
        var housePreferences = _housePreferenceService.GetHousePreferences(_person.Id);
        var houses = _houseService.GetByPreferences(housePreferences);
        Chats = new ObservableCollection<Chat>(houses);
    }*/
    
    private async Task GetCurrentPerson()
    {
        var session = await _authenticationService.GetSession();
        if (session != null)
        {
            _person = _personService.GetPersonById(session.personId);
        }
    }
}