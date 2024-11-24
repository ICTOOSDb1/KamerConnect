using KamerConnect.Models;
namespace KamerConnect.View.MAUI;

public partial class HomePreferencesPage : ContentPage
{
    Person newPerson;
    public HomePreferencesPage(Person person)
    {
        InitializeComponent();
        newPerson = person;
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
        var preferences = new HousePreferences(homePreferencesForm.Budget, homePreferencesForm.Area, homePreferencesForm.Type);

    }
}