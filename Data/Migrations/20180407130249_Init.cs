using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CZE.Data.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Gradovi",
                columns: table => new
                {
                    GradId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Naziv = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gradovi", x => x.GradId);
                });

            migrationBuilder.CreateTable(
                name: "Osobe",
                columns: table => new
                {
                    OsobaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Adresa = table.Column<string>(maxLength: 100, nullable: false),
                    AdresaFirme = table.Column<string>(maxLength: 100, nullable: true),
                    BrojMobitela = table.Column<string>(maxLength: 15, nullable: false),
                    BrojTelefona = table.Column<string>(maxLength: 15, nullable: true),
                    BrojTelefonaFirme = table.Column<string>(maxLength: 15, nullable: true),
                    DatumRodjenja = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(maxLength: 100, nullable: false),
                    EmailFirme = table.Column<string>(maxLength: 100, nullable: true),
                    GradFirmeId = table.Column<int>(nullable: true),
                    Ime = table.Column<string>(maxLength: 50, nullable: false),
                    MjestoBoravkaId = table.Column<int>(nullable: false),
                    MjestoRodjenjaId = table.Column<int>(nullable: false),
                    NazivFirme = table.Column<string>(maxLength: 100, nullable: true),
                    PodaciOFirmi = table.Column<bool>(nullable: false),
                    Prezime = table.Column<string>(maxLength: 50, nullable: false),
                    Spol = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Osobe", x => x.OsobaId);
                    table.ForeignKey(
                        name: "FK_Osobe_Gradovi_GradFirmeId",
                        column: x => x.GradFirmeId,
                        principalTable: "Gradovi",
                        principalColumn: "GradId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Osobe_Gradovi_MjestoBoravkaId",
                        column: x => x.MjestoBoravkaId,
                        principalTable: "Gradovi",
                        principalColumn: "GradId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Osobe_Gradovi_MjestoRodjenjaId",
                        column: x => x.MjestoRodjenjaId,
                        principalTable: "Gradovi",
                        principalColumn: "GradId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Osobe_GradFirmeId",
                table: "Osobe",
                column: "GradFirmeId");

            migrationBuilder.CreateIndex(
                name: "IX_Osobe_MjestoBoravkaId",
                table: "Osobe",
                column: "MjestoBoravkaId");

            migrationBuilder.CreateIndex(
                name: "IX_Osobe_MjestoRodjenjaId",
                table: "Osobe",
                column: "MjestoRodjenjaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Osobe");

            migrationBuilder.DropTable(
                name: "Gradovi");
        }
    }
}
