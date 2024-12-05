using System.ComponentModel;
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
    public string Residents => ResidentsInput.Text ?? string.Empty;
    public HouseType Type =>  _housePreferences?.Type ?? HouseTypeChanged();
    public PreferenceChoice SmokingPreference => _housePreferences?.Smoking ?? PreferenceChoiceTypeChanged(SmokingTypePicker);
    public PreferenceChoice PetPreference => _housePreferences?.Pet ?? PreferenceChoiceTypeChanged(PetTypePicker);
    public PreferenceChoice InteriorPreference => _housePreferences?.Interior ?? PreferenceChoiceTypeChanged(InteriorTypePicker);
    public PreferenceChoice ParkingPreference => _housePreferences?.Parking ?? PreferenceChoiceTypeChanged(ParkingTypePicker);

    public RegisterHomePreferencesForm()
	{
		InitializeComponent();
        
        SmokingTypePicker.SelectedItem = "Geen voorkeur";
        PetTypePicker.SelectedItem = "Geen voorkeur";
        InteriorTypePicker.SelectedItem = "Geen voorkeur";
        ParkingTypePicker.SelectedItem = "Geen voorkeur";
        HouseTypePicker.SelectedItem = "Appartement";
    }
    public RegisterHomePreferencesForm(HousePreferenceService housePreferenceService, Person person)
    {
        _currentPerson = person;
        _housePreferenceService = housePreferenceService;
        _housePreferences = _housePreferenceService.GetHousePreferences(person.Id);
        BindingContext = _housePreferences;
        InitializeComponent();
        
        SmokingTypePicker.SelectedItem = _housePreferences?.Smoking.GetDisplayName();
        PetTypePicker.SelectedItem = _housePreferences?.Pet.GetDisplayName();
        InteriorTypePicker.SelectedItem = _housePreferences?.Interior.GetDisplayName();
        ParkingTypePicker.SelectedItem = _housePreferences?.Parking.GetDisplayName();
        HouseTypePicker.SelectedItem = _housePreferences?.Type.GetDisplayName();
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

        return PreferenceChoice.No_preference;
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
    
    private  HouseType HouseTypeChanged()
    {
        switch ($"{HouseTypePicker.SelectedItem}")
        {
            case "Appartement":
                return HouseType.Apartment;
            case "Huis":
                return HouseType.House;
            case "Studio":
                return HouseType.Studio;
        }

        return HouseType.Apartment;
    }
}