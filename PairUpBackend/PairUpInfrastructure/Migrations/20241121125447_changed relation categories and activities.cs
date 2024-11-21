using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PairUpInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changedrelationcategoriesandactivities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_activity_category_CategoryId",
                table: "activity");

            migrationBuilder.DropIndex(
                name: "IX_activity_CategoryId",
                table: "activity");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "activity");

            migrationBuilder.CreateTable(
                name: "activity_categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ActivityId = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_activity_categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_activity_categories_activity_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "activity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_activity_categories_category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_activity_categories_ActivityId",
                table: "activity_categories",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_activity_categories_CategoryId",
                table: "activity_categories",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "activity_categories");

            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                table: "activity",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_activity_CategoryId",
                table: "activity",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_activity_category_CategoryId",
                table: "activity",
                column: "CategoryId",
                principalTable: "category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
