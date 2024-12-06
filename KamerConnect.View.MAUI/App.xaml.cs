
using KamerConnect.EnvironmentVariables;
using KamerConnect.View.MAUI.Pages;

namespace KamerConnect.View.MAUI;

public partial class App : Application
{
	private IServiceProvider _serviceProvider;

	public App(IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider;
		InitializeComponent();
		InitializeAppAsync().GetAwaiter().GetResult();
		MainPage = new MatchRequestsPage();
	}

	private async Task InitializeAppAsync()
	{
		var authService = _serviceProvider.GetService<AuthenticationService>();
		if (authService == null)
		{
			return;
		}

		if (await authService.CheckSession())
		{
			MainPage = new NavigationPage(_serviceProvider.GetRequiredService<MainPage>());
		}
		else
		{
			MainPage = new NavigationPage(_serviceProvider.GetRequiredService<LoginPage>());
		}
	}
}
