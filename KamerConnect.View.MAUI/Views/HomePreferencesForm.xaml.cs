using KamerConnect.Models;
using KamerConnect.Services;
using Npgsql;

namespace KamerConnect.View.MAUI.Views;

public partial class HomePreferencesForm : ContentView
{
    private readonly HousePreferenceService _housePreferenceService;
    private Person? _currentPerson;
    private HousePreferences? _housePreferences;
    private HouseType houseTypePreference;
    public HouseType HouseTypePreference
    {
        get => houseTypePreference;
        set
        {
            houseTypePreference = value;
            OnPropertyChanged();
        }
    }

    public HomePreferencesForm(HousePreferenceService housePreferenceService, Person person)
    {
        _currentPerson = person;
        _housePreferenceService = housePreferenceService;
        _housePreferences = _housePreferenceService.GetHousePreferences(person.Id);
       
        BindingContext = _housePreferences;
        InitializeComponent();
    }

    private async void Button_Update_house_preferences(object sender, EventArgs e)
    {
        if (!ValidateForm()) return;
        ;
        _housePreferenceService.UpdateHousePreferences(_housePreferences);
        await Application.Current?.MainPage?.DisplayAlert("Voorkeuren opgeslagen", "Succesvol opgeslagen!", "Ga verder");
    }

    public bool ValidateForm()
    {
        priceEntry.Validate();
        surfaceEntry.Validate();

        return priceEntry.IsValid &&
               surfaceEntry.IsValid;
    }
}