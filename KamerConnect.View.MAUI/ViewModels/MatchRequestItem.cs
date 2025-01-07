using System.Collections.ObjectModel;
using System.Windows.Input;
using KamerConnect.Models;
using KamerConnect.Services;
using KamerConnect.Utils;
using KamerConnect.View.MAUI.Pages;

namespace KamerConnect.View.MAUI.ViewModels;

public class MatchRequestItem
{
    private readonly ObservableCollection<MatchRequestItem> _parentCollection;

    public string ProfilePicturePath { get; set; }
    private readonly ChatService _chatService; 
    private readonly IServiceProvider _serviceProvider;
    private readonly PersonService _personService;
    private readonly AuthenticationService _authenticationService;
    public string Field1 { get; set; }
    public string Field2 { get; set; }
    public string Field3 { get; set; }
    public string Field4 { get; set; }
    public string StatusText { get; set; }
    public Color StatusColor { get; set; }
    public bool ShowStatusButtons { get; set; }
    public bool ShowRevertButton { get; set; }
    public string RevertButtonText { get; set; }

    public Match Match { get; set; }

    public ICommand AcceptCommand { get; }
    public ICommand RejectCommand { get; }
    public ICommand RevertStatusCommand { get; }
    public ICommand GoToPage { get; }

    public MatchRequestItem(IServiceProvider serviceProvider, Person person, Match match, FileService fileService, ProfilePage profilePage,
        ObservableCollection<MatchRequestItem> parentCollection)
    {
        _serviceProvider = serviceProvider;
        _parentCollection = parentCollection;
        _chatService = _serviceProvider.GetService<ChatService>();
        _personService = _serviceProvider.GetService<PersonService>();
        _authenticationService = _serviceProvider.GetService<AuthenticationService>();
        
        Match = match;

        ProfilePicturePath = person.ProfilePicturePath != null
            ? fileService.GetFilePath("profilepictures", person.ProfilePicturePath)
            : "user.png";
        Field1 = string.Join(" ", [person.FirstName, person.MiddleName, person.Surname]);
        Field2 = person.Gender.GetDisplayName();
        Field3 = person.Personality?.Study ?? "";
        Field4 = person.BirthDate.ToShortDateString();

        ShowStatusButtons = true;
        ShowRevertButton = false;

        AcceptCommand = new Command(() =>
        {
            UpdateStatus(match, Status.Accepted);
            
            Person CurrentPerson = _personService.GetPersonById(_authenticationService.GetSession().Result.personId);
            List<Person> persons = new List<Person> {person, CurrentPerson};
            _chatService.Create(new Chat(Guid.NewGuid(), match.matchId, persons, new List<ChatMessage>()));
        });
        RejectCommand = new Command(() => UpdateStatus(match, Status.Rejected));
        
        RevertStatusCommand = new Command(() => UpdateStatus(match, match.Status == Status.Accepted ? Status.Rejected : Status.Accepted));

        
        
        if (match.Status != Status.Pending)
        {
            ShowRevertButton = true;
            RevertButtonText = match.Status == Status.Accepted ? "Afwijzen" : "Accepteren";
        }

        GoToPage = new Command(() => { App.Current.MainPage = new NavigationPage(profilePage); });
    }

    public MatchRequestItem(House house, Match match, FileService fileService, HousePage housePage,
        ObservableCollection<MatchRequestItem> parentCollection)
    {
        _parentCollection = parentCollection;

        Match = match;

        ProfilePicturePath = house.HouseImages[0].Path != null
            ? house.HouseImages[0].FullPath
            : "house.png";
        Field1 = house.Street + " " + house.HouseNumber + house.HouseNumberAddition;
        Field2 = house.City ?? "";
        Field3 = house.Type.GetDisplayName() ?? "";
        Field4 = "€" + house.Price;

        ShowStatusButtons = false;

        (StatusText, StatusColor) = GetStatusDisplay(match.Status);

        GoToPage = new Command(() => { App.Current.MainPage = new NavigationPage(housePage); });
    }

    private static (string Text, Color Color) GetStatusDisplay(Status status) =>
        status switch
        {
            Status.Accepted => (Status.Accepted.GetDisplayName(), Colors.Green),
            Status.Pending => (Status.Pending.GetDisplayName(), Colors.Orange),
            Status.Rejected => (Status.Rejected.GetDisplayName(), Colors.Red),
            _ => ("", Colors.Gray)
        };

    private void UpdateStatus(Match match, Status status)
    {
        var matchService = MauiProgram.Services.GetRequiredService<MatchService>();
        matchService.UpdateStatusMatch(match, status);
        if (match.Status != status)
        {
            _parentCollection.Remove(this);
            match.Status = status;
            
            if (match.Status != Status.Pending)
            {
                ShowRevertButton = true;
                RevertButtonText = match.Status == Status.Accepted ? "Afwijzen" : "Accepteren";
            }
        }
        
        
    }
}