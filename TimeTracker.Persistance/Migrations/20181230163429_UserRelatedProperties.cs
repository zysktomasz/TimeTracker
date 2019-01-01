using Microsoft.EntityFrameworkCore.Migrations;

namespace TimeTracker.Persistance.Migrations
{
    public partial class UserRelatedProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserAccountId",
                table: "Projects",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserAccountId",
                table: "Activities",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_UserAccountId",
                table: "Projects",
                column: "UserAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_UserAccountId",
                table: "Activities",
                column: "UserAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_AspNetUsers_UserAccountId",
                table: "Activities",
                column: "UserAccountId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_AspNetUsers_UserAccountId",
                table: "Projects",
                column: "UserAccountId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_AspNetUsers_UserAccountId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_AspNetUsers_UserAccountId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_UserAccountId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Activities_UserAccountId",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "UserAccountId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "UserAccountId",
                table: "Activities");
        }
    }
}
