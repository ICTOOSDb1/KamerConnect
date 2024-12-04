using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KamerConnect.Services;
using KamerConnect.View.MAUI;
using KamerConnect.View.MAUI.Views;

namespace KamerConnect.View.MAUI.Pages;

public partial class MatchRequestsPage : ContentPage
{
    public MatchRequestsPage()
    {
        InitializeComponent();
        MatchContainer.Content = new MatchRequestsView(new SentMatchesService());
    }
    
}