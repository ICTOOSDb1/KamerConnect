using KamerConnect.DataAccess.Postgres.Repositys;
using KamerConnect.Repositories;
using KamerConnect.Services;
using KamerConnect.View.MAUI.Pages;

namespace KamerConnect.View.MAUI;

public partial class App : Application
{
	private IServiceProvider serviceProvider;
	public App(IServiceProvider serviceProvider)
	{
		this.serviceProvider = serviceProvider;
		InitializeComponent();
		_ = InitializeAppAsync(); // Fire-and-forget the async initialization
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
