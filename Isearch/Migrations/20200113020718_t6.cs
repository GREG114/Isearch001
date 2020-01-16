using Microsoft.EntityFrameworkCore.Migrations;

namespace Isearch.Migrations
{
    public partial class t6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "真实培训时间",
                table: "TrainingFeedBacks",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "真实培训时间",
                table: "TrainingFeedBacks");
        }
    }
}
