using Microsoft.EntityFrameworkCore.Migrations;

namespace FedSurvey.Migrations
{
    public partial class PartOfPercentage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PartOfPercentage",
                table: "PossibleResponses",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.InsertData(
                table: "PossibleResponses",
                columns: new[] { "Id", "QuestionTypeId", "PartOfPercentage" },
                values: new object[,] { { 4, 1, false } }
            );

            migrationBuilder.InsertData(
                table: "PossibleResponseStrings",
                columns: new[] { "Id", "PossibleResponseId", "Name" },
                values: new object[,] { { 4, 4, "Do Not Know/ No Basis to Judge" } }
            );

            // This uses the same random number for each row, which is not ideal, but it sounds too non-trivial
            // to immediately fix.
            migrationBuilder.Sql("INSERT INTO Responses (QuestionExecutionId, PossibleResponseId, DataGroupId, Count) SELECT QuestionExecutions.Id, 4, 1, RAND() * 50 FROM QuestionExecutions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PartOfPercentage",
                table: "PossibleResponses");
        }
    }
}
