using KamerConnect.DataAccess.Postgres.Repositys;
using KamerConnect.Models;
using KamerConnect.Services;

namespace KamerConnect.View.MAUI;

public partial class RegisterHomePreferencesPage : ContentPage
{
    private Person _person;
    private string _password;
    public RegisterHomePreferencesPage(Person person, string password)
    {
        InitializeComponent();
        _person = person;
        _password = password;
    }

    private async void Back(object sender, EventArgs e)
    {
        if (Navigation.NavigationStack.Count > 1)
        {
            await Navigation.PopAsync();
        }
    }

    private async void Submit(object sender, EventArgs e)
    {
        HousePreferences preferences = new HousePreferences(Convert.ToDouble(homePreferencesForm.Budget), double.Parse(homePreferencesForm.Area), homePreferencesForm.Type, int.Parse(homePreferencesForm.Residents));
        PersonService personService = new PersonService(new PersonRepository());
        AuthenticationService authentication = new AuthenticationService(personService, new AuthenticationRepository());

        Guid preferencesId = personService.CreateHousePreferences(preferences);
        _person.HousePreferencesId = preferencesId;

        authentication.Register(_person, _password);
    }
}