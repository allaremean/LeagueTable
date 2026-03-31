using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LaLiga.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    TeamID = table.Column<string>(type: "char(3)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Stadium = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.TeamID);
                });

            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    MatchID = table.Column<string>(type: "char(6)", nullable: false),
                    HomeTeamID = table.Column<string>(type: "char(3)", nullable: false),
                    AwayTeamID = table.Column<string>(type: "char(3)", nullable: false),
                    HomeScore = table.Column<int>(type: "int", nullable: false),
                    AwayScore = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.MatchID);
                    table.ForeignKey(
                        name: "FK_Matches_Teams_AwayTeamID",
                        column: x => x.AwayTeamID,
                        principalTable: "Teams",
                        principalColumn: "TeamID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Matches_Teams_HomeTeamID",
                        column: x => x.HomeTeamID,
                        principalTable: "Teams",
                        principalColumn: "TeamID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Teams",
                columns: new[] { "TeamID", "City", "Name", "Stadium" },
                values: new object[,]
                {
                    { "ATM", "Madrid", "Atletico Madrid", "Wanda Metropolitano" },
                    { "FCB", "Barcelona", "FC Barcelona", "Spotify Camp Nou" },
                    { "RMA", "Madrid", "Real Madrid", "Esatdio Santiago Bernabeu" }
                });

            migrationBuilder.InsertData(
                table: "Matches",
                columns: new[] { "MatchID", "AwayScore", "AwayTeamID", "HomeScore", "HomeTeamID" },
                values: new object[,]
                {
                    { "ATMFCB", 0, "FCB", 4, "ATM" },
                    { "RMAFCB", 1, "FCB", 2, "RMA" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Matches_AwayTeamID",
                table: "Matches",
                column: "AwayTeamID");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_HomeTeamID",
                table: "Matches",
                column: "HomeTeamID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropTable(
                name: "Teams");
        }
    }
}
