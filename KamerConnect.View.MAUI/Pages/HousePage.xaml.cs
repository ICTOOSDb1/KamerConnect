using KamerConnect.Exceptions;
using KamerConnect.Models;
using KamerConnect.View.MAUI.Views;

namespace KamerConnect.View.MAUI.Pages;

public partial class HousePage : ContentPage
{
    private readonly IServiceProvider _serviceProvider;

    public HousePage(IServiceProvider serviceProvider, House house)
    {
        _serviceProvider = serviceProvider;
        
        NavigationPage.SetHasNavigationBar(this, false);
        
        InitializeComponent();
        
        var navbar = serviceProvider.GetRequiredService<Navbar>();
        NavbarContainer.Content = navbar;
        
        SetupInfo(house);
    }

    private void SetupInfo(House house)
    {
        ImageSlideShow.Images = house.HouseImages;
        StreetLabel.Text = house.Street;
        CityLabel.Text = house.City;
        PostalcodeLabel.Text = house.PostalCode;
        FullnameLabel.Text = "Niek van den Berg";
        SurfaceLabel.Text = house.Surface.ToString();
        ResidentsLabel.Text = house.Residents.ToString();
    }
}