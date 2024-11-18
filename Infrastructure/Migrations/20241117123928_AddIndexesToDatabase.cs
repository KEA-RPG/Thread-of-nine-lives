using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexesToDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Decks_UserId",
                table: "Decks");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "idx_username",
                table: "Users",
                column: "Username")
                .Annotation("SqlServer:Include", new[] { "PasswordHash", "Role" });

            migrationBuilder.CreateIndex(
                name: "idx_is_public",
                table: "Decks",
                column: "IsPublic")
                .Annotation("SqlServer:Include", new[] { "UserId", "Name" });

            migrationBuilder.CreateIndex(
                name: "idx_user_id",
                table: "Decks",
                column: "UserId")
                .Annotation("SqlServer:Include", new[] { "Name", "IsPublic" });

            migrationBuilder.CreateIndex(
                name: "idx_deck_id",
                table: "Comments",
                column: "DeckId")
                .Annotation("SqlServer:Include", new[] { "Text", "CreatedAt", "UserId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "idx_username",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "idx_is_public",
                table: "Decks");

            migrationBuilder.DropIndex(
                name: "idx_user_id",
                table: "Decks");

            migrationBuilder.DropIndex(
                name: "idx_deck_id",
                table: "Comments");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(512)",
                oldMaxLength: 512);

            migrationBuilder.CreateIndex(
                name: "IX_Decks_UserId",
                table: "Decks",
                column: "UserId");
        }
    }
}
