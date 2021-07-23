using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FedSurvey.Migrations
{
    public partial class Timestamps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedTime",
                table: "Responses",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedTime",
                table: "Responses",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedTime",
                table: "QuestionTypeStrings",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedTime",
                table: "QuestionTypeStrings",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedTime",
                table: "QuestionTypes",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedTime",
                table: "QuestionTypes",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedTime",
                table: "Questions",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedTime",
                table: "Questions",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedTime",
                table: "QuestionExecutions",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedTime",
                table: "QuestionExecutions",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedTime",
                table: "PossibleResponseStrings",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedTime",
                table: "PossibleResponseStrings",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedTime",
                table: "PossibleResponses",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedTime",
                table: "PossibleResponses",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedTime",
                table: "Executions",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedTime",
                table: "Executions",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedTime",
                table: "DataGroupStrings",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedTime",
                table: "DataGroupStrings",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedTime",
                table: "DataGroups",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedTime",
                table: "DataGroups",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedTime",
                table: "Responses");

            migrationBuilder.DropColumn(
                name: "UpdatedTime",
                table: "Responses");

            migrationBuilder.DropColumn(
                name: "CreatedTime",
                table: "QuestionTypeStrings");

            migrationBuilder.DropColumn(
                name: "UpdatedTime",
                table: "QuestionTypeStrings");

            migrationBuilder.DropColumn(
                name: "CreatedTime",
                table: "QuestionTypes");

            migrationBuilder.DropColumn(
                name: "UpdatedTime",
                table: "QuestionTypes");

            migrationBuilder.DropColumn(
                name: "CreatedTime",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "UpdatedTime",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "CreatedTime",
                table: "QuestionExecutions");

            migrationBuilder.DropColumn(
                name: "UpdatedTime",
                table: "QuestionExecutions");

            migrationBuilder.DropColumn(
                name: "CreatedTime",
                table: "PossibleResponseStrings");

            migrationBuilder.DropColumn(
                name: "UpdatedTime",
                table: "PossibleResponseStrings");

            migrationBuilder.DropColumn(
                name: "CreatedTime",
                table: "PossibleResponses");

            migrationBuilder.DropColumn(
                name: "UpdatedTime",
                table: "PossibleResponses");

            migrationBuilder.DropColumn(
                name: "CreatedTime",
                table: "Executions");

            migrationBuilder.DropColumn(
                name: "UpdatedTime",
                table: "Executions");

            migrationBuilder.DropColumn(
                name: "CreatedTime",
                table: "DataGroupStrings");

            migrationBuilder.DropColumn(
                name: "UpdatedTime",
                table: "DataGroupStrings");

            migrationBuilder.DropColumn(
                name: "CreatedTime",
                table: "DataGroups");

            migrationBuilder.DropColumn(
                name: "UpdatedTime",
                table: "DataGroups");
        }
    }
}
