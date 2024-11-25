using KamerConnect.DataAccess.Postgres.Repositys;
using KamerConnect.Repositories;
using KamerConnect.Services;

namespace KamerConnect.View.MAUI;

public partial class App : Application
{
	private IServiceProvider serviceProvider;
	public App(IServiceProvider serviceProvider)
	{
		InitializeComponent();
		_ = InitializeAppAsync(); // Fire-and-forget the async initialization
	}
	
	private async Task InitializeAppAsync()
	{
		if (serviceProvider.GetService<AuthenticationService>() == null)
		{
			return;
		}
		
		if (await serviceProvider.GetService<AuthenticationService>()?.CheckSession())
		{
			MainPage = new MainPage();
		}
		else
		{
			MainPage = serviceProvider.GetRequiredService<LoginPage>();
		}
	}
}
