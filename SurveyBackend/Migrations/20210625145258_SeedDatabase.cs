using CsvHelper;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Globalization;
using System.IO;

namespace Survey.Migrations
{
    public partial class SeedDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Initialize the base question type.
            migrationBuilder.InsertData(
                table: "QuestionTypes",
                columns: new[] { "Name" },
                values: new object[] { "Core Survey" }
            );
            Console.WriteLine(migrationBuilder.Sql("SCOPE_IDENTITY();"));
            return;

            Console.WriteLine(migrationBuilder.InsertData(
                table: "Questions",
                columns: new string[0],
                values: new object[0]
            ));

            using (StreamReader reader = new StreamReader("Models\\SeedData\\questions.csv"))
            using (CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();

                while (csv.Read())
                {
                    
                }
            }
            //migrationBuilder.InsertData(
            //    table: "Executions",
            //    columns: new[] { "Key" },
            //    values: new object[] { "2016" }
            //);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Process will be to destroy all data from tables that were created in this migration.
        }
    }
}
