
using KamerConnect.Models;

namespace KamerConnect.View.MAUI;

public partial class HomePreferencesForm : ContentView
{

    private string Budget => BudgetInput.DefaultText ?? string.Empty;
    private string Area => AreaInput.DefaultText ?? string.Empty;
    private string HouseType;
    private string BedroomAmount;



    public HomePreferencesForm()
	{
		InitializeComponent();
	}

    private void Button_Clicked(object sender, EventArgs e)
    {

    }
	private async void BedroomAmountChanged(object sender, EventArgs e)
	{
        if (BedroomAmountPicker.SelectedIndex != 0)
        {
            BedroomAmount = $"{BedroomAmountPicker.SelectedItem}";
        }
        else
        {
            BedroomAmount = null;
        }
    }
	private async void HouseTypeChanged(object sender, EventArgs e)
	{
        if ($"{HousetypePicker.SelectedItem}" == "Studio")
        {
            BedroomAmountPicker.IsVisible = false;
            StudioRoomAmount.IsVisible = true;
            BedroomAmount = "1";
        }
        else
        {
            BedroomAmountPicker.IsVisible = true;
            StudioRoomAmount.IsVisible = false;
        }

        //int currentIndex = .SelectedIndex;
        //if(currentIndex != -1)
        //{

        //	if(currentIndex == 1)
        //	{

        //	}

        //}

    }
}