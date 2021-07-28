using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FedSurvey.Migrations
{
    public partial class DataGroupLinks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DataGroupLinks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentId = table.Column<int>(type: "int", nullable: false),
                    ChildId = table.Column<int>(type: "int", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataGroupLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataGroupLinks_DataGroups_ChildId",
                        column: x => x.ChildId,
                        principalTable: "DataGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    // only putting NoAction due to SQL error - reconsider later
                    table.ForeignKey(
                        name: "FK_DataGroupLinks_DataGroups_ParentId",
                        column: x => x.ParentId,
                        principalTable: "DataGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    // only putting NoAction due to SQL error
                });

            migrationBuilder.CreateIndex(
                name: "IX_DataGroupLinks_ChildId",
                table: "DataGroupLinks",
                column: "ChildId");

            migrationBuilder.CreateIndex(
                name: "IX_DataGroupLinks_ParentId",
                table: "DataGroupLinks",
                column: "ParentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataGroupLinks");
        }
    }
}
