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
            migrationBuilder.CreateTable(
                name: "BillProducts",
                columns: table => new
                {
                    SpeakerId = table.Column<int>(type: "int", nullable: false),
                    BillId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillProducts", x => new { x.BillId, x.SpeakerId });
                });

            migrationBuilder.CreateTable(
                name: "Bills",
                columns: table => new
                {
                    BuildId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tax = table.Column<double>(type: "float", nullable: true),
                    Discount = table.Column<double>(type: "float", nullable: true),
                    Total = table.Column<double>(type: "float", nullable: true),
                    TotalPrice = table.Column<double>(type: "float", nullable: true),
                    Payment = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bills", x => x.BuildId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BillProducts");

            migrationBuilder.DropTable(
                name: "Bills");
        }
    }
}
