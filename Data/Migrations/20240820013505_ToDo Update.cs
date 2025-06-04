using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDo_Web_App.Data.Migrations
{
    /// <inheritdoc />
    public partial class ToDoUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignment_AspNetUsers_UserId",
                table: "Assignment");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Assignment",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignment_AspNetUsers_UserId",
                table: "Assignment",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignment_AspNetUsers_UserId",
                table: "Assignment");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Assignment",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Assignment_AspNetUsers_UserId",
                table: "Assignment",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
