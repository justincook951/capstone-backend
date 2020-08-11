using Microsoft.EntityFrameworkCore.Migrations;

namespace CapstoneQuizAPI.Migrations
{
    public partial class FinishManyToMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SessionQuestion_TestSession_TestSessionId",
                table: "SessionQuestion");

            migrationBuilder.DropColumn(
                name: "SessionToken",
                table: "TestSession");

            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "SessionQuestion");

            migrationBuilder.AlterColumn<long>(
                name: "TestSessionId",
                table: "SessionQuestion",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SessionQuestion_ResultTypeId",
                table: "SessionQuestion",
                column: "ResultTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_SessionQuestion_ResultType_ResultTypeId",
                table: "SessionQuestion",
                column: "ResultTypeId",
                principalTable: "ResultType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SessionQuestion_TestSession_TestSessionId",
                table: "SessionQuestion",
                column: "TestSessionId",
                principalTable: "TestSession",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SessionQuestion_ResultType_ResultTypeId",
                table: "SessionQuestion");

            migrationBuilder.DropForeignKey(
                name: "FK_SessionQuestion_TestSession_TestSessionId",
                table: "SessionQuestion");

            migrationBuilder.DropIndex(
                name: "IX_SessionQuestion_ResultTypeId",
                table: "SessionQuestion");

            migrationBuilder.AddColumn<bool>(
                name: "SessionToken",
                table: "TestSession",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<long>(
                name: "TestSessionId",
                table: "SessionQuestion",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<long>(
                name: "SessionId",
                table: "SessionQuestion",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddForeignKey(
                name: "FK_SessionQuestion_TestSession_TestSessionId",
                table: "SessionQuestion",
                column: "TestSessionId",
                principalTable: "TestSession",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
