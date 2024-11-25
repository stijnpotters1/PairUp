using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PairUpInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changedcategoriesintotwolevelcategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_activity_categories_category_CategoryId",
                table: "activity_categories");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "activity_categories",
                newName: "SubLevelCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_activity_categories_CategoryId",
                table: "activity_categories",
                newName: "IX_activity_categories_SubLevelCategoryId");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "activity",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TopLevelCategory",
                table: "activity",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_activity_categories_category_SubLevelCategoryId",
                table: "activity_categories",
                column: "SubLevelCategoryId",
                principalTable: "category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_activity_categories_category_SubLevelCategoryId",
                table: "activity_categories");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "activity");

            migrationBuilder.DropColumn(
                name: "TopLevelCategory",
                table: "activity");

            migrationBuilder.RenameColumn(
                name: "SubLevelCategoryId",
                table: "activity_categories",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_activity_categories_SubLevelCategoryId",
                table: "activity_categories",
                newName: "IX_activity_categories_CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_activity_categories_category_CategoryId",
                table: "activity_categories",
                column: "CategoryId",
                principalTable: "category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
