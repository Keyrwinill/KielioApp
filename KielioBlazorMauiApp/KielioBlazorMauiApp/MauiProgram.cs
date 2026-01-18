using KielioBlazorMauiApp.Services;
using KielioBlazorMauiApp.Shared.Services;
using Microsoft.Extensions.Logging;

namespace KielioBlazorMauiApp
{
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
				});

			// Add device-specific services used by the KielioBlazorMauiApp.Shared project
			builder.Services.AddSingleton<IFormFactor, FormFactor>();

			builder.Services.AddMauiBlazorWebView();

#if DEBUG
	        builder.Services.AddBlazorWebViewDeveloperTools();
	        builder.Logging.AddDebug();
#endif

			return builder.Build();
		}
	}
}
