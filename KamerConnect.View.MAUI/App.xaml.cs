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
		var authService = serviceProvider.GetService<AuthenticationService>();
		if (authService == null)
		{
			return;
		}
    
		if (await authService.CheckSession())
		{
			MainPage = new NavigationPage(new MainPage());
		}
		else
		{
			MainPage = new NavigationPage(serviceProvider.GetRequiredService<LoginPage>());
		}
	}

}
