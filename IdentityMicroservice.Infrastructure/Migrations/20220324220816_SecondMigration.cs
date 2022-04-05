using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityMicroservice.Infrastructure.Migrations
{
    public partial class SecondMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Screen",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChangedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChangeDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ScreenCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ScreenName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Start = table.Column<DateTime>(type: "datetime2", nullable: false),
                    End = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Screen", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WOType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChangedByUserId = table.Column<Guid>(type: "uniqueidentifier", maxLength: 50, nullable: false),
                    ChangeDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WOTypeCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    WOTypeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DurationMinutes = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WOType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WOTypeScenario",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WOTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScenarioCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ScenarioName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    Start = table.Column<DateTime>(type: "datetime2", nullable: false),
                    End = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WOTypeScenario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WOTypeScenario_WOType_WOTypeId",
                        column: x => x.WOTypeId,
                        principalTable: "WOType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WOTypeScenarioStep",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StepNumber = table.Column<int>(type: "int", nullable: false),
                    StepName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ScreenCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    WOScenarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScreenId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WOTypeScenarioStep", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WOTypeScenarioStep_Screen_ScreenId",
                        column: x => x.ScreenId,
                        principalTable: "Screen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WOTypeScenarioStep_WOTypeScenario_WOScenarioId",
                        column: x => x.WOScenarioId,
                        principalTable: "WOTypeScenario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WOTypeScenario_WOTypeId",
                table: "WOTypeScenario",
                column: "WOTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_WOTypeScenarioStep_ScreenId",
                table: "WOTypeScenarioStep",
                column: "ScreenId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WOTypeScenarioStep_WOScenarioId",
                table: "WOTypeScenarioStep",
                column: "WOScenarioId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WOTypeScenarioStep");

            migrationBuilder.DropTable(
                name: "Screen");

            migrationBuilder.DropTable(
                name: "WOTypeScenario");

            migrationBuilder.DropTable(
                name: "WOType");
        }
    }
}
