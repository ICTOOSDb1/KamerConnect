using KamerConnect.Exceptions;
using KamerConnect.View.MAUI.Views;

namespace KamerConnect.View.MAUI.Pages;

public partial class HousePage : ContentPage
{
    private readonly IServiceProvider _serviceProvider;

    public HousePage(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        
        NavigationPage.SetHasNavigationBar(this, false);
        
        InitializeComponent();
        
        var navbar = serviceProvider.GetRequiredService<Navbar>();
        NavbarContainer.Content = navbar;
    }
}