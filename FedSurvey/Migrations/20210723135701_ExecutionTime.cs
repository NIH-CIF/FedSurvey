using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FedSurvey.Migrations
{
    public partial class ExecutionTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "OccurredTime",
                table: "Executions",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.UpdateData(
                table: "Executions",
                keyColumn: "Id",
                keyValue: 1,
                column: "OccurredTime",
                value: new DateTime(2016, 1, 1)
            );

            migrationBuilder.UpdateData(
                table: "Executions",
                keyColumn: "Id",
                keyValue: 2,
                column: "OccurredTime",
                value: new DateTime(2017, 1, 1)
            );

            migrationBuilder.UpdateData(
                table: "Executions",
                keyColumn: "Id",
                keyValue: 3,
                column: "OccurredTime",
                value: new DateTime(2018, 1, 1)
            );

            migrationBuilder.UpdateData(
                table: "Executions",
                keyColumn: "Id",
                keyValue: 4,
                column: "OccurredTime",
                value: new DateTime(2019, 1, 1)
            );

            migrationBuilder.UpdateData(
                table: "Executions",
                keyColumn: "Id",
                keyValue: 5,
                column: "OccurredTime",
                value: new DateTime(2020, 1, 1)
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OccurredTime",
                table: "Executions");
        }
    }
}
