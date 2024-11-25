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
		MainPage = _serviceProvider.GetService<UpdateAccount>();
		// var authenticationService = _serviceProvider.GetService<AuthenticationService>();

		// if (authenticationService == null)
		// {
		// 	MainPage = new MainPage();
		// 	return;
		// }

		// if (await _serviceProvider.GetService<AuthenticationService>()?.CheckSession())
		// {
		// 	MainPage = new MainPage();
		// }
		// else
		// {
		// 	MainPage = _serviceProvider.GetRequiredService<LoginPage>();
		// }
	}
}
