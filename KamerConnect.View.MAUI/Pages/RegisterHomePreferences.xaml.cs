using KamerConnect.DataAccess.Postgres.Repositories;

using KamerConnect.Models;
using KamerConnect.Services;
using Microsoft.Extensions.DependencyInjection;
using KamerConnect.View.MAUI.Pages;



namespace KamerConnect.View.MAUI;

public partial class RegisterHomePreferencesPage : ContentPage
{
    private Person _person;
    private string _password;
    private readonly IServiceProvider _serviceProvider;
  
    public RegisterHomePreferencesPage(IServiceProvider serviceProvider, Person person, string password)
    {
        InitializeComponent();
        _person = person;
        _password = password;
        _serviceProvider =  serviceProvider;
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
        if (homePreferencesForm.ValidateAll())
        {
            HousePreferences preferences = new HousePreferences(Convert.ToDouble(homePreferencesForm.Budget), double.Parse(homePreferencesForm.Area), homePreferencesForm.Type, int.Parse(homePreferencesForm.Residents), null);
            PersonService personService = new PersonService(new PersonRepository());
            AuthenticationService authentication = new AuthenticationService(personService, new AuthenticationRepository());

            Guid preferencesId = personService.CreateHousePreferences(preferences);
            _person.HousePreferencesId = preferencesId;

            authentication.Register(_person, _password);
            if (Application.Current.MainPage is NavigationPage navigationPage)
            {
                await navigationPage.Navigation.PushAsync(_serviceProvider.GetRequiredService<LoginPage>());
            }
        }

    }
}