using KamerConnect.Models;

namespace KamerConnect.View.MAUI;

public partial class HomePreferencesForm : ContentView
{

    public string Budget => BudgetInput.DefaultText ?? string.Empty;
    public string Area => AreaInput.DefaultText ?? string.Empty;
    public HouseType Type;
 



    public HomePreferencesForm()
	{
		InitializeComponent();
	}

    private void Button_Clicked(object sender, EventArgs e)
    {

    }
	
	private async void HouseTypeChanged(object sender, EventArgs e)
	{        
        switch (HousetypePicker.SelectedIndex)
        {
            case 1:
                Type = HouseType.House; 
                break;
            case 2:
                Type = HouseType.Apartment; 
                break;
            case 3:
                Type = HouseType.Studio; 
                break;
            }
    }
}