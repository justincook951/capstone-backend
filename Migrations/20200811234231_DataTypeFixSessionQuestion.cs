using Microsoft.EntityFrameworkCore.Migrations;

namespace CapstoneQuizAPI.Migrations
{
    public partial class DataTypeFixSessionQuestion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "QuestionId",
                table: "SessionQuestion",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "QuestionId",
                table: "SessionQuestion",
                type: "bit",
                nullable: false,
                oldClrType: typeof(long));
        }
    }
}
