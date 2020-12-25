using Microsoft.EntityFrameworkCore.Migrations;

namespace covrd.Migrations
{
    public partial class v4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Tokens",
                table: "Abstract",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tokens",
                table: "Abstract");
        }
    }
}
