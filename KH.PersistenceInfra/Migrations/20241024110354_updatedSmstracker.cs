using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KH.PersistenceInfra.Migrations
{
    /// <inheritdoc />
    public partial class updatedSmstracker : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ScheduleSendDate",
                table: "SmsTracker",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScheduleSendDate",
                table: "SmsTracker");
        }
    }
}
