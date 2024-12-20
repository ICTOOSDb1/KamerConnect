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
    
    private ObservableCollection<Chat> _chats = new ObservableCollection<Chat>();
    public ObservableCollection<Chat> Chats
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
}