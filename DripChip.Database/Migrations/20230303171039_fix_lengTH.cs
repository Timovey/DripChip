using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DripChip.Database.Migrations
{
    /// <inheritdoc />
    public partial class fix_lengTH : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Lenght",
                table: "Animals",
                newName: "Length");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Length",
                table: "Animals",
                newName: "Lenght");
        }
    }
}
