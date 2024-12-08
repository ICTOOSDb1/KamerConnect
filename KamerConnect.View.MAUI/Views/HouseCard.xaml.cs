using KamerConnect.Models;
using KamerConnect.View.MAUI.Pages;

namespace KamerConnect.View.MAUI.Views;

public partial class HouseCard : ContentView
{
    public HouseCard()
    {
        InitializeComponent();
        BindingContextChanged += OnBindingContextChanged;
    }

    private void OnHouseCardTapped(object sender, EventArgs e)
    {
        if (BindingContext is House house)
        {
            var housePage = MauiProgram.Services.GetRequiredService<HousePage>();
            housePage.BindingContext = house;

            App.Current.MainPage = new NavigationPage(housePage);
        }
    }

    private void OnBindingContextChanged(object sender, EventArgs e)
    {
        if (BindingContext is House house)
        {
            if (house.HouseImages?.Count > 0)
            {
                ImageControl.Source = house.HouseImages[0].FullPath;
            }
            else
            {
                ImageControl.Source = null;
            }
        }
    }
}