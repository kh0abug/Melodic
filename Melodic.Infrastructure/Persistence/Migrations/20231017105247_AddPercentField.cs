using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Melodic.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPercentField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "EVouchers",
                keyColumn: "Id",
                keyValue: 1,
                column: "Percent",
                value: 0.05);

            migrationBuilder.UpdateData(
                table: "EVouchers",
                keyColumn: "Id",
                keyValue: 2,
                column: "Percent",
                value: 0.10000000000000001);

            migrationBuilder.UpdateData(
                table: "EVouchers",
                keyColumn: "Id",
                keyValue: 3,
                column: "Percent",
                value: 0.14999999999999999);

            migrationBuilder.UpdateData(
                table: "EVouchers",
                keyColumn: "Id",
                keyValue: 4,
                column: "Percent",
                value: 0.20000000000000001);

            migrationBuilder.UpdateData(
                table: "EVouchers",
                keyColumn: "Id",
                keyValue: 5,
                column: "Percent",
                value: 0.25);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "EVouchers",
                keyColumn: "Id",
                keyValue: 1,
                column: "Percent",
                value: 5.0);

            migrationBuilder.UpdateData(
                table: "EVouchers",
                keyColumn: "Id",
                keyValue: 2,
                column: "Percent",
                value: 10.0);

            migrationBuilder.UpdateData(
                table: "EVouchers",
                keyColumn: "Id",
                keyValue: 3,
                column: "Percent",
                value: 15.0);

            migrationBuilder.UpdateData(
                table: "EVouchers",
                keyColumn: "Id",
                keyValue: 4,
                column: "Percent",
                value: 20.0);

            migrationBuilder.UpdateData(
                table: "EVouchers",
                keyColumn: "Id",
                keyValue: 5,
                column: "Percent",
                value: 25.0);
        }
    }
}
