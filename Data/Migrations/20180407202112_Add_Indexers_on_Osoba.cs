using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CZE.Data.Migrations
{
    public partial class Add_Indexers_on_Osoba : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Osobe_Email",
                table: "Osobe",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Osobe_Ime_Prezime",
                table: "Osobe",
                columns: new[] { "Ime", "Prezime" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Osobe_Email",
                table: "Osobe");

            migrationBuilder.DropIndex(
                name: "IX_Osobe_Ime_Prezime",
                table: "Osobe");
        }
    }
}
