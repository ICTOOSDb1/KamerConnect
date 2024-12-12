using KamerConnect.Models;
using KamerConnect.Services;
using KamerConnect.View.MAUI.Pages;

namespace KamerConnect.View.MAUI.Views;

public partial class Navbar : ContentView
{
    public static IServiceProvider _serviceProvider;
    public static PersonService _personService;
    public static AuthenticationService _authenticationService;
    public Person _person;

    public Navbar(IServiceProvider serviceProvider, PersonService personService, AuthenticationService authenticationService)
    {
        _serviceProvider = serviceProvider;
        _personService = personService;
        _authenticationService = authenticationService;

        InitializeComponent();
        checkRole();
    }

    private async Task GetCurrentPerson()
    {
        var session = await _authenticationService.GetSession();
        if (session != null)
        {
            _person = _personService.GetPersonById(session.personId);
        }
    }
    private async void checkRole()
    {
        GetCurrentPerson().GetAwaiter().GetResult();
        if (_person.Role == Role.Offering)
        {
            search.IsVisible = false;
        }

    }
    private async void OnChatsTapped(object sender, TappedEventArgs e)
    {
        if (Application.Current.MainPage is NavigationPage navigationPage)
        {
            App.Current.MainPage = new NavigationPage(_serviceProvider.GetRequiredService<MatchRequestsPage>());
        }
    }

    private async void OnExploreTapped(object sender, TappedEventArgs e)
    {
        if (Application.Current.MainPage is NavigationPage navigationPage)
        {
            App.Current.MainPage = new NavigationPage(_serviceProvider.GetRequiredService<MainPage>());
        }
    }

    private async void OnProfileTapped(object sender, TappedEventArgs e)
    {
        if (Application.Current.MainPage is NavigationPage navigationPage)
        {
            App.Current.MainPage = new NavigationPage(_serviceProvider.GetRequiredService<UpdateAccount>());
        }
    }
}
