using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using KamerConnect.Models;
using KamerConnect.Services;
using KamerConnect.Utils;
using KamerConnect.View.MAUI.Pages;
using KamerConnect.View.MAUI.ViewModels;

namespace KamerConnect.View.MAUI.Views;

public partial class MatchRequestsView : ContentView
{
    private readonly Person _person;
    private readonly IServiceProvider _serviceProvider;
    private readonly MatchService _matchService;
    private readonly PersonService _personService;
    private readonly HouseService _houseService;
    private readonly FileService _fileService;

    public ObservableCollection<MatchRequestItem> MatchRequests { get; set; }
    public ObservableCollection<MatchRequestItem> FilteredMatchRequests { get; set; }
    public bool IsEmpty => MatchRequests.Count == 0;

    public event PropertyChangedEventHandler PropertyChanged;

    public MatchRequestsView(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _matchService = serviceProvider.GetRequiredService<MatchService>();
        _personService = serviceProvider.GetRequiredService<PersonService>();
        _houseService = serviceProvider.GetRequiredService<HouseService>();
        _fileService = serviceProvider.GetRequiredService<FileService>();

        var authService = serviceProvider.GetRequiredService<AuthenticationService>();
        var session = authService.GetSession().Result;

        MatchRequests = new ObservableCollection<MatchRequestItem>();
        MatchRequests.CollectionChanged += (s, e) => OnPropertyChanged(nameof(IsEmpty));

        FilteredMatchRequests = new ObservableCollection<MatchRequestItem>();
        FilteredMatchRequests.CollectionChanged += (s, e) => OnPropertyChanged(nameof(IsEmpty));

        InitializeComponent();

        if (session != null)
        {
            _person = _personService.GetPersonById(session.personId);
            LoadMatchRequests();
        }

        StatusPicker.SelectedItem = "In behandeling";

        BindingContext = this;
    }

    private void LoadMatchRequests()
    {
        MatchRequests.Clear();

        if (_person.Role == Role.Offering)
        {
            SetupOffering();
        }
        else if (_person.Role == Role.Seeking)
        {
            SetupSeeking();
        }

        OnPropertyChanged(nameof(IsEmpty));
    }

    private void SetupSeeking()
    {
        LegendField1.Text = "Straat";
        LegendField2.Text = "Stad";
        LegendField3.Text = "Type";
        LegendField4.Text = "Prijs per maand";

        var matches = _matchService.GetMatchesById(_person.Id);
        foreach (var match in matches)
        {
            var house = _houseService.Get(match.houseId);
            var housePage = MauiProgram.Services.GetRequiredService<HousePage>();
            housePage.BindingContext = house;
            MatchRequests.Add(new MatchRequestItem(house, match, _fileService, housePage, FilteredMatchRequests));
        }
    }

    private void SetupOffering()
    {
        LegendField1.Text = "Naam";
        LegendField2.Text = "Geslacht";
        LegendField3.Text = "Opleiding";
        LegendField4.Text = "Geboortedatum";

        var house = _houseService.GetByPersonId(_person.Id);
        var matches = _matchService.GetMatchesById(house.Id);

        foreach (var match in matches)
        {
            var person = _personService.GetPersonById(match.personId);

            var profilePage = MauiProgram.Services.GetRequiredService<ProfilePage>();
            profilePage.BindingContext = match;
            MatchRequests.Add(new MatchRequestItem(_serviceProvider,person, match, _fileService, profilePage, FilteredMatchRequests));
        }
    }
    

    private void OnStatusFilterChanged(object sender, EventArgs e)
    {
        FilterStatus();
    }

    private void FilterStatus()
    {
        var filtered = new List<MatchRequestItem>();
        
        FilteredMatchRequests.Clear();
        foreach (var matchRequest in MatchRequests)
        {
            FilteredMatchRequests.Add(matchRequest);
        }

        switch ($"{StatusPicker.SelectedItem}")
        {
            case "Geaccepteerd":
                filtered = FilteredMatchRequests.Where(match => match.Match.Status == Status.Accepted).ToList();
                break;
            case "In behandeling":
                filtered = FilteredMatchRequests.Where(match => match.Match.Status == Status.Pending).ToList();
                break;
            case "Geweigerd":
                filtered = FilteredMatchRequests.Where(match => match.Match.Status == Status.Rejected).ToList();
                break;
        }

        FilteredMatchRequests.Clear();
        foreach (var match in filtered)
        {
            FilteredMatchRequests.Add(match);
        }


        OnPropertyChanged(nameof(IsEmpty));
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
  
}

