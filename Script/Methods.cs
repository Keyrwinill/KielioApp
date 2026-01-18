using DataAccessLibrary;
using DataAccessLibrary.Interfaces;
using DataAccessLibrary.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Script;

public static class Methods
{
	public static IHost ConnectDB()
	{
		IHost host = Host.CreateDefaultBuilder()				
						 .ConfigureAppConfiguration((context, config) =>
						 {
							 config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
						 })
						 .ConfigureServices((context, services) =>
						 {
							 services.AddDbContext<KieliodbContext>(options =>
								 options.UseSqlServer(
									 context.Configuration.GetConnectionString("DefaultConnection")
									 ));

							 services.AddScoped<IDeutschAdjektivRepository, DeutschAdjektivRepository>();
							 services.AddScoped<ILanguageRepository, LanguageRepository>();
							 services.AddScoped<IVerbRepository, VerbRepository>();
							 services.AddScoped<ITenseRepository, TenseRepository>();
							 services.AddScoped<IVerbPersonRepository, VerbPersonRepository>();
							 services.AddScoped<IConjugationRepository, ConjugationRepository>();
							 services.AddScoped<IMoodRepository, MoodRepository>();
							 services.AddScoped<IMidMoodTenseRepository, MidMoodTenseRepository>();
							 services.AddScoped<IMidMoodPersonRepository, MidMoodPersonRepository>();
						 })
						 .Build();
		return host;
	}
	public static string GetRelativePath(IHost host)
	{
		IConfiguration config = host.Services.GetRequiredService<IConfiguration>();
		string basePath = config.GetValue<string>("FileRouteSettings:BasePath") ?? throw new Exception("BasePath is not configured");
		return basePath;
	}
}
