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
    private readonly IServiceProvider _serviceProvider;
    private Person _CurrentPerson;
    private List<Chat> chatsList;
    private ChatView chatView;

    public ChatPage(IServiceProvider serviceProvider, AuthenticationService authenticationService, ChatService chatService, PersonService personService)
    {
        _serviceProvider = serviceProvider;
        _personService = personService;
        _chatService = chatService;
        _authenticationService = authenticationService;

        GetCurrentPerson().GetAwaiter().GetResult();
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
            Label chatIDText = new Label
            {
                Text = chat.ChatId.ToString()
            };

            // Create a TapGestureRecognizer
            TapGestureRecognizer tapGesture = new TapGestureRecognizer();

            // Add a method to be executed when the label is tapped
            tapGesture.Tapped += (s, e) =>
            {
                int index = chatsList.IndexOf(chat);
                OnChatTapped(chat, index);
            };

            chatIDText.GestureRecognizers.Add(tapGesture);
            chats.Children.Add(chatIDText);
        }
    }

    private void OnChatTapped(Chat chat, int index)
    {
        chatView = new ChatView(_serviceProvider, chat, index, _CurrentPerson);
        BindingContext = chatView;
        chatMessages.Content = chatView;

    }



}