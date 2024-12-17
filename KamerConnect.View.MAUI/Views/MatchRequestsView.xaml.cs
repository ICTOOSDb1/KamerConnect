﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using KamerConnect.Models;
using KamerConnect.Services;
using KamerConnect.Utils;
using KamerConnect.View.MAUI.Pages;
using KamerConnect.View.MAUI.Utils;
using Microsoft.Maui.Controls.Shapes;

namespace KamerConnect.View.MAUI.Views;

public partial class MatchRequestsView : ContentView
{
    private readonly Person _person;
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
        
        StatusPicker.SelectedItem = "Pending";

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
        LegendField4.Text = "Prijs";

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
        LegendField1.Text = "Voornaam";
        LegendField2.Text = "School";
        LegendField3.Text = "Opleiding";
        LegendField4.Text = "Geboortedatum";

        var house = _houseService.GetByPersonId(_person.Id);
        var matches = _matchService.GetMatchesById(house.Id);

        foreach (var match in matches)
        {
            var person = _personService.GetPersonById(match.personId);

            var profilePage = MauiProgram.Services.GetRequiredService<ProfilePage>();
            profilePage.BindingContext = match;
            MatchRequests.Add(new MatchRequestItem(person, match, _fileService, profilePage, FilteredMatchRequests));
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
            case "Accepted":
                filtered = FilteredMatchRequests.Where(match => match.Match.Status == Status.Accepted).ToList();
                break;
            case "Pending":
                filtered = FilteredMatchRequests.Where(match => match.Match.Status == Status.Pending).ToList();
                break;
            case "Rejected":
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

public class MatchRequestItem
{
    private readonly ObservableCollection<MatchRequestItem> _parentCollection;

    public string ProfilePicturePath { get; set; }
    public string Field1 { get; set; }
    public string Field2 { get; set; }
    public string Field3 { get; set; }
    public string Field4 { get; set; }
    public string StatusText { get; set; }
    public Color StatusColor { get; set; }
    public bool ShowStatusButtons { get; set; }

    public Match Match { get; set; }

    public ICommand AcceptCommand { get; }
    public ICommand RejectCommand { get; }
    public ICommand GoToPage { get; }

    public MatchRequestItem(Person person, Match match, FileService fileService, ProfilePage profilePage,
        ObservableCollection<MatchRequestItem> parentCollection)
    {
        _parentCollection = parentCollection;

        Match = match;

        ProfilePicturePath = person.ProfilePicturePath != null
            ? fileService.GetFilePath("profilepictures", person.ProfilePicturePath)
            : "user.png";
        Field1 = person.FirstName;
        Field2 = person.Personality?.School ?? "";
        Field3 = person.Personality?.Study ?? "";
        Field4 = person.BirthDate.ToShortDateString();

        ShowStatusButtons = true;

        AcceptCommand = new Command(() => UpdateStatus(match, Status.Accepted));
        RejectCommand = new Command(() => UpdateStatus(match, Status.Rejected));

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
        Field1 = house.Street;
        Field2 = house.City ?? "";
        Field3 = house.Type.GetDisplayName() ?? "";
        Field4 = house.Price.ToString();

        ShowStatusButtons = false;

        (StatusText, StatusColor) = GetStatusDisplay(match.Status);

        GoToPage = new Command(() => { App.Current.MainPage = new NavigationPage(housePage); });
    }

    private static (string Text, Color Color) GetStatusDisplay(Status status) =>
        status switch
        {
            Status.Accepted => ("Geaccepteerd", Colors.Green),
            Status.Pending => ("In behandeling", Colors.Orange),
            Status.Rejected => ("Geweigerd", Colors.Red),
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
        }
        
    }
}