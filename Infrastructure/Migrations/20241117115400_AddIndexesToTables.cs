using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexesToTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Index for Comments on DeckId (retained)
            migrationBuilder.CreateIndex(
                name: "idx_deck_id",
                table: "Comments",
                column: "DeckId")
                .Annotation("SqlServer:Include", new[] { "Text", "CreatedAt", "UserId" });

            // Index for Users on Username with included columns
            migrationBuilder.CreateIndex(
                name: "idx_username",
                table: "Users",
                column: "Username")
                .Annotation("SqlServer:Include", new[] { "PasswordHash", "Role" });

            // Index for Decks on UserId with included columns
            migrationBuilder.CreateIndex(
                name: "idx_user_id",
                table: "Decks",
                column: "UserId")
                .Annotation("SqlServer:Include", new[] { "Name", "IsPublic" });

            // Index for Decks on IsPublic with included columns
            migrationBuilder.CreateIndex(
                name: "idx_is_public",
                table: "Decks",
                column: "IsPublic")
                .Annotation("SqlServer:Include", new[] { "UserId", "Name" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove the index for Decks on IsPublic
            migrationBuilder.DropIndex(
                name: "idx_is_public",
                table: "Decks");

            // Remove the index for Decks on UserId
            migrationBuilder.DropIndex(
                name: "idx_user_id",
                table: "Decks");

            // Remove the index for Users on Username
            migrationBuilder.DropIndex(
                name: "idx_username",
                table: "Users");

            // Remove the index for Comments on DeckId
            migrationBuilder.DropIndex(
                name: "idx_deck_id",
                table: "Comments");
        }
    }
}
