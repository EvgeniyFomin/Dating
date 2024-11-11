using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dating.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateConnectionEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Connections");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Connections",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Connections");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Connections",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
