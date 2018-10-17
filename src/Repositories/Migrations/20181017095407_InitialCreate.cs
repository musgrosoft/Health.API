using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Repositories.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActivitySummaries",
                columns: table => new
                {
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    SedentaryMinutes = table.Column<int>(nullable: false),
                    LightlyActiveMinutes = table.Column<int>(nullable: false),
                    FairlyActiveMinutes = table.Column<int>(nullable: false),
                    VeryActiveMinutes = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivitySummaries", x => x.CreatedDate);
                });

            migrationBuilder.CreateTable(
                name: "AlcoholIntakes",
                columns: table => new
                {
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Units = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlcoholIntakes", x => x.CreatedDate);
                });

            migrationBuilder.CreateTable(
                name: "BloodPressures",
                columns: table => new
                {
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Diastolic = table.Column<double>(nullable: true),
                    Systolic = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BloodPressures", x => x.CreatedDate);
                });

            migrationBuilder.CreateTable(
                name: "Ergos",
                columns: table => new
                {
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Time = table.Column<TimeSpan>(nullable: false),
                    Metres = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ergos", x => x.CreatedDate);
                });

            migrationBuilder.CreateTable(
                name: "HeartRateSummaries",
                columns: table => new
                {
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    OutOfRangeMinutes = table.Column<int>(nullable: true),
                    FatBurnMinutes = table.Column<int>(nullable: true),
                    CardioMinutes = table.Column<int>(nullable: true),
                    PeakMinutes = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeartRateSummaries", x => x.CreatedDate);
                });

            migrationBuilder.CreateTable(
                name: "RestingHeartRates",
                columns: table => new
                {
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Beats = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestingHeartRates", x => x.CreatedDate);
                });

            migrationBuilder.CreateTable(
                name: "Runs",
                columns: table => new
                {
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Time = table.Column<TimeSpan>(nullable: false),
                    Metres = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Runs", x => x.CreatedDate);
                });

            migrationBuilder.CreateTable(
                name: "StepCounts",
                columns: table => new
                {
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Count = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StepCounts", x => x.CreatedDate);
                });

            migrationBuilder.CreateTable(
                name: "Tokens",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tokens", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Weights",
                columns: table => new
                {
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Kg = table.Column<double>(nullable: true),
                    FatRatioPercentage = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weights", x => x.CreatedDate);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivitySummaries");

            migrationBuilder.DropTable(
                name: "AlcoholIntakes");

            migrationBuilder.DropTable(
                name: "BloodPressures");

            migrationBuilder.DropTable(
                name: "Ergos");

            migrationBuilder.DropTable(
                name: "HeartRateSummaries");

            migrationBuilder.DropTable(
                name: "RestingHeartRates");

            migrationBuilder.DropTable(
                name: "Runs");

            migrationBuilder.DropTable(
                name: "StepCounts");

            migrationBuilder.DropTable(
                name: "Tokens");

            migrationBuilder.DropTable(
                name: "Weights");
        }
    }
}
