using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CZE.Data.Migrations
{
    public partial class Migracija2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DatumRodjenja",
                table: "Osobe",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime));

            migrationBuilder.CreateTable(
                name: "Centri",
                columns: table => new
                {
                    CentarId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Adresa = table.Column<string>(maxLength: 100, nullable: false),
                    BrojMobitela = table.Column<string>(maxLength: 15, nullable: true),
                    BrojTelefona = table.Column<string>(maxLength: 15, nullable: true),
                    GradId = table.Column<int>(nullable: false),
                    Naziv = table.Column<string>(maxLength: 100, nullable: false),
                    UrlLokacija = table.Column<string>(maxLength: 2100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Centri", x => x.CentarId);
                    table.ForeignKey(
                        name: "FK_Centri_Gradovi_GradId",
                        column: x => x.GradId,
                        principalTable: "Gradovi",
                        principalColumn: "GradId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KursKategorije",
                columns: table => new
                {
                    KursKategorijaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Naziv = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KursKategorije", x => x.KursKategorijaId);
                });

            migrationBuilder.CreateTable(
                name: "KursTipovi",
                columns: table => new
                {
                    KursTipId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Casova = table.Column<int>(nullable: false),
                    Cijena = table.Column<decimal>(type: "decimal(7,2)", nullable: false),
                    KursKategorijaId = table.Column<int>(nullable: false),
                    Naziv = table.Column<string>(maxLength: 100, nullable: false),
                    Opis = table.Column<string>(maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KursTipovi", x => x.KursTipId);
                    table.ForeignKey(
                        name: "FK_KursTipovi_KursKategorije_KursKategorijaId",
                        column: x => x.KursKategorijaId,
                        principalTable: "KursKategorije",
                        principalColumn: "KursKategorijaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Kursevi",
                columns: table => new
                {
                    KursId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    KursTipId = table.Column<int>(nullable: false),
                    Naziv = table.Column<string>(maxLength: 100, nullable: false),
                    Opis = table.Column<string>(maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kursevi", x => x.KursId);
                    table.ForeignKey(
                        name: "FK_Kursevi_KursTipovi_KursTipId",
                        column: x => x.KursTipId,
                        principalTable: "KursTipovi",
                        principalColumn: "KursTipId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Grupe",
                columns: table => new
                {
                    GrupaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AktivnaZaPrijave = table.Column<bool>(nullable: false),
                    Casova = table.Column<int>(nullable: true),
                    CentarId = table.Column<int>(nullable: false),
                    Cijena = table.Column<decimal>(type: "decimal(7,2)", nullable: true),
                    Kraj = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    KursId = table.Column<int>(nullable: false),
                    Naziv = table.Column<string>(maxLength: 100, nullable: false),
                    Pocetak = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    Slika = table.Column<byte[]>(nullable: true),
                    SlikaThumb = table.Column<byte[]>(nullable: true),
                    SlikaThumbUrl = table.Column<string>(maxLength: 2100, nullable: true),
                    SlikaUrl = table.Column<string>(maxLength: 2100, nullable: true),
                    UToku = table.Column<bool>(nullable: false),
                    ZaposlenikId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grupe", x => x.GrupaId);
                    table.ForeignKey(
                        name: "FK_Grupe_Centri_CentarId",
                        column: x => x.CentarId,
                        principalTable: "Centri",
                        principalColumn: "CentarId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Grupe_Kursevi_KursId",
                        column: x => x.KursId,
                        principalTable: "Kursevi",
                        principalColumn: "KursId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Grupe_Zaposlenici_ZaposlenikId",
                        column: x => x.ZaposlenikId,
                        principalTable: "Zaposlenici",
                        principalColumn: "ZaposlenikId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Centri_GradId",
                table: "Centri",
                column: "GradId");

            migrationBuilder.CreateIndex(
                name: "IX_Grupe_CentarId",
                table: "Grupe",
                column: "CentarId");

            migrationBuilder.CreateIndex(
                name: "IX_Grupe_KursId",
                table: "Grupe",
                column: "KursId");

            migrationBuilder.CreateIndex(
                name: "IX_Grupe_ZaposlenikId",
                table: "Grupe",
                column: "ZaposlenikId");

            migrationBuilder.CreateIndex(
                name: "IX_Kursevi_KursTipId",
                table: "Kursevi",
                column: "KursTipId");

            migrationBuilder.CreateIndex(
                name: "IX_KursTipovi_KursKategorijaId",
                table: "KursTipovi",
                column: "KursKategorijaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Grupe");

            migrationBuilder.DropTable(
                name: "Centri");

            migrationBuilder.DropTable(
                name: "Kursevi");

            migrationBuilder.DropTable(
                name: "KursTipovi");

            migrationBuilder.DropTable(
                name: "KursKategorije");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DatumRodjenja",
                table: "Osobe",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");
        }
    }
}
