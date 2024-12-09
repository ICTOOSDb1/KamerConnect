using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KamerConnect.Models;
using KamerConnect.Services;
using Microsoft.Maui.Controls.Shapes;
using KamerConnect.View.MAUI.Utils;

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
        else
        {
            AddLegend("Straat", "Stad", "Type","Prijs");
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
    Match[] matches;
    House house = _houseService.GetByPersonId(_person.Id);
    if (house == null) return;
    matches = _matchService.GetMatchesById(house.Id);
    if (matches == null) return;
    for (int i = 1; i < matches.Length + 1; i++)
    {
        Person person = _personService.GetPersonById(matches[i - 1].personId);
        

        MatchRequests.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100, GridUnitType.Absolute) });
        
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
                    : "user.png",
                Aspect = Aspect.AspectFit,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            }
        };
        
        var separator = CreateSeparator();
        var label1 = new Label
        {
            Text = person.FirstName,
            FontFamily = "OpenSansRegular",
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };
        var label2 = new Label
        {
            Text = person.Personality?.School ?? "",
            FontFamily = "OpenSansRegular",
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };
        var label3 = new Label
        {
            Text = person.Personality?.Study ?? "",
            FontFamily = "OpenSansRegular",
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };
        var label4 = new Label
        {
            Text = person.BirthDate.ToShortDateString(),
            FontFamily = "OpenSansRegular",
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };
        
        MatchRequests.Add(separator, 0, i);
        Grid.SetColumnSpan(separator, 6);
        MatchRequests.Add(border, 0, i);
        MatchRequests.Add(label1, 1, i);
        MatchRequests.Add(label2, 2, i);
        MatchRequests.Add(label3, 3, i);
        MatchRequests.Add(label4, 4, i);
        
        DisplayStatus(i, matches[i - 1].Status);
    }
}


    public void GetMatchRequestsSeeking()
    {
        Match[] matches;
        
        Person person = _personService.GetPersonById(_person.Id);
        if (person == null) return;
        matches =_matchService.GetMatchesById(person.Id);
        if (matches == null) return;
        
        for (int i = 1; i < matches.Length+1 ; i++)
        {

            House house = _houseService.Get(matches[i-1].houseId);

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
                    Source = house.HouseImages[0].Path != null
                        ? house.HouseImages[0].FullPath
                : "house.png",
                    Aspect = Aspect.AspectFit, 
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center
                }
            };
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
            var label1 = new Label { Text = house.Street,  FontFamily = "OpenSansRegular", HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center};
            var label2 = new Label { Text = house.City,  FontFamily = "OpenSansRegular", HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center};
            var label3 = new Label { Text = houseTypeTranslation,  FontFamily = "OpenSansRegular", HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center};
            var label4 = new Label { Text = "€"+house.Price,  FontFamily = "OpenSansRegular", HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center};
            MatchRequests.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100, GridUnitType.Absolute) });
            MatchRequests.Add(separator, 0, i);
            Grid.SetColumnSpan(separator, 6);
            MatchRequests.Add(border, 0, i);
            MatchRequests.Add(label1, 1, i);
            MatchRequests.Add(label2, 2, i);
            MatchRequests.Add(label3, 3, i);
            MatchRequests.Add(label4, 4, i);
        }
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
            new Label { Text = "Match request", FontFamily= "InterBold",HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center }
        };
        
        for (int i = 0; i < columns.Count; i++)
        {
            MatchRequests.Add(columns[i], i + 1, 0);
            /*MatchRequests.Add(new Image
            {
                Scale = 0.3,
                Source = "arrowupdown.png",
                Aspect = Aspect.AspectFit,
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Center
            }, i + 1, 0);*/
            var separator = CreateSeparator();
            MatchRequests.Add(separator, 0, 0);
            Grid.SetColumnSpan(separator, 6);
        }
    }
    
    public void DisplayStatus(int row, status status)
    {
        var statusLabel = new Label
        {
            FontFamily = "InterRegular",
            VerticalTextAlignment = TextAlignment.Center
        };
        var statusImage = new Label
        {
            Text = "\u25CF",
            Scale = 3
        };
        switch (status)
        {
            case status.Accepted:
                statusImage.TextColor = Colors.Green;
                statusLabel.Text = "Geaccepteerd";
                break;
            case status.Pending:
                statusImage.TextColor = Colors.Yellow;
                statusLabel.Text = "In behandeling";
                break;
            case status.Rejected:
                statusImage.TextColor = Colors.Red;
                statusLabel.Text = "Geweigerd";
                break;
        }
        var buttonContainer = new HorizontalStackLayout
        {
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            Spacing = 20
            
        };
        buttonContainer.Children.Add(statusImage);
        buttonContainer.Children.Add(statusLabel);
        MatchRequests.Add(buttonContainer, 5, row);
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
}