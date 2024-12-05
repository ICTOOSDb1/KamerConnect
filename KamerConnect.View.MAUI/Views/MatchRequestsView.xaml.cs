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
        Match[] matches;
        House house = _houseService.GetByPersonId(_person.Id);
        matches =_matchService.GetMatchesByHouseId(house.Id);
        
        for (int i = 1; i < matches.Length+1 ; i++)
        {

            Person person = _personService.GetPersonById(matches[i-1].personId);
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
            var label1 = new Label { Text = person.FirstName, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center};
            var label2 = new Label { Text = person.Personality.School, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center};
            var label3 = new Label { Text = person.Personality.Study, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center};
            var label4 = new Label { Text = person.BirthDate.ToShortDateString(), HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center};
            MatchRequests.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100, GridUnitType.Absolute) });
            MatchRequests.Add(border, 0, i);
            MatchRequests.Add(label1, 1, i);
            MatchRequests.Add(label2, 2, i);
            MatchRequests.Add(label3, 3, i);
            MatchRequests.Add(label4, 4, i);
            var tapGestureRecognizer = new TapGestureRecognizer
            {
                CommandParameter = matches[i-1]
            };
            tapGestureRecognizer.Tapped += ToProfile_OnTapped;

            // Add the TapGestureRecognizer to the border element
            border.GestureRecognizers.Add(tapGestureRecognizer);
        }
        AddLegend("Voornaam", "School", "Opleiding","Geboortedatum");
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


}