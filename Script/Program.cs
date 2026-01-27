using DataAccessLibrary;
using DataAccessLibrary.Dictionaries;
using DataAccessLibrary.Entities;
using DataAccessLibrary.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Script;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

static class Program
{
	static async Task Main(string[] args)
	{
		//>>Always keep this
		var host = Methods.ConnectDB();
		using var scope = host.Services.CreateScope();
		//for calling general method. E.g. save data 
		using var context = scope.ServiceProvider.GetRequiredService<KieliodbContext>();
		var uow = new UnitOfWork(context);
		//<<Always keep this

		var languageRepo = scope.ServiceProvider.GetRequiredService<ILanguageRepository>();
		var tenseRepo = scope.ServiceProvider.GetRequiredService<ITenseRepository>();
		var moodRepo = scope.ServiceProvider.GetRequiredService<IMoodRepository>();
		var verbPersonRepo = scope.ServiceProvider.GetRequiredService<IVerbPersonRepository>();
		var midMoodTenseRepo = scope.ServiceProvider.GetRequiredService<IMidMoodTenseRepository>();
		var midMoodPersonRepo = scope.ServiceProvider.GetRequiredService<IMidMoodPersonRepository>();
		var verbRepo = scope.ServiceProvider.GetRequiredService<IVerbRepository>();
		var conjugationRepo = scope.ServiceProvider.GetRequiredService<IConjugationRepository>();

		var language = languageRepo.GetAll().FirstOrDefault(lang => lang.Name == "German");
		//var tenses = tenseRepo.GetAll().Where(t => t.Language == language).ToList();
		//var verbPersons = verbPersonRepo.GetAll().Where(vb => vb.Language == language && vb.Type != "2f").ToList();
		
		//var mood = moodRepo.GetAll().FirstOrDefault(m => m.Type == "Indikativ" && m.Language == language);
		
		var conjugations = conjugationRepo.GetAll()
										  .ToList();

		var midMoodTenses = conjugations.Select(conj => conj.MidMoodTense).ToList();
		var midMoodPersons = conjugations.Select(conj => conj.MidMoodPerson).ToList();

		foreach (var midMoodTense in midMoodTenses)
		{
			Console.WriteLine($"MoodType: ${midMoodTense.MoodType}");
		}


		// for saving DB
		//uow.Save();

		/*
		Mood mood = new Mood
		{
			Type = "Indikativ", LinkLanguage = language.Oid, Language = language
		};
		*/

		/*
		//import data from file
		var basePath = Methods.GetRelativePath(host);
		var filePath = Path.Combine(basePath, "GermanAdjective.txt");
		var lines = File.ReadAllLines(filePath);
		var adjs = lines.Skip(1)
						.Where(l => !string.IsNullOrWhiteSpace(l))
						.Select(line =>
						{
							var parts = line.Split(',');
							return new DeutschAdjektiv
							{
								Oid = Guid.NewGuid(),
								Type = parts[0].Trim(),
								Gender = parts[1].Trim(),
								Case = parts[2].Trim(),
								ArticleEnding = parts[3].Trim(),
								AdjectiveEnding = parts[4].Trim()
							};
						}).ToList();

		//output each data to check if imported correctly
		foreach (var adj in adjs)
			Console.WriteLine($"Type: {adj.Type}, Gender: {adj.Gender}, Case: {adj.Case}, Article Ending: {adj.ArticleEnding}, Adjective Ending: {adj.AdjectiveEnding}");

		//import data to database
		await adjRepo.AddRangeAsync(adjs);
		*/

		/*
		// create data without importing from file
		await adjRepo.AddAsync(new DeutschAdjektiv
		{
			Oid = Guid.NewGuid(),
			Type = type,
			Gender = gender,
			Case = germanCase,
			ArticleEnding = articleEnding,
			AdjectiveEnding = adjectiveEnding
		});
		*/

		/*
		//get data
		var adjs = adjRepo.GetAll().Where(a => a.Type == "Definite");

		foreach (var adj in adjs)
		{
			Console.WriteLine($"Type: {adj.Type}, Gender: {adj.Gender}, Case: {adj.Case}, Article Ending: {adj.ArticleEnding}, Adjective Ending: {adj.AdjectiveEnding}");
		}
		*/
	}
}
