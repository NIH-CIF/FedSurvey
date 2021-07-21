using Microsoft.EntityFrameworkCore.Migrations;

namespace FedSurvey.Migrations
{
    public partial class SurveyMonkeyPossibleResponseStrings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PossibleResponses",
                columns: new[] { "Id", "QuestionTypeId" },
                values: new object[,]
                {
                    { 5, 1 }
                }
            );

            migrationBuilder.InsertData(
                table: "PossibleResponseStrings",
                columns: new[] { "Id", "PossibleResponseId", "Name" },
                values: new object[,]
                {
                    { 5, 1, "Strongly agree" },
                    { 6, 1, "Agree" },
                    { 7, 2, "Neither agree nor disagree" },
                    { 8, 3, "Disagree" },
                    { 9, 3, "Strongly disagree" },
                    { 10, 5, "Skipped" },
                    { 11, 4, "Do Not Know" },
                    { 12, 4, "No Basis to Judge" },
                    // wut
                    { 13, 4, "No Not Know" },
                    { 14, 1, "Very Good" },
                    { 15, 1, "Good" },
                    { 16, 2, "Fair" },
                    { 17, 3, "Poor" },
                    { 18, 3, "Very Poor" },
                    { 19, 1, "Very satisfied" },
                    { 20, 1, "Satisfied" },
                    { 21, 2, "Neither satisfied nor dissatisfied" },
                    { 22, 3, "Dissatisfied" },
                    { 23, 3, "Very dissatisfied" }
                }
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
