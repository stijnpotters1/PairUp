using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PairUpInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initial_database : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "activity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Url = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Price = table.Column<double>(type: "double precision", nullable: false),
                    Age = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Duration = table.Column<string>(type: "text", nullable: false),
                    PlaceId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_activity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_activity_place_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "place",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_activity_PlaceId",
                table: "activity",
                column: "PlaceId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "activity");

            migrationBuilder.DropTable(
                name: "place");
        }
    }
}
