using Microsoft.EntityFrameworkCore.Migrations;

namespace CapstoneQuizAPI.Migrations
{
    public partial class AddRelationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Topic");

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "Topic",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "SessionQuestionId",
                table: "Question",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TestSession",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SessionToken = table.Column<bool>(nullable: false),
                    LastVisitedTime = table.Column<long>(nullable: false),
                    SessionClosedTime = table.Column<long>(nullable: true),
                    UserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestSession", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestSession_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SessionQuestion",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<bool>(nullable: false),
                    ResultTypeId = table.Column<long>(nullable: false),
                    SessionId = table.Column<long>(nullable: false),
                    TestSessionId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionQuestion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SessionQuestion_TestSession_TestSessionId",
                        column: x => x.TestSessionId,
                        principalTable: "TestSession",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Topic_UserId",
                table: "Topic",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Question_SessionQuestionId",
                table: "Question",
                column: "SessionQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Question_TopicId",
                table: "Question",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_Answer_QuestionId",
                table: "Answer",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionQuestion_TestSessionId",
                table: "SessionQuestion",
                column: "TestSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_TestSession_UserId",
                table: "TestSession",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Answer_Question_QuestionId",
                table: "Answer",
                column: "QuestionId",
                principalTable: "Question",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Question_SessionQuestion_SessionQuestionId",
                table: "Question",
                column: "SessionQuestionId",
                principalTable: "SessionQuestion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Question_Topic_TopicId",
                table: "Question",
                column: "TopicId",
                principalTable: "Topic",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Topic_User_UserId",
                table: "Topic",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answer_Question_QuestionId",
                table: "Answer");

            migrationBuilder.DropForeignKey(
                name: "FK_Question_SessionQuestion_SessionQuestionId",
                table: "Question");

            migrationBuilder.DropForeignKey(
                name: "FK_Question_Topic_TopicId",
                table: "Question");

            migrationBuilder.DropForeignKey(
                name: "FK_Topic_User_UserId",
                table: "Topic");

            migrationBuilder.DropTable(
                name: "SessionQuestion");

            migrationBuilder.DropTable(
                name: "TestSession");

            migrationBuilder.DropIndex(
                name: "IX_Topic_UserId",
                table: "Topic");

            migrationBuilder.DropIndex(
                name: "IX_Question_SessionQuestionId",
                table: "Question");

            migrationBuilder.DropIndex(
                name: "IX_Question_TopicId",
                table: "Question");

            migrationBuilder.DropIndex(
                name: "IX_Answer_QuestionId",
                table: "Answer");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Topic");

            migrationBuilder.DropColumn(
                name: "SessionQuestionId",
                table: "Question");
        }
    }
}
