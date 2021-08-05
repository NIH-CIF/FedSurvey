using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FedSurvey.Migrations
{
    public partial class QuestionGroups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuestionGroups",
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
                    table.PrimaryKey("PK_QuestionGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuestionLinks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionGroupId = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionLinks_QuestionGroups_QuestionGroupId",
                        column: x => x.QuestionGroupId,
                        principalTable: "QuestionGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionLinks_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuestionLinks_QuestionGroupId",
                table: "QuestionLinks",
                column: "QuestionGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionLinks_QuestionId",
                table: "QuestionLinks",
                column: "QuestionId");

            migrationBuilder.InsertData(
                table: "QuestionGroups",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Core Survey" }
            );

            migrationBuilder.Sql("INSERT INTO QuestionLinks (QuestionGroupId, QuestionId) SELECT 1 AS QuestionGroupId, Id FROM Questions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuestionLinks");

            migrationBuilder.DropTable(
                name: "QuestionGroups");
        }
    }
}
