using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RssAggregator.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureSubscriptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscription_AppUsers_AppUserId",
                table: "Subscription");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscription_Feeds_FeedId",
                table: "Subscription");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subscription",
                table: "Subscription");

            migrationBuilder.RenameTable(
                name: "Subscription",
                newName: "Subscriptions");

            migrationBuilder.RenameIndex(
                name: "IX_Subscription_FeedId",
                table: "Subscriptions",
                newName: "IX_Subscriptions_FeedId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subscriptions",
                table: "Subscriptions",
                columns: new[] { "AppUserId", "FeedId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_AppUsers_AppUserId",
                table: "Subscriptions",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Feeds_FeedId",
                table: "Subscriptions",
                column: "FeedId",
                principalTable: "Feeds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_AppUsers_AppUserId",
                table: "Subscriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Feeds_FeedId",
                table: "Subscriptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subscriptions",
                table: "Subscriptions");

            migrationBuilder.RenameTable(
                name: "Subscriptions",
                newName: "Subscription");

            migrationBuilder.RenameIndex(
                name: "IX_Subscriptions_FeedId",
                table: "Subscription",
                newName: "IX_Subscription_FeedId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subscription",
                table: "Subscription",
                columns: new[] { "AppUserId", "FeedId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Subscription_AppUsers_AppUserId",
                table: "Subscription",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscription_Feeds_FeedId",
                table: "Subscription",
                column: "FeedId",
                principalTable: "Feeds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
