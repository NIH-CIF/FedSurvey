using CsvHelper;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Globalization;
using System.IO;

namespace FedSurvey.Migrations
{
    public partial class DummySeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "QuestionTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Core Survey" }
            );

            migrationBuilder.InsertData(
                table: "PossibleResponses",
                columns: new[] { "Id", "QuestionTypeId" },
                values: new object[,] { { 1, 1 }, { 2, 1 }, { 3, 1 } }
            );

            migrationBuilder.InsertData(
                table: "PossibleResponseStrings",
                columns: new[] { "Id", "PossibleResponseId", "Name" },
                values: new object[,] { { 1, 1, "Positive" }, { 2, 2, "Neutral" }, { 3, 3, "Negative" } }
            );

            migrationBuilder.InsertData(
                table: "DataGroups",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Dummy" }
            );

            migrationBuilder.InsertData(
                table: "Executions",
                columns: new[] { "Id", "Key" },
                values: new object[,] { { 1, "2016" }, { 2, "2017" }, { 3, "2018" }, { 4, "2019" }, { 5, "2020" } }
            );

            // Actual response data needs to be seeded from a question database.
            if (!File.Exists("Models\\SeedData\\questions.csv"))
            {
                return;
            }

            int QuestionId = 1;
            int QuestionExecutionId = 1;

            // May be worth rewriting this using the ExcelDataReader CSV reader to use fewer libraries.
            using (var reader = new StreamReader("Models\\SeedData\\questions.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();

                string[] keys = new string[] { "2016", "2017", "2018", "2019", "2020" };

                while (csv.Read())
                {
                    migrationBuilder.InsertData(
                        table: "Questions",
                        columns: new[] { "Id", "QuestionTypeId" },
                        values: new object[] { QuestionId, 1 }
                    );

                    string[] questionText = new string[5];

                    for (int i = 0; i < 5; i++)
                    {
                        string key = keys[i];
                        int yearInt = Int32.Parse(key);

                        if (yearInt >= 2020 && !String.IsNullOrEmpty(csv.GetField("2020 Text Change")))
                        {
                            questionText[i] = csv.GetField("2020 Text Change");
                        }
                        else if (yearInt >= 2018 && !String.IsNullOrEmpty(csv.GetField("2018 Text Change")))
                        {
                            questionText[i] = csv.GetField("2018 Text Change");
                        }
                        else
                        {
                            questionText[i] = csv.GetField("Question Text");
                        }
                    }

                    for (int i = 0; i < 5; i++)
                    {
                        if (String.IsNullOrEmpty(csv.GetField(keys[i])))
                        {
                            continue;
                        }

                        migrationBuilder.InsertData(
                            table: "QuestionExecutions",
                            columns: new[] { "Id", "QuestionId", "ExecutionId", "Position", "Body" },
                            values: new object[]
                            {
                                QuestionExecutionId,
                                QuestionId,
                                i + 1,
                                csv.GetField<int>(keys[i]),
                                questionText[i]
                            }
                        );

                        Random rand = new Random();

                        // Need to figure out why this is giving integer values,
                        // but will build API routes first.
                        migrationBuilder.InsertData(
                            table: "Responses",
                            columns: new[] { "QuestionExecutionId", "PossibleResponseId", "DataGroupId", "Count" },
                            values: new object[,]
                            {
                                {
                                    QuestionExecutionId,
                                    1,
                                    1,
                                    rand.NextDouble() * 50.0
                                },
                                {
                                    QuestionExecutionId,
                                    2,
                                    1,
                                    rand.NextDouble() * 50.0
                                },
                                {
                                    QuestionExecutionId,
                                    3,
                                    1,
                                    rand.NextDouble() * 50.0
                                },
                            }
                        );

                        QuestionExecutionId++;
                    }

                    QuestionId++;
                }
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM QuestionTypes");
            migrationBuilder.Sql("DELETE FROM PossibleResponses");
            migrationBuilder.Sql("DELETE FROM PossibleResponseStrings");
            migrationBuilder.Sql("DELETE FROM DataGroups");
            migrationBuilder.Sql("DELETE FROM Executions");
        }
    }
}
