using KamerConnect.Exceptions;

namespace KamerConnect.View.MAUI.Pages;

public partial class LoginPage : ContentPage
{
    private readonly AuthenticationService authService;
    private readonly IServiceProvider _serviceProvider;

    public LoginPage(IServiceProvider serviceProvider, AuthenticationService authService)
    {
        this.authService = authService;
        _serviceProvider = serviceProvider;

    
        NavigationPage.SetHasNavigationBar(this, false);
        
        InitializeComponent();
    }


    private async void LoginButton_Clicked(object sender, System.EventArgs e)
    {
        string email = emailEntry.Text;
        string password = passwordEntry.Text;

        string? token = await authService.Authenticate(email, password);

        if (token != null)
        {
            App.Current.MainPage = new MainPage();
        }
        else
        {
            
            Console.WriteLine(e);
        }
    }

    public async void NavigateToRegister(object sender, TappedEventArgs e)
    {
        if (Application.Current.MainPage is NavigationPage navigationPage)
        {
            navigationPage.Navigation.PushAsync(_serviceProvider.GetRequiredService<Registration>());
        }
            await DisplayAlert("Error", "Invalid credentials, please try again.", "OK");
        }
}