using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Isearch.Migrations
{
    public partial class t1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Trainings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    课程名称 = table.Column<string>(nullable: true),
                    培训时常 = table.Column<int>(nullable: false),
                    培训讲师 = table.Column<string>(nullable: true),
                    培训时间 = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trainings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainingFeedBacks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    fb1 = table.Column<int>(nullable: false),
                    fb2 = table.Column<int>(nullable: false),
                    fb3 = table.Column<int>(nullable: false),
                    fb4 = table.Column<int>(nullable: false),
                    fb5 = table.Column<int>(nullable: false),
                    fb6 = table.Column<int>(nullable: false),
                    fb7 = table.Column<int>(nullable: false),
                    fb8 = table.Column<int>(nullable: false),
                    fb9 = table.Column<int>(nullable: false),
                    fb10 = table.Column<int>(nullable: false),
                    fb11 = table.Column<int>(nullable: false),
                    fb12 = table.Column<int>(nullable: false),
                    fb13 = table.Column<int>(nullable: false),
                    fb14 = table.Column<int>(nullable: false),
                    fb15 = table.Column<int>(nullable: false),
                    fb16 = table.Column<string>(nullable: true),
                    fb17 = table.Column<string>(nullable: true),
                    fb18 = table.Column<string>(nullable: true),
                    fb19 = table.Column<string>(nullable: true),
                    fb20 = table.Column<string>(nullable: true),
                    fb21 = table.Column<string>(nullable: true),
                    fb22 = table.Column<string>(nullable: true),
                    fb23 = table.Column<string>(nullable: true),
                    fb24 = table.Column<string>(nullable: true),
                    fb25 = table.Column<string>(nullable: true),
                    fb26 = table.Column<string>(nullable: true),
                    fb27 = table.Column<string>(nullable: true),
                    fb28 = table.Column<string>(nullable: true),
                    fb29 = table.Column<string>(nullable: true),
                    fb30 = table.Column<string>(nullable: true),
                    TrainingId = table.Column<int>(nullable: true),
                    TriningId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingFeedBacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingFeedBacks_Trainings_TrainingId",
                        column: x => x.TrainingId,
                        principalTable: "Trainings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrainingFeedBacks_TrainingId",
                table: "TrainingFeedBacks",
                column: "TrainingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainingFeedBacks");

            migrationBuilder.DropTable(
                name: "Trainings");
        }
    }
}
