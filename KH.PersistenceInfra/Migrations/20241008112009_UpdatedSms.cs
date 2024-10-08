using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KH.PersistenceInfra.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedSms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "SmsTemplates");

            migrationBuilder.AlterColumn<long>(
                name: "ModelId",
                table: "SmsTracker",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "SmsType",
                table: "SmsTemplates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "SmsTemplates",
                columns: new[] { "Id", "CreatedById", "CreatedDate", "DeletedById", "DeletedDate", "IsDeleted", "SmsType", "TextAr", "TextEn", "UpdatedById", "UpdatedDate" },
                values: new object[,]
                {
                    { 1L, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "WelcomeUser", "مرحبًا {FirstName}، رمز التحقق الخاص بك هو {OtpCode}.", "Welcome {FirstName}, your OTP code is {OtpCode}.", null, null },
                    { 2L, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, "OTP", "مرحبًا {Username}، استخدم هذا الرمز {OtpCode} لإعادة تعيين كلمة المرور.", "Hello {Username}, use this OTP {OtpCode} to reset your password.", null, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SmsTemplates",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "SmsTemplates",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DropColumn(
                name: "SmsType",
                table: "SmsTemplates");

            migrationBuilder.AlterColumn<int>(
                name: "ModelId",
                table: "SmsTracker",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "SmsTemplates",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
