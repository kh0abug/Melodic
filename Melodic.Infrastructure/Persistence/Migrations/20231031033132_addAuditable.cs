using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Melodic.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addAuditable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Speakers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Speakers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "Speakers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "Speakers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "Speakers");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Speakers");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "Speakers");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "Speakers");
        }
    }
}
