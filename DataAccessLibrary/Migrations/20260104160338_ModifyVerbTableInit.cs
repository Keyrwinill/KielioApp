using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLibrary.Migrations
{
    /// <inheritdoc />
    public partial class ModifyVerbTableInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /*
            migrationBuilder.CreateTable(
                name: "DeutschAdjektiv",
                columns: table => new
                {
                    Oid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Type = table.Column<string>(type: "nvarchar(30)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(30)", nullable: true),
                    Case = table.Column<string>(type: "nvarchar(30)", nullable: true),
                    ArticleEnding = table.Column<string>(type: "nvarchar(30)", nullable: true),
                    AdjectiveEnding = table.Column<string>(type: "nvarchar(30)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeutschAdjektiv", x => x.Oid);
                });
            */

            migrationBuilder.CreateTable(
                name: "Language",
                columns: table => new
                {
                    Oid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Name = table.Column<string>(type: "nvarchar(30)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Language", x => x.Oid);
                });

            migrationBuilder.CreateTable(
                name: "Mood",
                columns: table => new
                {
                    Oid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Type = table.Column<string>(type: "nvarchar(30)", nullable: true),
                    LinkLanguage = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mood", x => x.Oid);
                    table.ForeignKey(
                        name: "FK_Mood_Language_LinkLanguage",
                        column: x => x.LinkLanguage,
                        principalTable: "Language",
                        principalColumn: "Oid");
                });

            migrationBuilder.CreateTable(
                name: "Tense",
                columns: table => new
                {
                    Oid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Name = table.Column<string>(type: "nvarchar(30)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(30)", nullable: true),
                    LinkLanguage = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tense", x => x.Oid);
                    table.ForeignKey(
                        name: "FK_Tense_Language_LinkLanguage",
                        column: x => x.LinkLanguage,
                        principalTable: "Language",
                        principalColumn: "Oid");
                });

            migrationBuilder.CreateTable(
                name: "Verb",
                columns: table => new
                {
                    Oid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Infinitive = table.Column<string>(type: "nvarchar(30)", nullable: true),
                    Reflexive = table.Column<bool>(type: "bit", nullable: false),
                    Translation = table.Column<string>(type: "nvarchar(30)", nullable: true),
                    Regular = table.Column<bool>(type: "bit", nullable: false),
                    Separable = table.Column<bool>(type: "bit", nullable: false),
                    PastParticiple = table.Column<string>(type: "nvarchar(30)", nullable: true),
                    LinkLanguage = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Verb", x => x.Oid);
                    table.ForeignKey(
                        name: "FK_Verb_Language_LinkLanguage",
                        column: x => x.LinkLanguage,
                        principalTable: "Language",
                        principalColumn: "Oid");
                });

            migrationBuilder.CreateTable(
                name: "VerbPerson",
                columns: table => new
                {
                    Oid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Pronoun = table.Column<string>(type: "nvarchar(30)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(30)", nullable: true),
                    LinkLanguage = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VerbPerson", x => x.Oid);
                    table.ForeignKey(
                        name: "FK_VerbPerson_Language_LinkLanguage",
                        column: x => x.LinkLanguage,
                        principalTable: "Language",
                        principalColumn: "Oid");
                });

            migrationBuilder.CreateTable(
                name: "MidMoodTense",
                columns: table => new
                {
                    Oid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    LinkTense = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LinkMood = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MidMoodTense", x => x.Oid);
                    table.ForeignKey(
                        name: "FK_MidMoodTense_Mood_LinkMood",
                        column: x => x.LinkMood,
                        principalTable: "Mood",
                        principalColumn: "Oid");
                    table.ForeignKey(
                        name: "FK_MidMoodTense_Tense_LinkTense",
                        column: x => x.LinkTense,
                        principalTable: "Tense",
                        principalColumn: "Oid");
                });

            migrationBuilder.CreateTable(
                name: "MidMoodPerson",
                columns: table => new
                {
                    Oid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    LinkVerbPerson = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LinkMood = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MidMoodPerson", x => x.Oid);
                    table.ForeignKey(
                        name: "FK_MidMoodPerson_Mood_LinkMood",
                        column: x => x.LinkMood,
                        principalTable: "Mood",
                        principalColumn: "Oid");
                    table.ForeignKey(
                        name: "FK_MidMoodPerson_VerbPerson_LinkVerbPerson",
                        column: x => x.LinkVerbPerson,
                        principalTable: "VerbPerson",
                        principalColumn: "Oid");
                });

            migrationBuilder.CreateTable(
                name: "Conjugation",
                columns: table => new
                {
                    Oid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Form = table.Column<string>(type: "nvarchar(30)", nullable: true),
                    LinkVerb = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LinkTense = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LinkVerbPerson = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LinkMood = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MidMoodPersonOid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MidMoodTenseOid = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conjugation", x => x.Oid);
                    table.ForeignKey(
                        name: "FK_Conjugation_MidMoodPerson_MidMoodPersonOid",
                        column: x => x.MidMoodPersonOid,
                        principalTable: "MidMoodPerson",
                        principalColumn: "Oid");
                    table.ForeignKey(
                        name: "FK_Conjugation_MidMoodTense_MidMoodTenseOid",
                        column: x => x.MidMoodTenseOid,
                        principalTable: "MidMoodTense",
                        principalColumn: "Oid");
                    table.ForeignKey(
                        name: "FK_Conjugation_Mood_LinkMood",
                        column: x => x.LinkMood,
                        principalTable: "Mood",
                        principalColumn: "Oid");
                    table.ForeignKey(
                        name: "FK_Conjugation_Tense_LinkTense",
                        column: x => x.LinkTense,
                        principalTable: "Tense",
                        principalColumn: "Oid");
                    table.ForeignKey(
                        name: "FK_Conjugation_VerbPerson_LinkVerbPerson",
                        column: x => x.LinkVerbPerson,
                        principalTable: "VerbPerson",
                        principalColumn: "Oid");
                    table.ForeignKey(
                        name: "FK_Conjugation_Verb_LinkVerb",
                        column: x => x.LinkVerb,
                        principalTable: "Verb",
                        principalColumn: "Oid");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Conjugation_LinkMood",
                table: "Conjugation",
                column: "LinkMood");

            migrationBuilder.CreateIndex(
                name: "IX_Conjugation_LinkTense",
                table: "Conjugation",
                column: "LinkTense");

            migrationBuilder.CreateIndex(
                name: "IX_Conjugation_LinkVerb",
                table: "Conjugation",
                column: "LinkVerb");

            migrationBuilder.CreateIndex(
                name: "IX_Conjugation_LinkVerbPerson",
                table: "Conjugation",
                column: "LinkVerbPerson");

            migrationBuilder.CreateIndex(
                name: "IX_Conjugation_MidMoodPersonOid",
                table: "Conjugation",
                column: "MidMoodPersonOid");

            migrationBuilder.CreateIndex(
                name: "IX_Conjugation_MidMoodTenseOid",
                table: "Conjugation",
                column: "MidMoodTenseOid");

            migrationBuilder.CreateIndex(
                name: "IX_MidMoodPerson_LinkMood",
                table: "MidMoodPerson",
                column: "LinkMood");

            migrationBuilder.CreateIndex(
                name: "IX_MidMoodPerson_LinkVerbPerson",
                table: "MidMoodPerson",
                column: "LinkVerbPerson");

            migrationBuilder.CreateIndex(
                name: "IX_MidMoodTense_LinkMood",
                table: "MidMoodTense",
                column: "LinkMood");

            migrationBuilder.CreateIndex(
                name: "IX_MidMoodTense_LinkTense",
                table: "MidMoodTense",
                column: "LinkTense");

            migrationBuilder.CreateIndex(
                name: "IX_Mood_LinkLanguage",
                table: "Mood",
                column: "LinkLanguage");

            migrationBuilder.CreateIndex(
                name: "IX_Tense_LinkLanguage",
                table: "Tense",
                column: "LinkLanguage");

            migrationBuilder.CreateIndex(
                name: "IX_Verb_LinkLanguage",
                table: "Verb",
                column: "LinkLanguage");

            migrationBuilder.CreateIndex(
                name: "IX_VerbPerson_LinkLanguage",
                table: "VerbPerson",
                column: "LinkLanguage");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Conjugation");

            migrationBuilder.DropTable(
                name: "DeutschAdjektiv");

            migrationBuilder.DropTable(
                name: "MidMoodPerson");

            migrationBuilder.DropTable(
                name: "MidMoodTense");

            migrationBuilder.DropTable(
                name: "Verb");

            migrationBuilder.DropTable(
                name: "VerbPerson");

            migrationBuilder.DropTable(
                name: "Mood");

            migrationBuilder.DropTable(
                name: "Tense");

            migrationBuilder.DropTable(
                name: "Language");
        }
    }
}
