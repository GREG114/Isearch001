using Microsoft.EntityFrameworkCore.Migrations;

namespace Isearch.Migrations
{
    public partial class t4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrainingFeedBacks_Trainings_TrainingId",
                table: "TrainingFeedBacks");

            migrationBuilder.DropColumn(
                name: "TriningId",
                table: "TrainingFeedBacks");

            migrationBuilder.RenameColumn(
                name: "TrainingId",
                table: "TrainingFeedBacks",
                newName: "TrainingID");

            migrationBuilder.RenameIndex(
                name: "IX_TrainingFeedBacks_TrainingId",
                table: "TrainingFeedBacks",
                newName: "IX_TrainingFeedBacks_TrainingID");

            migrationBuilder.AlterColumn<int>(
                name: "TrainingID",
                table: "TrainingFeedBacks",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingFeedBacks_Trainings_TrainingID",
                table: "TrainingFeedBacks",
                column: "TrainingID",
                principalTable: "Trainings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrainingFeedBacks_Trainings_TrainingID",
                table: "TrainingFeedBacks");

            migrationBuilder.RenameColumn(
                name: "TrainingID",
                table: "TrainingFeedBacks",
                newName: "TrainingId");

            migrationBuilder.RenameIndex(
                name: "IX_TrainingFeedBacks_TrainingID",
                table: "TrainingFeedBacks",
                newName: "IX_TrainingFeedBacks_TrainingId");

            migrationBuilder.AlterColumn<int>(
                name: "TrainingId",
                table: "TrainingFeedBacks",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "TriningId",
                table: "TrainingFeedBacks",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingFeedBacks_Trainings_TrainingId",
                table: "TrainingFeedBacks",
                column: "TrainingId",
                principalTable: "Trainings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
