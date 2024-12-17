using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KamerConnect.View.MAUI.Views;

public partial class ChatPerson : ContentView
{
    public ChatPerson()
    {
        InitializeComponent();
    }
    
    /*private void OnChatTapped(object sender, EventArgs e)
    {
        if (BindingContext is House house)
        {
            var housePage = MauiProgram.Services.GetRequiredService<HousePage>();
            housePage.BindingContext = house;

            App.Current.MainPage = new NavigationPage(housePage);
        }
    }*/
}