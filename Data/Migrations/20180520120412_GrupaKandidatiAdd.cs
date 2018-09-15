using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CZE.Data.Migrations
{
    public partial class GrupaKandidatiAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Grupa_Kandidati",
                columns: table => new
                {
                    GrupaKandidatiId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GrupaId = table.Column<int>(nullable: false),
                    KandidatId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grupa_Kandidati", x => x.GrupaKandidatiId);
                    table.ForeignKey(
                        name: "FK_Grupa_Kandidati_Grupe_GrupaId",
                        column: x => x.GrupaId,
                        principalTable: "Grupe",
                        principalColumn: "GrupaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Grupa_Kandidati_Kandidati_KandidatId",
                        column: x => x.KandidatId,
                        principalTable: "Kandidati",
                        principalColumn: "KandidatId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Grupa_Kandidati_GrupaId",
                table: "Grupa_Kandidati",
                column: "GrupaId");

            migrationBuilder.CreateIndex(
                name: "IX_Grupa_Kandidati_KandidatId",
                table: "Grupa_Kandidati",
                column: "KandidatId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Grupa_Kandidati");
        }
    }
}
