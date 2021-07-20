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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PartOfPercentage",
                table: "PossibleResponses");
        }
    }
}
