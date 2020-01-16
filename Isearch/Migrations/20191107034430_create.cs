using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Isearch.Migrations
{
    public partial class create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NTQ",
                columns: table => new
                {
                    姓名 = table.Column<string>(nullable: false),
                    总体满意度 = table.Column<string>(nullable: true),
                    部门 = table.Column<string>(nullable: true),
                    无线稳定性 = table.Column<string>(nullable: true),
                    无线下载速度 = table.Column<string>(nullable: true),
                    无线打开速度 = table.Column<string>(nullable: true),
                    无线可访问性 = table.Column<string>(nullable: true),
                    有线稳定性 = table.Column<string>(nullable: true),
                    有线下载速度 = table.Column<string>(nullable: true),
                    有线打开速度 = table.Column<string>(nullable: true),
                    有线可访问性 = table.Column<string>(nullable: true),
                    updatetime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NTQ", x => x.姓名);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NTQ");
        }
    }
}
