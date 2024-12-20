using KamerConnect.Exceptions;
using KamerConnect.Models;
using KamerConnect.Services;
using KamerConnect.Utils;
using KamerConnect.View.MAUI.Views;

namespace KamerConnect.View.MAUI.Pages;

public partial class HousePage : ContentPage
{
    private readonly IServiceProvider _serviceProvider;
    private readonly AuthenticationService _authenticationService;
    private readonly MatchService _matchService;
    private readonly PersonService _personService;

    private Person? owner { get; set; }
    public HousePage(IServiceProvider serviceProvider, MatchService matchService, PersonService personService,
        AuthenticationService authenticationService)
    {
        _serviceProvider = serviceProvider;
        _matchService = matchService;
        _personService = personService;
        _authenticationService = authenticationService;

        NavigationPage.SetHasNavigationBar(this, false);

        InitializeComponent();

        var navbar = serviceProvider.GetRequiredService<Navbar>();
        NavbarContainer.Content = navbar;

        BindingContextChanged += OnBindingContextChanged;
    }

    private async void OnBindingContextChanged(object sender, System.EventArgs e)
    {
        if (BindingContext is House house)
        {
            owner = _personService.GetPersonByHouseId(house.Id);

            await CheckIfAlreadyMatched();

            HouseTypeLabel.Text = house.Type.GetDisplayName();

            FullnameLabel.Text = $" {owner.FirstName} {owner.MiddleName} {owner.Surname}";

            ImageSlideShow.Images = house.HouseImages?.Any() == true ? house.HouseImages : null;
        }
    }

    private async Task CheckIfAlreadyMatched()
    {
        var session = await _authenticationService.GetSession();
        if (BindingContext is House house)
        {
            var list = _matchService.GetMatchesById(house.Id);

            if (list == null)
            {
                RegisterButton.IsVisible = true;
                StateLabel.IsVisible = false;
                return;
            }

            if (list.Any(m => m.personId == session.personId))
            {
                RegisterButton.IsVisible = false;

                Match currentMatch = list.SingleOrDefault(m => m.personId == session.personId);

                StateLabel.Text = currentMatch.Status.GetDisplayName();
                StateLabel.IsVisible = true;
            }
        }
    }


    private async void MakeMatch(object sender, System.EventArgs e)
    {
        var session = await _authenticationService.GetSession();

        string result = await MotivationForm.DisplayMotivationPopup();
        if (result == null) return;

        if (BindingContext is House house)
        {
            if (session != null)
            {
                _matchService.CreateMatch(new Match(
                    Guid.Empty, session.personId, house.Id, Status.Pending, result));
            }
            RegisterButton.IsVisible = false;
            StateLabel.Text = Status.Pending.GetDisplayName();
            StateLabel.IsVisible = true;
        }
    }
}