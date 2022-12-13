using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateApp.Infrastructure.Identity.Migrations
{
    public partial class AddIDCardtoUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IDCard",
                schema: "Identity",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IDCard",
                schema: "Identity",
                table: "Users");
        }
    }
}
