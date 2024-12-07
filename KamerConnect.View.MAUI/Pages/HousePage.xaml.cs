using KamerConnect.Exceptions;
using KamerConnect.Models;
using KamerConnect.Services;
using KamerConnect.View.MAUI.Views;

namespace KamerConnect.View.MAUI.Pages;

public partial class HousePage : ContentPage
{
    private readonly IServiceProvider _serviceProvider;
    private readonly AuthenticationService _authenticationService;
    private readonly MatchService _matchService;
    public HousePage(IServiceProvider serviceProvider, MatchService matchService, AuthenticationService authenticationService)
    {
        _serviceProvider = serviceProvider;
        _matchService = matchService;
        _authenticationService = authenticationService;
        
        NavigationPage.SetHasNavigationBar(this, false);
        
        InitializeComponent();
      
        var navbar = serviceProvider.GetRequiredService<Navbar>();
        NavbarContainer.Content = navbar;
        
        BindingContextChanged += OnBindingContextChanged;
    }
    
    private void OnBindingContextChanged(object sender, System.EventArgs e)
    {
        if (BindingContext is House house)
        {
            if (house.HouseImages?.Count > 0)
            {
                ImageSlideShow.Images = house.HouseImages;
            }
            else
            {
                ImageSlideShow.Images = null;
            }
        }
    }

    private async void MakeMatch(object sender, System.EventArgs e)
    {
        var session = await _authenticationService.GetSession();
        
        if (BindingContext is House house)
        {
            if (session != null)
                _matchService.CreateMatch(new Match(
                    Guid.Empty, session.personId, house.Id, status.Pending, ""));
        }
    }
    
}