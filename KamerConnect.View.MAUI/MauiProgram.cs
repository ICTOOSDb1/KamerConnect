using KamerConnect.DataAccess.GeoLocation.Repositories;
using Microsoft.Extensions.Logging;
using KamerConnect.EnvironmentVariables;
using KamerConnect.DataAccess.Minio;
using KamerConnect.DataAccess.Postgres.Repositories;
using KamerConnect.View.MAUI.Views;
using KamerConnect.Services;
using KamerConnect.View.MAUI.Pages;

namespace KamerConnect.View.MAUI;

public static class MauiProgram
{
	public static IServiceProvider Services { get; private set; }
	
	public static MauiApp CreateMauiApp()
	{
		EnvVariables.Load();

		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("Inter-Black.ttf", "InterBlack");
				fonts.AddFont("Inter-Bold.ttf", "InterBold");
				fonts.AddFont("Inter-ExtraBold.ttf", "InterExtraBold");
				fonts.AddFont("Inter-ExtraLight.ttf", "InterExtraLight");
				fonts.AddFont("Inter-Italic.ttf", "InterItalic");
				fonts.AddFont("Inter-Light.ttf", "InterLight");
				fonts.AddFont("Inter-Medium.ttf", "InterMedium");
				fonts.AddFont("Inter-Regular.ttf", "InterRegular");
				fonts.AddFont("Inter-SemiBold.ttf", "InterSemiBold");
				fonts.AddFont("Inter-Thin.ttf", "InterThin");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		builder.Services.AddSingleton<PersonService>(sp => new PersonService(new PersonRepository()));
		builder.Services.AddSingleton<AuthenticationService>(sp => new AuthenticationService(sp.GetRequiredService<PersonService>(), new AuthenticationRepository()));
		builder.Services.AddSingleton<FileService>(sp => new FileService(new FileRepository()));
		builder.Services.AddSingleton<HouseService>(sp => new HouseService(new HouseRepository()));
		builder.Services.AddSingleton<HousePreferenceService>(sp => new HousePreferenceService(new HousePreferenceRepository()));
		builder.Services.AddSingleton<GeoLocationService>(sp => new GeoLocationService(new GeoLocationRepository()));

		builder.Services.AddTransient<LoginPage>();
		builder.Services.AddTransient<MainPage>();
		builder.Services.AddTransient<UpdateAccount>();
		builder.Services.AddTransient<RegisterHomePreferencesPage>();
		builder.Services.AddTransient<UpdateAccountsForm>();
		builder.Services.AddTransient<Navbar>();
		builder.Services.AddTransient<RegisterHomePreferencesForm>();
		builder.Services.AddTransient<InterestsForm>();
		builder.Services.AddTransient<Registration>();
		builder.Services.AddTransient<HousePage>();

		builder.Services.AddFilePicker();
		
		var app = builder.Build();
		
		Services = app.Services;

		return app;
	}
}
