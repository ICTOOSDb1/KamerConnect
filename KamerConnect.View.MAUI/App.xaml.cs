namespace KamerConnect.View.MAUI;

public partial class App : Application
{
	public App(IServiceProvider serviceProvider)
	{
		InitializeComponent();

		MainPage = new NavigationPage(new LoginPage());
	}
}
