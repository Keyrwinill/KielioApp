/*
	Update date			Description 
	------------------------------------------------
	20251125			Add DeutschAdjektivService
    20251223			Add VerbService
    20260112            Add MoodSettingService
*/
using DataAccessLibrary;
using DataAccessLibrary.Interfaces;
using DataAccessLibrary.Repositories;
using Microsoft.EntityFrameworkCore;
using WebService;
using WebService.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//+>>20251125
// Add DeutschAdjektivService
builder.Services.AddScoped<IDeutschAdjektivRepository, DeutschAdjektivRepository>();
builder.Services.AddScoped<IDeutschAdjektivService, DeutschAdjektivService>();
//+<<20251125
//+>>20251223
// Add VerbService
builder.Services.AddScoped<ILanguageRepository, LanguageRepository>();
builder.Services.AddScoped<IVerbRepository, VerbRepository>();
builder.Services.AddScoped<ITenseRepository, TenseRepository>();
builder.Services.AddScoped<IVerbPersonRepository, VerbPersonRepository>();
builder.Services.AddScoped<IConjugationRepository, ConjugationRepository>();
builder.Services.AddScoped<IMoodRepository, MoodRepository>();

builder.Services.AddScoped<IConjugationService, ConjugationService>();
//+<<20251223
//+>>20260112
builder.Services.AddScoped<IUnitofWork, UnitOfWork>();
builder.Services.AddScoped<IMidMoodTenseRepository, MidMoodTenseRepository>();
builder.Services.AddScoped<IMidMoodPersonRepository, MidMoodPersonRepository>();

builder.Services.AddScoped<IMoodSettingService, MoodSettingService>();
//+<<20260112
//+>>20251225
// Add DB connection
builder.Services.AddDbContext<KieliodbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    );
//+<<20251225

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
