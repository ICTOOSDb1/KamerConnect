using KamerConnect.EnvironmentVariables;
using KamerConnect.View.MAUI.Pages;

namespace KamerConnect.View.MAUI;

public partial class App : Application
{
	private IServiceProvider _serviceProvider;

	public App(IServiceProvider serviceProvider)
	{
		EnvVariables.Load();

		_serviceProvider = serviceProvider;
		InitializeComponent();
		InitializeAppAsync().GetAwaiter().GetResult();
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
			MainPage = new NavigationPage(_serviceProvider.GetRequiredService<UpdateAccount>());
		}
		else
		{
			MainPage = new NavigationPage(_serviceProvider.GetRequiredService<LoginPage>());
		}
	}

}
