using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CZE.Data.Migrations
{
    public partial class UplateKandidataAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UplateKandidata",
                columns: table => new
                {
                    UplataKandidataId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Biljeske = table.Column<string>(maxLength: 1000, nullable: true),
                    DatumUplate = table.Column<DateTime>(type: "smalldatetime", nullable: false, defaultValueSql: "getdate()"),
                    GrupaKandidatiId = table.Column<int>(nullable: false),
                    Kolicina = table.Column<decimal>(type: "decimal(7,2)", nullable: false),
                    ZaposlenikId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UplateKandidata", x => x.UplataKandidataId);
                    table.ForeignKey(
                        name: "FK_UplateKandidata_Grupa_Kandidati_GrupaKandidatiId",
                        column: x => x.GrupaKandidatiId,
                        principalTable: "Grupa_Kandidati",
                        principalColumn: "GrupaKandidatiId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UplateKandidata_Zaposlenici_ZaposlenikId",
                        column: x => x.ZaposlenikId,
                        principalTable: "Zaposlenici",
                        principalColumn: "ZaposlenikId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UplateKandidata_GrupaKandidatiId",
                table: "UplateKandidata",
                column: "GrupaKandidatiId");

            migrationBuilder.CreateIndex(
                name: "IX_UplateKandidata_ZaposlenikId",
                table: "UplateKandidata",
                column: "ZaposlenikId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UplateKandidata");
        }
    }
}
