/*
	Update date			Description 
	--------------------------------
	20260126			Initial
*/
using DataAccessLibrary;
using DataAccessLibrary.Entities;
using DataAccessLibrary.Interfaces;
using ViewModel.Models.NeutralModel;
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
	public ConjugationModel GetConjugation(string infinitive)
	{
		var verb = _verb.GetAll().FirstOrDefault(v => v.Infinitive == infinitive);
		var language = _language.GetAll().FirstOrDefault(l => l.Oid == verb.LinkLanguage)?.Name;
		var conjugations = _conjugation.GetAll().Where(c => c.LinkVerb == verb.Oid)
												.Select(c => new
												{
													Mood = c.MidMoodTense.Mood.Type,
													Tense = c.MidMoodTense.Tense.Name,
													Pronoun = c.MidMoodPerson.VerbPerson.Pronoun,
													Form = c.Form
												})
												.ToList();
		if (verb == null)
		{
			throw new Exception("Verb not found");
		}
		else
		{
			var conjugationForms = conjugations.GroupBy(c => new { c.Mood, c.Tense })
											   .Select(g => new ConjugationFormModel
											   {
												   Mood = g.Key.Mood,
												   Tense = g.Key.Tense,
												   Forms = g.ToDictionary(x => x.Pronoun, x => x.Form)
											   })
											   .ToList();

			var conjugationModel = new ConjugationModel
			{
				Language = language,
				Infinitive = verb.Infinitive,
				Translation = verb.Translation,
				PastParticiple = verb.PastParticiple,
				Reflexive = verb.Reflexive,
				Separable = verb.Separable,
				Conjugations = conjugationForms
			};

			return conjugationModel;
		}

		Dictionary<string, string> GetForms(List<Conjugation> conjugations)
		{
			var forms = new Dictionary<string, string>();

			foreach (var conj in conjugations)
			{
				var mood = conj.MidMoodPerson.Mood.Type;
				var prnoun = conj.MidMoodPerson.VerbPerson.Pronoun;
				var form = conj.Form;

				if (!forms.ContainsKey(prnoun))
				forms.Add( prnoun, form );
			}
			return forms;
		}
	}

	//POST
	public async Task SaveVerb(ConjugationModel request)
	{
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
	}
}
