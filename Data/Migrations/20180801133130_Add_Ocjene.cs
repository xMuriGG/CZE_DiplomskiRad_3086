using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CZE.Data.Migrations
{
    public partial class Add_Ocjene : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ocjene",
                columns: table => new
                {
                    OcjenaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GrupaKandidatiId = table.Column<int>(nullable: false),
                    Ospis = table.Column<string>(maxLength: 300, nullable: true),
                    Silenced = table.Column<bool>(nullable: false, defaultValue: false),
                    Vrijednost = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ocjene", x => x.OcjenaId);
                    table.ForeignKey(
                        name: "FK_Ocjene_Grupa_Kandidati_GrupaKandidatiId",
                        column: x => x.GrupaKandidatiId,
                        principalTable: "Grupa_Kandidati",
                        principalColumn: "GrupaKandidatiId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ocjene_GrupaKandidatiId",
                table: "Ocjene",
                column: "GrupaKandidatiId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ocjene");
        }
    }
}
