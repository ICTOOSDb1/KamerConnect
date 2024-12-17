using System.Collections.ObjectModel;
using System.ComponentModel;
using KamerConnect.Models;
using KamerConnect.Services;
using Microsoft.AspNetCore.SignalR.Client;
using Syncfusion.Maui.Chat;

namespace KamerConnect.View.MAUI.Views;

public partial class ChatView : ContentView, INotifyPropertyChanged
{
    private HubConnection _connection;
    private readonly ChatService _chatService;
    private readonly AuthenticationService _authenticationService;
    private readonly PersonService _personService;
    private readonly MatchService _matchService;
    private readonly HouseService _houseService;

    public Person Sender { get; private set; }
    public Chat SelectedChat { get; private set; }
    private ObservableCollection<object> _messages = new();
   
    public ObservableCollection<object> Messages
    {
        get
        {
            return _messages;
        }
        set
        {
            _messages = value;
        }
    } 
    private Author _currentUser;
    public Author CurrentUser
            {
                get
                {
                    return _currentUser;
                }
                set
                {
                    _currentUser = value;
                    RaisePropertyChanged("CurrentUser");
                }
            }
    public event PropertyChangedEventHandler? PropertyChanged;
        
    public void RaisePropertyChanged(string propName)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }  

    public ChatView(HouseService houseService, MatchService matchService,
                    ChatService chatService, AuthenticationService authenticationService,
                    PersonService personService)
    {
        InitializeComponent();

        _houseService = houseService;
        _matchService = matchService;
        _authenticationService = authenticationService;
        _chatService = chatService;
        _personService = personService;

        BindingContext = this;

        InitializeChat();
    }

    private async void InitializeChat()
    {
        await LoadSender();
        SelectedChat = _chatService.GetChatByMatchId(GetCurrentMatch());
        CurrentUser = new Author { Name = Sender.FirstName };
        LoadMessages();
        InitializeSignalR();
       
    }
    

    private async Task LoadSender()
    {
        var session = await _authenticationService.GetSession();
        if (session != null)
        {
            Sender = _personService.GetPersonById(session.personId);
        }
    }

    private Guid GetCurrentMatch()
    {
        if (Sender.Role == Role.Offering)
        {
            var house = _houseService.GetByPersonId(Sender.Id);
            var matches = _matchService.GetMatchesById(house.Id);
            return matches.FirstOrDefault()?.matchId ?? Guid.Empty;
        }
        else
        {
            var matches = _matchService.GetMatchesById(Sender.Id);
            return matches.FirstOrDefault()?.matchId ?? Guid.Empty;
        }
    }

    private void LoadMessages()
    {
        var messages = _chatService.GetChatMessages(SelectedChat.ChatId);
        foreach (var message in messages)
        {
            _messages.Add(ConvertToSyncfusionMessage(message));
        }
    }

    private TextMessage ConvertToSyncfusionMessage(ChatMessage chatMessage)
    {
        Author incomingAuthor = new Author { Name = "Them" };
        return new TextMessage
        {
            Text = chatMessage.Message,
            Author = chatMessage.SenderId == Sender.Id ? CurrentUser : incomingAuthor,
            DateTime = chatMessage.SendAt
        };
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
                var isCurrentUser = senderId == Sender.Id;

                var newMessage = new TextMessage
                {
                    Text = message,
                    Author = isCurrentUser ? _currentUser : new Author { Name = "Them" },
                    DateTime = DateTime.Now
                };
                if(isCurrentUser)
                    return;
                _messages.Add(newMessage);
            });
        });
       

        try
        {
            await _connection.StartAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error connecting to SignalR: {ex.Message}");
        }
    }

    

    private void ChatControl_SendMessage(object? sender, SendMessageEventArgs e)
    {
        
        if (string.IsNullOrWhiteSpace(e.Message.Text))
            return;

        var messageText = e.Message.Text;

        var chatMessage = new ChatMessage(
            Guid.NewGuid(), Sender.Id, messageText, DateTime.Now);
        
        var newChatMessage = new ChatMessage
        (
            Guid.NewGuid(),
            Sender.Id,
            messageText,
            DateTime.Now
        );
        

        try
        {
            // Explicitly match SignalR's InvokeAsync overload
             _connection.InvokeAsync("SendMessage", 
                Sender.Id.ToString(), 
                SelectedChat.ChatId.ToString(), 
                messageText);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending message: {ex.Message}");
        }

        _chatService.CreateMessage(newChatMessage, SelectedChat.ChatId);
    }
}
