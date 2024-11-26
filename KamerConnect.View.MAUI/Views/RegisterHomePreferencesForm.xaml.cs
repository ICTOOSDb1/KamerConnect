using KamerConnect.Models;

namespace KamerConnect.View.MAUI;

public partial class RegisterHomePreferencesForm : ContentView
{

    public string Budget => BudgetInput.Text ?? string.Empty;
    public string Area => AreaInput.Text ?? string.Empty;
    public HouseType Type;
    public string Residents => ResidentsInput.Text ?? string.Empty;

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
    public bool ValidateAll()
    {
        BudgetInput?.Validate();
        AreaInput?.Validate();
        ResidentsInput?.Validate();




        return (BudgetInput?.IsValid ?? true) &&
               (AreaInput?.IsValid ?? true) &&
               (ResidentsInput?.IsValid ?? true);
    }
}