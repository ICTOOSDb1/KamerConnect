using KamerConnect.Models;

namespace KamerConnect.View.MAUI.Views;

public partial class HouseCard : ContentView
{
	public HouseCard()
	{
		InitializeComponent();
		BindingContextChanged += OnBindingContextChanged;
	}

	private void OnHouseCardTapped(object sender, EventArgs e)
	{
		// Implement house navigation here
	}

	private void OnBindingContextChanged(object sender, EventArgs e)
	{
		if (BindingContext is House house)
		{
			if (house.HouseImages?.Count > 0)
			{
				ImageControl.Source = house.HouseImages[0].FullPath;
			}
			else
			{
				ImageControl.Source = null;
			}
		}
	}
}