namespace KamerConnect.View.MAUI;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new NavigationPage(new MainPage()); // Wrap LoginPage in a NavigationPage

	}
}
