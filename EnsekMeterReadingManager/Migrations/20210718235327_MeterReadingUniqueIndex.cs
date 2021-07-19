using Microsoft.EntityFrameworkCore.Migrations;

namespace EnsekMeterReadingManager.Migrations
{
    public partial class MeterReadingUniqueIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MeterReadings_AccountId",
                table: "MeterReadings");

            migrationBuilder.CreateIndex(
                name: "IX_MeterReadings_AccountId_MeterReadingDateTime",
                table: "MeterReadings",
                columns: new[] { "AccountId", "MeterReadingDateTime" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MeterReadings_AccountId_MeterReadingDateTime",
                table: "MeterReadings");

            migrationBuilder.CreateIndex(
                name: "IX_MeterReadings_AccountId",
                table: "MeterReadings",
                column: "AccountId");
        }
    }
}
