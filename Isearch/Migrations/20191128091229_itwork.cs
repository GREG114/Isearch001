using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Isearch.Migrations
{
    public partial class itwork : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ITWork",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Workclass = table.Column<string>(nullable: true),
                    Creator = table.Column<string>(nullable: true),
                    Target = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    satisfied = table.Column<int>(nullable: false),
                    Create_at = table.Column<DateTime>(nullable: false),
                    Update_at = table.Column<DateTime>(nullable: false),
                    Finish_at = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ITWork", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ITWork");
        }
    }
}
