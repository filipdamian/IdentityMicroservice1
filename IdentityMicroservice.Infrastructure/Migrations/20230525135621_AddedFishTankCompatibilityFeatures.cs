using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityMicroservice.Infrastructure.Migrations
{
    public partial class AddedFishTankCompatibilityFeatures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FishSpecs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Species = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WaterType = table.Column<int>(type: "int", nullable: false),
                    Diet = table.Column<int>(type: "int", nullable: false),
                    WaterAcidity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WaterTemperature = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FishSize = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FishSpecs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FishTanks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Lenght = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Height = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Width = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Volume = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FishTanks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FishTanks_IdentityUser_UserId",
                        column: x => x.UserId,
                        principalTable: "IdentityUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PetFish",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FishName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetFish", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FishSpecsFishTank",
                columns: table => new
                {
                    FishSpecsId = table.Column<int>(type: "int", nullable: false),
                    FishTanksId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FishSpecsFishTank", x => new { x.FishSpecsId, x.FishTanksId });
                    table.ForeignKey(
                        name: "FK_FishSpecsFishTank_FishSpecs_FishSpecsId",
                        column: x => x.FishSpecsId,
                        principalTable: "FishSpecs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FishSpecsFishTank_FishTanks_FishTanksId",
                        column: x => x.FishTanksId,
                        principalTable: "FishTanks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FishTankPetFish",
                columns: table => new
                {
                    FishTanksId = table.Column<int>(type: "int", nullable: false),
                    PetFishId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FishTankPetFish", x => new { x.FishTanksId, x.PetFishId });
                    table.ForeignKey(
                        name: "FK_FishTankPetFish_FishTanks_FishTanksId",
                        column: x => x.FishTanksId,
                        principalTable: "FishTanks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FishTankPetFish_PetFish_PetFishId",
                        column: x => x.PetFishId,
                        principalTable: "PetFish",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FishSpecsFishTank_FishTanksId",
                table: "FishSpecsFishTank",
                column: "FishTanksId");

            migrationBuilder.CreateIndex(
                name: "IX_FishTankPetFish_PetFishId",
                table: "FishTankPetFish",
                column: "PetFishId");

            migrationBuilder.CreateIndex(
                name: "IX_FishTanks_UserId",
                table: "FishTanks",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FishSpecsFishTank");

            migrationBuilder.DropTable(
                name: "FishTankPetFish");

            migrationBuilder.DropTable(
                name: "FishSpecs");

            migrationBuilder.DropTable(
                name: "FishTanks");

            migrationBuilder.DropTable(
                name: "PetFish");
        }
    }
}
