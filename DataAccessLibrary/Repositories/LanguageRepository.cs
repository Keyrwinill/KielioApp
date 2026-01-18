/*
	Update date			Description 
	--------------------------------
	20251130			Initial
	20260109			Add set methods	
*/
using DataAccessLibrary.Entities;
using DataAccessLibrary.Interfaces;

namespace DataAccessLibrary.Repositories;

public class LanguageRepository : ILanguageRepository
{
	private readonly KieliodbContext _context;
	public LanguageRepository(KieliodbContext context)
	{
		_context = context;
	}

	public Language GetByOid(Guid oid)
	{
		return _context.Languages.FirstOrDefault(lang => lang.Oid == oid);
	}
	public IEnumerable<Language> GetAll()
	{
		return _context.Languages;
	}

	//+>>20260109
	public async Task AddAsync(Language language)
	{
		await _context.Languages.AddAsync(language);
		await _context.SaveChangesAsync();
	}
	public async Task AddRangeAsync(IEnumerable<Language> languages)
	{
		await _context.Languages.AddRangeAsync(languages);
		await _context.SaveChangesAsync();
	}
	//+<<20260109
}
