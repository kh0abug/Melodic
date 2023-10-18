using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Melodic.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddQuantityField : Migration
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Speakers");
        }
    }
}
