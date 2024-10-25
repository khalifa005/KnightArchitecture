using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KH.PersistenceInfra.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastAssignDateAsAssignTo",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastAssignDateAsCaseOwner",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastAssignDateAsSupervisor",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastAssignDateAsAssignTo",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastAssignDateAsCaseOwner",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastAssignDateAsSupervisor",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "LastAssignDateAsAssignTo", "LastAssignDateAsCaseOwner", "LastAssignDateAsSupervisor" },
                values: new object[] { null, null, null });
        }
    }
}
