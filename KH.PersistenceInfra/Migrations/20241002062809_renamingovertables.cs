using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KH.PersistenceInfra.Migrations
{
    /// <inheritdoc />
    public partial class renamingovertables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleFunctions");

            migrationBuilder.DropTable(
                name: "SystemFunctions");

            migrationBuilder.CreateTable(
                name: "SystemActions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentID = table.Column<long>(type: "bigint", nullable: true),
                    SortKey = table.Column<int>(type: "int", nullable: false),
                    DependOnID = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SystemActions_SystemActions_ParentID",
                        column: x => x.ParentID,
                        principalTable: "SystemActions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SystemActionsId = table.Column<long>(type: "bigint", nullable: false),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RolePermissions_SystemActions_SystemActionsId",
                        column: x => x.SystemActionsId,
                        principalTable: "SystemActions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "SystemActions",
                columns: new[] { "Id", "CreatedById", "CreatedDate", "DeletedById", "DeletedDate", "DependOnID", "Description", "IsDeleted", "NameAr", "NameEn", "ParentID", "SortKey", "UpdatedById", "UpdatedDate" },
                values: new object[,]
                {
                    { 1L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "البيانات الاساسية", "lookups", null, 1, null, null },
                    { 69L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "الإدارة", "user-management", null, 2, null, null },
                    { 78L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "الخدمة الاولى", "servic-one", null, 3, null, null },
                    { 86L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "العملاء", "Customers", null, 4, null, null },
                    { 2L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "فئات الطلبات", "categories", 1L, 1, null, null },
                    { 6L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "حالات الطلبات", "status", 1L, 2, null, null },
                    { 18L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "المدن", "cities", 1L, 5, null, null },
                    { 22L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "المجموعات", "groups", 1L, 6, null, null },
                    { 30L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "الأدوار", "roles", 1L, 8, null, null },
                    { 33L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "الإدارات والأقسام", "departments", 1L, 9, null, null },
                    { 70L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "إدارة أدوار المستخدمين", "roles-management", 69L, 1, null, null },
                    { 71L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "المستخدمين", "users", 69L, 2, null, null },
                    { 75L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "التورايخ", "calender", 69L, 3, null, null },
                    { 80L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "إضافة شكوى او طلب", "add-service-one", 78L, 2, null, null },
                    { 81L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "تعديل شكوى او طلب", "edit-service-one", 78L, 3, null, null },
                    { 82L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "تعديل شكوى او طلب", "close-service-one", 78L, 4, null, null },
                    { 84L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "تغيير فئة شكوى او طلب", "change-category-service-one", 78L, 6, null, null },
                    { 85L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "إضافة تعليق على شكوى او طلب", "add-comment-service-one", 78L, 7, null, null },
                    { 87L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "إضافة عميل", "add-Customer", 86L, 1, null, null },
                    { 88L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "تعديل عميل", "edit-Customer", 86L, 2, null, null },
                    { 89L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "حذف عميل", "delete-Customer", 86L, 3, null, null },
                    { 3L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "إضافة فئة طلب", "add-category", 2L, 1, null, null },
                    { 4L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "تعديل فئة طلب", "edit-category", 2L, 2, null, null },
                    { 5L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "حذف فئة طلب", "delete-category", 2L, 3, null, null },
                    { 8L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "تعديل حالة طلب", "edit-status", 6L, 2, null, null },
                    { 9L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "حذف حالة طلب", "delete-status", 6L, 3, null, null },
                    { 19L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "إضافة مدينه", "add-city", 18L, 1, null, null },
                    { 20L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "تعديل مدينه", "edit-city", 18L, 2, null, null },
                    { 21L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "حذف مدينه", "delete-city", 18L, 3, null, null },
                    { 23L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "إضافة مجموعه", "add-group", 22L, 1, null, null },
                    { 24L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "تعديل مجموعه", "edit-group", 22L, 2, null, null },
                    { 25L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "حذف مجموعه", "delete-group", 22L, 3, null, null },
                    { 31L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "تعديل دور مستخدم", "edit-role", 30L, 1, null, null },
                    { 32L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "حذف دور مستخدم", "delete-role", 30L, 2, null, null },
                    { 34L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "إضافة قسم", "add-department", 33L, 1, null, null },
                    { 35L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "تعديل قسم", "edit-department", 33L, 2, null, null },
                    { 36L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "حذف قسم", "delete-department", 33L, 3, null, null },
                    { 72L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, " إضافة مستخدم", "add-user", 71L, 1, null, null },
                    { 73L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "تعديل مستخدم ", "edit-user", 71L, 2, null, null },
                    { 74L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "حذف مستخدم ", "delete-user", 71L, 3, null, null },
                    { 76L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, " إضافة تاريخ اجازة", "add-calender-holiday", 75L, 1, null, null },
                    { 77L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "تعديل تاريخ اجازة ", "edit-calender-holiday", 75L, 2, null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_RoleId",
                table: "RolePermissions",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_SystemActionsId",
                table: "RolePermissions",
                column: "SystemActionsId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemActions_ParentID",
                table: "SystemActions",
                column: "ParentID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "SystemActions");

            migrationBuilder.CreateTable(
                name: "SystemFunctions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentID = table.Column<long>(type: "bigint", nullable: true),
                    SortKey = table.Column<int>(type: "int", nullable: false),
                    DependOnID = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemFunctions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SystemFunctions_SystemFunctions_ParentID",
                        column: x => x.ParentID,
                        principalTable: "SystemFunctions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RoleFunctions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SystemFunctionId = table.Column<long>(type: "bigint", nullable: false),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleFunctions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleFunctions_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RoleFunctions_SystemFunctions_SystemFunctionId",
                        column: x => x.SystemFunctionId,
                        principalTable: "SystemFunctions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "SystemFunctions",
                columns: new[] { "Id", "CreatedById", "CreatedDate", "DeletedById", "DeletedDate", "DependOnID", "Description", "IsDeleted", "NameAr", "NameEn", "ParentID", "SortKey", "UpdatedById", "UpdatedDate" },
                values: new object[,]
                {
                    { 1L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "البيانات الاساسية", "lookups", null, 1, null, null },
                    { 69L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "الإدارة", "user-management", null, 2, null, null },
                    { 78L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "الخدمة الاولى", "servic-one", null, 3, null, null },
                    { 86L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "العملاء", "Customers", null, 4, null, null },
                    { 2L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "فئات الطلبات", "categories", 1L, 1, null, null },
                    { 6L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "حالات الطلبات", "status", 1L, 2, null, null },
                    { 18L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "المدن", "cities", 1L, 5, null, null },
                    { 22L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "المجموعات", "groups", 1L, 6, null, null },
                    { 30L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "الأدوار", "roles", 1L, 8, null, null },
                    { 33L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "الإدارات والأقسام", "departments", 1L, 9, null, null },
                    { 70L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "إدارة أدوار المستخدمين", "roles-management", 69L, 1, null, null },
                    { 71L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "المستخدمين", "users", 69L, 2, null, null },
                    { 75L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "التورايخ", "calender", 69L, 3, null, null },
                    { 80L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "إضافة شكوى او طلب", "add-service-one", 78L, 2, null, null },
                    { 81L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "تعديل شكوى او طلب", "edit-service-one", 78L, 3, null, null },
                    { 82L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "تعديل شكوى او طلب", "close-service-one", 78L, 4, null, null },
                    { 84L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "تغيير فئة شكوى او طلب", "change-category-service-one", 78L, 6, null, null },
                    { 85L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "إضافة تعليق على شكوى او طلب", "add-comment-service-one", 78L, 7, null, null },
                    { 87L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "إضافة عميل", "add-Customer", 86L, 1, null, null },
                    { 88L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "تعديل عميل", "edit-Customer", 86L, 2, null, null },
                    { 89L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "حذف عميل", "delete-Customer", 86L, 3, null, null },
                    { 3L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "إضافة فئة طلب", "add-category", 2L, 1, null, null },
                    { 4L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "تعديل فئة طلب", "edit-category", 2L, 2, null, null },
                    { 5L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "حذف فئة طلب", "delete-category", 2L, 3, null, null },
                    { 8L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "تعديل حالة طلب", "edit-status", 6L, 2, null, null },
                    { 9L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "حذف حالة طلب", "delete-status", 6L, 3, null, null },
                    { 19L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "إضافة مدينه", "add-city", 18L, 1, null, null },
                    { 20L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "تعديل مدينه", "edit-city", 18L, 2, null, null },
                    { 21L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "حذف مدينه", "delete-city", 18L, 3, null, null },
                    { 23L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "إضافة مجموعه", "add-group", 22L, 1, null, null },
                    { 24L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "تعديل مجموعه", "edit-group", 22L, 2, null, null },
                    { 25L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "حذف مجموعه", "delete-group", 22L, 3, null, null },
                    { 31L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "تعديل دور مستخدم", "edit-role", 30L, 1, null, null },
                    { 32L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "حذف دور مستخدم", "delete-role", 30L, 2, null, null },
                    { 34L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "إضافة قسم", "add-department", 33L, 1, null, null },
                    { 35L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "تعديل قسم", "edit-department", 33L, 2, null, null },
                    { 36L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "حذف قسم", "delete-department", 33L, 3, null, null },
                    { 72L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, " إضافة مستخدم", "add-user", 71L, 1, null, null },
                    { 73L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "تعديل مستخدم ", "edit-user", 71L, 2, null, null },
                    { 74L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "حذف مستخدم ", "delete-user", 71L, 3, null, null },
                    { 76L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, " إضافة تاريخ اجازة", "add-calender-holiday", 75L, 1, null, null },
                    { 77L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "تعديل تاريخ اجازة ", "edit-calender-holiday", 75L, 2, null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoleFunctions_RoleId",
                table: "RoleFunctions",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleFunctions_SystemFunctionId",
                table: "RoleFunctions",
                column: "SystemFunctionId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemFunctions_ParentID",
                table: "SystemFunctions",
                column: "ParentID");
        }
    }
}
