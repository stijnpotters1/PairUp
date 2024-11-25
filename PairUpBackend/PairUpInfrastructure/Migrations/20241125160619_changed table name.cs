using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PairUpInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changedtablename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_activity_categories_category_SubLevelCategoryId",
                table: "activity_categories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_category",
                table: "category");

            migrationBuilder.RenameTable(
                name: "category",
                newName: "sub_level_category");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sub_level_category",
                table: "sub_level_category",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_activity_categories_sub_level_category_SubLevelCategoryId",
                table: "activity_categories",
                column: "SubLevelCategoryId",
                principalTable: "sub_level_category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_activity_categories_sub_level_category_SubLevelCategoryId",
                table: "activity_categories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sub_level_category",
                table: "sub_level_category");

            migrationBuilder.RenameTable(
                name: "sub_level_category",
                newName: "category");

            migrationBuilder.AddPrimaryKey(
                name: "PK_category",
                table: "category",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_activity_categories_category_SubLevelCategoryId",
                table: "activity_categories",
                column: "SubLevelCategoryId",
                principalTable: "category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
