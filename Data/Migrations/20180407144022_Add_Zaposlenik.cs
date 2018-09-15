using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CZE.Data.Migrations
{
    public partial class Add_Zaposlenik : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kandidat_Osobe_KandidatId",
                table: "Kandidat");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Kandidat",
                table: "Kandidat");

            migrationBuilder.RenameTable(
                name: "Kandidat",
                newName: "Kandidati");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Kandidati",
                table: "Kandidati",
                column: "KandidatId");

            migrationBuilder.CreateTable(
                name: "Zaposlenici",
                columns: table => new
                {
                    ZaposlenikId = table.Column<int>(nullable: false),
                    BrojLicneKarte = table.Column<string>(maxLength: 9, nullable: false),
                    BrojRacuna = table.Column<string>(maxLength: 20, nullable: false),
                    StepenObrazovanja = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zaposlenici", x => x.ZaposlenikId);
                    table.ForeignKey(
                        name: "FK_Zaposlenici_Osobe_ZaposlenikId",
                        column: x => x.ZaposlenikId,
                        principalTable: "Osobe",
                        principalColumn: "OsobaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Kandidati_Osobe_KandidatId",
                table: "Kandidati",
                column: "KandidatId",
                principalTable: "Osobe",
                principalColumn: "OsobaId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kandidati_Osobe_KandidatId",
                table: "Kandidati");

            migrationBuilder.DropTable(
                name: "Zaposlenici");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Kandidati",
                table: "Kandidati");

            migrationBuilder.RenameTable(
                name: "Kandidati",
                newName: "Kandidat");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Kandidat",
                table: "Kandidat",
                column: "KandidatId");

            migrationBuilder.AddForeignKey(
                name: "FK_Kandidat_Osobe_KandidatId",
                table: "Kandidat",
                column: "KandidatId",
                principalTable: "Osobe",
                principalColumn: "OsobaId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
