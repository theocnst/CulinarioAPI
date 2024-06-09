using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CulinarioAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddCountryAndRecipeTypeToRecipe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "Recipes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RecipeType",
                table: "Recipes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    CountryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.CountryId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_CountryId",
                table: "Recipes",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_Countries_CountryId",
                table: "Recipes",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "CountryId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_Countries_CountryId",
                table: "Recipes");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_Recipes_CountryId",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "RecipeType",
                table: "Recipes");
        }
    }
}
