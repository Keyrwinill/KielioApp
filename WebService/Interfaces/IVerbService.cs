/*
	Update date			Description 
	--------------------------------
	20251222			Initial
*/
using ViewModel.Models.ResponseModels;

namespace WebService.Interfaces;

public interface IVerbService
{
	VerbResponseModel GetVerbView(string languageName, string infinitiveVerb);
}