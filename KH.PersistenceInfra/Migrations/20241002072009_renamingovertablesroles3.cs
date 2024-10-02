using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KH.PersistenceInfra.Migrations
{
    /// <inheritdoc />
    public partial class renamingovertablesroles3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_Permissions_ParentID",
                table: "Permissions");

            migrationBuilder.RenameColumn(
                name: "ParentID",
                table: "Permissions",
                newName: "ParentId");

            migrationBuilder.RenameColumn(
                name: "DependOnID",
                table: "Permissions",
                newName: "DependOnId");

            migrationBuilder.RenameIndex(
                name: "IX_Permissions_ParentID",
                table: "Permissions",
                newName: "IX_Permissions_ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_Permissions_ParentId",
                table: "Permissions",
                column: "ParentId",
                principalTable: "Permissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_Permissions_ParentId",
                table: "Permissions");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "Permissions",
                newName: "ParentID");

            migrationBuilder.RenameColumn(
                name: "DependOnId",
                table: "Permissions",
                newName: "DependOnID");

            migrationBuilder.RenameIndex(
                name: "IX_Permissions_ParentId",
                table: "Permissions",
                newName: "IX_Permissions_ParentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_Permissions_ParentID",
                table: "Permissions",
                column: "ParentID",
                principalTable: "Permissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
