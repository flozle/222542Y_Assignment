using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _222542Y_Assignment.Migrations
{
    public partial class _8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OldPassword1",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OldPassword2",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OldPassword3",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OldPassword1",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "OldPassword2",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "OldPassword3",
                table: "AspNetUsers");
        }
    }
}
