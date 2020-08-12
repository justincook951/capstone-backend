using Microsoft.EntityFrameworkCore.Migrations;

namespace CapstoneQuizAPI.Migrations
{
    public partial class TopicToSessionsCorrelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Question_SessionQuestion_SessionQuestionId",
                table: "Question");

            migrationBuilder.DropIndex(
                name: "IX_Question_SessionQuestionId",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "SessionQuestionId",
                table: "Question");

            migrationBuilder.AddColumn<long>(
                name: "TopicId",
                table: "TestSession",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TestSession_TopicId",
                table: "TestSession",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionQuestion_QuestionId",
                table: "SessionQuestion",
                column: "QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_SessionQuestion_Question_QuestionId",
                table: "SessionQuestion",
                column: "QuestionId",
                principalTable: "Question",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_TestSession_Topic_TopicId",
                table: "TestSession",
                column: "TopicId",
                principalTable: "Topic",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SessionQuestion_Question_QuestionId",
                table: "SessionQuestion");

            migrationBuilder.DropForeignKey(
                name: "FK_TestSession_Topic_TopicId",
                table: "TestSession");

            migrationBuilder.DropIndex(
                name: "IX_TestSession_TopicId",
                table: "TestSession");

            migrationBuilder.DropIndex(
                name: "IX_SessionQuestion_QuestionId",
                table: "SessionQuestion");

            migrationBuilder.DropColumn(
                name: "TopicId",
                table: "TestSession");

            migrationBuilder.AddColumn<long>(
                name: "SessionQuestionId",
                table: "Question",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Question_SessionQuestionId",
                table: "Question",
                column: "SessionQuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Question_SessionQuestion_SessionQuestionId",
                table: "Question",
                column: "SessionQuestionId",
                principalTable: "SessionQuestion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
