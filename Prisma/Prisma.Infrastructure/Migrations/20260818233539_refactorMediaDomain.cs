using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prisma.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class refactorMediaDomain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "media_list_items");

            migrationBuilder.DropTable(
                name: "media_lists");

            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "media");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "media");

            migrationBuilder.DropColumn(
                name: "LinkUrl",
                table: "media");

            migrationBuilder.DropColumn(
                name: "ReleaseDate",
                table: "media");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "media");

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "media",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Url",
                table: "media");

            migrationBuilder.AddColumn<string>(
                name: "ExternalId",
                table: "media",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "media",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkUrl",
                table: "media",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReleaseDate",
                table: "media",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Source",
                table: "media",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "media_lists",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsPublic = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_media_lists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_media_lists_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "media_list_items",
                columns: table => new
                {
                    MediaListId = table.Column<Guid>(type: "uuid", nullable: false),
                    MediaId = table.Column<Guid>(type: "uuid", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_media_list_items", x => new { x.MediaListId, x.MediaId });
                    table.ForeignKey(
                        name: "FK_media_list_items_media_MediaId",
                        column: x => x.MediaId,
                        principalTable: "media",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_media_list_items_media_lists_MediaListId",
                        column: x => x.MediaListId,
                        principalTable: "media_lists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_media_list_items_MediaId",
                table: "media_list_items",
                column: "MediaId");

            migrationBuilder.CreateIndex(
                name: "IX_media_lists_UserId",
                table: "media_lists",
                column: "UserId");
        }
    }
}
