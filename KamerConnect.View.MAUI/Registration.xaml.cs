namespace KamerConnect.View.MAUI;

public partial class Registration : ContentPage
{
	public Registration()
	{
		InitializeComponent();
	}
	private async void Button_Clicked(object sender, EventArgs e){

        
    }
	private async void Terug(object sender, EventArgs e){
        await Shell.Current.GoToAsync("MainPage");

        
    }
}