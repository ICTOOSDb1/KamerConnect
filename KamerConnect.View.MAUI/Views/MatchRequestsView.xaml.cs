﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KamerConnect.Models;
using KamerConnect.Services;
using KamerConnect.View.MAUI.Pages;
using Microsoft.Maui.Controls.Shapes;
using AbsoluteLayout = Microsoft.Maui.Controls.Compatibility.AbsoluteLayout;

namespace KamerConnect.View.MAUI.Views;

public partial class MatchRequestsView : ContentView
{
    private readonly FileService _fileService;
    private readonly MatchService _matchService;
    private readonly PersonService _personService;
    private readonly AuthenticationService _authenticationService;
    private const string _bucketName = "profilepictures";
    private IServiceProvider _serviceProvider;
    private Person _person;
    private readonly HouseService _houseService;
    
    public MatchRequestsView(HouseService houseService, FileService fileService, AuthenticationService authenticationService, PersonService personService, IServiceProvider serviceProvider, MatchService matchService)
    {
         _houseService = houseService;
        _serviceProvider = serviceProvider;
        NavigationPage.SetHasNavigationBar(this, false);
        InitializeComponent();
        _fileService = fileService;
        _matchService = matchService;
        _authenticationService = authenticationService;
        _personService = personService;
        GetCurrentPerson().GetAwaiter().GetResult();
        if (_person.Role == Role.Offering)
        {
            AddLegend("Voornaam", "School", "Opleiding","Geboortedatum");
            GetMatchRequestsOffering(); 
        }
        else if (_person.Role == Role.Seeking)
        {
            AddLegend("Straat", "Stad", "Type","Prijs");
        }
        
     
    }
    private async Task GetCurrentPerson()
    {
        var session = await _authenticationService.GetSession();
        if (session != null)
        {
            _person = _personService.GetPersonById(session.personId);
        }
    }

    public void GetMatchRequestsOffering()
    {
        List<Match> matches;
        House house = _houseService.GetByPersonId(_person.Id);
        if (house != null)
        {
            matches = _matchService.GetMatchesById(house.Id);

            if (matches != null)
            {
                for (int i = 1; i < matches.Count + 1; i++)
                {

                    Person person = _personService.GetPersonById(matches[i - 1].personId);

                    var border = AddProfilePicture(person);
                    var FirstNameLabel = new Label
                    {
                        Text = person.FirstName, HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center
                    };
                    var SchoolLabel = new Label();
                    var StudyLabel = new Label();
                    if (person.Personality != null)
                    {
                        SchoolLabel.Text = person.Personality.School;
                        SchoolLabel.HorizontalOptions = LayoutOptions.Center;
                        SchoolLabel.VerticalOptions = LayoutOptions.Center;

                        StudyLabel.Text = person.Personality.Study;
                        StudyLabel.HorizontalOptions = LayoutOptions.Center;
                        StudyLabel.VerticalOptions = LayoutOptions.Center;
                    }

                    var BirthLabel = new Label
                    {
                        Text = person.BirthDate.ToShortDateString(), HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center
                    };
                    var horizontalStack = new HorizontalStackLayout
                    {
                        HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, Spacing = -30
                    };
                    Button rejectButton = new Button
                    {
                        ImageSource = "reject.png",
                        Scale = 0.3,
                        BackgroundColor = Colors.Transparent,
                        CommandParameter = matches[i - 1],
                        WidthRequest = 110,
                        HeightRequest = 110
                    };
                    horizontalStack.Children.Add(rejectButton);
                    rejectButton.Clicked += RejectButton_OnClicked;

                    Button acceptButton = new Button
                    {
                        ImageSource = "accept.png",
                        Scale = 0.3,
                        BackgroundColor = Colors.Transparent,
                        CommandParameter = matches[i - 1],
                        WidthRequest = 110,
                        HeightRequest = 110
                    };
                    horizontalStack.Children.Add(acceptButton);
                    acceptButton.Clicked += AcceptButton_OnClicked;
                    var separator = new BoxView
                    {
                        HeightRequest = 2,
                        BackgroundColor = Colors.LightGray,
                        HorizontalOptions = LayoutOptions.Fill,
                        VerticalOptions = LayoutOptions.End
                    };

                    MatchRequests.RowDefinitions.Add(new RowDefinition
                        { Height = new GridLength(100, GridUnitType.Absolute) });
                    MatchRequests.Add(separator, 0, i);
                    Grid.SetColumnSpan(separator, 6);
                    MatchRequests.Add(AddProfilePicture(person), 0, i);
                    MatchRequests.Add(FirstNameLabel, 1, i);
                    MatchRequests.Add(SchoolLabel, 2, i);
                    MatchRequests.Add(StudyLabel, 3, i);
                    MatchRequests.Add(BirthLabel, 4, i);
                    MatchRequests.Add(horizontalStack, 5, i);
                    var tapGestureRecognizer = new TapGestureRecognizer
                    {
                        CommandParameter = matches[i - 1]
                    };
                    tapGestureRecognizer.Tapped += ToProfile_OnTapped;

                    border.GestureRecognizers.Add(tapGestureRecognizer);
                }
            }
            else
            {
                DisplayNoMatchRequests();
            }
        }
    }

    public Border AddProfilePicture(Person person)
    {
        var border = new Border
        {
            WidthRequest = 100,
            HeightRequest = 100,
            StrokeShape = new RoundRectangle
            {
                CornerRadius = new CornerRadius(10)
            },
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Center,
            Content = new Image
            {
                Source = person.ProfilePicturePath != null
                    ? _fileService.GetFilePath(_bucketName, person.ProfilePicturePath)
                    : "geenProfiel.png",
                Aspect = Aspect.AspectFit,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            }
        };
        return border;
    }

    public void DisplayNoMatchRequests()
    {
        var noMatchRequests = new Label
        {
            Text = "Er zijn geen matchverzoeken te weergeven",
            FontSize = 25,
            FontFamily = "InterLight",
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            TextColor = Colors.Black
        };
        MatchRequests.RowDefinitions.Add(new RowDefinition
            { Height = new GridLength(100, GridUnitType.Absolute) });
        MatchRequests.Add(noMatchRequests, 0, 1);
        Grid.SetColumnSpan(noMatchRequests, 6);
    }

    public void AddLegend(string label1, string label2, string label3, string label4)
    {
        MatchRequests.RowDefinitions.Add(new RowDefinition { Height = new GridLength(40, GridUnitType.Absolute) });
        var columns = new List<Label>
        {
            new Label { Text = label1, FontFamily = "OpenSansSemibold", HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center },
            new Label { Text = label2, FontFamily = "OpenSansSemibold", HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center },
            new Label { Text = label3, FontFamily = "OpenSansSemibold", HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center },
            new Label { Text = label4, FontFamily = "OpenSansSemibold", HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center },
            new Label { Text = "Match request", FontFamily = "OpenSansSemibold", HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center }
        };
        
        for (int i = 0; i < columns.Count; i++)
        {
            MatchRequests.Add(columns[i], i + 1, 0);
        }
        var separator = new BoxView
        {
            HeightRequest = 2,
            BackgroundColor = Colors.LightGray,
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.End
        };
        MatchRequests.Add(separator, 0, 0);
        Grid.SetColumnSpan(separator, 6);
    }
    private void AcceptButton_OnClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is Match match)
        {
            _matchService.UpdateMatch(match, Status.Accepted);
            RefreshPage();
        }
    }

    private void RejectButton_OnClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is Match match)
        {
            _matchService.UpdateMatch(match, Status.Rejected);
            RefreshPage();
        }
    }
    private async void ToProfile_OnTapped(object? sender, TappedEventArgs e)
    {
        if (e.Parameter is Match match)
        {
            if (Application.Current.MainPage is NavigationPage navigationPage)
            {
                var profilePage = _serviceProvider.GetRequiredService<ProfilePage>();
                profilePage.BindingContext = match;
                await navigationPage.Navigation.PushAsync(profilePage);
            }
        }
    }

    private async void RefreshPage()
    {
        if (Application.Current.MainPage is NavigationPage navigationPage)
        {
            await navigationPage.Navigation.PushAsync(_serviceProvider.GetRequiredService<MatchRequestsPage>());
        }
    }
}