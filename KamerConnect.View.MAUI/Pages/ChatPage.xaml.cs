using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KamerConnect.Models;
using KamerConnect.Services;
using KamerConnect.View.MAUI.Views;

namespace KamerConnect.View.MAUI.Pages;

public partial class ChatPage : ContentPage
{
    private readonly AuthenticationService _authenticationService;
    private readonly PersonService _personService;
    private readonly IServiceProvider _serviceProvider;
    private readonly ChatService _chatService;
    private Person _person;

    private ObservableCollection<Chat>? _chats;
    public ObservableCollection<Chat>? Chats
    {
        get => _chats;
        set
        {
            if (_chats != value)
            {
                _chats = value;
                OnPropertyChanged(nameof(Chats));
            }
        }
    }
    private Chat? _selectedChat;

    public Chat? SelectedChat
    {
        get => _selectedChat;
        set
        {
            if (_selectedChat != value)
            {
                _selectedChat = value;
                OnPropertyChanged(nameof(SelectedChat));
            }
        }
    }
    public ChatPage(AuthenticationService authenticationService, PersonService personService, IServiceProvider serviceProvider, ChatService chatService)
    {
        _personService = personService;
        _authenticationService = authenticationService;
        _serviceProvider = serviceProvider;
        _chatService = chatService;
        GetCurrentPerson().GetAwaiter().GetResult();
        LoadChats();
        InitializeComponent();
        
        NavigationPage.SetHasNavigationBar(this, false);
        var navbar = _serviceProvider.GetRequiredService<Navbar>();
        NavbarContainer.Content = navbar;
        BindingContext = this;
        ChatMessages.Content = new ChatViewPlaceholder();
    }
    
    private void LoadChats()
    {
        List<Chat> chats = _chatService.GetChatsFromPerson(_person.Id);
        foreach (Chat chat in chats)
        {
            Guid targetPersonId = _person.Id; 
            chat.PersonsInChat = chat.PersonsInChat
                .Where(person => person.Id != targetPersonId)
                .Concat(chat.PersonsInChat.Where(person => person.Id == targetPersonId))
                .ToList();
        }
        Chats = new ObservableCollection<Chat>(chats);
    }
    
    private async Task GetCurrentPerson()
    {
        var session = await _authenticationService.GetSession();
        if (session != null)
        {
            _person = _personService.GetPersonById(session.personId);
        }
    }
    
    void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Chat selectedChat)
        {
            SelectedChat = selectedChat;
            Console.WriteLine(selectedChat.ChatId);
            
            //TO DO: call the chat window here to be created
        }
    }
}