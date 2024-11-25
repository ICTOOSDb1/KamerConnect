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
    
        //authService.Register(new Person("niek2004@icloud.com", "Niek", null, "van den Berg", null, new DateTime(2004, 6 ,23), Gender.Male, Role.Seeking, null), "wachtwoord");
        
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