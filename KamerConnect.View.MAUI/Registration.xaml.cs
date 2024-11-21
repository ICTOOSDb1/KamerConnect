using KamerConnect.View.MAUI.ViewModels;

namespace KamerConnect.View.MAUI;

public partial class Registration : ContentPage
{
	public Registration()
	{
		InitializeComponent();
		BindingContext = new RegisterViewModel();
	}
	
	private async void Terug(object sender, EventArgs e)
	{
		if (Navigation.NavigationStack.Count > 1)
		{
			await Navigation.PopAsync();
		}
		else
		{
		}
	}
	private void OnCheckedChanged(object sender, CheckedChangedEventArgs e)
{
    if (sender is RadioButton radioButton && e.Value)
    {
        
    }
}

}