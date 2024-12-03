using KamerConnect.Models;
using KamerConnect.Services;
using KamerConnect.Utils;

namespace KamerConnect.View.MAUI;

public partial class RegisterHomePreferencesForm : ContentView
{
    private Person _currentPerson;
    private HousePreferenceService _housePreferenceService;
    private HousePreferences? _housePreferences; 
    public string MinBudget => MinBudgetInput.Text ?? string.Empty;
    public string MaxBudget => MaxBudgetInput.Text ?? string.Empty;
    public string Area => AreaInput.Text ?? string.Empty;
    public HouseType Type;
    public string Residents => ResidentsInput.Text ?? string.Empty;
    public PreferenceChoice SmokingPreference => PreferenceChoiceTypeChanged(SmokingTypePicker);
    public PreferenceChoice PetPreference => PreferenceChoiceTypeChanged(PetTypePicker);
    public PreferenceChoice InteriorPreference => PreferenceChoiceTypeChanged(InteriorTypePicker);
    public PreferenceChoice ParkingPreference => PreferenceChoiceTypeChanged(ParkingTypePicker);

    public RegisterHomePreferencesForm()
	{
		InitializeComponent();
	}
    public RegisterHomePreferencesForm(HousePreferenceService housePreferenceService, Person person)
    {
        _currentPerson = person;
        _housePreferenceService = housePreferenceService;
        _housePreferences = _housePreferenceService.GetHousePreferences(person.Id);
        BindingContext = _housePreferences;
        InitializeComponent();

    }

    private PreferenceChoice PreferenceChoiceTypeChanged(Picker picker)
    {        
        switch ($"{picker.SelectedItem}")
        {
            case "Ja":
                 return PreferenceChoice.Yes;
            case "Nee":
                return PreferenceChoice.No; 
            case "Geen voorkeur":
                return PreferenceChoice.No_preference; 
        }

        return PreferenceChoice.No;
    }
    public async void Button_Update_house_preferences()
    {
        if (!ValidateAll()) return;
        _housePreferenceService.UpdateHousePreferences(_housePreferences);
        await Application.Current?.MainPage?.DisplayAlert("Voorkeuren opgeslagen", "Succesvol opgeslagen!", "Ga verder");

    }
    public bool ValidateAll()
    {
        MinBudgetInput?.Validate();
        MaxBudgetInput?.Validate();
        AreaInput?.Validate();
        ResidentsInput?.Validate();
        
        return (MinBudgetInput?.IsValid ?? true) &&
               (MaxBudgetInput?.IsValid ?? true) &&
               (AreaInput?.IsValid ?? true) &&
               (ResidentsInput?.IsValid ?? true);
    }
    
    private async void HouseTypeChanged(object sender, EventArgs e)
    {
        switch ($"{HouseTypePicker.SelectedItem}")
        {
            case "Appartement":
                Type = HouseType.Apartment;
                break;
            case "Huis":
                Type = HouseType.House;
                break;
            case "Studio":
                Type = HouseType.Studio;
                break;
        }
    }
}