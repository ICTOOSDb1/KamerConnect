using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KamerConnect.Exceptions;

namespace KamerConnect.View.MAUI.Pages;

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
    private void NavigateToRegister(object sender, TappedEventArgs e)
    {
        if (Application.Current.MainPage is NavigationPage navigationPage)
        {
            navigationPage.Navigation.PushAsync(new Registration());
        }
    }


    
}