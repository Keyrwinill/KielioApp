/*
	Update date			Description 
	--------------------------------
	20251125			Initial
*/
using DataAccessLibrary.Entities;

namespace DataAccessLibrary.Interfaces;

public interface IDeutschAdjektivRepository
{
	//DeutschAdjektiv GetByOid(int oid);		//for test
	DeutschAdjektiv GetByOid(Guid oid);
	IEnumerable<DeutschAdjektiv> GetAll();
	Task AddAsync(DeutschAdjektiv adj);
	Task AddRangeAsync(IEnumerable<DeutschAdjektiv> adjs);
}
