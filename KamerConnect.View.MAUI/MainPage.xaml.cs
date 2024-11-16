using KamerConnect.View.MAUI.reusableItems;
using KamerConnect.View.MAUI.ViewModels;

namespace KamerConnect.View.MAUI;

public partial class MainPage : ContentPage
{
	

	public MainPage()
	{
		InitializeComponent();
		BindingContext = new LoginViewModel();
		
	}
	
}

