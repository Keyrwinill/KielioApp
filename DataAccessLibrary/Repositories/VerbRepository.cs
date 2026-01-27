/*
	Update date			Description 
	--------------------------------------------------------------------------------
	20251125			Initial
	20260104			Modify structure
	20260108			Create methods for Mood, MidMoodTEnse, MidMoodPerson
	20260109			Create methods for Verb, VerbPerson, Conjugation, Tense
	20260120			Create create and delete methods for Mood, MidMoodTEnse, MidMoodPerson
	20260126			Create Delete method for Conjugation and Verb
*/
using DataAccessLibrary.Entities;
using DataAccessLibrary.Interfaces;

namespace DataAccessLibrary.Repositories;

public class VerbRepository : IVerbRepository
{
	private readonly KieliodbContext _context;
	public VerbRepository(KieliodbContext context)
	{
		_context = context;
	}

	public Verb GetByOid(Guid oid)
	{
		return _context.Verbs.FirstOrDefault(verb => verb.Oid == oid);
	}
	public IEnumerable<Verb> GetAll()
	{
		return _context.Verbs;
	}
	public IEnumerable<Verb> GetByLanguageOid(Guid languageOid)
	{
		return _context.Verbs.Where(verb => verb.LinkLanguage == languageOid);
	}
	//+>>20260109
	public async Task AddAsync(Verb verb)
	{
		await _context.Verbs.AddAsync(verb);
	}
	public async Task AddRangeAsync(IEnumerable<Verb> verbs)
	{
		await _context.Verbs.AddRangeAsync(verbs);
	}
	//+<<20260109
	//+>>20260126
	public void RemoveConjugation(Verb verb)
	{
		var conjugations = _context.Conjugations.Where(c => c.Verb == verb).ToList();
		foreach (var conj in conjugations)
		{
			_context.Conjugations.Remove(conj);
		}
	}
	//+<<20260126
}

public class TenseRepository : ITenseRepository
{
	private readonly KieliodbContext _context;
	public TenseRepository(KieliodbContext context)
	{
		_context = context;
	}

	public Tense GetByOid(Guid oid)
	{
		return _context.Tenses.FirstOrDefault(tense => tense.Oid == oid);
	}

	public Tense GetByLanguageName(string languageName)
	{
		return _context.Tenses.FirstOrDefault(tense => tense.Language.Name == languageName);
	}
	public IEnumerable<Tense> GetAll()
	{
		return _context.Tenses;
	}
	//+>>20260109
	public async Task AddAsync(Tense tense)
	{
		await _context.Tenses.AddAsync(tense);
	}
	public async Task AddRangeAsync(IEnumerable<Tense> tenses)
	{
		await _context.Tenses.AddRangeAsync(tenses);
	}
	//+<<20260109
}

public class VerbPersonRepository : IVerbPersonRepository
{
	private readonly KieliodbContext _context;
	public VerbPersonRepository(KieliodbContext context)
	{
		_context = context;
	}

	public VerbPerson GetByOid(Guid oid)
	{
		return _context.VerbPersons.FirstOrDefault(vp => vp.Oid == oid);
	}
	public IEnumerable<VerbPerson> GetAll()
	{
		return _context.VerbPersons;
	}
	//+>>20260109
	public async Task AddAsync(VerbPerson verbPerson)
	{
		await _context.VerbPersons.AddAsync(verbPerson);
	}
	public async Task AddRangeAsync(IEnumerable<VerbPerson> verbPersons)
	{
		await _context.VerbPersons.AddRangeAsync(verbPersons);
	}
	//+<<20260109
}

public class ConjugationRepository : IConjugationRepository
{
	private readonly KieliodbContext _context;
	public ConjugationRepository(KieliodbContext context)
	{
		_context = context;
	}

	public Conjugation GetByOid(Guid oid)
	{
		return _context.Conjugations.FirstOrDefault(conj => conj.Oid == oid);
	}
	public IQueryable<Conjugation> GetAll()
	{
		return _context.Conjugations;
	}
	//+>>20260109
	public async Task AddAsync(Conjugation conjugation)
	{
		await _context.Conjugations.AddAsync(conjugation);
	}
	public async Task AddRangeAsync(IEnumerable<Conjugation> conjugations)
	{
		await _context.Conjugations.AddRangeAsync(conjugations);
	}
	//+<<20260109
	//+>>20260126
	public void Delete(Conjugation conjugation)
	{
		if (conjugation == null)
			return;

		_context.Conjugations.Remove(conjugation);
	}
	//+<<20260126
}

//+>20260104
public class MoodRepository : IMoodRepository
{
	private readonly KieliodbContext _context;
	public MoodRepository(KieliodbContext context)
	{
		_context = context;
	}

	public Mood GetByOid(Guid oid)
	{
		return _context.Moods.FirstOrDefault(vm => vm.Oid == oid);
	}
	public IEnumerable<Mood> GetAll()
	{
		return _context.Moods;
	}

	//+>>20260108
	public async Task AddAsync(Mood mood)
	{
		await _context.Moods.AddAsync(mood);
	}
	public async Task AddRangeAsync(IEnumerable<Mood> moods)
	{
		await _context.Moods.AddRangeAsync(moods);
	}
	//+<<20260108

	//+>>20260120
	public async Task CreateMoodAsync(Language language, string moodType, List<Tense> selectedTenses, List<VerbPerson> selectedPersons)
	{
		var mood = new Mood()
		{
			LinkLanguage = language.Oid,
			Language = language,
			Type = moodType
		};

		await AddAsync(mood);
	}
	public void Delete(string languageName, string moodType)
	{
		var moods = _context.Moods.Where(m => m.Language.Name == languageName && m.Type == moodType).ToList();

		if (!moods.Any())
			return;

		_context.Moods.RemoveRange(moods);
	}
	//+<<20260120
}

public class MidMoodTenseRepository : IMidMoodTenseRepository
{
	private readonly KieliodbContext _context;
	public MidMoodTenseRepository(KieliodbContext context)
	{
		_context = context;
	}
	public MidMoodTense GetByOid(Guid oid)
	{
		return _context.MidMoodTenses.FirstOrDefault(mmt => mmt.Oid == oid);
	}
	public IEnumerable<MidMoodTense> GetAll()
	{
		return _context.MidMoodTenses;
	}

	//+>>20260108
	public async Task AddAsync(MidMoodTense midMoodTense)
	{
		await _context.MidMoodTenses.AddAsync(midMoodTense);
	}
	public async Task AddRangeAsync(IEnumerable<MidMoodTense> midMoodTenses)
	{
		await _context.MidMoodTenses.AddRangeAsync(midMoodTenses);
	}
	//+<<20260108
	//+>>20260120
	public async Task CreateMidMoodTenseAsync(Mood mood, List<Tense> selectedTenses)
	{
		foreach (var tense in selectedTenses)
		{
			var midMoodTense = new MidMoodTense()
			{
				MoodType = mood.Type,
				TenseName = tense.Name,
				LinkMood = mood.Oid,
				Mood = mood,
				LinkTense = tense.Oid,
				Tense = tense
			};
			await AddAsync(midMoodTense);
		}
	}

	public void Delete(Mood mood, List<Tense?> unselectedTenses)
	{
		var midmoodTenses = _context.MidMoodTenses.Where(midMT => midMT.Mood == mood && unselectedTenses.Contains(midMT.Tense)).ToList();

		if (!midmoodTenses.Any())
			return;

		foreach (var midmoodTense in midmoodTenses)
		{
			_context.MidMoodTenses.RemoveRange(midmoodTense);
		}
	}
	//+<<20260120
}

public class MidMoodPersonRepository : IMidMoodPersonRepository
{
	private readonly KieliodbContext _context;
	public MidMoodPersonRepository(KieliodbContext context)
	{
		_context = context;
	}
	public MidMoodPerson GetByOid(Guid oid)
	{
		return _context.MidMoodPersons.FirstOrDefault(mmp => mmp.Oid == oid);
	}
	public IEnumerable<MidMoodPerson> GetAll()
	{
		return _context.MidMoodPersons;
	}

	//+>>20260108
	public async Task AddAsync(MidMoodPerson midMoodPerson)
	{
		await _context.MidMoodPersons.AddAsync(midMoodPerson);
	}
	public async Task AddRangeAsync(IEnumerable<MidMoodPerson> midMoodPersons)
	{
		await _context.MidMoodPersons.AddRangeAsync(midMoodPersons);
	}
	//+<<20260108
	//+>>20260120
	public async Task CreateMidMoodPersonAsync(Mood mood, List<VerbPerson> selectedPersons)
	{
		foreach (var person in selectedPersons)
		{
			var midMoodTense = new MidMoodPerson()
			{
				MoodType = mood.Type,
				PersonType = person.Type,
				LinkMood = mood.Oid,
				Mood = mood,
				LinkVerbPerson = person.Oid,
				VerbPerson = person
			};
			await AddAsync(midMoodTense);
		}
	}

	public void Delete(Mood mood, List<VerbPerson?> unselectedPersons)
	{
		var midmoodPersons = _context.MidMoodPersons.Where(midMP => midMP.Mood == mood && unselectedPersons.Contains(midMP.VerbPerson)).ToList();

		if (!midmoodPersons.Any())
			return;

		foreach (var midmoodPerson in midmoodPersons)
		{
			_context.MidMoodPersons.RemoveRange(midmoodPerson);
		}
	}
	//+<<20260120
}
//+<<20260104
