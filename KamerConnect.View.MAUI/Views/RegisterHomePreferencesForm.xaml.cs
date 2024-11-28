using KamerConnect.Models;
using KamerConnect.Utils;

namespace KamerConnect.View.MAUI;

public partial class RegisterHomePreferencesForm : ContentView
{

    public string MinBudget => MinBudgetInput.Text ?? string.Empty;
    public string MaxBudget => MaxBudgetInput.Text ?? string.Empty;
    public string Area => AreaInput.Text ?? string.Empty;
    public HouseType Type;
    public string Residents => ResidentsInput.Text ?? string.Empty;
    public PreferenceChoice SmokingPreference => PreferenceChoiceTypeChanged(SmokersPicker);
    public PreferenceChoice PetPreference => PreferenceChoiceTypeChanged(PetPicker);
    public PreferenceChoice InteriorPreference => PreferenceChoiceTypeChanged(InteriorPicker);
    public PreferenceChoice ParkingPreference => PreferenceChoiceTypeChanged(ParkingPicker);

    public RegisterHomePreferencesForm()
	{
		InitializeComponent();
	}
	
	private async void HouseTypeChanged(object sender, EventArgs e)
	{        
        switch ($"{HousetypePicker.SelectedItem}")
        {
            case "Huis":
                Type = HouseType.House; 
                break;
            case "Appartement":
                Type = HouseType.Apartment; 
                break;
            case "Studio":
                Type = HouseType.Studio; 
                break;
            }
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
                return PreferenceChoice.No_Preferences; 
        }

        return PreferenceChoice.No;
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
}