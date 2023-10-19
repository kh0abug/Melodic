using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Melodic.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Speakers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Percent",
                table: "EVouchers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.UpdateData(
                table: "EVouchers",
                keyColumn: "Id",
                keyValue: 1,
                column: "Percent",
                value: 0.5);

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
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Speakers");

            migrationBuilder.DropColumn(
                name: "Percent",
                table: "EVouchers");
        }
    }
}
