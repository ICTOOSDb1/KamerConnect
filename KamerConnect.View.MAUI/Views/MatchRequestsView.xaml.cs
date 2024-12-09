using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KamerConnect.Models;
using KamerConnect.Services;
using KamerConnect.View.MAUI.Pages;
using Microsoft.Maui.Controls.Shapes;

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

    public MatchRequestsView(IServiceProvider serviceProvider)
    {
        _houseService = serviceProvider.GetRequiredService<HouseService>();
        _serviceProvider = serviceProvider;
        _fileService = serviceProvider.GetRequiredService<FileService>(); ;
        _matchService = serviceProvider.GetRequiredService<MatchService>();
        _authenticationService = serviceProvider.GetRequiredService<AuthenticationService>(); ;
        _personService = serviceProvider.GetRequiredService<PersonService>();
        GetCurrentPerson().GetAwaiter().GetResult();

        InitializeComponent();

        if (_person.Role == Role.Offering)
        {
            GetMatchRequestsOffering();
        }
        else
        {

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

            for (int i = 1; i < matches.Count + 1; i++)
            {
                Person person = _personService.GetPersonById(matches[i - 1].personId);

                var border = AddProfilePicture(person);
                var FirstNameLabel = new Label
                {
                    Text = person.FirstName,
                    HorizontalOptions = LayoutOptions.Center,
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
                    Text = person.BirthDate.ToShortDateString(),
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                };
                
                var horizontalstack = new HorizontalStackLayout { HorizontalOptions = LayoutOptions.Center };
                Button rejectButton = new Button
                {
                    Text = "✖",
                    BackgroundColor = Color.FromRgb(255, 0, 0),
                    TextColor = Color.FromRgb(255, 255, 255),
                    CornerRadius = 20,
                    WidthRequest = 20,
                    HeightRequest = 20,
                    HorizontalOptions = LayoutOptions.End,
                    CommandParameter = matches[i - 1]

                };
                horizontalstack.Add(rejectButton);
                rejectButton.Clicked += RejectButton_OnClicked;


                Button acceptButton = new Button
                {
                    Text = "✔",
                    BackgroundColor = Color.FromRgb(0, 255, 0),
                    TextColor = Color.FromRgb(255, 255, 255),
                    CornerRadius = 20,
                    WidthRequest = 20,
                    HeightRequest = 20,
                    HorizontalOptions = LayoutOptions.End,
                    CommandParameter = matches[i - 1]
                };
                horizontalstack.Add(acceptButton);
                acceptButton.Clicked += AcceptButton_OnClicked;

                MatchRequests.RowDefinitions.Add(new RowDefinition
                { Height = new GridLength(100, GridUnitType.Absolute) });
                MatchRequests.Add(border, 0, i);
                MatchRequests.Add(FirstNameLabel, 1, i);
                MatchRequests.Add(SchoolLabel, 2, i);
                MatchRequests.Add(StudyLabel, 3, i);
                MatchRequests.Add(BirthLabel, 4, i);
                MatchRequests.Add(horizontalstack, 5, i);
                var tapGestureRecognizer = new TapGestureRecognizer
                {
                    CommandParameter = matches[i - 1]
                };
                tapGestureRecognizer.Tapped += ToProfile_OnTapped;

                border.GestureRecognizers.Add(tapGestureRecognizer);
            }
        }
        AddLegend("Voornaam", "School", "Opleiding", "Geboortedatum");
    }

    public Border AddProfilePicture(Person person)
    {
        var border = new Border
        {
            WidthRequest = 100,
            HeightRequest = 100,
            Stroke = Brush.Transparent,
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



    public void AddLegend(string label1, string label2, string label3, string label4)
    {
        var columns = new List<Label>
        {
            new Label { Text = label1, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center },
            new Label { Text = label2, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center },
            new Label { Text = label3, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center },
            new Label { Text = label4, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center },
            new Label { Text = "Match request", HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center }
        };

        for (int i = 0; i < columns.Count; i++)
        {
            MatchRequests.Add(columns[i], i + 1, 0);

        }
    }
    private void AcceptButton_OnClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is Match match)
        {
            _matchService.UpdateMatch(match, status.Accepted);
            RefreshPage();
        }
    }

    private void RejectButton_OnClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is Match match)
        {
            _matchService.UpdateMatch(match, status.Rejected);
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