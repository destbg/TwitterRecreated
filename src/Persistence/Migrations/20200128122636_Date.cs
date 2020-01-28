using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class Date : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LikedOn",
                table: "LikedPosts",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.AddColumn<DateTime>(
                name: "AddedOn",
                table: "Bookmarks",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.CreateIndex(
                name: "IX_PollVotes_OptionId",
                table: "PollVotes",
                column: "OptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_PollVotes_PollOptions_OptionId",
                table: "PollVotes",
                column: "OptionId",
                principalTable: "PollOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PollVotes_PollOptions_OptionId",
                table: "PollVotes");

            migrationBuilder.DropIndex(
                name: "IX_PollVotes_OptionId",
                table: "PollVotes");

            migrationBuilder.DropColumn(
                name: "LikedOn",
                table: "LikedPosts");

            migrationBuilder.DropColumn(
                name: "AddedOn",
                table: "Bookmarks");
        }
    }
}
