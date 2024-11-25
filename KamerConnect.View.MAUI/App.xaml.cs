using KamerConnect.DataAccess.Postgres.Repositys;
using KamerConnect.EnvironmentVariables;
using KamerConnect.Repositories;
using KamerConnect.Services;

namespace KamerConnect.View.MAUI;

public partial class App : Application
{
	private IServiceProvider serviceProvider;
	public App(IServiceProvider serviceProvider)
	{
		EnvVariables.Load();
		
		this.serviceProvider = serviceProvider;
		
		InitializeComponent();
		InitializeAppAsync().GetAwaiter().GetResult();
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
