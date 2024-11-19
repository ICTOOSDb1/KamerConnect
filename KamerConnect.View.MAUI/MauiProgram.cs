using Microsoft.Extensions.Logging;
using KamerConnect.EnvironmentVariables;
using KamerConnect.Repositories;
using KamerConnect.DataAccess.Minio;

namespace KamerConnect.View.MAUI;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		EnvVariables.Load();
		IFileRepository fileRepository = new FileRepository();

		Task.Run(async () =>
		{

			string bucketName = "mijn-bucket";
			string objectName = "dummy-bestand.txt";
			byte[] fileBytes = System.Text.Encoding.UTF8.GetBytes("Dit is een dummy bestand dat geüpload wordt naar MinIO.");
			string contentType = "text/plain";

			await fileRepository.UploadFileAsync(bucketName, objectName, fileBytes, contentType);
		});

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
