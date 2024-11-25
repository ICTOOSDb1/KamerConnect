using KamerConnect.DataAccess.Postgres.Repositys;
using KamerConnect.Exceptions;
using KamerConnect.Models;
using KamerConnect.Repositories;
using KamerConnect.Services;

namespace KamerConnect.View.MAUI;

public partial class LoginPage : ContentPage
{
    private readonly AuthenticationService authService;

    public LoginPage(AuthenticationService authService)
    {
        this.authService = authService;
        
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
            await DisplayAlert("Error", "Invalid credentials, please try again.", "OK");
        }
       
     
    }
}