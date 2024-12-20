using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KamerConnect.Models;
using KamerConnect.Services;
using KamerConnect.View.MAUI.Pages;
using KamerConnect.View.MAUI.Utils;
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
        InitializeComponent();
        _houseService = serviceProvider.GetRequiredService<HouseService>();
        _serviceProvider = serviceProvider;
        _fileService = serviceProvider.GetRequiredService<FileService>();
        _matchService = serviceProvider.GetRequiredService<MatchService>();
        _authenticationService = serviceProvider.GetRequiredService<AuthenticationService>();
        _personService = serviceProvider.GetRequiredService<PersonService>();
        GetCurrentPerson().GetAwaiter().GetResult();
        if (_person.Role == Role.Offering)
        {
            GetMatchRequestsOffering();
        }
        else if (_person.Role == Role.Seeking)
        {
            GetMatchRequestsSeeking();
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
        matches = _matchService.GetPendingMatchesById(house.Id);
        if (matches.Count != 0)
        {
            AddLegend("Voornaam", "School", "Opleiding", "Geboortedatum");

            for (int i = 1; i < matches.Count + 1; i++)
            {

                Person person = _personService.GetPersonById(matches[i - 1].personId);

                ImageSource imageSource = person.ProfilePicturePath != null
                    ? _fileService.GetFilePath(_bucketName, person.ProfilePicturePath)
                    : "user.png";
                Border profilePicture = AddPicture(imageSource);
                Label FullNameLabel = new Label
                {
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    Text = person.FullName,
                };
                Label SchoolLabel = new Label();
                Label StudyLabel = new Label();
                if (person.Personality != null)
                {
                    SchoolLabel.Text = person.Personality.School;
                    SchoolLabel.HorizontalOptions = LayoutOptions.Center;
                    SchoolLabel.VerticalOptions = LayoutOptions.Center;

                    StudyLabel.Text = person.Personality.Study;
                    StudyLabel.HorizontalOptions = LayoutOptions.Center;
                    StudyLabel.VerticalOptions = LayoutOptions.Center;
                }

                Label BirthLabel = new Label
                {
                    Text = person.BirthDate.ToShortDateString(),
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                };
                var horizontalButtonStack = CreateButtons(matches, i);
                var separator = CreateSeparator();

                MatchRequests.RowDefinitions.Add(new RowDefinition
                { Height = new GridLength(120, GridUnitType.Absolute) });
                MatchRequests.Add(separator, 0, i);
                Grid.SetColumnSpan(separator, 6);
                MatchRequests.Add(profilePicture, 0, i);
                MatchRequests.Add(FullNameLabel, 1, i);
                MatchRequests.Add(SchoolLabel, 2, i);
                MatchRequests.Add(StudyLabel, 3, i);
                MatchRequests.Add(BirthLabel, 4, i);
                MatchRequests.Add(horizontalButtonStack, 5, i);
                var tapGestureRecognizer = new TapGestureRecognizer
                {
                    CommandParameter = matches[i - 1]
                };
                tapGestureRecognizer.Tapped += ToProfile_OnTapped;

                profilePicture.GestureRecognizers.Add(tapGestureRecognizer);
            }
        }
        else
        {
            DisplayNoMatchRequests();
        }
    }


    public void GetMatchRequestsSeeking()
    {
        List<Match> matches;

        Person person = _personService.GetPersonById(_person.Id);
        if (person == null)
        {
            DisplayNoMatchRequests();
            return;
        }
        matches = _matchService.GetMatchesById(person.Id);
        if (matches == null)
        {
            DisplayNoMatchRequests();
            return;
        }
        AddLegend("Straat", "Stad", "Type", "Prijs");

        for (int i = 1; i < matches.Count + 1; i++)
        {
            House house = _houseService.Get(matches[i - 1].houseId);

            ImageSource imageSource = house.HouseImages[0].Path != null
                ? house.HouseImages[0].FullPath
                : "house.png";
            Border housePicture = AddPicture(imageSource);

            string houseTypeTranslation = "";
            switch (house.Type)
            {
                case HouseType.Apartment:
                    houseTypeTranslation = "Appartement";
                    break;
                case HouseType.House:
                    houseTypeTranslation = "Huis";
                    break;
                case HouseType.Studio:
                    houseTypeTranslation = "Studio";
                    break;
            }
            var separator = CreateSeparator();
            Label label1 = new Label { Text = house.Street, FontFamily = "InterRegular", HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center };
            Label label2 = new Label { Text = house.City, FontFamily = "InterRegular", HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center };
            Label label3 = new Label { Text = houseTypeTranslation, FontFamily = "InterRegular", HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center };
            Label label4 = new Label { Text = "€" + house.Price, FontFamily = "InterRegular", HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center };
            MatchRequests.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100, GridUnitType.Absolute) });
            MatchRequests.Add(separator, 0, i);
            Grid.SetColumnSpan(separator, 6);
            MatchRequests.Add(housePicture, 0, i);
            MatchRequests.Add(label1, 1, i);
            MatchRequests.Add(label2, 2, i);
            MatchRequests.Add(label3, 3, i);
            MatchRequests.Add(label4, 4, i);
            DisplayStatus(i, matches[i - 1].Status);
            var tapGestureRecognizer = new TapGestureRecognizer
            {
                CommandParameter = matches[i - 1]
            };
            tapGestureRecognizer.Tapped += ToHouse_OnTapped;

            housePicture.GestureRecognizers.Add(tapGestureRecognizer);
        }
    }

    public Border AddPicture(ImageSource imageSource)
    {
        var border = new Border
        {
            WidthRequest = 100,
            HeightRequest = 100,
            Stroke = null,
            StrokeThickness = 0,
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Center,
            Content = new Image
            {
                Source = imageSource,
                Aspect = Aspect.AspectFill,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            }
        };
        return border;
    }

    public void AddLegend(string label1, string label2, string label3, string label4)
    {
        MatchRequests.RowDefinitions.Add(new RowDefinition { Height = new GridLength(40, GridUnitType.Absolute) });
        var columns = new List<Label>
        {
            new Label { Text = label1, FontFamily= "InterBold", HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center },
            new Label { Text = label2, FontFamily= "InterBold",HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center },
            new Label { Text = label3, FontFamily= "InterBold",HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center },
            new Label { Text = label4, FontFamily= "InterBold",HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center },
            new Label { Text = "Status", FontFamily= "InterBold",HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center }
        };

        for (int i = 0; i < columns.Count; i++)
        {
            MatchRequests.Add(columns[i], i + 1, 0);
            var separator = CreateSeparator();
            MatchRequests.Add(separator, 0, 0);
            Grid.SetColumnSpan(separator, 6);
        }
    }

    private HorizontalStackLayout CreateButtons(List<Match> matches, int iterator)
    {
        var horizontalStack = new HorizontalStackLayout
        {
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            Spacing = -30
        };
        Button rejectButton = new Button
        {
            Text = Icon.CircleXmark,
            TextColor = Colors.Red,
            FontFamily = "FaIcons",
            FontSize = 32,
            BackgroundColor = Colors.Transparent,
            CommandParameter = matches[iterator - 1],
            WidthRequest = 110,
            HeightRequest = 110
        };
        horizontalStack.Children.Add(rejectButton);
        rejectButton.Clicked += RejectButton_OnClicked;

        Button acceptButton = new Button
        {
            Text = Icon.CircleCheck,
            TextColor = Colors.Green,
            FontFamily = "FaIcons",
            FontSize = 32,
            BackgroundColor = Colors.Transparent,
            CommandParameter = matches[iterator - 1],
            WidthRequest = 110,
            HeightRequest = 110
        };
        horizontalStack.Children.Add(acceptButton);
        acceptButton.Clicked += AcceptButton_OnClicked;
        return horizontalStack;
    }

    public void DisplayStatus(int row, Status status)
    {
        var statusLabel = new Label
        {
            FontFamily = "InterRegular",
            VerticalTextAlignment = TextAlignment.Center
        };
        var statusImage = new Label
        {
            Text = "\u25CF",
            FontSize = 32
        };
        switch (status)
        {
            case Status.Accepted:
                statusImage.TextColor = Colors.Green;
                statusLabel.Text = "Geaccepteerd";
                break;
            case Status.Pending:
                statusImage.TextColor = Colors.Orange;
                statusLabel.Text = "In behandeling";
                break;
            case Status.Rejected:
                statusImage.TextColor = Colors.Red;
                statusLabel.Text = "Geweigerd";
                break;

        }
        var separator = new BoxView
        {
            HeightRequest = 2,
            BackgroundColor = Colors.LightGray,
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.End
        };
        var buttonContainer = new HorizontalStackLayout
        {
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            Spacing = 20

        };
        buttonContainer.Children.Add(statusImage);
        buttonContainer.Children.Add(statusLabel);
        MatchRequests.Add(separator, 0, 0);
        MatchRequests.Add(buttonContainer, 5, row);
        Grid.SetColumnSpan(separator, 6);
    }
    private void AcceptButton_OnClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is Match match)
        {
            _matchService.UpdateStatusMatch(match, Status.Accepted);
            RefreshPage();
        }
    }

    private void RejectButton_OnClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is Match match)
        {
            _matchService.UpdateStatusMatch(match, Status.Rejected);
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
                Application.Current.MainPage = new NavigationPage(profilePage);
            }
        }
    }

    private async void ToHouse_OnTapped(object? sender, TappedEventArgs e)
    {
        if (e.Parameter is Match match)
        {
            if (Application.Current.MainPage is NavigationPage navigationPage)
            {
                House house = _houseService.Get(match.houseId);
                var housePage = _serviceProvider.GetRequiredService<HousePage>();
                housePage.BindingContext = house;
                Application.Current.MainPage = new NavigationPage(housePage);
            }
        }
    }
    private async void RefreshPage()
    {
        if (Application.Current.MainPage is NavigationPage navigationPage)
        {
            App.Current.MainPage = new NavigationPage(_serviceProvider.GetRequiredService<MatchRequestsPage>());

        }
    }

    private BoxView CreateSeparator()
    {
        return new BoxView
        {
            HeightRequest = 2,
            BackgroundColor = Colors.LightGray,
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.End
        };
    }

    public void DisplayNoMatchRequests()
    {
        var noMatchRequests = new Label
        {
            Text = "Er zijn geen matchverzoeken weer te geven.",
            FontSize = 25,
            FontFamily = "InterLight",
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            TextColor = Colors.Black
        };
        MatchRequests.RowDefinitions.Add(new RowDefinition
        { Height = new GridLength(100, GridUnitType.Absolute) });
        MatchRequests.Add(noMatchRequests, 0, 0);
        Grid.SetColumnSpan(noMatchRequests, 6);
    }
}