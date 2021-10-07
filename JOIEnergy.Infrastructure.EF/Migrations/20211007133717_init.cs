using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace JOIEnergy.Infrastructure.EF.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EnergyCompanies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Supplier = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnergyCompanies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SmartMeters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SmartMeterId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EnergyCompanyPricePlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmartMeters", x => x.Id);
                    table.UniqueConstraint("AK_SmartMeters_SmartMeterId", x => x.SmartMeterId);
                });

            migrationBuilder.CreateTable(
                name: "PricePlans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EnergyCompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitRate = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PricePlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PricePlans_EnergyCompanies_EnergyCompanyId",
                        column: x => x.EnergyCompanyId,
                        principalTable: "EnergyCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ElectricityReadings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SmartMeterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reading = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElectricityReadings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ElectricityReadings_SmartMeters_SmartMeterId",
                        column: x => x.SmartMeterId,
                        principalTable: "SmartMeters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PeakTimeMultipliers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PricePlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    Multiplier = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeakTimeMultipliers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PeakTimeMultipliers_PricePlans_PricePlanId",
                        column: x => x.PricePlanId,
                        principalTable: "PricePlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ElectricityReadings_SmartMeterId",
                table: "ElectricityReadings",
                column: "SmartMeterId");

            migrationBuilder.CreateIndex(
                name: "IX_PeakTimeMultipliers_PricePlanId",
                table: "PeakTimeMultipliers",
                column: "PricePlanId");

            migrationBuilder.CreateIndex(
                name: "IX_PricePlans_EnergyCompanyId",
                table: "PricePlans",
                column: "EnergyCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartMeters_SmartMeterId",
                table: "SmartMeters",
                column: "SmartMeterId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "ElectricityReadings");

            migrationBuilder.DropTable(
                name: "PeakTimeMultipliers");

            migrationBuilder.DropTable(
                name: "SmartMeters");

            migrationBuilder.DropTable(
                name: "PricePlans");

            migrationBuilder.DropTable(
                name: "EnergyCompanies");
        }
    }
}
