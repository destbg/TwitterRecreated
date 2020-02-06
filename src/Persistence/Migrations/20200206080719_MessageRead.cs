using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class MessageRead : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MessageRead");

            migrationBuilder.AddColumn<long>(
                name: "MessageReadId",
                table: "ChatUsers",
                nullable: true,
                defaultValue: null);

            migrationBuilder.CreateIndex(
                name: "IX_ChatUsers_MessageReadId",
                table: "ChatUsers",
                column: "MessageReadId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatUsers_Messages_MessageReadId",
                table: "ChatUsers",
                column: "MessageReadId",
                principalTable: "Messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatUsers_Messages_MessageReadId",
                table: "ChatUsers");

            migrationBuilder.DropIndex(
                name: "IX_ChatUsers_MessageReadId",
                table: "ChatUsers");

            migrationBuilder.DropColumn(
                name: "MessageReadId",
                table: "ChatUsers");

            migrationBuilder.CreateTable(
                name: "MessageRead",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<string>(type: "char(36)", fixedLength: true, maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageRead", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MessageRead_Messages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_MessageRead_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MessageRead_MessageId",
                table: "MessageRead",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageRead_UserId",
                table: "MessageRead",
                column: "UserId");
        }
    }
}
