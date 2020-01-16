using Microsoft.EntityFrameworkCore.Migrations;

namespace Isearch.Migrations
{
    public partial class t8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "关闭",
                table: "Trainings",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "fb21",
                table: "TrainingFeedBacks",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "关闭",
                table: "Trainings");
        }
    }
}
