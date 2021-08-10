using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FedSurvey.Migrations
{
    public partial class Views : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Views",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Views", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ViewConfigs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ViewId = table.Column<int>(type: "int", nullable: false),
                    VariableName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    VariableValue = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ViewConfigs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ViewConfigs_Views_ViewId",
                        column: x => x.ViewId,
                        principalTable: "Views",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ViewConfigs_ViewId",
                table: "ViewConfigs",
                column: "ViewId");

            // Maybe would be better outside of a migration, but time constraints has led it to be here.
            migrationBuilder.InsertData(
                table: "Views",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Top Positive Responses" },
                    { 2, "Top Neutral Responses" },
                    { 3, "Top Negative Responses" },
                    { 4, "Top Positive Responses Compared to 2016 on" },
                    { 5, "Top Negative Responses Compared to 2016 on" },
                    { 6, "Top Positive Response Increases" },
                    { 7, "Top Positive Response Decreases" },
                    { 8, "Top Positive Response Strengths Relative to DPCPSI" },
                    { 9, "Top Positive Response Weaknesses Relative to DPCPSI" }
                }
            );

            // Start with one view as a test.
            migrationBuilder.InsertData(
                table: "ViewConfigs",
                columns: new[] { "Id", "ViewId", "VariableName", "VariableValue" },
                values: new object[,]
                {
                    // Top Positive Responses
                    { 1, 1, "sortingVariable", "dataGroupName" },
                    { 2, 1, "groupingVariable", "questionId" },
                    { 3, 1, "filters.possible-response-names", "Positive" },
                    { 4, 1, "filters.execution-keys", "$(latestExecutionNames1)" }, // arbitrary replacement
                    { 5, 1, "filters.data-group-names", "OSC TOTAL" },
                    { 6, 1, "filters.data-group-names", "DIV PRGM COORD, PLNG & STRATEGIC INITIATIVES" },
                    { 7, 1, "filters.data-group-names", "OFFICE OF THE DIRECTOR (OD)" },
                    { 8, 1, "filters.question-group-ids", "1" },
                    { 9, 1, "sort.header", "OSC TOTAL" },
                    { 10, 1, "sort.direction", "desc" },

                    // Top Neutral Responses
                    { 11, 2, "sortingVariable", "dataGroupName" },
                    { 12, 2, "groupingVariable", "questionId" },
                    { 13, 2, "filters.possible-response-names", "Neutral" },
                    { 14, 2, "filters.execution-keys", "$(latestExecutionNames1)" }, // arbitrary replacement
                    { 15, 2, "filters.data-group-names", "OSC TOTAL" },
                    { 16, 2, "filters.data-group-names", "DIV PRGM COORD, PLNG & STRATEGIC INITIATIVES" },
                    { 17, 2, "filters.data-group-names", "OFFICE OF THE DIRECTOR (OD)" },
                    { 18, 2, "filters.question-group-ids", "1" },
                    { 19, 2, "sort.header", "OSC TOTAL" },
                    { 20, 2, "sort.direction", "desc" },

                    // Top Negative Responses
                    { 21, 3, "sortingVariable", "dataGroupName" },
                    { 22, 3, "groupingVariable", "questionId" },
                    { 23, 3, "filters.possible-response-names", "Negative" },
                    { 24, 3, "filters.execution-keys", "$(latestExecutionNames1)" }, // arbitrary replacement
                    { 25, 3, "filters.data-group-names", "OSC TOTAL" },
                    { 26, 3, "filters.data-group-names", "DIV PRGM COORD, PLNG & STRATEGIC INITIATIVES" },
                    { 27, 3, "filters.data-group-names", "OFFICE OF THE DIRECTOR (OD)" },
                    { 28, 3, "filters.question-group-ids", "1" },
                    { 29, 3, "sort.header", "OSC TOTAL" },
                    { 30, 3, "sort.direction", "desc" },

                    // Top Positive Responses Compared to 2016 on
                    { 31, 4, "sortingVariable", "executionName" },
                    { 32, 4, "groupingVariable", "questionId" },
                    { 33, 4, "filters.possible-response-names", "Positive" },
                    { 34, 4, "filters.data-group-names", "OSC TOTAL" },
                    { 35, 4, "filters.question-group-ids", "1" },
                    { 36, 4, "showDifference", "true" },
                    { 37, 4, "sort.header", "$(latestExecutionNames1)" },
                    { 38, 4, "sort.direction", "desc" },

                    // Top Negative Responses Compared to 2016 on
                    { 39, 5, "sortingVariable", "executionName" },
                    { 40, 5, "groupingVariable", "questionId" },
                    { 41, 5, "filters.possible-response-names", "Negative" },
                    { 42, 5, "filters.data-group-names", "OSC TOTAL" },
                    { 43, 5, "filters.question-group-ids", "1" },
                    { 44, 5, "showDifference", "true" },
                    { 45, 5, "sort.header", "$(latestExecutionNames1)" },
                    { 46, 5, "sort.direction", "desc" },

                    // Top Positive Response Increases
                    { 47, 6, "sortingVariable", "executionName" },
                    { 48, 6, "groupingVariable", "questionId" },
                    { 49, 6, "filters.possible-response-names", "Positive" },
                    { 50, 6, "filters.data-group-names", "OSC TOTAL" },
                    { 51, 6, "filters.execution-keys", "$(latestExecutionNames2)" },
                    { 52, 6, "filters.question-group-ids", "1" },
                    { 53, 6, "showDifference", "true" },
                    { 54, 6, "sort.index", "2" },
                    { 55, 6, "sort.direction", "desc" },

                    // Top Positive Response Decreases
                    { 56, 7, "sortingVariable", "executionName" },
                    { 57, 7, "groupingVariable", "questionId" },
                    { 58, 7, "filters.possible-response-names", "Positive" },
                    { 59, 7, "filters.data-group-names", "OSC TOTAL" },
                    { 60, 7, "filters.execution-keys", "$(latestExecutionNames2)" },
                    { 61, 7, "filters.question-group-ids", "1" },
                    { 62, 7, "showDifference", "true" },
                    { 63, 7, "sort.index", "2" },
                    { 64, 7, "sort.direction", "asc" },

                    // Top Positive Response Strengths Relative to DPCPSI
                    { 65, 8, "sortingVariable", "dataGroupName" },
                    { 66, 8, "groupingVariable", "questionId" },
                    { 67, 8, "filters.possible-response-names", "Positive" },
                    { 68, 8, "filters.data-group-names", "OSC TOTAL" },
                    { 69, 8, "filters.data-group-names", "DIV PRGM COORD, PLNG & STRATEGIC INITIATIVES" },
                    { 70, 8, "filters.execution-keys", "$(latestExecutionNames1)" },
                    { 71, 8, "filters.question-group-ids", "1" },
                    { 72, 8, "showDifference", "true" },
                    { 73, 8, "sort.index", "2" },
                    { 74, 8, "sort.direction", "desc" },

                    // Top Positive Response Weaknesses Relative to DPCPSI
                    { 75, 9, "sortingVariable", "dataGroupName" },
                    { 76, 9, "groupingVariable", "questionId" },
                    { 77, 9, "filters.possible-response-names", "Positive" },
                    { 78, 9, "filters.data-group-names", "OSC TOTAL" },
                    { 79, 9, "filters.data-group-names", "DIV PRGM COORD, PLNG & STRATEGIC INITIATIVES" },
                    { 80, 9, "filters.execution-keys", "$(latestExecutionNames1)" },
                    { 81, 9, "filters.question-group-ids", "1" },
                    { 82, 9, "showDifference", "true" },
                    { 83, 9, "sort.index", "2" },
                    { 84, 9, "sort.direction", "asc" },
                }
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ViewConfigs");

            migrationBuilder.DropTable(
                name: "Views");
        }
    }
}
