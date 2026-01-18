/*
	Update date			Description 
	-------------------------------------------------------------------
	20260106			Initial creation for methods work on each table
*/
using DataAccessLibrary.Interfaces;
using DataAccessLibrary.Repositories;

namespace DataAccessLibrary;

public interface IUnitofWork
{
	IDeutschAdjektivRepository DeutschAdjektivs { get; }
	ILanguageRepository Languages { get; }
	IVerbRepository Verbs { get; }
	ITenseRepository Tenses { get; }
	IMoodRepository Moods { get; }
	IVerbPersonRepository VerbPersons { get; }
	IConjugationRepository Conjugations { get; }
	IMidMoodTenseRepository MidMoodTenses { get; }
	IMidMoodPersonRepository MidMoodPersons { get; }

	void Save();
	Task SaveAsync();
}

public class UnitOfWork : IUnitofWork
{
	private readonly KieliodbContext _context;

	public IDeutschAdjektivRepository DeutschAdjektivs { get; }
	public ILanguageRepository Languages { get; }
	public IVerbRepository Verbs { get; }
	public ITenseRepository Tenses { get; }
	public IMoodRepository Moods { get; }
	public IVerbPersonRepository VerbPersons { get; }
	public IConjugationRepository Conjugations { get; }
	public IMidMoodTenseRepository MidMoodTenses { get; }
	public IMidMoodPersonRepository MidMoodPersons { get; }

	public UnitOfWork(KieliodbContext context)
	{
		_context = context;

		DeutschAdjektivs = new DeutschAdjektivRepository(context);
		Languages = new LanguageRepository(context);
		Verbs = new VerbRepository(context);
		Tenses = new TenseRepository(context);
		Moods = new MoodRepository(context);
		VerbPersons = new VerbPersonRepository(context);
		Conjugations = new ConjugationRepository(context);
		MidMoodTenses = new MidMoodTenseRepository(context);
		MidMoodPersons = new MidMoodPersonRepository(context);
	}

	public void Save()
	{
		_context.SaveChanges();
	}
	async public Task SaveAsync()
	{
		await _context.SaveChangesAsync();
	}
}
