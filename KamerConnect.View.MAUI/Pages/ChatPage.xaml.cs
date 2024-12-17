using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KamerConnect.Models;
using KamerConnect.View.MAUI.Views;

namespace KamerConnect.View.MAUI.Pages;

public partial class ChatPage : ContentPage
{
    
    public ChatPage(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        var chatView = serviceProvider.GetRequiredService<ChatView>();
        BindingContext = chatView; 
        chatMessages.Content = chatView;
    }
    
}