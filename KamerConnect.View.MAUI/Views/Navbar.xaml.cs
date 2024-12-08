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

	private async void OnChatsTapped(object sender, TappedEventArgs e)
	{
		if (Application.Current.MainPage is NavigationPage navigationPage)
		{
			App.Current.MainPage = new NavigationPage(_serviceProvider.GetRequiredService<MatchRequestsPage>());
		}
	}

	private async void OnExploreTapped(object sender, TappedEventArgs e)
	{
		if (Application.Current.MainPage is NavigationPage navigationPage)
		{
			App.Current.MainPage = new NavigationPage(_serviceProvider.GetRequiredService<MainPage>());
		}
	}

	private async void OnProfileTapped(object sender, TappedEventArgs e)
	{
		if (Application.Current.MainPage is NavigationPage navigationPage)
		{
			App.Current.MainPage = new NavigationPage(_serviceProvider.GetRequiredService<UpdateAccount>());
		}
	}
}
