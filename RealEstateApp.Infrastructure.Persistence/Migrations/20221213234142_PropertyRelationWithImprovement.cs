using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateApp.Infrastructure.Persistence.Migrations
{
    public partial class PropertyRelationWithImprovement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_Improvements_TypeOfPropertyId",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "ImprovementsId",
                table: "Properties");

            migrationBuilder.AlterColumn<string>(
                name: "ImagePathOne",
                table: "Properties",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "ImprovementsProperties",
                columns: table => new
                {
                    ImprovementsId = table.Column<int>(type: "int", nullable: false),
                    PropertiesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImprovementsProperties", x => new { x.ImprovementsId, x.PropertiesId });
                    table.ForeignKey(
                        name: "FK_ImprovementsProperties_Improvements_ImprovementsId",
                        column: x => x.ImprovementsId,
                        principalTable: "Improvements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ImprovementsProperties_Properties_PropertiesId",
                        column: x => x.PropertiesId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImprovementsProperties_PropertiesId",
                table: "ImprovementsProperties",
                column: "PropertiesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImprovementsProperties");

            migrationBuilder.AlterColumn<string>(
                name: "ImagePathOne",
                table: "Properties",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ImprovementsId",
                table: "Properties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_Improvements_TypeOfPropertyId",
                table: "Properties",
                column: "TypeOfPropertyId",
                principalTable: "Improvements",
                principalColumn: "Id");
        }
    }
}
