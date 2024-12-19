using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KamerConnect.Models;
using KamerConnect.Services;
using KamerConnect.View.MAUI.Views;

namespace KamerConnect.View.MAUI.Pages;

public partial class ChatPage : ContentPage
{
    private readonly ChatService _chatService;
    private readonly AuthenticationService _authenticationService;
    private readonly PersonService _personService;
    private Person _CurrentPerson;
    private List<Chat> chatsList;
    
    public ChatPage(IServiceProvider serviceProvider, AuthenticationService authenticationService, ChatService chatService, PersonService personService)
    {
        _personService = personService;
        _chatService = chatService;
        _authenticationService = authenticationService;
        
        GetCurrentPerson().GetAwaiter().GetResult();
        // var chatView = serviceProvider.GetRequiredService<ChatView>();
        // BindingContext = chatView; 
        // chatMessages.Content = chatView;
        
        InitializeComponent();
        FillFist();
    }

    private async Task GetCurrentPerson()
    {
        var session = await _authenticationService.GetSession();
        if (session != null)
        {
            _CurrentPerson = _personService.GetPersonById(session.personId);
        }
        chatsList = _chatService.GetChatsFromPerson(_CurrentPerson.Id);
    }

    private void FillFist()
    {
        foreach (var chat in chatsList)
        {
            Label chatIDText = new Label();
            chatIDText.Text = chat.ChatId.ToString();
            chats.Children.Add(chatIDText);
        }
    } 
    

}