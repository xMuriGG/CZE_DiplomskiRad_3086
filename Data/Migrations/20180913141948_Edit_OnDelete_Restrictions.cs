using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CZE.Data.Migrations
{
    public partial class Edit_OnDelete_Restrictions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grupa_Kandidati_Grupe_GrupaId",
                table: "Grupa_Kandidati");

            migrationBuilder.DropForeignKey(
                name: "FK_PrisustvoTermin_Grupe_GrupaId",
                table: "PrisustvoTermin");

            migrationBuilder.AddForeignKey(
                name: "FK_Grupa_Kandidati_Grupe_GrupaId",
                table: "Grupa_Kandidati",
                column: "GrupaId",
                principalTable: "Grupe",
                principalColumn: "GrupaId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PrisustvoTermin_Grupe_GrupaId",
                table: "PrisustvoTermin",
                column: "GrupaId",
                principalTable: "Grupe",
                principalColumn: "GrupaId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grupa_Kandidati_Grupe_GrupaId",
                table: "Grupa_Kandidati");

            migrationBuilder.DropForeignKey(
                name: "FK_PrisustvoTermin_Grupe_GrupaId",
                table: "PrisustvoTermin");

            migrationBuilder.AddForeignKey(
                name: "FK_Grupa_Kandidati_Grupe_GrupaId",
                table: "Grupa_Kandidati",
                column: "GrupaId",
                principalTable: "Grupe",
                principalColumn: "GrupaId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PrisustvoTermin_Grupe_GrupaId",
                table: "PrisustvoTermin",
                column: "GrupaId",
                principalTable: "Grupe",
                principalColumn: "GrupaId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
