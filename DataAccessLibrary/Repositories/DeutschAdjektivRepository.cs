/*
	Update date			Description 
	--------------------------------
	20251125			Initial
*/
using DataAccessLibrary.Entities;
using DataAccessLibrary.Interfaces;

namespace DataAccessLibrary.Repositories;

public class DeutschAdjektivRepository : IDeutschAdjektivRepository
{
	private readonly KieliodbContext _context;
	public DeutschAdjektivRepository(KieliodbContext context)
	{
		_context = context;
	}

	public DeutschAdjektiv GetByOid(Guid oid)
	{
		return _context.DeutschAdjektivs.FirstOrDefault(adj => adj.Oid == oid);
	}

	public IEnumerable<DeutschAdjektiv> GetAll()
	{
		return _context.DeutschAdjektivs;
	}

	public async Task AddAsync(DeutschAdjektiv adj)
	{
		await _context.DeutschAdjektivs.AddAsync(adj);
	}
	public async Task AddRangeAsync(IEnumerable<DeutschAdjektiv> adjs)
	{
		await _context.DeutschAdjektivs.AddRangeAsync(adjs);
	}
}
