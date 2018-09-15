using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CZE.Data.Migrations
{
    public partial class Slika_Kategorija : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "KursKategorijaId",
                table: "Slike",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Slike_KursKategorijaId",
                table: "Slike",
                column: "KursKategorijaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Slike_KursKategorije_KursKategorijaId",
                table: "Slike",
                column: "KursKategorijaId",
                principalTable: "KursKategorije",
                principalColumn: "KursKategorijaId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Slike_KursKategorije_KursKategorijaId",
                table: "Slike");

            migrationBuilder.DropIndex(
                name: "IX_Slike_KursKategorijaId",
                table: "Slike");

            migrationBuilder.DropColumn(
                name: "KursKategorijaId",
                table: "Slike");
        }
    }
}
