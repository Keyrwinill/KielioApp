/*
	Update date			Description 
	--------------------------------
	20251125			Initial
*/
using DataAccessLibrary.Entities;
using ViewModel.Models.ResponseModels;

namespace WebService.Interfaces;
public interface IDeutschAdjektivService
{
	List<DeutschAdjResponseViewModel> GetViewByType(string articleType);
}
