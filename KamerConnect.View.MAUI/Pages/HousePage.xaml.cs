using KamerConnect.Exceptions;
using KamerConnect.Models;
using KamerConnect.View.MAUI.Views;

namespace KamerConnect.View.MAUI.Pages;

public partial class HousePage : ContentPage
{
    private readonly IServiceProvider _serviceProvider;
    private House _house;

    public HousePage(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        
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
    
}