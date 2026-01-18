/*
	Update date			Description 
	--------------------------------
	20251225			Initial
	20260104			Modify structure
	20260109			New fields for seach order
*/
using DataAccessLibrary.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLibrary;

public class KieliodbContext : DbContext
{
	public KieliodbContext(){ }
	public KieliodbContext(DbContextOptions<KieliodbContext> options)
		: base(options)
	{
	}

	public virtual DbSet<DeutschAdjektiv> DeutschAdjektivs { get; set; }
	public virtual DbSet<Language> Languages { get; set; }
	public virtual DbSet<Verb> Verbs { get; set; }
	public virtual DbSet<Tense> Tenses { get; set; }
	public virtual DbSet<VerbPerson> VerbPersons { get; set; }
	public virtual DbSet<Conjugation> Conjugations { get; set; }
	public virtual DbSet<Mood> Moods { get; set; }
	//+>>20260104
	public virtual DbSet<MidMoodTense> MidMoodTenses { get; set; }
	public virtual DbSet<MidMoodPerson> MidMoodPersons { get; set; }
	//+<<20260104

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		=> optionsBuilder.UseSqlServer("Server=localhost;Database=KielioDB;User ID=erwin031823;Password=valor1826;TrustServerCertificate=true");

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		// DeutschAdjektiv
		modelBuilder.Entity<DeutschAdjektiv>(entity =>
		{
			entity.ToTable("DeutschAdjektiv");
			entity.HasKey(e => e.Oid);

			entity.Property(e => e.Oid)
				  .HasDefaultValueSql("(newid())")
				  .HasColumnName("Oid");
			entity.Property(e => e.Type)
				  .HasColumnName("Type")
				  .HasColumnType("nvarchar(30)");
			entity.Property(e => e.Gender)
				  .HasColumnName("Gender")
				  .HasColumnType("nvarchar(30)");
			entity.Property(e => e.Case)
				  .HasColumnName("Case")
				  .HasColumnType("nvarchar(30)");
			entity.Property(e => e.ArticleEnding)
				  .HasColumnName("ArticleEnding")
				  .HasColumnType("nvarchar(30)");
			entity.Property(e => e.AdjectiveEnding)
				  .HasColumnName("AdjectiveEnding")
				  .HasColumnType("nvarchar(30)");
		});

		// Language
		modelBuilder.Entity<Language>(entity =>
		{
			entity.ToTable("Language")
				  .HasKey(e => e.Oid);

			entity.Property(e => e.Oid)
				  .HasDefaultValueSql("(newid())")
				  .HasColumnName("Oid");
			entity.Property(e => e.Name)
				  .HasColumnName("Name")
				  .HasColumnType("nvarchar(30)");
		});

		// Verb
		modelBuilder.Entity<Verb>(entity =>
		{
			entity.ToTable("Verb")
				  .HasKey(e => e.Oid);

			entity.HasOne(e => e.Language)
				  .WithMany(m => m.Verbs)
				  .HasForeignKey(e => e.LinkLanguage)
				  .OnDelete(DeleteBehavior.NoAction);

			entity.Property(e => e.Oid)
				  .HasDefaultValueSql("(newid())")
				  .HasColumnName("Oid");
			entity.Property(e => e.Infinitive)
				  .HasColumnName("Infinitive")
				  .HasColumnType("nvarchar(30)");
			entity.Property(e => e.Reflexive)
				  .HasColumnName("Reflexive")
				  .HasColumnType("bit");
			entity.Property(e => e.Translation)
				  .HasColumnName("Translation")
				  .HasColumnType("nvarchar(30)");
			//+>>20260104
			entity.Property(e => e.Regular)
				  .HasColumnName("Regular")
				  .HasColumnType("bit");
			entity.Property(e => e.Separable)
				  .HasColumnName("Separable")
				  .HasColumnType("bit");
			entity.Property(e => e.PastParticiple)
				  .HasColumnName("PastParticiple")
				  .HasColumnType("nvarchar(30)");
			//+<<20260104
		});

		// Tense
		modelBuilder.Entity<Tense>(entity =>
		{
			entity.ToTable("Tense")
				  .HasKey(e => e.Oid);

			entity.HasOne(e => e.Language)
				  .WithMany(m => m.Tenses)
				  .HasForeignKey(e => e.LinkLanguage)
				  .OnDelete(DeleteBehavior.NoAction);

			entity.Property(e => e.Oid)
				  .HasDefaultValueSql("(newid())")
				  .HasColumnName("Oid");
			entity.Property(e => e.Name)
				  .HasColumnName("Name")
				  .HasColumnType("nvarchar(30)");
			entity.Property(e => e.Description)
				  .HasColumnName("Description")
				  .HasColumnType("nvarchar(30)");
			//+>>20260109
			entity.Property(e => e.SortOrder)
				  .HasColumnName("SortOrder")
				  .HasColumnType("int");
			//+<<20260109
		});

		// VerbPerson
		modelBuilder.Entity<VerbPerson>(entity =>
		{
			entity.ToTable("VerbPerson")
				  .HasKey(e => e.Oid);

			entity.HasOne(e => e.Language)
				  .WithMany(m => m.VerbPersons)
				  .HasForeignKey(e => e.LinkLanguage)
				  .OnDelete(DeleteBehavior.NoAction);

			entity.Property(e => e.Oid)
				  .HasDefaultValueSql("(newid())")
				  .HasColumnName("Oid");
			entity.Property(e => e.Pronoun)
				  .HasColumnName("Pronoun")
				  .HasColumnType("nvarchar(30)");
			entity.Property(e => e.Type)
				  .HasColumnName("Type")
				  .HasColumnType("nvarchar(30)");
			//+>>20260109
			entity.Property(e => e.SortOrder)
				  .HasColumnName("SortOrder")
				  .HasColumnType("int");
			//+<<20260109
		});

		// Conjugation
		modelBuilder.Entity<Conjugation>(entity =>
		{
			entity.ToTable("Conjugation")
				  .HasKey(e => e.Oid);

			entity.HasOne(e => e.Verb)
				  .WithMany(m => m.Conjugations)
				  .HasForeignKey(e => e.LinkVerb)
				  .OnDelete(DeleteBehavior.NoAction);

			entity.HasOne(e => e.MidMoodTense)
				  .WithMany(m => m.Conjugations)
				  .HasForeignKey(e => e.LinkMidMoodTense)
				  .OnDelete(DeleteBehavior.NoAction);

			entity.HasOne(e => e.MidMoodPerson)
				  .WithMany(m => m.Conjugations)
				  .HasForeignKey(e => e.LinkMidMoodPerson)
				  .OnDelete(DeleteBehavior.NoAction);

			entity.Property(e => e.Oid)
				  .HasDefaultValueSql("(newid())")
				  .HasColumnName("Oid");
			entity.Property(e => e.Form)
				  .HasColumnName("Form")
				  .HasColumnType("nvarchar(30)");
		});

		//+>>20260104
		// Mood
		modelBuilder.Entity<Mood>(entity =>
		{
			entity.ToTable("Mood")
				  .HasKey(e => e.Oid);

			entity.HasOne(e => e.Language)
				  .WithMany(m => m.Moods)
				  .HasForeignKey(e => e.LinkLanguage)
				  .OnDelete(DeleteBehavior.NoAction);

			entity.Property(e => e.Oid)
				  .HasDefaultValueSql("(newid())")
				  .HasColumnName("Oid");
			entity.Property(e => e.Type)
				  .HasColumnName("Type")
				  .HasColumnType("nvarchar(30)");
			//+>>20260109
			entity.Property(e => e.SortOrder)
				  .HasColumnName("SortOrder")
				  .HasColumnType("int");
			//+<<20260109
		});

		// MidMoodTense
		modelBuilder.Entity<MidMoodTense>(entity =>
		{
			entity.ToTable("MidMoodTense")
				  .HasKey(e => e.Oid);

			entity.HasOne(e => e.Mood)
				  .WithMany(m => m.MidMoodTenses)
				  .HasForeignKey(e => e.LinkMood)
				  .OnDelete(DeleteBehavior.NoAction);

			entity.HasOne(e => e.Tense)
				  .WithMany(m => m.MidMoodTenses)
				  .HasForeignKey(e => e.LinkTense)
				  .OnDelete(DeleteBehavior.NoAction);

			entity.Property(e => e.Oid)
				  .HasDefaultValueSql("(newid())")
				  .HasColumnName("Oid");
			entity.Property(e => e.MoodType)
				  .HasColumnName("MoodType")
				  .HasColumnType("nvarchar(30)");
			entity.Property(e => e.TenseName)
				  .HasColumnName("TenseName")
				  .HasColumnType("nvarchar(30)");
		});

		// MidMoodPerson
		modelBuilder.Entity<MidMoodPerson>(entity =>
		{
			entity.ToTable("MidMoodPerson")
				  .HasKey(e => e.Oid);
			
			entity.HasOne(e => e.Mood)
				  .WithMany(m => m.MidMoodPersons)
				  .HasForeignKey(e => e.LinkMood)
				  .OnDelete(DeleteBehavior.NoAction);

			entity.HasOne(e => e.VerbPerson)
				  .WithMany(m => m.MidMoodPersons)
				  .HasForeignKey(e => e.LinkVerbPerson)
				  .OnDelete(DeleteBehavior.NoAction);

			entity.Property(e => e.Oid)
				  .HasDefaultValueSql("(newid())")
				  .HasColumnName("Oid");
			entity.Property(e => e.MoodType)
				  .HasColumnName("MoodType")
				  .HasColumnType("nvarchar(30)");
			entity.Property(e => e.PersonType)
				  .HasColumnName("PersonType")
				  .HasColumnType("nvarchar(30)");
		});
		//+<<20260104
	}
}
