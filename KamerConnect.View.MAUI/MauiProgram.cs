using Microsoft.Extensions.Logging;

namespace KamerConnect.View.MAUI;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
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

		return builder.Build();
	}
}
