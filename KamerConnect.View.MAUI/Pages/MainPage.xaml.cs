
namespace KamerConnect.View.MAUI.Pages;

public partial class MainPage : ContentPage
{
    private IServiceProvider _serviceProvider;
    public MainPage(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        NavigationPage.SetHasNavigationBar(this, false);
        
        InitializeComponent();
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("UpdateAccount");
    }

    private async void ToAccount(object sender, EventArgs e)
    {
        if (Application.Current.MainPage is NavigationPage navigationPage)
        {
            await navigationPage.Navigation.PushAsync(_serviceProvider.GetRequiredService<UpdateAccount>());
        }
    }
}

