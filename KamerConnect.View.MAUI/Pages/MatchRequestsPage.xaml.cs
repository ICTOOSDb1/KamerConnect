using KamerConnect.View.MAUI.Views;

namespace KamerConnect.View.MAUI.Pages;

public partial class MatchRequestsPage : ContentPage
{
    public MatchRequestsPage(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        MatchView.Content = serviceProvider.GetRequiredService<MatchRequestsView>();

        NavigationPage.SetHasNavigationBar(this, false);
        var navbar = serviceProvider.GetRequiredService<Navbar>();
        NavbarContainer.Content = navbar;
    }

}