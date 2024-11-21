using KamerConnect.View.MAUI.ViewModels;
using KamerConnect.Services;
using KamerConnect.Models;
using System.ComponentModel;
namespace KamerConnect.View.MAUI;
public delegate void sendInfo();
public partial class Registration : ContentPage
{
    
    public Registration()
	{
		InitializeComponent();
		BindingContext = new RegisterViewModel();
	}
	
	private async void Back(object sender, EventArgs e)
	{
		if (Navigation.NavigationStack.Count > 1)
		{
			await Navigation.PopAsync();
		}
		else
		{
		}
	}

	private async void CreatePerson(object sender, EventArgs e)
	{

		PersonalInformationForm personform = new PersonalInformationForm();
		
    }
	

}