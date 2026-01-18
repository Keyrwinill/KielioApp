/*
	Update date			Description 
	--------------------------------
	20251125			Initial
*/
using DataAccessLibrary.Entities;
using DataAccessLibrary.Interfaces;
using WebService.Interfaces;

namespace WebService;

public class DeutschAdjektivService : IDeutschAdjektivService
{
	private readonly IDeutschAdjektivRepository _detschadj;

	public DeutschAdjektivService(IDeutschAdjektivRepository detschadj)
	{
		_detschadj = detschadj;
	}

	//public DeutschAdjektiv GetOne(int oid)      //for test
	public DeutschAdjektiv GetOne(Guid oid)
	{
		return _detschadj.GetByOid(oid);
	}

	public IEnumerable<DeutschAdjektiv> GetAll()
	{
		return _detschadj.GetAll();
	}
	public async Task Add(DeutschAdjektiv adj)
	{
		_detschadj.AddAsync(adj);
	}
	public async Task AddRangeAsync(IEnumerable<DeutschAdjektiv> adjs)
	{
		await _detschadj.AddRangeAsync(adjs);
	}
}
