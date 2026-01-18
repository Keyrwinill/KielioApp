/*
	Update date			Description 
	--------------------------------
	20251125			Initial
*/
using DataAccessLibrary.Entities;

namespace WebService.Interfaces;
public interface IDeutschAdjektivService
{
	//DeutschAdjektiv GetOne(int oid);      //for test
	DeutschAdjektiv GetOne(Guid oid);
	IEnumerable<DeutschAdjektiv> GetAll();
	Task Add(DeutschAdjektiv adj);
	Task AddRangeAsync(IEnumerable<DeutschAdjektiv> adjs);
}
