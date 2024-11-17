using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedFightAndEveyrOtherChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropColumn(
                name: "CardId",
                table: "GameActions");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Fights");

            migrationBuilder.AlterColumn<int>(
                name: "Value",
                table: "GameActions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FightId",
                table: "GameActions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EnemyId",
                table: "Fights",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Fights",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Fights_EnemyId",
                table: "Fights",
                column: "EnemyId");

            migrationBuilder.CreateIndex(
                name: "IX_Fights_UserId",
                table: "Fights",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fights_Enemies_EnemyId",
                table: "Fights",
                column: "EnemyId",
                principalTable: "Enemies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Fights_Users_UserId",
                table: "Fights",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fights_Enemies_EnemyId",
                table: "Fights");

            migrationBuilder.DropForeignKey(
                name: "FK_Fights_Users_UserId",
                table: "Fights");

            migrationBuilder.DropIndex(
                name: "IX_Fights_EnemyId",
                table: "Fights");

            migrationBuilder.DropIndex(
                name: "IX_Fights_UserId",
                table: "Fights");

            migrationBuilder.DropColumn(
                name: "FightId",
                table: "GameActions");

            migrationBuilder.DropColumn(
                name: "EnemyId",
                table: "Fights");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Fights");

            migrationBuilder.AlterColumn<int>(
                name: "Value",
                table: "GameActions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CardId",
                table: "GameActions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Fights",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Health = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });
        }
    }
}
