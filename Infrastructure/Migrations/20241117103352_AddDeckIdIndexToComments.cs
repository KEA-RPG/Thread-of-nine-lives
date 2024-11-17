using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDeckIdIndexToComments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "idx_deck_id",
                table: "Comments",
                column: "DeckId")
                .Annotation("SqlServer:Include", new[] { "Text", "CreatedAt", "UserId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "idx_deck_id",
                table: "Comments");
        }
    }
}
