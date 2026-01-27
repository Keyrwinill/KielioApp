/*
	Update date			Description 
	---------------------------------------------------------------
	20251130			Initial
	20260104			Modify structure
	20260108			Create methods for Mood, MidMoodTense, MidMoodPerson
	20260109			Create methods for Verb, VerbPerson, Conjugation, Tense
	20260120			Create create and delete methods for Mood, MidMoodTEnse, MidMoodPerson
	20260126			Create Delete method for Conjugation and Verb
*/
using DataAccessLibrary.Entities;

namespace DataAccessLibrary.Interfaces;

public interface IVerbRepository
{
	Verb GetByOid(Guid oid);
	IEnumerable<Verb> GetAll();
	//+>>20260109
	Task AddAsync(Verb verb);
	Task AddRangeAsync(IEnumerable<Verb> verbs);
	//+<<20260109
	void RemoveConjugation(Verb verb);        //+20260126
}
public interface ITenseRepository
{
	Tense GetByOid(Guid oid);
	Tense GetByLanguageName(string languageName);
	IEnumerable<Tense> GetAll();
	//+>>20260109
	Task AddAsync(Tense tense);
	Task AddRangeAsync(IEnumerable<Tense> tenses);
	//+<<20260109
}

public interface IVerbPersonRepository
{
	VerbPerson GetByOid(Guid oid);
	IEnumerable<VerbPerson> GetAll();
	//+>>20260109
	Task AddAsync(VerbPerson verbPerson);
	Task AddRangeAsync(IEnumerable<VerbPerson> verbPersons);
	//+<<20260109
}

public interface IConjugationRepository
{
	Conjugation GetByOid(Guid oid);
	IQueryable<Conjugation> GetAll();
	//+>>20260109
	Task AddAsync(Conjugation conjugation);
	Task AddRangeAsync(IEnumerable<Conjugation> conjugations);
	//+<<20260109
	void Delete(Conjugation conjugation);       //+20260126
}

//+>20260104
public interface IMoodRepository
{
	Mood GetByOid(Guid oid);
	IEnumerable<Mood> GetAll();
	//+>>20260108
	Task AddAsync(Mood mood);
	Task AddRangeAsync(IEnumerable<Mood> moods);
	//+<<20260108
	//+>>20260120
	Task CreateMoodAsync(Language language, string moodType, List<Tense> selectedTenses, List<VerbPerson> selectedPersons);
	void Delete(string languageName, string moodType);
	//+<<20260120
}

public interface IMidMoodTenseRepository
{
	MidMoodTense GetByOid(Guid oid);
	IEnumerable<MidMoodTense> GetAll();
	//+>>20260108
	Task AddAsync(MidMoodTense midMoodTense);
	Task AddRangeAsync(IEnumerable<MidMoodTense> midMoodTenses);
	//+<<20260108
	//+>>20260120
	Task CreateMidMoodTenseAsync(Mood mood, List<Tense> selectedTenses);
	void Delete(Mood mood, List<Tense?> unselectedTenses);
	//+<<20260120
}

public interface IMidMoodPersonRepository
{
	MidMoodPerson GetByOid(Guid oid);
	IEnumerable<MidMoodPerson> GetAll();
	//+>>20260108
	Task AddAsync(MidMoodPerson midMoodPerson);
	Task AddRangeAsync(IEnumerable<MidMoodPerson> midMoodPersons);
	//+<<20260108
	//+>>20260120
	Task CreateMidMoodPersonAsync(Mood mood, List<VerbPerson> selectedPersons);
	void Delete(Mood mood, List<VerbPerson?> unselectedPersons);
	//+<<20260120
}
//+<<20260104