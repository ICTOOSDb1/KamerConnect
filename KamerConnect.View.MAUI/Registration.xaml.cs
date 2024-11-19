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
		await Shell.Current.Navigation.PopAsync();
	}
	private void OnCheckedChanged(object sender, CheckedChangedEventArgs e)
{
    if (sender is RadioButton radioButton && e.Value)
    {
        
    }
}

}