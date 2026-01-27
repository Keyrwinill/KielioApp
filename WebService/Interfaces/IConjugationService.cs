/*
	Update date			Description 
	--------------------------------
	20260126			Initial
*/

using ViewModel.Models.NeutralModel;

namespace WebService.Interfaces;

public interface IConjugationService
{
	ConjugationModel GetConjugation(string infinitive);
	Task SaveVerb(ConjugationModel request);
}
