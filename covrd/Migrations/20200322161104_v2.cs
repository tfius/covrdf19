using Microsoft.EntityFrameworkCore.Migrations;

namespace covrd.Migrations
{
    public partial class v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Features",
                table: "Papers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tokens",
                table: "Papers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Features",
                table: "Papers");

            migrationBuilder.DropColumn(
                name: "Tokens",
                table: "Papers");
        }
    }
}
