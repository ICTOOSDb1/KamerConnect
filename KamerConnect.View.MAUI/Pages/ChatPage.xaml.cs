using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using KamerConnect.Models;
using KamerConnect.Services;
using Microsoft.AspNetCore.SignalR.Client;

namespace KamerConnect.View.MAUI.Pages;

public partial class ChatPage : ContentPage
{
    private readonly HubConnection _connection;
    private readonly MatchService _matchService;
    private readonly PersonService _personService;
    private readonly AuthenticationService _authenticationService;
    private readonly ChatService _chatService;
    private IServiceProvider _serviceProvider;
    private Person _person;
    private Match _match;

    public ObservableCollection<ChatMessage> Messages { get; set; } = new();

    public ChatPage(AuthenticationService authenticationService, PersonService personService, MatchService matchService,
        IServiceProvider serviceProvider, ChatService chatService)
    {
        InitializeComponent();
        BindingContext = this;
        _authenticationService = authenticationService;
        _personService = personService;
        _matchService = matchService;
        _serviceProvider = serviceProvider;
        _chatService = chatService;

        // Initialize SignalR connection
        _connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5068/chat")
            .WithAutomaticReconnect()
            .AddJsonProtocol(options =>
            {
                options.PayloadSerializerOptions.PropertyNamingPolicy = null;
            })
            .Build();

        // Load current user and initialize chat connection
        GetCurrentPerson().GetAwaiter().GetResult();
        InitializeChatConnection();
    }

    private async Task GetCurrentPerson()
    {
        var session = await _authenticationService.GetSession();
        if (session != null)
        {
            _person = _personService.GetPersonById(session.personId);
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is Match match)
        {
            _match = match;
        }
    }

    private async void SendButton_OnClicked(object? sender, EventArgs e)
    {
        string messageText = myChatMessage.Text;
        if (!string.IsNullOrWhiteSpace(messageText))
        {
            var chatMessage = new ChatMessage(_match.matchId, _person.Id, messageText);

            try
            {
                await _connection.SendAsync("SendMessage", chatMessage);
                _chatService.sendMessage(chatMessage);
                Messages.Add(chatMessage);
                int aantal = Messages.Count;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending message: {ex.Message}");
            }

            myChatMessage.Text = string.Empty;
        }
    }

    private void InitializeChatConnection()
    {
        _connection.On<ChatMessage>("ReceiveMessage", message =>
        {
            Dispatcher.Dispatch(() =>
            {
                Messages.Add(message);
            });
        });

        _connection.Closed += async (error) =>
        {
            Console.WriteLine($"Connection closed: {error?.Message}");
            await Task.Delay(5000); // Wait 5 seconds before reconnecting
            try
            {
                await _connection.StartAsync();
                Console.WriteLine("Reconnected successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Reconnection failed: {ex.Message}");
            }
        };

        Task.Run(async () =>
        {
            try
            {
                await _connection.StartAsync();
                Console.WriteLine("Connection started.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection failed: {ex.Message}");
            }
        });
    }
}
