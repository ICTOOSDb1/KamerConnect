using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KamerConnect.Services;
using KamerConnect.View.MAUI.Views;
using KamerConnect.Models;

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