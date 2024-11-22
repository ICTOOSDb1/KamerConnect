using KamerConnect.DataAccess.Postgres.Repositys;
using KamerConnect.Repositories;
using KamerConnect.Services;

namespace KamerConnect.View.MAUI;

public partial class App : Application
{
	private IPersonRepository personDataAcces;
	private IAuthenticationRepository authenDataAcces;
	private PersonService personSerivce;
	private AuthenticationService authService;
	public App()
	{
		personDataAcces = new PersonRepository();
		authenDataAcces = new AuthenticationRepository();
		personSerivce = new PersonService(personDataAcces);
		authService = new AuthenticationService(personSerivce, authenDataAcces);
		
		InitializeComponent();
		_ = InitializeAppAsync(); // Fire-and-forget the async initialization
	}
	
	private async Task InitializeAppAsync()
	{
		if (await authService.CheckSession())
		{
			MainPage = new MainPage();
		}
		else
		{
			MainPage = new LoginPage();
		}
	}
}
