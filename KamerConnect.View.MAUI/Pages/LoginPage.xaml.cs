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

    public async void LoginButton_Clicked(object sender, System.EventArgs e)
    {
        string email = emailEntry.Text;
        string password = passwordEntry.Text;

        try
        {
            await authService.Authenticate(email, password);
        }
        catch (InvalidCredentialsException ex)
        {
            Console.WriteLine(e);
            emailEntry.Placeholder = ex.Message;
            throw;
        }

    }
    private void NavigateToRegister(object sender, TappedEventArgs e)
    {
        if (Application.Current.MainPage is NavigationPage navigationPage)
        {
            navigationPage.Navigation.PushAsync(_serviceProvider.GetRequiredService<Registration>());
        }
    }
}