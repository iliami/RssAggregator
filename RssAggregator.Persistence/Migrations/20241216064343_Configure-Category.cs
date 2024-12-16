using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RssAggregator.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FeedId",
                table: "Categories",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Categories_FeedId",
                table: "Categories",
                column: "FeedId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Feeds_FeedId",
                table: "Categories",
                column: "FeedId",
                principalTable: "Feeds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Feeds_FeedId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_FeedId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "FeedId",
                table: "Categories");
        }
    }
}
