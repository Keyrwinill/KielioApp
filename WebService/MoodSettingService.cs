/*
	Update date			Description 
	----------------------------------------
	20260108			Initial
	20260120			POST Methods
*/

using DataAccessLibrary;
using DataAccessLibrary.Dictionaries;
using DataAccessLibrary.Entities;
using DataAccessLibrary.Interfaces;
using ViewModel.Models.RequestModels;
using ViewModel.Models.ResponseModels;
using ViewModel.Models.Shared;
using WebService.Interfaces;

namespace WebService;

public class MoodSettingService : IMoodSettingService
{
	private readonly IUnitofWork _unitOfWork;

	private readonly ILanguageRepository _language;
	private readonly ITenseRepository _tense;
	private readonly IVerbPersonRepository _verbPerson;
	private readonly IMoodRepository _mood;
	private readonly IMidMoodTenseRepository _midMoodTense;
	private readonly IMidMoodPersonRepository _midMoodPerson;
	

	public MoodSettingService(IUnitofWork unitofWork,
							  ILanguageRepository language,
							  ITenseRepository tense,
							  IVerbPersonRepository verbPerson,
							  IMoodRepository mood,
							  IMidMoodTenseRepository midMoodTense,
							  IMidMoodPersonRepository midMoodPerson
							 )
	{
		_unitOfWork = unitofWork;
		_language = language;
		_tense = tense;
		_verbPerson = verbPerson;
		_mood = mood;
		_midMoodTense = midMoodTense;
		_midMoodPerson = midMoodPerson;
	}

	//GET
	public MoodSettingResponseModel GetMood(string? languageName, string? moodType)
	{
		var languages = _language.GetAll().ToList();
		var moods = _mood.GetAll()
						 .Where(m => m.Language?.Name == languageName)
						 .OrderBy(m => m.SortOrder)
						 .ToList();
		var allTenses = _tense.GetAll().ToList();
		var allPersons = _verbPerson.GetAll().ToList();

		var selectedMood = !string.IsNullOrEmpty(moodType) 
						   ? moods.FirstOrDefault(m => m.Type == moodType)
						   : moods.FirstOrDefault();

		var midMoodTenses = selectedMood != null
								? _midMoodTense.GetAll()
											   .Where(mmt => mmt.Mood == selectedMood)
											   .ToList()
								: [];

		var midMoodPersons = selectedMood != null
								? _midMoodPerson.GetAll()
											   .Where(mmp => mmp.Mood == selectedMood)
											   .ToList()
								: [];

		var response = new MoodSettingResponseModel()
		{
			LanguageName = !string.IsNullOrEmpty(languageName) ? languageName : "",

			MoodType = !string.IsNullOrEmpty(moodType) ? moodType : "",

			LanguageList = _language.GetAll()
								    .Select(l => l.Name)
								    .ToList(),

			MoodList = moods.OrderBy(m => m.SortOrder)
							.Select(m => m.Type)
							.Distinct()
							.ToList(),

			TenseList = _tense.GetAll()
							  .OrderBy(t => t.SortOrder)
							  .Select(t => new CheckboxViewModel
							  {
								  Name = t.Name,
								  IsChecked = midMoodTenses.Any(midMT => midMT.Tense == t) == true
							  })
							  .ToList(),

			PersonList = _verbPerson.GetAll()
									.OrderBy(p => p.SortOrder)
									.Select(p => new CheckboxViewModel
									{
										Name = PersonTypeDictionary.PersonType.ContainsKey(p.Type) 
												? PersonTypeDictionary.PersonType[p.Type] 
												: p.Type,
										IsChecked = midMoodPersons.Any(midMP => midMP.VerbPerson == p) == true
									}).ToList()
		};

		return (response);
	}

	//+>>20260120
	//POST (rebuild response)
	public MoodSettingResponseModel RebuildResponse(MoodSettingRequestModel request)
	{
		var languageName = request != null ? request.LanguageName : "";
		var moodType = request != null ? request.MoodType : "";
		var response = GetMood(languageName, moodType);

		response.TenseList.ForEach(t => t.IsChecked = request.TenseList.Any(x => x.Name == t.Name && x.IsChecked));
		response.PersonList.ForEach(p => p.IsChecked = request.PersonList.Any(x => x.Name == p.Name && x.IsChecked));

		return response;
	}

	//POST (save)
	public async Task SaveMood(MoodSettingRequestModel request)
	{
		var languageName = request != null ? request.LanguageName : "";
		var moodType = request != null ? request.MoodType : "";
		var tenseList = request != null ? request.TenseList : [];
		var personList = request != null ? request.PersonList : [];

		var language = _language.GetAll().FirstOrDefault(l => l.Name == languageName) ?? throw new Exception("Language not found!");
		var mood = _mood.GetAll().FirstOrDefault(m => m?.Language?.Name == languageName && m.Type == moodType);

		if (mood == null)
		{
			// Create new mood
			mood = new Mood
			{
				Oid = Guid.NewGuid(),
				Type = moodType,
				LinkLanguage = language.Oid,
				SortOrder = _mood.GetAll().Where(m => m.LinkLanguage == language.Oid).Count() + 1
			};
			await _mood.AddAsync(mood);
		}
		else
		{
			// Modify existing mood
			// Clear existing relations
			var existingMidMoodTenses = _midMoodTense.GetAll().Where(t => t.Mood == mood).ToList();
			var existTenses = existingMidMoodTenses.Select(t => t.Tense).ToList();
			_midMoodTense.Delete(mood, existTenses);

			var existingMidMoodPersons = _midMoodPerson.GetAll().Where(p => p.Mood == mood).ToList();
			var existPersons = existingMidMoodPersons.Select(p => p.VerbPerson).ToList();
			_midMoodPerson.Delete(mood, existPersons);
		}

		_unitOfWork.Save();

		// Add new relations
		var selectedTenses = _tense.GetAll()
								   .Where(t => tenseList.Any(x => x.Name == t.Name && x.IsChecked))
								   .ToList();

		foreach (var tense in selectedTenses)
		{
			var midMoodTense = new MidMoodTense
			{
				LinkMood = mood.Oid,
				Mood = mood,
				MoodType = mood.Type,
				LinkTense = tense.Oid,
				Tense = tense,
				TenseName = tense.Name
			};
			await _midMoodTense.AddAsync(midMoodTense);
		}

		var personCode = PersonTypeDictionary.PersonType.ToDictionary(x => x.Value, x => x.Key);
		var selectedPersonCodes = personList.Where(x => x.IsChecked)
											.Select(x => personCode[x.Name])
											.ToHashSet();
		var selectedPersons = _verbPerson.GetAll()
										 .Where(p => selectedPersonCodes.Contains(p.Type))
										 .ToList();

		foreach (var person in selectedPersons)
		{
			var midMoodPerson = new MidMoodPerson
			{
				LinkMood = mood.Oid,
				Mood = mood,
				MoodType = mood.Type,
				LinkVerbPerson = person.Oid,
				VerbPerson = person,
				PersonType = person.Type
			};
			await _midMoodPerson.AddAsync(midMoodPerson);
		}

		_unitOfWork.Save();
	}
	//+<<20260120
}
