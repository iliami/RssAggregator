using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RssAggregator.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureFeeds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Feeds",
                type: "character varying(2048)",
                maxLength: 2048,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Feeds");
        }
    }
}
