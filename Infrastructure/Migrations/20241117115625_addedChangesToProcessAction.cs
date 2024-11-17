using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedChangesToProcessAction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fights_Enemies_EnemyId",
                table: "Fights");

            migrationBuilder.DropForeignKey(
                name: "FK_Fights_Users_UserId",
                table: "Fights");

            migrationBuilder.CreateIndex(
                name: "IX_GameActions_FightId",
                table: "GameActions",
                column: "FightId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fights_Enemies_EnemyId",
                table: "Fights",
                column: "EnemyId",
                principalTable: "Enemies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Fights_Users_UserId",
                table: "Fights",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GameActions_Fights_FightId",
                table: "GameActions",
                column: "FightId",
                principalTable: "Fights",
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

            migrationBuilder.DropForeignKey(
                name: "FK_GameActions_Fights_FightId",
                table: "GameActions");

            migrationBuilder.DropIndex(
                name: "IX_GameActions_FightId",
                table: "GameActions");

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
    }
}
