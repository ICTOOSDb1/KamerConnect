using KamerConnect.Exceptions;
using KamerConnect.Models;
using KamerConnect.Services;

namespace KamerConnect.View.MAUI.Pages;

public partial class LoginPage : ContentPage
{
    private readonly AuthenticationService _authService;
    private readonly PersonService _personService;
    private readonly IServiceProvider _serviceProvider;

    public LoginPage(IServiceProvider serviceProvider, AuthenticationService authService, PersonService personService)
    {
        _authService = authService;
        _personService = personService;
        _serviceProvider = serviceProvider;


        NavigationPage.SetHasNavigationBar(this, false);

        InitializeComponent();
    }


    private async void LoginButton_Clicked(object sender, System.EventArgs e)
    {
        string email = emailEntry.Text;
        string password = passwordEntry.Text;

        string? token = await _authService.Authenticate(email, password);

        if (Application.Current.MainPage is NavigationPage navigationPage)
        {
            if (token != null)
            {
                var session = await _authService.GetSession();
                if (session != null)
                {
                    if (_personService.GetPersonById(session.personId)?.Role == Role.Seeking)
                    {
                        App.Current.MainPage = new NavigationPage(_serviceProvider.GetRequiredService<MainPage>());
                    }
                    else App.Current.MainPage = new NavigationPage(_serviceProvider.GetRequiredService<UpdateAccount>());
                }

            }
            else
            {
                passwordEntry.SetValidation("Email of wachtwoord klopt niet!");
            }
        }
    }

    public void NavigateToRegister(object sender, TappedEventArgs e)
    {
        if (Application.Current.MainPage is NavigationPage navigationPage)
        {
            navigationPage.Navigation.PushAsync(_serviceProvider.GetRequiredService<Registration>());
        }
    }
}