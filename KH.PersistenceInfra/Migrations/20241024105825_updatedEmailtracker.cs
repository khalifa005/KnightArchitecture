using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KH.PersistenceInfra.Migrations
{
    /// <inheritdoc />
    public partial class updatedEmailtracker : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ScheduleSendDate",
                table: "EmailTracker",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Calendar",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScheduleSendDate",
                table: "EmailTracker");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Calendar",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
