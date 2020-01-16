using Microsoft.EntityFrameworkCore.Migrations;

namespace Isearch.Migrations
{
    public partial class t3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "培训时长",
                table: "Trainings",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "培训时长",
                table: "Trainings",
                nullable: false,
                oldClrType: typeof(float));
        }
    }
}
