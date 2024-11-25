using KamerConnect.Models;

namespace KamerConnect.View.MAUI;

public partial class RegisterHomePreferencesForm : ContentView
{

    public string Budget => BudgetInput.DefaultText ?? string.Empty;
    public string Area => AreaInput.DefaultText ?? string.Empty;
    public HouseType Type;
 



    public RegisterHomePreferencesForm()
	{
		InitializeComponent();
	}

    private void Button_Clicked(object sender, EventArgs e)
    {

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
}