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
    private readonly IServiceProvider _serviceProvider;
    private readonly ChatService _chatService;
    private readonly AuthenticationService _authenticationService;
    private readonly PersonService _personService;
    private readonly HouseService _houseService;

    public Person Sender { get; private set; }
    private Author _incomingAuthor;
    private Chat _selectedChat;
    public Chat SelectedChat
    {
        get => _selectedChat;
        set
        {
            _selectedChat = value;
            RaisePropertyChanged(nameof(SelectedChat));
        }
    }
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

    public ChatView(IServiceProvider serviceProvider, Chat chat, int index, Person sender)
    {
        InitializeComponent();
        _serviceProvider = serviceProvider;
        Sender = sender;
        _houseService = _serviceProvider.GetRequiredService<HouseService>();
        _chatService = _serviceProvider.GetRequiredService<ChatService>();
        _authenticationService = _serviceProvider.GetRequiredService<AuthenticationService>();
        _personService = _serviceProvider.GetRequiredService<PersonService>();
        SelectedChat = _chatService.GetChatsFromPerson(Sender.Id)[index];
        BindingContext = this;
        InitializeChat();
    }

    private async void InitializeChat()
    {

        CurrentUser = new Author { Name = Sender.FirstName };
        GetIncomingAuthor();
        LoadMessages();
        InitializeSignalR();

    }


    private void GetIncomingAuthor()
    {
        _incomingAuthor = _selectedChat.PersonsInChat
            .Where(person => person.Id != Sender.Id)
            .Select(person => new Author { Name = person.FirstName })
            .FirstOrDefault();
    }

    private void LoadMessages()
    {

        foreach (var message in _selectedChat.Messages)
        {
            _messages.Add(ConvertToSyncfusionMessage(message));
        }
    }

    private TextMessage ConvertToSyncfusionMessage(ChatMessage chatMessage)
    {
        TextMessage textMessage = new TextMessage
        {
            Text = chatMessage.Message,
            Author = chatMessage.SenderId == Sender.Id ? CurrentUser : _incomingAuthor,
            DateTime = chatMessage.SendAt,
        };
        return textMessage;
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
                    Author = isCurrentUser ? _currentUser : _incomingAuthor,
                    DateTime = DateTime.Now
                };
                if (newMessage.Author == _currentUser)
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
