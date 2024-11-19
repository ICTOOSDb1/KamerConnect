namespace KamerConnect.View.MAUI;

public partial class Registration : ContentPage
{
	public Registration()
	{
		InitializeComponent();
	}
	
	private async void Terug(object sender, EventArgs e){
        await Shell.Current.GoToAsync("MainPage");

        
    }
	private void OnCheckedChanged(object sender, CheckedChangedEventArgs e)
{
    if (sender is RadioButton radioButton && e.Value)
    {
        
    }
}

}