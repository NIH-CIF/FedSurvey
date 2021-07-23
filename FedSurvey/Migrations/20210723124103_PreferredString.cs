using Microsoft.EntityFrameworkCore.Migrations;

namespace FedSurvey.Migrations
{
    public partial class PreferredString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Preferred",
                table: "QuestionTypeStrings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Preferred",
                table: "PossibleResponseStrings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Preferred",
                table: "DataGroupStrings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "DataGroupStrings",
                keyColumn: "Id",
                keyValue: 1,
                column: "Preferred",
                value: true
            );

            migrationBuilder.UpdateData(
                table: "PossibleResponseStrings",
                keyColumn: "Id",
                keyValue: 1,
                column: "Preferred",
                value: true
            );

            migrationBuilder.UpdateData(
                table: "PossibleResponseStrings",
                keyColumn: "Id",
                keyValue: 2,
                column: "Preferred",
                value: true
            );

            migrationBuilder.UpdateData(
                table: "PossibleResponseStrings",
                keyColumn: "Id",
                keyValue: 3,
                column: "Preferred",
                value: true
            );

            migrationBuilder.UpdateData(
                table: "PossibleResponseStrings",
                keyColumn: "Id",
                keyValue: 4,
                column: "Preferred",
                value: true
            );

            migrationBuilder.UpdateData(
                table: "PossibleResponseStrings",
                keyColumn: "Id",
                keyValue: 10,
                column: "Preferred",
                value: true
            );

            migrationBuilder.UpdateData(
                table: "QuestionTypeStrings",
                keyColumn: "Id",
                keyValue: 1,
                column: "Preferred",
                value: true
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Preferred",
                table: "QuestionTypeStrings");

            migrationBuilder.DropColumn(
                name: "Preferred",
                table: "PossibleResponseStrings");

            migrationBuilder.DropColumn(
                name: "Preferred",
                table: "DataGroupStrings");
        }
    }
}
