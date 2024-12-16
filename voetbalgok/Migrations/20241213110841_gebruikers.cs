using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace voetbalgok.Migrations
{
    /// <inheritdoc />
    public partial class gebruikers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Naam",
                table: "Gebruikers",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Naam",
                table: "Gebruikers");
        }
    }
}
