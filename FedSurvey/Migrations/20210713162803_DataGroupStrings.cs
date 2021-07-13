using Microsoft.EntityFrameworkCore.Migrations;

namespace FedSurvey.Migrations
{
    public partial class DataGroupStrings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionTypeStrings_QuestionTypes_QuestionTypeId",
                table: "QuestionTypeStrings");

            migrationBuilder.CreateTable(
                name: "DataGroupStrings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataGroupId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(300)", unicode: false, maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataGroupStrings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataGroupStrings_DataGroups_DataGroupId",
                        column: x => x.DataGroupId,
                        principalTable: "DataGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.Sql("INSERT INTO DataGroupStrings (DataGroupId, Name) SELECT Id AS DataGroupId, Name FROM DataGroups");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "DataGroups");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "QuestionTypeStrings",
                type: "varchar(300)",
                unicode: false,
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300);

            migrationBuilder.CreateIndex(
                name: "IX_DataGroupStrings_DataGroupId",
                table: "DataGroupStrings",
                column: "DataGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionTypeStrings_QuestionTypes_QuestionTypeId",
                table: "QuestionTypeStrings",
                column: "QuestionTypeId",
                principalTable: "QuestionTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionTypeStrings_QuestionTypes_QuestionTypeId",
                table: "QuestionTypeStrings");

            migrationBuilder.DropTable(
                name: "DataGroupStrings");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "QuestionTypeStrings",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(300)",
                oldUnicode: false,
                oldMaxLength: 300);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "DataGroups",
                type: "varchar(300)",
                unicode: false,
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionTypeStrings_QuestionTypes_QuestionTypeId",
                table: "QuestionTypeStrings",
                column: "QuestionTypeId",
                principalTable: "QuestionTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
