using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KH.PersistenceInfra.Migrations
{
    /// <inheritdoc />
    public partial class updateroletable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Roles",
                type: "rowversion",
                rowVersion: true,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Roles");
        }
    }
}
