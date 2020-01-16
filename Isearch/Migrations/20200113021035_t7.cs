using Microsoft.EntityFrameworkCore.Migrations;

namespace Isearch.Migrations
{
    public partial class t7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "真实培训时间",
                table: "TrainingFeedBacks",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "真实培训时间",
                table: "TrainingFeedBacks",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
