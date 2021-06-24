using Microsoft.EntityFrameworkCore.Migrations;
using Survey.Services;

namespace Survey.Migrations
{
    public partial class SeedDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            UploadService service = new Services.UploadService();
            service.Test();

            migrationBuilder.InsertData(
                table: "Executions",
                columns: new[] { "Key" },
                values: new object[] { "2016" }
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
