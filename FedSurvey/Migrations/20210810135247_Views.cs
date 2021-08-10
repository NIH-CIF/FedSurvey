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
                    { 1, 1, "sortingVariable", "dataGroupName" },
                    { 2, 1, "groupingVariable", "questionId" },
                    { 3, 1, "filters.possible-response-names", "Positive" },
                    { 4, 1, "filters.execution-keys", "$(latestExecutionNames1)" }, // arbitrary replacement
                    { 5, 1, "filters.data-group-names", "OSC TOTAL" },
                    { 6, 1, "filters.data-group-names", "DIV PRGM COORD, PLNG & STRATEGIC INITIATIVES" },
                    { 7, 1, "filters.data-group-names", "OFFICE OF THE DIRECTOR (OD)" },
                    { 8, 1, "filters.question-group-ids", "1" },
                    { 9, 1, "sort.header", "OSC TOTAL" },
                    { 10, 1, "sort.direction", "desc" }
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
