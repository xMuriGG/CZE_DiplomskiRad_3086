using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CZE.Data.Migrations
{
    public partial class Edit_Grupa_Kandidat_OnDeleteRestrict : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grupa_Kandidati_Kandidati_KandidatId",
                table: "Grupa_Kandidati");

            migrationBuilder.AddForeignKey(
                name: "FK_Grupa_Kandidati_Kandidati_KandidatId",
                table: "Grupa_Kandidati",
                column: "KandidatId",
                principalTable: "Kandidati",
                principalColumn: "KandidatId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grupa_Kandidati_Kandidati_KandidatId",
                table: "Grupa_Kandidati");

            migrationBuilder.AddForeignKey(
                name: "FK_Grupa_Kandidati_Kandidati_KandidatId",
                table: "Grupa_Kandidati",
                column: "KandidatId",
                principalTable: "Kandidati",
                principalColumn: "KandidatId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
