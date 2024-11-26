
namespace KamerConnect.View.MAUI.Pages;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("UpdateAccount");
    }

    private async void ToRegistration(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("Registration");
    }
}

