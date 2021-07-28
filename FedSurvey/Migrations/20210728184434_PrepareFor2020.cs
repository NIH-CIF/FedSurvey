using Microsoft.EntityFrameworkCore.Migrations;

namespace FedSurvey.Migrations
{
    public partial class PrepareFor2020 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "QuestionTypeStrings",
                columns: new[] { "Id", "QuestionTypeId", "Name", "Preferred" },
                values: new object[] { 2, 1, "Core Q1-10, 12-38", false }
            );

            migrationBuilder.InsertData(
                table: "PossibleResponseStrings",
                columns: new[] { "Id", "PossibleResponseId", "Name", "Preferred" },
                values: new object[,]
                {
                    { 24, 1, "Pos", false },
                    { 25, 2, "Neu", false },
                    { 26, 3, "Neg", false }
                }
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
