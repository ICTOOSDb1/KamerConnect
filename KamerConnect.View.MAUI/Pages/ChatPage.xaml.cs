using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public ChatPage(AuthenticationService authenticationService, PersonService personService, MatchService matchService, IServiceProvider serviceProvider, ChatService chatService)
    {
        InitializeComponent();
        _authenticationService = authenticationService;
        _personService = personService;
        _matchService = matchService;
        _serviceProvider = serviceProvider;
        _chatService = chatService;
        GetCurrentPerson().GetAwaiter().GetResult();

        _connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5068/chat")
            .WithAutomaticReconnect()
            .Build();

        _connection.On<string>("MessageReceived", message =>
        {
            Dispatcher.Dispatch(() =>
            {
                if (ChatMessages != null)
                {
                    List<ChatMessage> messages = _chatService.getChatMessages(_match.matchId, _person.Id);
                    foreach (ChatMessage message in messages)
                    {
                        if (message.senderId == _person.Id)
                        {
                            Label label1 = new Label { Text = message.Message, TextColor = Colors.Red};
                            StackLayout.Children.Add(label1);
                        }
                        else
                        {
                            Label label1 = new Label { Text = message.Message, TextColor = Colors.Blue};
                            StackLayout.Children.Add(label1);
                        }
                    }
                    
                }
            });
        });
        _connection.Closed += async (error) =>
        {
            Console.WriteLine($"Connection closed: {error?.Message}");
            await Task.Delay(5000); // Wait 5 seconds before reconnecting
            await _connection.StartAsync();
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
        await _connection.InvokeCoreAsync("sendMessage", args: new[] { myChatMessage.Text });
        myChatMessage.Text = string.Empty;
    }
}