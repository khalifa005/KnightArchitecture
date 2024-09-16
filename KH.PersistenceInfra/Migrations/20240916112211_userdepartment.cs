using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KH.PersistenceInfra.Migrations
{
    /// <inheritdoc />
    public partial class userdepartment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserDepartment_Users_UserId",
                table: "UserDepartment");

            migrationBuilder.AddForeignKey(
                name: "FK_UserDepartment_Users_UserId",
                table: "UserDepartment",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserDepartment_Users_UserId",
                table: "UserDepartment");

            migrationBuilder.AddForeignKey(
                name: "FK_UserDepartment_Users_UserId",
                table: "UserDepartment",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
