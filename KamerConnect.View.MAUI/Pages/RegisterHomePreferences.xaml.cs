using KamerConnect.Models;
using KamerConnect.Services;
using KamerConnect.View.MAUI.Pages;

namespace KamerConnect.View.MAUI;

public partial class RegisterHomePreferencesPage : ContentPage
{
    private Person _person;
    private string _password;
    private readonly HousePreferenceService _housePreferenceService;
    private readonly AuthenticationService _authenticationService;
    private readonly IServiceProvider _serviceProvider;

    public RegisterHomePreferencesPage(IServiceProvider serviceProvider, Person person, string password)
    {
        InitializeComponent();

        _person = person;
        _password = password;
        _serviceProvider = serviceProvider;
        _housePreferenceService = _serviceProvider.GetRequiredService<HousePreferenceService>();
        _authenticationService = _serviceProvider.GetRequiredService<AuthenticationService>();
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
            HousePreferences preferences = new HousePreferences(
                double.Parse(homePreferencesForm.MaxBudget),
                double.Parse(homePreferencesForm.MaxBudget),
                double.Parse(homePreferencesForm.Area),
                PickerOptions.TranslateHouseType(homePreferencesForm.Type),
                int.Parse(homePreferencesForm.Residents),
                homePreferencesForm.SmokingPreference,
                homePreferencesForm.PetPreference,
                homePreferencesForm.InteriorPreference,
                homePreferencesForm.ParkingPreference,
                Guid.NewGuid()
            );

            _housePreferenceService.CreateHousePreferences(preferences);

            _authenticationService.Register(_person, _password);

            if (Application.Current.MainPage is NavigationPage navigationPage)
            {
                await navigationPage.Navigation.PushAsync(_serviceProvider.GetRequiredService<LoginPage>());
            }
        }
    }
}