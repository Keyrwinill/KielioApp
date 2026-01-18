/*
	Update date			Description 
	--------------------------------
	20251222			Initial
*/
using DataAccessLibrary.Entities;
using DataAccessLibrary.Interfaces;
using ViewModel.Models.ResponseModels;
using WebService.Interfaces;

namespace WebService;

public class VerbService : IVerbService
{
	private readonly ILanguageRepository _language;
	private readonly IVerbRepository _verb;
	private readonly ITenseRepository _tense;
	private readonly IVerbPersonRepository _verbPerson;
	private readonly IConjugationRepository _conjugation;

	public VerbService(ILanguageRepository language, 
					   IVerbRepository verb, 
					   ITenseRepository tense, 
					   IVerbPersonRepository verbPerson, 
					   IConjugationRepository conjugation)
	{
		_language = language;
		_verb = verb;
		_tense = tense;
		_verbPerson = verbPerson;
		_conjugation = conjugation;
	}

	public VerbResponseModel GetVerbView(string languageName, string infinitiveVerb)
	{
		var language = _language.GetAll().FirstOrDefault(l => l.Name == languageName);
		if (language != null)
		{
			var verb = _verb.GetAll().FirstOrDefault(v => v.Infinitive == infinitiveVerb && v.LinkLanguage == language.Oid);
			var tenses = _tense.GetAll().Where(t => t.LinkLanguage == language.Oid).ToList();
			var verbPersons = _verbPerson.GetAll().Where(vp => vp.LinkLanguage == language.Oid).ToList();

			if (verb != null)
			{
				var verbView = new VerbResponseModel
				{
					LanguageName = language.Name,
					Infinitive = verb.Infinitive,
					Reflexive = verb.Reflexive,
					Translation = verb.Translation,
					Conjugations = GetConjugationList(language, verb)
				};
				return verbView;
			}
			else
			{
				return null;
			}
		}
		else
		{
			return null;
		}
	}

	public List<ConjugationViewModel> GetConjugationList(Language language, Verb verb)
	{
		var conjugations = new List<ConjugationViewModel>();
		var tenses = _tense.GetAll().Where(t => t.LinkLanguage == language.Oid).ToList();

		foreach (var tense in tenses)
		{
			conjugations.Add(new ConjugationViewModel
			{
				Tense = tense.Name,
				PersonForms = GetPersonFormList(language, verb, tense)
			});
		}
		return conjugations;
	}

	public List<PersonFormViewModel> GetPersonFormList(Language language, Verb verb, Tense tense)
	{
		var personForms = new List<PersonFormViewModel>();
		string[] personOrder = ["First Person Singular", "Second Person Singular", "Third Person Singular", "First Person Plural", "Second Person Plural", "Third Person Plural"];
		var verbPersons = new List<VerbPerson>();
		foreach (var person in personOrder)
		{
			var verbPersonSort = _verbPerson.GetAll().FirstOrDefault(vp => vp.LinkLanguage == language.Oid && vp.Type == person);
			if (verbPersonSort != null)
			{
				verbPersons.Add(verbPersonSort);
			}
		}
			
		foreach (var verbPerson in verbPersons)
		{
			// TBD
			var midMoodTense = tense.MidMoodTenses;
			var conjuagations = _conjugation.GetAll().Where(c => c.LinkVerb == verb.Oid && c.LinkMidMoodTense == tense.Oid && c.LinkMidMoodPerson == verbPerson.Oid);
			foreach (var conj in conjuagations)
			{
				personForms.Add(new PersonFormViewModel
				{
					Pronoun = verbPerson.Pronoun,
					Form = conj.Form
				});
			}
		}
		return personForms;
	}
}
