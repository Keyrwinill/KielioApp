using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLibrary.Migrations
{
    /// <inheritdoc />
    public partial class ModifyStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Conjugation_MidMoodPerson_MidMoodPersonOid",
                table: "Conjugation");

            migrationBuilder.DropForeignKey(
                name: "FK_Conjugation_MidMoodTense_MidMoodTenseOid",
                table: "Conjugation");

            migrationBuilder.DropForeignKey(
                name: "FK_Conjugation_Mood_LinkMood",
                table: "Conjugation");

            migrationBuilder.DropForeignKey(
                name: "FK_Conjugation_Tense_LinkTense",
                table: "Conjugation");

            migrationBuilder.DropForeignKey(
                name: "FK_Conjugation_VerbPerson_LinkVerbPerson",
                table: "Conjugation");

            migrationBuilder.DropIndex(
                name: "IX_Conjugation_LinkMood",
                table: "Conjugation");

            migrationBuilder.DropIndex(
                name: "IX_Conjugation_MidMoodPersonOid",
                table: "Conjugation");

            migrationBuilder.DropIndex(
                name: "IX_Conjugation_MidMoodTenseOid",
                table: "Conjugation");

            migrationBuilder.DropColumn(
                name: "LinkMood",
                table: "Conjugation");

            migrationBuilder.DropColumn(
                name: "MidMoodPersonOid",
                table: "Conjugation");

            migrationBuilder.DropColumn(
                name: "MidMoodTenseOid",
                table: "Conjugation");

            migrationBuilder.RenameColumn(
                name: "LinkVerbPerson",
                table: "Conjugation",
                newName: "LinkMidMoodTense");

            migrationBuilder.RenameColumn(
                name: "LinkTense",
                table: "Conjugation",
                newName: "LinkMidMoodPerson");

            migrationBuilder.RenameIndex(
                name: "IX_Conjugation_LinkVerbPerson",
                table: "Conjugation",
                newName: "IX_Conjugation_LinkMidMoodTense");

            migrationBuilder.RenameIndex(
                name: "IX_Conjugation_LinkTense",
                table: "Conjugation",
                newName: "IX_Conjugation_LinkMidMoodPerson");

            migrationBuilder.AddColumn<string>(
                name: "MoodType",
                table: "MidMoodTense",
                type: "nvarchar(30)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenseName",
                table: "MidMoodTense",
                type: "nvarchar(30)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MoodType",
                table: "MidMoodPerson",
                type: "nvarchar(30)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PersonType",
                table: "MidMoodPerson",
                type: "nvarchar(30)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Conjugation_MidMoodPerson_LinkMidMoodPerson",
                table: "Conjugation",
                column: "LinkMidMoodPerson",
                principalTable: "MidMoodPerson",
                principalColumn: "Oid");

            migrationBuilder.AddForeignKey(
                name: "FK_Conjugation_MidMoodTense_LinkMidMoodTense",
                table: "Conjugation",
                column: "LinkMidMoodTense",
                principalTable: "MidMoodTense",
                principalColumn: "Oid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Conjugation_MidMoodPerson_LinkMidMoodPerson",
                table: "Conjugation");

            migrationBuilder.DropForeignKey(
                name: "FK_Conjugation_MidMoodTense_LinkMidMoodTense",
                table: "Conjugation");

            migrationBuilder.DropColumn(
                name: "MoodType",
                table: "MidMoodTense");

            migrationBuilder.DropColumn(
                name: "TenseName",
                table: "MidMoodTense");

            migrationBuilder.DropColumn(
                name: "MoodType",
                table: "MidMoodPerson");

            migrationBuilder.DropColumn(
                name: "PersonType",
                table: "MidMoodPerson");

            migrationBuilder.RenameColumn(
                name: "LinkMidMoodTense",
                table: "Conjugation",
                newName: "LinkVerbPerson");

            migrationBuilder.RenameColumn(
                name: "LinkMidMoodPerson",
                table: "Conjugation",
                newName: "LinkTense");

            migrationBuilder.RenameIndex(
                name: "IX_Conjugation_LinkMidMoodTense",
                table: "Conjugation",
                newName: "IX_Conjugation_LinkVerbPerson");

            migrationBuilder.RenameIndex(
                name: "IX_Conjugation_LinkMidMoodPerson",
                table: "Conjugation",
                newName: "IX_Conjugation_LinkTense");

            migrationBuilder.AddColumn<Guid>(
                name: "LinkMood",
                table: "Conjugation",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "MidMoodPersonOid",
                table: "Conjugation",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MidMoodTenseOid",
                table: "Conjugation",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Conjugation_LinkMood",
                table: "Conjugation",
                column: "LinkMood");

            migrationBuilder.CreateIndex(
                name: "IX_Conjugation_MidMoodPersonOid",
                table: "Conjugation",
                column: "MidMoodPersonOid");

            migrationBuilder.CreateIndex(
                name: "IX_Conjugation_MidMoodTenseOid",
                table: "Conjugation",
                column: "MidMoodTenseOid");

            migrationBuilder.AddForeignKey(
                name: "FK_Conjugation_MidMoodPerson_MidMoodPersonOid",
                table: "Conjugation",
                column: "MidMoodPersonOid",
                principalTable: "MidMoodPerson",
                principalColumn: "Oid");

            migrationBuilder.AddForeignKey(
                name: "FK_Conjugation_MidMoodTense_MidMoodTenseOid",
                table: "Conjugation",
                column: "MidMoodTenseOid",
                principalTable: "MidMoodTense",
                principalColumn: "Oid");

            migrationBuilder.AddForeignKey(
                name: "FK_Conjugation_Mood_LinkMood",
                table: "Conjugation",
                column: "LinkMood",
                principalTable: "Mood",
                principalColumn: "Oid");

            migrationBuilder.AddForeignKey(
                name: "FK_Conjugation_Tense_LinkTense",
                table: "Conjugation",
                column: "LinkTense",
                principalTable: "Tense",
                principalColumn: "Oid");

            migrationBuilder.AddForeignKey(
                name: "FK_Conjugation_VerbPerson_LinkVerbPerson",
                table: "Conjugation",
                column: "LinkVerbPerson",
                principalTable: "VerbPerson",
                principalColumn: "Oid");
        }
    }
}
