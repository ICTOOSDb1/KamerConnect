using KamerConnect.DataAccess.Postgres.Repositys;
using KamerConnect.Exceptions;
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
    
    public void LoginButton_Clicked(object sender, System.EventArgs e)
    {
        string email = emailEntry.Text;
        string password = passwordEntry.Text;
        
        try
        {
            authService.Authenticate(email, password);
        }
        catch (InvalidCredentialsException ex)
        {
            Console.WriteLine(e);
            emailEntry.Placeholder = ex.Message;
            throw;
        }
     
    }
}