
using KamerConnect.EnvironmentVariables;
using KamerConnect.Models;
using KamerConnect.Services;
using KamerConnect.View.MAUI.Pages;

namespace KamerConnect.View.MAUI;

public partial class App : Application
{
    private IServiceProvider _serviceProvider;

    public App(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NMaF5cXmBCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdnWH1fd3VQQmhYUEx0V0E=");

        InitializeComponent();
        InitializeAppAsync().GetAwaiter().GetResult();
    }

    private async Task InitializeAppAsync()
    {
        var authService = _serviceProvider.GetService<AuthenticationService>();
        var personService = _serviceProvider.GetRequiredService<PersonService>();
        if (authService == null && personService == null)
        {
            return;
        }

        var session = await authService.GetSession();
        if (session != null)
        {
            if (personService.GetPersonById(session.personId)?.Role == Role.Seeking)
            {
                MainPage = new NavigationPage(_serviceProvider.GetRequiredService<MainPage>());
            }
            else MainPage = new NavigationPage(_serviceProvider.GetRequiredService<UpdateAccount>());
        }
        else
        {
            MainPage = new NavigationPage(_serviceProvider.GetRequiredService<LoginPage>());
        }


    }
}
