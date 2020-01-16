using Microsoft.EntityFrameworkCore.Migrations;

namespace Isearch.Migrations
{
    public partial class t9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "fb21",
                table: "TrainingFeedBacks",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "fb31",
                table: "TrainingFeedBacks",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "fb31",
                table: "TrainingFeedBacks");

            migrationBuilder.AlterColumn<string>(
                name: "fb21",
                table: "TrainingFeedBacks",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
