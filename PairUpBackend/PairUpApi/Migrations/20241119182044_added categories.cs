using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PairUpApi.Migrations
{
    /// <inheritdoc />
    public partial class addedcategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Price",
                table: "activity",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                table: "activity",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "category",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_category", x => x.Id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_activity_category_CategoryId",
                table: "activity");

            migrationBuilder.DropTable(
                name: "category");

            migrationBuilder.DropIndex(
                name: "IX_activity_CategoryId",
                table: "activity");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "activity");

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "activity",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);
        }
    }
}
