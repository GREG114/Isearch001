using Microsoft.EntityFrameworkCore.Migrations;

namespace Isearch.Migrations
{
    public partial class t2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "培训时常",
                table: "Trainings",
                newName: "培训时长");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "培训时长",
                table: "Trainings",
                newName: "培训时常");
        }
    }
}
