using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaLiga.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "HomeTeamID",
                table: "Matches",
                type: "char(3)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(3)");

            migrationBuilder.AlterColumn<string>(
                name: "AwayTeamID",
                table: "Matches",
                type: "char(3)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(3)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "HomeTeamID",
                table: "Matches",
                type: "char(3)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "char(3)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AwayTeamID",
                table: "Matches",
                type: "char(3)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "char(3)",
                oldNullable: true);
        }
    }
}
