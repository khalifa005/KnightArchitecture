using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KH.PersistenceInfra.Migrations
{
    /// <inheritdoc />
    public partial class refactoringpropsnaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CorrelationId",
                table: "AuditTrails",
                newName: "RequestId");

            migrationBuilder.AddColumn<long>(
                name: "CreatedById",
                table: "EmailTracker",
                type: "bigint",
                nullable: true)
                .Annotation("Relational:ColumnOrder", 102);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "EmailTracker",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified))
                .Annotation("Relational:ColumnOrder", 101);

            migrationBuilder.AddColumn<long>(
                name: "DeletedById",
                table: "EmailTracker",
                type: "bigint",
                nullable: true)
                .Annotation("Relational:ColumnOrder", 107);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "EmailTracker",
                type: "datetime2",
                nullable: true)
                .Annotation("Relational:ColumnOrder", 106);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "EmailTracker",
                type: "bit",
                nullable: false,
                defaultValue: false)
                .Annotation("Relational:ColumnOrder", 105);

            migrationBuilder.AddColumn<long>(
                name: "UpdatedById",
                table: "EmailTracker",
                type: "bigint",
                nullable: true)
                .Annotation("Relational:ColumnOrder", 104);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "EmailTracker",
                type: "datetime2",
                nullable: true)
                .Annotation("Relational:ColumnOrder", 103);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AuditTrails",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "OldValues",
                table: "AuditTrails",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "NewValues",
                table: "AuditTrails",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "EmailTracker");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "EmailTracker");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "EmailTracker");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "EmailTracker");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "EmailTracker");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "EmailTracker");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "EmailTracker");

            migrationBuilder.RenameColumn(
                name: "RequestId",
                table: "AuditTrails",
                newName: "CorrelationId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AuditTrails",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OldValues",
                table: "AuditTrails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NewValues",
                table: "AuditTrails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
