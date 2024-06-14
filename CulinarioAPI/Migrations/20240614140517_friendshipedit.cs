using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CulinarioAPI.Migrations
{
    /// <inheritdoc />
    public partial class friendshipedit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Friendships");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Friendships",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
