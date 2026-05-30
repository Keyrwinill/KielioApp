/*
	Update date			Description 
	--------------------------------
	20260126			Initial
	20240418			Add Error handeling
*/
using DataAccessLibrary;
using DataAccessLibrary.Entities;
using DataAccessLibrary.Interfaces;
using ViewModel.Models.NeutralModels;
using ViewModel.Models.ResponseModels;
using WebService.Interfaces;

namespace WebService;

public class ConjugationService : IConjugationService
{
	private readonly IUnitofWork _unitOfWork;

	private readonly ILanguageRepository _language;
	private readonly ITenseRepository _tense;
	private readonly IVerbPersonRepository _verbPerson;
	private readonly IMoodRepository _mood;
	private readonly IMidMoodTenseRepository _midMoodTense;
	private readonly IMidMoodPersonRepository _midMoodPerson;
	private readonly IVerbRepository _verb;
	private readonly IConjugationRepository _conjugation;


	public ConjugationService(IUnitofWork unitofWork,
							  ILanguageRepository language,
							  ITenseRepository tense,
							  IVerbPersonRepository verbPerson,
							  IMoodRepository mood,
							  IMidMoodTenseRepository midMoodTense,
							  IMidMoodPersonRepository midMoodPerson,
							  IVerbRepository verb,
							  IConjugationRepository conjugation
							 )
	{
		_unitOfWork = unitofWork;
		_language = language;
		_tense = tense;
		_verbPerson = verbPerson;
		_mood = mood;
		_midMoodTense = midMoodTense;
		_midMoodPerson = midMoodPerson;
		_verb = verb;
		_conjugation = conjugation;
	}

	//GET
	public ConjugationModel? GetConjugation(string? languageName, string? verb, string? mood, string? tense)
	{
		var isInitial = IsInitial(verb, mood, tense);
		var language = _language.GetAll().FirstOrDefault(l => l.Name == NormalizeLanguageName(languageName));
		var verbObj = language != null ? FindInfinitiveForm(language, verb) : null;
		var isSuccess = verbObj != null;

		return new ConjugationModel
		{
			Language = languageName,
			MoodList = string.IsNullOrEmpty(languageName) ? new List<string>() : GetMoodList(languageName),
			TenseList = string.IsNullOrEmpty(mood) ? new List<string>() : GetTenseList(mood),
			Infinitive = verbObj?.Infinitive ?? "",
			Translation = "",                                   //keep it for future design
			PastParticiple = "",                                //keep it for future design
			Reflexive = false,                                  //keep it for future design
			Separable = false,                                  //keep it for future design
			Mood = mood,                                        //chosen mood
			Tense = tense,                                      //chosen tense
			Forms = GetFormByTense(verbObj?.Infinitive ?? "", mood, tense)
		};

		bool IsInitial(string? verb, string? mood, string? tense)
		{
			return string.IsNullOrEmpty(verb) && string.IsNullOrEmpty(mood) && string.IsNullOrEmpty(tense);
		}

		Verb? FindInfinitiveForm(Language language, string? verb)
		{
			var infinitiveVerb = (from v in _verb.GetAll()
									where v.LinkLanguage == language.Oid && v.Infinitive == verb
									select v)
									.FirstOrDefault();


			var conjugationVerb = (from c in _conjugation.GetAll()
									where c.Verb!.LinkLanguage == language.Oid && c.Form == verb
									select c)
									.FirstOrDefault(c => c.Form == verb);
			if (infinitiveVerb != null)
			{
				return infinitiveVerb;
			}
			else if (conjugationVerb != null)
			{
				return _verb.GetAll().FirstOrDefault(v => v.Oid == conjugationVerb.LinkVerb);
			}
			else
			{
				return null;
			}
			}
	}

	public string NormalizeLanguageName(string? languageName)
	{
		return languageName!.ToLower() switch
		{
			"deutsch" => "German",
			"français" => "French",
			_ => languageName
		};
	}

	public bool Exists(string languageName, string verb)
	{
		var language = _language.GetAll().FirstOrDefault(l => l.Name == NormalizeLanguageName(languageName));
		var infinite = _verb.GetAll().Any(v => v.Language == language && v.Infinitive == verb);
		var conjugation = _conjugation.GetAll().Any(c => c.Form == verb);

		return (infinite || conjugation);
	}

	public Dictionary<string, string> GetFormByTense(string? verb, string? mood, string? tense)
	{
		var conjugations = _conjugation.GetConjugationByMoodTense(verb, mood, tense).ToList();
		var midMoodPersons = _midMoodPerson.GetAll().ToList();
		var verbPersons = _verbPerson.GetAll().ToList();

		var form = (from mmp in midMoodPersons
					join vp in verbPersons on mmp.LinkVerbPerson equals vp.Oid
					join c in conjugations on mmp.Oid equals c.LinkMidMoodPerson into conjugationGroup
					from c in conjugationGroup.DefaultIfEmpty()
					where mmp.MoodType == mood
					orderby vp.SortOrder
					select new
					{
						Pronoun = vp.Pronoun,
						Form = c != null ? c.Form : ""
					})
					.ToDictionary(x => x.Pronoun, x => x.Form);
		
		return form;
	}

	public List<string> GetMoodList(string languageName)
	{
		var languageNameEng = NormalizeLanguageName(languageName);
		var moods = _mood.GetMoodsByLanguage(languageNameEng)
						 .OrderBy(m => m.SortOrder)
						 .Select(m => m.Type)
						 .Distinct()
						 .ToList();
		return moods;
	}

	public List<string> GetTenseList(string mood)
	{
		var tenses = _tense.GetTensesByMood(mood)
						   .OrderBy(t => t.SortOrder)
						   .Select(t => t.Name)
						   .Distinct()
						   .ToList();
		return tenses;
	}

	public Dictionary<string, string> GetPersonList(string mood)
	{
		var persons = _verbPerson.GetVerbPersonsByMood(mood)
								 .OrderBy(vp => vp.SortOrder)
								 .Select(vp => new
								 {
								 	Type = vp.Type,
								 	Pronoun = vp.Pronoun
								 })
								 .Distinct()
								 .ToDictionary(x => x.Type, x => x.Pronoun) ?? new Dictionary<string, string>();

		return persons;
	}

	//POST
	public async Task SaveVerb(ConjugationModel request, string languageName)
	{	
		var verb = _verb.GetAll().FirstOrDefault(v => v.Infinitive == request.Infinitive);
		var mood = _mood.GetAll().FirstOrDefault(m => m.Type == request.Mood);
		var tense = _tense.GetAll().FirstOrDefault(t => t.Name == request.Tense);

		if (verb != null)
		{
			_verb.RemoveConjugation(verb, mood, tense);			
			await CreateConjugations(request, verb, mood, tense);
		}
		else
		{
			var newVerb = await CreateVerb(request);
			await CreateConjugations(request, newVerb, mood, tense);
		}
		await _unitOfWork.SaveAsync();

		async Task<Verb> CreateVerb(ConjugationModel request)
		{
			var language = _language.GetAll().FirstOrDefault(l => l.Name == NormalizeLanguageName(languageName));
			var verb = new Verb
			{
				Infinitive = request.Infinitive,
				Translation = request.Translation,
				PastParticiple = request.PastParticiple,
				Reflexive = request.Reflexive,
				Separable = request.Separable,
				Language = language,
				LinkLanguage = language!.Oid
			};
			await _verb.AddAsync(verb);
			return verb;
		}

		async Task CreateConjugations(ConjugationModel request, Verb verb, Mood mood, Tense tense)
		{
			var midMoodTense = _midMoodTense.GetAll().FirstOrDefault(mmt => mmt.LinkMood == mood.Oid && mmt.LinkTense == tense.Oid);
			foreach (var item in request.Forms)
			{
				var person = _verbPerson.GetAll().FirstOrDefault(p => p.Pronoun == item.Key);
				var midMoodPerson = _midMoodPerson.GetAll().FirstOrDefault(mmp => mmp.MoodType == request.Mood && mmp.LinkVerbPerson == person.Oid);
				var conjugation = new Conjugation
				{
					Form = item.Value,
					Verb = verb,
					LinkVerb = verb.Oid,
					MidMoodTense = midMoodTense,
					LinkMidMoodTense = midMoodTense.Oid,
					MidMoodPerson = midMoodPerson,
					LinkMidMoodPerson = midMoodPerson.Oid
				};
				await _conjugation.AddAsync(conjugation);
			}
		}
	}
}
