using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dating.DAL.Migrations
{
    /// <inheritdoc />
    public partial class update_user_entity_typo_fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Passwordalt",
                table: "Users",
                newName: "PasswordSalt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PasswordSalt",
                table: "Users",
                newName: "Passwordalt");
        }
    }
}
