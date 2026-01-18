/*
	Update date			Description 
	---------------------------------------------------------------
	20251130			Initial
	20260104			Modify structure
	20260108			Create methods for Mood, MidMoodTense, MidMoodPerson
	20260109			Create methods for Verb, VerbPerson, Conjugation, Tense
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
	IEnumerable<Conjugation> GetAll();
	//+>>20260109
	Task AddAsync(Conjugation conjugation);
	Task AddRangeAsync(IEnumerable<Conjugation> conjugations);
	//+<<20260109
}

//+>20260104
public interface IMoodRepository
{
	Mood GetByOid(Guid oid);
	IEnumerable<Mood> GetAll();
	//+>>20260108
	Task AddAsync(Mood mood);
	Task AddRangeAsync(IEnumerable<Mood> moods);
	Task<Mood> CreateByTypeAsync(Language language, string type);
	//+<<20260108
}

public interface IMidMoodTenseRepository
{
	MidMoodTense GetByOid(Guid oid);
	IEnumerable<MidMoodTense> GetAll();
	//+>>20260108
	Task AddAsync(MidMoodTense midMoodTense);
	Task AddRangeAsync(IEnumerable<MidMoodTense> midMoodTenses);
	Task<MidMoodTense> SetMoodTenseAsync(Mood mood, Tense tense);
	//+<<20260108
}

public interface IMidMoodPersonRepository
{
	MidMoodPerson GetByOid(Guid oid);
	IEnumerable<MidMoodPerson> GetAll();
	//+>>20260108
	Task AddAsync(MidMoodPerson midMoodPerson);
	Task AddRangeAsync(IEnumerable<MidMoodPerson> midMoodPersons);
	Task<MidMoodPerson> SetMoodPersonAsync(Mood mood, VerbPerson verbPerson);
	//+<<20260108
}
//+<<20260104