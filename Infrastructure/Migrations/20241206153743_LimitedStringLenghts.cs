using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LimitedStringLenghts : Migration
    {
        const string tableCards = "Cards";
        const string varchar60 = "nvarchar(60)";
        const string varcharMax = "nvarchar(max)";
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Decks",
                type: varchar60,
                maxLength: 60,
                nullable: false,
                oldClrType: typeof(string),
                oldType: varcharMax);

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Comments",
                type: "nvarchar(255)",
                maxLength: 350,
                nullable: false,
                oldClrType: typeof(string),
                oldType: varcharMax);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Cards",
                type: varchar60,
                maxLength: 60,
                nullable: false,
                oldClrType: typeof(string),
                oldType: varcharMax);

            migrationBuilder.AlterColumn<string>(
                name: "ImagePath",
                table: tableCards,
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: false,
                oldClrType: typeof(string),
                oldType: varcharMax);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: tableCards,
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: varcharMax);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Decks",
                type: varcharMax,
                nullable: false,
                oldClrType: typeof(string),
                oldType: varchar60,
                oldMaxLength: 60);

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 350);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: tableCards,
                type: varcharMax,
                nullable: false,
                oldClrType: typeof(string),
                oldType: varchar60,
                oldMaxLength: 60);

            migrationBuilder.AlterColumn<string>(
                name: "ImagePath",
                table: tableCards,
                type: varcharMax,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(80)",
                oldMaxLength: 80);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: tableCards,
                type: varcharMax,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300);
        }
    }
}
