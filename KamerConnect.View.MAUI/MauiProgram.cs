using KamerConnect.DataAccess.Postgres.Repositys;
using Microsoft.Extensions.Logging;
using KamerConnect.EnvironmentVariables;
using KamerConnect.Repositories;
using KamerConnect.Services;
using KamerConnect.View.MAUI.Pages;

namespace KamerConnect.View.MAUI;

public static class MauiProgram
{
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
			})
			.ConfigureEssentials();

#if DEBUG
		builder.Logging.AddDebug();
#endif
		builder.Services.AddSingleton<PersonService>(sp => new PersonService(new PersonRepository()));
		builder.Services.AddSingleton<AuthenticationService>(sp => new AuthenticationService(sp.GetRequiredService<PersonService>(), new AuthenticationRepository()));
		builder.Services.AddTransient<LoginPage>();
		builder.Services.AddTransient<MainPage>();

		return builder.Build();
	}
}
