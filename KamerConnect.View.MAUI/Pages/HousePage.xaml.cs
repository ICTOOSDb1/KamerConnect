using KamerConnect.Exceptions;
using KamerConnect.Models;
using KamerConnect.View.MAUI.Views;

namespace KamerConnect.View.MAUI.Pages;

public partial class HousePage : ContentPage
{

    public HousePage(IServiceProvider serviceProvider)
    {
        NavigationPage.SetHasNavigationBar(this, false);
        
        InitializeComponent();
      
        var navbar = MauiProgram.Services.GetRequiredService<Navbar>();
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

    private void MakeMatch(object sender, System.EventArgs e)
    {
        
    }
    
}