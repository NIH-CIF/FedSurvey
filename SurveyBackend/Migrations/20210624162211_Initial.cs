using Microsoft.EntityFrameworkCore.Migrations;

namespace Survey.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DataGroups",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(unicode: false, maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Executions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Key = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Executions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuestionTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(unicode: false, maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PossibleResponses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    QuestionTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PossibleResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PossibleResponses_QuestionTypes",
                        column: x => x.QuestionTypeId,
                        principalTable: "QuestionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    QuestionTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_QuestionTypes",
                        column: x => x.QuestionTypeId,
                        principalTable: "QuestionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PossibleResponseStrings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    PossibleResponseId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(unicode: false, maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PossibleResponseStrings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PossibleResponseStrings_PossibleResponses",
                        column: x => x.PossibleResponseId,
                        principalTable: "PossibleResponses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuestionExecutions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Position = table.Column<int>(nullable: false),
                    Body = table.Column<string>(type: "text", nullable: false),
                    ExecutionId = table.Column<int>(nullable: false),
                    QuestionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionExecutions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionExecutions_Executions",
                        column: x => x.ExecutionId,
                        principalTable: "Executions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionExecutions_Questions",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Responses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Count = table.Column<decimal>(type: "decimal(18, 0)", nullable: false),
                    QuestionExecutionId = table.Column<int>(nullable: false),
                    PossibleResponseId = table.Column<int>(nullable: false),
                    DataGroupId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Responses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Responses_DataGroups",
                        column: x => x.DataGroupId,
                        principalTable: "DataGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Responses_PossibleResponses",
                        column: x => x.PossibleResponseId,
                        principalTable: "PossibleResponses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Responses_QuestionExecutions",
                        column: x => x.QuestionExecutionId,
                        principalTable: "QuestionExecutions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "UQ__Executio__C41E02890F7150F6",
                table: "Executions",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PossibleResponses_QuestionTypeId",
                table: "PossibleResponses",
                column: "QuestionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PossibleResponseStrings_PossibleResponseId",
                table: "PossibleResponseStrings",
                column: "PossibleResponseId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionExecutions_ExecutionId",
                table: "QuestionExecutions",
                column: "ExecutionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionExecutions_QuestionId",
                table: "QuestionExecutions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_QuestionTypeId",
                table: "Questions",
                column: "QuestionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Responses_DataGroupId",
                table: "Responses",
                column: "DataGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Responses_PossibleResponseId",
                table: "Responses",
                column: "PossibleResponseId");

            migrationBuilder.CreateIndex(
                name: "IX_Responses_QuestionExecutionId",
                table: "Responses",
                column: "QuestionExecutionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PossibleResponseStrings");

            migrationBuilder.DropTable(
                name: "Responses");

            migrationBuilder.DropTable(
                name: "DataGroups");

            migrationBuilder.DropTable(
                name: "PossibleResponses");

            migrationBuilder.DropTable(
                name: "QuestionExecutions");

            migrationBuilder.DropTable(
                name: "Executions");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "QuestionTypes");
        }
    }
}
