/*
	Update date			Description 
	--------------------------------------------------------------------------
    20251225			Add DeutschAdjektivService VerbService, Db connection
*/
using DataAccessLibrary;
using DataAccessLibrary.Interfaces;
using DataAccessLibrary.Repositories;
using Microsoft.EntityFrameworkCore;
using WebService;
using WebService.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//+>>20251225
// Add DeutschAdjektivService
builder.Services.AddScoped<IDeutschAdjektivRepository, DeutschAdjektivRepository>();
builder.Services.AddScoped<IDeutschAdjektivService, DeutschAdjektivService>();

// Add VerbService
builder.Services.AddScoped<ILanguageRepository, LanguageRepository>();
builder.Services.AddScoped<IVerbRepository, VerbRepository>();
builder.Services.AddScoped<ITenseRepository, TenseRepository>();
builder.Services.AddScoped<IVerbPersonRepository, VerbPersonRepository>();
builder.Services.AddScoped<IConjugationRepository, ConjugationRepository>();
builder.Services.AddScoped<IMoodRepository, MoodRepository>();

builder.Services.AddScoped<IVerbService, VerbService>();

// Add DB connection
builder.Services.AddDbContext<KieliodbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
	);
//+<<20251225

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
