using Microsoft.Extensions.Logging;
using KamerConnect.EnvironmentVariables;

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
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
