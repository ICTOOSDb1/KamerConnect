namespace KamerConnect.View.MAUI;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
    
		Routing.RegisterRoute("UpdateAccount", typeof(UpdateAccount));
		Routing.RegisterRoute("Registration", typeof(Registration));
		Routing.RegisterRoute("MainPage", typeof(MainPage));
	}
}
