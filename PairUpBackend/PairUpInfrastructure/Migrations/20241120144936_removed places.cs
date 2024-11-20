using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PairUpInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removedplaces : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_activity_place_PlaceId",
                table: "activity");

            migrationBuilder.DropTable(
                name: "place");

            migrationBuilder.DropIndex(
                name: "IX_activity_PlaceId",
                table: "activity");

            migrationBuilder.DropColumn(
                name: "PlaceId",
                table: "activity");

            migrationBuilder.AlterColumn<string>(
                name: "Duration",
                table: "activity",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "FullAddress",
                table: "activity",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "activity",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "activity",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullAddress",
                table: "activity");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "activity");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "activity");

            migrationBuilder.AlterColumn<string>(
                name: "Duration",
                table: "activity",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AddColumn<Guid>(
                name: "PlaceId",
                table: "activity",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "place",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FullAddress = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_place", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_activity_PlaceId",
                table: "activity",
                column: "PlaceId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_activity_place_PlaceId",
                table: "activity",
                column: "PlaceId",
                principalTable: "place",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
