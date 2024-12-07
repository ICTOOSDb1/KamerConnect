using KamerConnect.View.MAUI.Pages;

namespace KamerConnect.View.MAUI.Views;

public partial class Navbar : ContentView
{
	public static IServiceProvider _serviceProvider;

	public Navbar(IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider;
		InitializeComponent();
	}

	private void OnChatsTapped(object sender, TappedEventArgs e)
	{
	}

	private async void OnExploreTapped(object sender, TappedEventArgs e)
	{
		if (Application.Current.MainPage is NavigationPage navigationPage)
		{
			await navigationPage.Navigation.PushAsync(_serviceProvider.GetRequiredService<MainPage>());
		}
	}

	private async void OnProfileTapped(object sender, TappedEventArgs e)
	{
		if (Application.Current.MainPage is NavigationPage navigationPage)
		{
			await navigationPage.Navigation.PushAsync(_serviceProvider.GetRequiredService<UpdateAccount>());
		}
	}
}
