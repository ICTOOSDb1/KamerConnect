using KamerConnect.Exceptions;

namespace KamerConnect.View.MAUI.Pages;

public partial class LoginPage : ContentPage
{
    private readonly AuthenticationService authService;

    public LoginPage(AuthenticationService authService)
    {
        this.authService = authService;
    
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
            
            throw;
        }

    private void NavigateToRegister(object sender, TappedEventArgs e)
    {
        if (Application.Current.MainPage is NavigationPage navigationPage)
        {
            navigationPage.Navigation.PushAsync(new Registration());
        }
            await DisplayAlert("Error", "Invalid credentials, please try again.", "OK");
        }
       
     
    }
}