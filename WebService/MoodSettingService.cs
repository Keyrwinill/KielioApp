/*
	Update date			Description 
	----------------------------------------
	20260108			Initial
*/

using DataAccessLibrary;
using DataAccessLibrary.Dictionaries;
using DataAccessLibrary.Entities;
using DataAccessLibrary.Interfaces;
using ViewModel.Enums;
using ViewModel.Models.RequestModels;
using ViewModel.Models.ResponseModels;
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

	public MoodSettingResponseModel GetMoodSettingView(MoodSettingRequestModel request)
	{		
		var languageName = request.LanguageName;
		var moodType = request.MoodType;
		var operation = request.Operation;

		var languages = _language.GetAll().ToList();

		//LanguageList
		List<string> languageList = new List<string>();
		foreach (var language in languages)
		{
			if (language != null)
			{ languageList.Add(language.Name); }
		}

		
		var moods = _mood.GetAll()
						 .Where(m => m.Language.Name == languageName)
						 .OrderBy(m => m.SortOrder)
						 .ToList();

		//MoodList
		List<string> moodList = new List<string>();	
		foreach (var mood in moods)
		{
			if (mood != null)
			{ moodList.Add(mood.Type); }
		}

		var tenses = _tense.GetAll()
						   .Where(t => t.Language.Name == languageName)
						   .OrderBy(t => t.SortOrder)
						   .ToList();

		//TenseList
		List<CheckboxViewModel> tenseList = new List<CheckboxViewModel>();
		foreach (var tense in tenses)
		{
			var mood = _mood.GetAll()
							.FirstOrDefault(m => m.Type == moodType);
			var checkboxInfo = GetTenseList(operation, mood, tense);
			tenseList.Add(checkboxInfo);
		}

		var persons = _verbPerson.GetAll()
								 .Where(p => p.Language.Name == languageName)
								 .OrderBy(p => p.SortOrder)
								 .ToList();

		//PersonList
		List<CheckboxViewModel> personList = new List<CheckboxViewModel>();
		foreach (var person in persons)
		{
			var mood = _mood.GetAll()
							.FirstOrDefault(m => m.Type == moodType);
			var checkboxInfo = GetPersonList(operation, mood, person);
			personList.Add(checkboxInfo);
		}

		//Status
		var status = operation switch
		{
			"Initial" => MoodSettingPageStatus.Initial.ToString(),
			"LanguageSelected" => MoodSettingPageStatus.LanguageSelected.ToString(),
			"MoodSelected" => MoodSettingPageStatus.View.ToString(),
			"Create" => MoodSettingPageStatus.Create.ToString(),
			"Modify" => MoodSettingPageStatus.Modify.ToString(),
			"Save" => MoodSettingPageStatus.View.ToString(),
			_ => null
		};

		//CanCreate
		bool canCreate = !string.IsNullOrEmpty(languageName) ? true : false;

		//CanUpdate
		bool canUpdate = (!string.IsNullOrEmpty(languageName) && !string.IsNullOrEmpty(moodType)) ? true : false;

		//CanSave
		bool canSave = (operation == "Create" || operation == "Modify") ? true : false;

		MoodSettingResponseModel responseModel = new MoodSettingResponseModel()
		{
			LanguageName = languageName,
			MoodType = moodType,
			LanguageList = languageList,
			MoodList = moodList,
			TenseList = tenseList,
			PersonList = personList,
			Status = status,
			CanCreate = canCreate,
			CanUpdate = canUpdate,
			CanSave = canSave,
		};

		return (responseModel);

		CheckboxViewModel GetTenseList(string operation, Mood mood, Tense tense)
		{
			var midMoodTense = _midMoodTense.GetAll().FirstOrDefault(midMT => midMT.Mood == mood && midMT.Tense == tense);
			string tenseName = tense.Name;
			bool isChecked;

			//IsChecked
			if (midMoodTense != null)
			{ isChecked = true; }
			else 
			{ isChecked = false; }

			//IsDisabled
			bool isDisabled;
			string[] editableStatus = ["Create", "Modify"];
			if (editableStatus.Contains(operation))
			{ isDisabled = false; }
			else
			{ isDisabled = true; }

			return new CheckboxViewModel
			{
				Name = tenseName,
				IsChecked = isChecked,
				IsDisabled = isDisabled,
			};
		}

		CheckboxViewModel GetPersonList(string operation, Mood mood, VerbPerson person)
		{
			var midMoodPerson = _midMoodPerson.GetAll().FirstOrDefault(midMP => midMP.Mood == mood && midMP.VerbPerson == person);
			string personType = person.Type;
			PersonTypeDictionary.PersonType.TryGetValue(personType, out string personTypeDisp);
			bool isChecked;

			//IsChecked
			if (midMoodPerson != null)
			{ isChecked = true; }
			else
			{ isChecked = false; }

			//IsDisabled
			bool isDisabled;
			string[] editableStatus = ["Create", "Modify"];
			if (editableStatus.Contains(operation))
			{ isDisabled = false; }
			else
			{ isDisabled = true; }

			return new CheckboxViewModel
			{
				Name = personTypeDisp,
				IsChecked = isChecked,
				IsDisabled = isDisabled
			};
		}
	}

	public async void SetLink(string operation, string languageName, string moodType, List<int> tenseSelectionList, List<int> personSelectionList)
	{
		var language = _language.GetAll().FirstOrDefault(lang => lang.Name == languageName);

		if (operation == "Create")
		{
			var mood = await _mood.CreateByTypeAsync(language, moodType);
			_unitOfWork.Save();

			SetMidTable(mood, tenseSelectionList, personSelectionList);
		}
		else
		{
			var mood = _mood.GetAll().FirstOrDefault(m => m.Type == moodType);
			SetMidTable(mood, tenseSelectionList, personSelectionList);
		}

		void SetMidTable(Mood mood, List<int> tenseSelectionList, List<int> personSelectionList)
		{
			foreach (var tenseSelection in tenseSelectionList)
			{
				var tense = _tense.GetAll().FirstOrDefault(t => t.SortOrder == tenseSelection);
				var midMoodTense = _midMoodTense.GetAll().FirstOrDefault(midMT => midMT.Mood == mood && midMT.Tense == tense);

				if (midMoodTense == null && tense != null)
				{					
					_midMoodTense.SetMoodTenseAsync(mood, tense);
				}
				_unitOfWork.Save();
			}

			foreach (var personSelection in personSelectionList)
			{
				var verbPerson = _verbPerson.GetAll().FirstOrDefault(vp => vp.SortOrder == personSelection);
				var midMoodPerson = _midMoodPerson.GetAll().FirstOrDefault(midMP => midMP.Mood == mood && midMP.VerbPerson == verbPerson);
				
				if (midMoodPerson == null && verbPerson != null)
				{
					_midMoodPerson.SetMoodPersonAsync(mood, verbPerson);
				}
				_unitOfWork.Save();
			}
		}
	}
}
