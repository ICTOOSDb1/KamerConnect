using KamerConnect.DataAccess.Postgres.Repositys;
using KamerConnect.Models;
using KamerConnect.Services;

namespace KamerConnect.View.MAUI;

public partial class RegisterHomePreferencesPage : ContentPage
{
    Person newPerson;
    public RegisterHomePreferencesPage(Person person)
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
        HousePreferences preferences = new HousePreferences(homePreferencesForm.Budget, homePreferencesForm.Area, homePreferencesForm.Type);
        AuthenticationService authentication = new AuthenticationService(new PersonRepository());
        authentication.Register(newPerson, "password123");
        DisplayAlert("test", "Succes!", "Ok.");
    }
}