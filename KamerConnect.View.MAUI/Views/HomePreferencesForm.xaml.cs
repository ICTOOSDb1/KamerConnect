using KamerConnect.Models;
using KamerConnect.Services;
using Npgsql;

namespace KamerConnect.View.MAUI.Views;

public partial class HomePreferencesForm : ContentView
{
    private readonly HousePreferenceService _housePreferenceService;
    private Person? _currentPerson;
    private HousePreferences? _housePreferences;
    private PickerOptions.DutchHouseType houseTypePreference;
    public PickerOptions.DutchHouseType HouseTypePreference
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
        if (_housePreferences != null)
        {
            HouseTypePreference = PickerOptions.TranslateHouseType(_housePreferences.Type);
        }
        BindingContext = _housePreferences;
        InitializeComponent();
    }

    private async void Button_Update_house_preferences(object sender, EventArgs e)
    {
        if (!ValidateForm()) return;

        CheckIfHomeTypeIsPicked(_currentPerson);
        _housePreferenceService.UpdateHousePreferences(_housePreferences);
        await Application.Current?.MainPage?.DisplayAlert("Voorkeuren opgeslagen", "Succesvol opgeslagen!", "Ga verder");
    }

    public void CheckIfHomeTypeIsPicked(Person person)
    {
        var selectedOption = HometypePicker.SelectedItem?.ToString();
        if (Enum.TryParse(selectedOption, true, out HouseType houseType))
        {
            _housePreferences.Type = houseType;
        }
    }

    public bool ValidateForm()
    {
        priceEntry.Validate();
        surfaceEntry.Validate();

        return priceEntry.IsValid &&
               surfaceEntry.IsValid;
    }
}