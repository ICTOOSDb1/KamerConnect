using System.ComponentModel;
using KamerConnect.Models;
using KamerConnect.Services;
using KamerConnect.Utils;

namespace KamerConnect.View.MAUI;

public partial class RegisterHomePreferencesForm : ContentView
{
    private HousePreferenceService _housePreferenceService;
    private readonly GeoLocationService _geoLocationService;
    private HousePreferences? _housePreferences;
    public string MinBudget => MinBudgetInput.Text ?? string.Empty;
    public string MaxBudget => MaxBudgetInput.Text ?? string.Empty;
    public string City => CityInput.Text ?? string.Empty;
    public string Area => AreaInput.Text ?? string.Empty;
    public string Residents => ResidentsInput.Text ?? string.Empty;
    public HouseType Type => _housePreferences?.Type ?? HouseTypeChanged();
    public PreferenceChoice SmokingPreference => _housePreferences?.Smoking ?? PreferenceChoiceTypeChanged(SmokingTypePicker);
    public PreferenceChoice PetPreference => _housePreferences?.Pet ?? PreferenceChoiceTypeChanged(PetTypePicker);
    public PreferenceChoice InteriorPreference => _housePreferences?.Interior ?? PreferenceChoiceTypeChanged(InteriorTypePicker);
    public PreferenceChoice ParkingPreference => _housePreferences?.Parking ?? PreferenceChoiceTypeChanged(ParkingTypePicker);
    public int TravelTime => int.Parse(TravelTimeInput.Text);
    public Profile TravelProfile => _housePreferences?.SearchArea.Profile ?? TravelingProfileChanged();

    public RegisterHomePreferencesForm()
    {
        InitializeComponent();

        SmokingTypePicker.SelectedItem = "Geen voorkeur";

        PetTypePicker.SelectedItem = "Geen voorkeur";
        InteriorTypePicker.SelectedItem = "Geen voorkeur";
        ParkingTypePicker.SelectedItem = "Geen voorkeur";
        HouseTypePicker.SelectedItem = "Appartement";
        HouseTypePicker.SelectedItem = "Fiets";
    }

    public RegisterHomePreferencesForm(HousePreferenceService housePreferenceService, GeoLocationService geoLocationService, Person person)
    {
        _housePreferenceService = housePreferenceService;
        _geoLocationService = geoLocationService;
        _housePreferences = _housePreferenceService.GetHousePreferences(person.Id);
        BindingContext = _housePreferences;
        InitializeComponent();

        SmokingTypePicker.SelectedItem = _housePreferences?.Smoking.GetDisplayName();
        PetTypePicker.SelectedItem = _housePreferences?.Pet.GetDisplayName();
        InteriorTypePicker.SelectedItem = _housePreferences?.Interior.GetDisplayName();
        ParkingTypePicker.SelectedItem = _housePreferences?.Parking.GetDisplayName();
        HouseTypePicker.SelectedItem = _housePreferences?.Type.GetDisplayName();
        TravelProfileInput.SelectedItem = _housePreferences.SearchArea.Profile.GetDisplayName();
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
        _housePreferences.Smoking = PreferenceChoiceTypeChanged(SmokingTypePicker);
        _housePreferences.Pet = PreferenceChoiceTypeChanged(PetTypePicker);
        _housePreferences.Interior = PreferenceChoiceTypeChanged(InteriorTypePicker);
        _housePreferences.Parking = PreferenceChoiceTypeChanged(ParkingTypePicker);
        _housePreferences.Type = HouseTypeChanged();
        _housePreferences.SearchArea.Profile = TravelingProfileChanged();

        _housePreferences.CityGeolocation = await _geoLocationService.GetGeoCode(_housePreferences.City);
        _housePreferences.SearchArea = new SearchArea(_housePreferences.SearchArea.Id, _housePreferences.SearchArea.Range, _housePreferences.SearchArea.Profile, await _geoLocationService.GetRangePolygon(_housePreferences.SearchArea.Range, _housePreferences.SearchArea.Profile, _housePreferences.CityGeolocation));
        _housePreferenceService.UpdateHousePreferences(_housePreferences);
        await Application.Current?.MainPage?.DisplayAlert("Voorkeuren opgeslagen", "Succesvol opgeslagen!", "Ga verder");
    }

    public bool ValidateAll()
    {
        MinBudgetInput?.Validate();
        MaxBudgetInput?.Validate();
        AreaInput?.Validate();
        ResidentsInput?.Validate();
        CityInput?.Validate();
        TravelTimeInput?.Validate();

        return (MinBudgetInput?.IsValid ?? true) &&
               (MaxBudgetInput?.IsValid ?? true) &&
               (AreaInput?.IsValid ?? true) &&
               (CityInput?.IsValid ?? true) &&
               (ResidentsInput?.IsValid ?? true) &&
               (TravelTimeInput?.IsValid ?? true);
    }

    private Profile TravelingProfileChanged()
    {
        switch ($"{TravelProfileInput.SelectedItem}")
        {
            case "Auto":
                return Profile.driving_car;
            case "Fiets":
                return Profile.cycling_regular;
        }

        return Profile.cycling_regular;
    }

    private HouseType HouseTypeChanged()
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