using Microsoft.EntityFrameworkCore.Migrations;

namespace CapstoneQuizAPI.Migrations
{
    public partial class RenameMigrate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TopicItems",
                table: "TopicItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ResultTypeItems",
                table: "ResultTypeItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuestionItems",
                table: "QuestionItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AnswerItems",
                table: "AnswerItems");

            migrationBuilder.RenameTable(
                name: "TopicItems",
                newName: "Topic");

            migrationBuilder.RenameTable(
                name: "ResultTypeItems",
                newName: "ResultType");

            migrationBuilder.RenameTable(
                name: "QuestionItems",
                newName: "Question");

            migrationBuilder.RenameTable(
                name: "AnswerItems",
                newName: "Answer");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Topic",
                table: "Topic",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ResultType",
                table: "ResultType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Question",
                table: "Question",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Answer",
                table: "Answer",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Topic",
                table: "Topic");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ResultType",
                table: "ResultType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Question",
                table: "Question");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Answer",
                table: "Answer");

            migrationBuilder.RenameTable(
                name: "Topic",
                newName: "TopicItems");

            migrationBuilder.RenameTable(
                name: "ResultType",
                newName: "ResultTypeItems");

            migrationBuilder.RenameTable(
                name: "Question",
                newName: "QuestionItems");

            migrationBuilder.RenameTable(
                name: "Answer",
                newName: "AnswerItems");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TopicItems",
                table: "TopicItems",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ResultTypeItems",
                table: "ResultTypeItems",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuestionItems",
                table: "QuestionItems",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AnswerItems",
                table: "AnswerItems",
                column: "Id");
        }
    }
}
