using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CZE.Data.Migrations
{
    public partial class GrupaKandidati_CompositeAKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Grupa_Kandidati_KandidatId",
                table: "Grupa_Kandidati");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Grupa_Kandidati_KandidatId_GrupaId",
                table: "Grupa_Kandidati",
                columns: new[] { "KandidatId", "GrupaId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Grupa_Kandidati_KandidatId_GrupaId",
                table: "Grupa_Kandidati");

            migrationBuilder.CreateIndex(
                name: "IX_Grupa_Kandidati_KandidatId",
                table: "Grupa_Kandidati",
                column: "KandidatId");
        }
    }
}
