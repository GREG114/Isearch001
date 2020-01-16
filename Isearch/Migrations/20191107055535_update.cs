using Microsoft.EntityFrameworkCore.Migrations;

namespace Isearch.Migrations
{
    public partial class update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SSID",
                table: "NTQ",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "所在办公区域",
                table: "NTQ",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SSID",
                table: "NTQ");

            migrationBuilder.DropColumn(
                name: "所在办公区域",
                table: "NTQ");
        }
    }
}
