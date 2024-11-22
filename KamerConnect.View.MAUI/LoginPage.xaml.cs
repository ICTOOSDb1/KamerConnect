using KamerConnect.DataAccess.Postgres.Repositys;
using KamerConnect.Exceptions;
using KamerConnect.Repositories;
using KamerConnect.Services;

namespace KamerConnect.View.MAUI;

public partial class LoginPage : ContentPage
{
  
    
    private IPersonRepository personDataAcces;
    private IAuthenticationRepository authenDataAcces;
    private PersonService personSerivce;
    private AuthenticationService authService;
    
   
    
    public LoginPage()
    {
        personDataAcces = new PersonRepository();
        authenDataAcces = new AuthenticationRepository();
        personSerivce = new PersonService(personDataAcces);
        authService = new AuthenticationService(personSerivce, authenDataAcces);
        
        InitializeComponent();

      
    }
    
    public void LoginButton_Clicked(object sender, System.EventArgs e)
    {
        // string email = emailEntry.;
        // string password = passwordEntry.Text;
        //
        // try
        // {
        //     authService.Authenticate(email, password);
        // }
        // catch (InvalidCredentialsException ex)
        // {
        //     Console.WriteLine(e);
        //     emailEntry.Placeholder = ex.Message;
        //     throw;
        // }
     
    }
}