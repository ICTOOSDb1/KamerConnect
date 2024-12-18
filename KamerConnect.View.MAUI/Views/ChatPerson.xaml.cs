using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KamerConnect.Models;
using KamerConnect.View.MAUI.Pages;

namespace KamerConnect.View.MAUI.Views;

public partial class ChatPerson : ContentView
{
    public ChatPerson()
    {
        InitializeComponent();
        
    }
    
    /*private void OnChatTapped(object sender, EventArgs e)
    {
        if (BindingContext is Chat chat)
        {
            var chatView = MauiProgram.Services.GetRequiredService<HousePage>();
            chatView.BindingContext = chat;

            App.Current.MainPage = new NavigationPage(housePage);
        }
    }*/
}