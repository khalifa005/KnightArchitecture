using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KH.PersistenceInfra.Migrations
{
    /// <inheritdoc />
    public partial class seededsuperadminper : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "CreatedById", "CreatedDate", "DeletedById", "DeletedDate", "DependOnId", "Description", "IsDeleted", "Key", "NameAr", "NameEn", "ParentId", "SortKey", "UpdatedById", "UpdatedDate" },
                values: new object[] { 700L, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, false, "super-admin", "المدير للنظام ", "Super admin", null, 1, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "Id", "CreatedById", "CreatedDate", "DeletedById", "DeletedDate", "IsDeleted", "PermissionId", "RoleId", "UpdatedById", "UpdatedDate" },
                values: new object[] { 2000L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, false, 700L, 1L, null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2000L);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 700L);
        }
    }
}
