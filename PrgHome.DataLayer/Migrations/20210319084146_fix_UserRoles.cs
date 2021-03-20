using Microsoft.EntityFrameworkCore.Migrations;

namespace PrgHome.DataLayer.Migrations
{
    public partial class fix_UserRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersRole_Roles_RoleId1",
                table: "UsersRole");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersRole_Users_UserId1",
                table: "UsersRole");

            migrationBuilder.DropIndex(
                name: "IX_UsersRole_RoleId1",
                table: "UsersRole");

            migrationBuilder.DropIndex(
                name: "IX_UsersRole_UserId1",
                table: "UsersRole");

            migrationBuilder.DropColumn(
                name: "RoleId1",
                table: "UsersRole");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "UsersRole");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RoleId1",
                table: "UsersRole",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "UsersRole",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsersRole_RoleId1",
                table: "UsersRole",
                column: "RoleId1");

            migrationBuilder.CreateIndex(
                name: "IX_UsersRole_UserId1",
                table: "UsersRole",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersRole_Roles_RoleId1",
                table: "UsersRole",
                column: "RoleId1",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersRole_Users_UserId1",
                table: "UsersRole",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
