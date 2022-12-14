using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateApp.Infrastructure.Persistence.Migrations
{
    public partial class RelationshipPropertyImprovement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImprovementsProperties");

            migrationBuilder.CreateTable(
                name: "PropertiesImprovements",
                columns: table => new
                {
                    PropertyId = table.Column<int>(type: "int", nullable: false),
                    ImprovementId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertiesImprovements", x => new { x.PropertyId, x.ImprovementId });
                    table.ForeignKey(
                        name: "FK_PropertiesImprovements_Improvements_ImprovementId",
                        column: x => x.ImprovementId,
                        principalTable: "Improvements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PropertiesImprovements_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PropertiesImprovements_ImprovementId",
                table: "PropertiesImprovements",
                column: "ImprovementId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PropertiesImprovements");

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
    }
}
