using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuppGamesBack.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAtivo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FavoriteGames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    e_favorite = table.Column<bool>(type: "bit", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Platform = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Excluded = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteGames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FavoriteGames_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FavoriteGames_Users_UserId1",
                        column: x => x.UserId1,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Annotations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FavoriteGameId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreateDateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Excluded = table.Column<bool>(type: "bit", nullable: false),
                    FavoriteGameId1 = table.Column<int>(type: "int", nullable: true),
                    UserId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Annotations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Annotations_FavoriteGames_FavoriteGameId",
                        column: x => x.FavoriteGameId,
                        principalTable: "FavoriteGames",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Annotations_FavoriteGames_FavoriteGameId1",
                        column: x => x.FavoriteGameId1,
                        principalTable: "FavoriteGames",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Annotations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Annotations_Users_UserId1",
                        column: x => x.UserId1,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Annotations_FavoriteGameId",
                table: "Annotations",
                column: "FavoriteGameId");

            migrationBuilder.CreateIndex(
                name: "IX_Annotations_FavoriteGameId1",
                table: "Annotations",
                column: "FavoriteGameId1");

            migrationBuilder.CreateIndex(
                name: "IX_Annotations_UserId",
                table: "Annotations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Annotations_UserId1",
                table: "Annotations",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteGames_UserId",
                table: "FavoriteGames",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteGames_UserId1",
                table: "FavoriteGames",
                column: "UserId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Annotations");

            migrationBuilder.DropTable(
                name: "FavoriteGames");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
