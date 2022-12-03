using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateApp.Infrastructure.Identity.Migrations
{
    public partial class ImagePathAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                schema: "Identity",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                schema: "Identity",
                table: "Users");
        }
    }
}
