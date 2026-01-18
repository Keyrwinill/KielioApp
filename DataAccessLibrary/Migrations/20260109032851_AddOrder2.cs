using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLibrary.Migrations
{
    /// <inheritdoc />
    public partial class AddOrder2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Order",
                table: "VerbPerson",
                newName: "SortOrder");

            migrationBuilder.RenameColumn(
                name: "Order",
                table: "Mood",
                newName: "SortOrder");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SortOrder",
                table: "VerbPerson",
                newName: "Order");

            migrationBuilder.RenameColumn(
                name: "SortOrder",
                table: "Mood",
                newName: "Order");
        }
    }
}
