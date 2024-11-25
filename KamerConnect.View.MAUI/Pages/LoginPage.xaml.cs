using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KamerConnect.View.MAUI;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
        BindingContext = this;
    }
    private async void NavigateToRegister(object sender, TappedEventArgs e)
    {
        if (Application.Current.MainPage is NavigationPage navigationPage)
        {
            await navigationPage.Navigation.PushAsync(new Registration());
        }
    }
    private void LoginButton_Clicked(object sender, EventArgs e) 
    {
    

    }
}