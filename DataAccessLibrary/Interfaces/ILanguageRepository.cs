/*
	Update date			Description 
	--------------------------------
	20251130			Initial
	20260109			Add set methods
*/
using DataAccessLibrary.Entities;

namespace DataAccessLibrary.Interfaces;

public interface ILanguageRepository
{
	Language GetByOid(Guid id);
	IEnumerable<Language> GetAll();

	//+>>20260109
	Task AddAsync(Language language);
	Task AddRangeAsync(IEnumerable<Language> languages);
	//+<<20260109
}
