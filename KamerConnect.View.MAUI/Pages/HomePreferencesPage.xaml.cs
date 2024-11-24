using KamerConnect.Models;
namespace KamerConnect.View.MAUI;

public partial class HomePreferencesPage : ContentPage
{

    public HomePreferencesPage()
    {
        InitializeComponent();
    }

    private async void Back(object sender, EventArgs e)
    {
        if (Navigation.NavigationStack.Count > 1)
        {
            await Navigation.PopAsync();
        }
        else
        {
            
        }
    }
    private async void Submit(object sender, EventArgs e)
    {
        var preferences = new HousePreferences();

    }
}