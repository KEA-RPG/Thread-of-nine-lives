using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CardDeckPrimaryKeyUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DeckCards",
                table: "DeckCards");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "DeckCards",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeckCards",
                table: "DeckCards",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_DeckCards_DeckId",
                table: "DeckCards",
                column: "DeckId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DeckCards",
                table: "DeckCards");

            migrationBuilder.DropIndex(
                name: "IX_DeckCards_DeckId",
                table: "DeckCards");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "DeckCards");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeckCards",
                table: "DeckCards",
                columns: new[] { "DeckId", "CardId" });
        }
    }
}
