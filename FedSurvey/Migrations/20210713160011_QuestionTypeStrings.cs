using Microsoft.EntityFrameworkCore.Migrations;

namespace FedSurvey.Migrations
{
    public partial class QuestionTypeStrings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuestionTypeStrings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionTypeId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionTypeString", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionTypeStrings_QuestionTypes_QuestionTypeId",
                        column: x => x.QuestionTypeId,
                        principalTable: "QuestionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.Sql("INSERT INTO QuestionTypeStrings (QuestionTypeId, Name) SELECT Id AS QuestionTypeId, Name FROM QuestionTypes");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "QuestionTypes");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionTypeStrings_QuestionTypeId",
                table: "QuestionTypeStrings",
                column: "QuestionTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuestionTypeStrings");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "QuestionTypes",
                type: "varchar(300)",
                unicode: false,
                maxLength: 300,
                nullable: true);
        }
    }
}
