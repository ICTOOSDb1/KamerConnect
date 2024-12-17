
using KamerConnect.Models;
using KamerConnect.Services;
using Microsoft.AspNetCore.SignalR.Client;

namespace KamerConnect.View.MAUI.Views;

public partial class ChatView : ContentView
{
    private HubConnection _connection;
    private readonly ChatService _chatService;
    private readonly AuthenticationService _authenticationService;
    private readonly PersonService _personService;
    private readonly MatchService _matchService;
    private readonly HouseService _houseService;
    private readonly IServiceProvider _serviceProvider;
    public Person Sender;
    public Chat SelectedChat { get; set; }

    public ChatView(IServiceProvider serviceProvider,HouseService houseService, MatchService matchService, ChatService chatService, AuthenticationService authenticationService, PersonService personService)
    {
        
        BindingContext = this;
        _serviceProvider = serviceProvider;
        _houseService = houseService;
        _matchService = matchService;
        _authenticationService = authenticationService;
        _chatService = chatService;
        _personService = personService;
        GetCurrentPerson().GetAwaiter().GetResult();
        SelectedChat = _chatService.GetChatByMatchId(GetCurrentMatch());
        LoadChatMessages(SelectedChat);
       InitializeComponent();
        InitializeSignalR();
        
    }
    private async Task GetCurrentPerson()
    {
        var session = await _authenticationService.GetSession();
        if (session != null)
        {
            Sender = _personService.GetPersonById(session.personId);
        }
    }

    private  Guid GetCurrentMatch()
    {
        if (Sender.Role == Role.Offering)
        {
            var house = _houseService.GetByPersonId(Sender.Id);
            List<Match> matches = _matchService.GetMatchesById(house.Id);
            return matches.FirstOrDefault().matchId;
        }
        else
        {
             List<Match> matches = _matchService.GetMatchesById(Sender.Id);
             return matches.FirstOrDefault().matchId;
        }
       
       
    }
    

    private async void InitializeSignalR()
    {
        _connection = new HubConnectionBuilder()
            .WithUrl($"http://localhost:5105/chathub?chatId={SelectedChat.ChatId}")
            .Build();

        _connection.On<Guid, string>("ReceiveMessage", (senderId, message) =>
        {
            Application.Current.Dispatcher.Dispatch(() =>
            {
                var chatMessage = new ChatMessage(
                    Guid.NewGuid(),
                    senderId,   
                    message,
                    DateTime.Now
                );
                if (senderId == Sender.Id)
                {
                    chatMessage.Message = "you" + chatMessage.Message;
                    SelectedChat.messages.Add(chatMessage);
                }
                else
                {
                    chatMessage.Message = "them" + chatMessage.Message;
                    SelectedChat.messages.Add(chatMessage);
                }
            });
        });


        try
        {
            await _connection.StartAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fout bij verbinden met SignalR: {ex.Message}");
        }
    }

    public void LoadChatMessages(Chat chat)
    {
        List<ChatMessage> messages = _chatService.GetChatMessages(chat.ChatId);
        
        foreach (var message in messages)
        {
            Application.Current.Dispatcher.Dispatch(() =>
            {
                if (message.SenderId == Sender.Id)
                {
                    message.Message = "you" + message.Message;
                    SelectedChat.messages.Add(message);
                }
                else
                {
                    message.Message = "them" + message.Message;
                    SelectedChat.messages.Add(message);
                }
                
            });
        }
    }

    private async void OnSendMessageClicked(object sender, EventArgs e)
    {
        if (SelectedChat == null || string.IsNullOrWhiteSpace(MessageEntry.Text))
        {
            return;
        }

        var newMessage = new ChatMessage(
            Guid.NewGuid(),
            Sender.Id,
            MessageEntry.Text,
            DateTime.Now);
        
        await _connection.InvokeAsync(
            "SendMessage", 
            Sender.Id.ToString(),
            SelectedChat.ChatId, 
            newMessage.Message 
        );

        _chatService.CreateMessage(newMessage, SelectedChat.ChatId);
        MessageEntry.Text = string.Empty;
    }
}