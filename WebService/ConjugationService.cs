/*
	Update date			Description 
	--------------------------------
	20260126			Initial
*/
using DataAccessLibrary;
using DataAccessLibrary.Entities;
using DataAccessLibrary.Interfaces;
using ViewModel.Models.NeutralModels;
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
	public ConjugationModel GetConjugation(string? languageName, string? verb, string? mood, string? tense)
	{
		var language = _language.GetAll().FirstOrDefault(l => l.Name == languageName);
		var verbObj = FindInfinitiveForm(language, verb);

		if (verbObj == null)
		{
			throw new Exception("Verb not found");
		}
		else
		{
			if (!string.IsNullOrEmpty(verb) && !string.IsNullOrEmpty(mood) && !string.IsNullOrEmpty(tense))
			{
				var conjugationModel = new ConjugationModel
				{
					Language = languageName,
					MoodList = GetMoodList(languageName),
					Infinitive = verbObj.Infinitive,
					Translation = verbObj.Translation,
					PastParticiple = verbObj.PastParticiple,
					Reflexive = verbObj.Reflexive,
					Separable = verbObj.Separable,
					Mood = mood,
					Tense = tense,
					Forms = GetFormByTense(verb, mood, tense)
				};
				return conjugationModel;
			}
			else
			{ 	
				var conjugationModel = new ConjugationModel
				{
					Language = languageName,
					MoodList = GetMoodList(languageName),
					Infinitive = verbObj.Infinitive,
					Translation = verbObj.Translation,
					PastParticiple = verbObj.PastParticiple,
					Reflexive = verbObj.Reflexive,
					Separable = verbObj.Separable,
					Mood = "",
					Tense = "",
					Forms = new Dictionary<string, string>()
				};
				return conjugationModel;
			}
		}

		Verb FindInfinitiveForm(Language language, string? verb)
		{
			var infinitiveVerb = (from v in _verb.GetAll()
								  where v.LinkLanguage == language.Oid && v.Infinitive == verb
								  select v)
								  .FirstOrDefault();


			var conjugationVerb = (from c in _conjugation.GetAll()
								   where c.Verb.LinkLanguage == language.Oid && c.Form == verb
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
		/*
		Dictionary<string, string> GetFormByTense(string? verb, string? mood, string? tense)
		{
			var form = (from c in _conjugation.GetConjugationByMoodTense(verb, mood, tense)
						join mmp in _midMoodPerson.GetAll() on c.LinkMidMoodPerson equals mmp.Oid
						join vp in _verbPerson.GetAll() on mmp.LinkVerbPerson equals vp.Oid
						where mmp.MoodType == mood
						orderby vp.SortOrder
						select new
						{
							Pronoun = vp.Pronoun,
							Form = c.Form
						})
						.ToDictionary(x => x.Pronoun, x => x.Form);
			return form;
		}
		*/
	}

	public Dictionary<string, string> GetFormByTense(string? verb, string? mood, string? tense)
	{
		var form = (from c in _conjugation.GetConjugationByMoodTense(verb, mood, tense)
					join mmp in _midMoodPerson.GetAll() on c.LinkMidMoodPerson equals mmp.Oid
					join vp in _verbPerson.GetAll() on mmp.LinkVerbPerson equals vp.Oid
					where mmp.MoodType == mood
					orderby vp.SortOrder
					select new
					{
						Pronoun = vp.Pronoun,
						Form = c.Form
					})
					.ToDictionary(x => x.Pronoun, x => x.Form);
		return form;
	}

	public List<string> GetMoodList(string? languageName)
	{
		var moods = _mood.GetMoodsByLanguage(languageName)
						 .Select(m => m.Type)
						 .ToList();
		return moods;
	}

	public List<string> GetTenseList(string? mood)
	{
		var tenses = _tense.GetTensesByMood(mood)
						   .OrderBy(t => t.SortOrder)
						   .Select(t => t.Name)
						   .Distinct()
						   .ToList();
		return tenses;
	}

	public Dictionary<string, string> GetPersonList(string? mood)
	{
		var persons = _verbPerson.GetVerbPersonsByMood(mood)
								 .OrderBy(vp => vp.SortOrder)
								 .Select(vp => new
								 {
								 	Type = vp.Type,
								 	Pronoun = vp.Pronoun
								 })
								 .Distinct()
								 .ToDictionary(x => x.Type, x => x.Pronoun);

		return persons;
	}

	//POST
	public async Task SaveVerb(ConjugationModel request)
	{
		/*
		if (_verb.GetAll().Any(v => v.Infinitive == request.Infinitive))
		{
			var verb = _verb.GetAll().FirstOrDefault(v => v.Infinitive == request.Infinitive);
			if (verb == null)
			{
				return;
			}
			_verb.RemoveConjugation(verb);			
			await CreateConjugations(request, verb);
		}
		else
		{
			var verb = await CreateVerb(request);
			await CreateConjugations(request, verb);
		}
		await _unitOfWork.SaveAsync();

		async Task<Verb> CreateVerb(ConjugationModel request)
		{
			if (request == null)
			{
				throw new Exception("Invalid request");
			}
			if (string.IsNullOrEmpty(request.Infinitive))
			{
				throw new Exception("Infinitive is required");
			}

			var language = _language.GetAll().FirstOrDefault(l => l.Name == request.Language);
			var verb = new Verb
			{
				Infinitive = request.Infinitive,
				Translation = request.Translation,
				PastParticiple = request.PastParticiple,
				Reflexive = request.Reflexive,
				Separable = request.Separable,
				Language = language,
				LinkLanguage = language.Oid
			};
			await _verb.AddAsync(verb);
			return verb;
		}

		async Task CreateConjugations(ConjugationModel request, Verb verb)
		{
			if (request == null)
			{
				throw new Exception("Invalid request");
			}

			var moodList = _mood.GetAll().Where(m => request.Conjugations.Select(c => c.Mood).Contains(m.Type)).ToList();
			var tenseList = _tense.GetAll().Where(t => request.Conjugations.Select(c => c.Tense).Contains(t.Name)).ToList();
			var personList = _verbPerson.GetAll().Where(p => request.Conjugations.SelectMany(c => c.Forms.Keys).Contains(p.Pronoun)).ToList();


			var midMoodTenses = _midMoodTense.GetAll().Where(mmt => moodList.Contains(mmt.Mood) &&
																	tenseList.Contains(mmt.Tense)).ToList();
			var midMoodPersons = _midMoodPerson.GetAll().Where(mmp => moodList.Contains(mmp.Mood) &&
																	  personList.Contains(mmp.VerbPerson)).ToList();

			foreach (var mmt in midMoodTenses)
			{
				var conjugationForm = request.Conjugations.FirstOrDefault(c => c.Mood == mmt.Mood.Type && c.Tense == mmt.Tense.Name);
				if (conjugationForm != null)
				{
					foreach (var form in conjugationForm.Forms)
					{
						var verbPerson = personList.FirstOrDefault(p => p.Pronoun == form.Key);
						var midMoodPerson = midMoodPersons.FirstOrDefault(mmp => mmp.Mood == mmt.Mood && mmp.VerbPerson == verbPerson);
						if (midMoodPerson != null)
						{
							var conjugation = new Conjugation
							{
								Form = form.Value,
								Verb = verb,
								LinkVerb = verb.Oid,
								MidMoodTense = mmt,
								LinkMidMoodTense = mmt.Oid,
								MidMoodPerson = midMoodPerson,
								LinkMidMoodPerson = midMoodPerson.Oid
							};
							await _conjugation.AddAsync(conjugation);
						}
					}
				}
			}
		}
		*/
	}
}
