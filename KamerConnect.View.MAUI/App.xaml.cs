using KamerConnect.DataAccess.Postgres.Repositys;
using KamerConnect.Repositories;
using KamerConnect.Services;

namespace KamerConnect.View.MAUI;

public partial class App : Application
{
	private IServiceProvider _serviceProvider;

	public App(IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider;
		InitializeComponent();
		InitializeAppAsync().GetAwaiter().GetResult();
	}

	private async Task InitializeAppAsync()
	{
		var authenticationService = _serviceProvider.GetService<AuthenticationService>();

		if (authenticationService == null)
		{
			MainPage = new MainPage();
			return;
		}

		if (await _serviceProvider.GetService<AuthenticationService>()?.CheckSession())
		{
			MainPage = new MainPage();
		}
		else
		{
			MainPage = _serviceProvider.GetRequiredService<LoginPage>();
		}
	}
}
