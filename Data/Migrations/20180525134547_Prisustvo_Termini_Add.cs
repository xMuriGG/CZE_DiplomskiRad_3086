using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CZE.Data.Migrations
{
    public partial class Prisustvo_Termini_Add : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PrisustvoTermin",
                columns: table => new
                {
                    PrisustvoTerminId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Casova = table.Column<int>(nullable: false),
                    DateTime = table.Column<DateTime>(type: "smalldatetime", nullable: false, defaultValueSql: "getdate()"),
                    GrupaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrisustvoTermin", x => x.PrisustvoTerminId);
                    table.ForeignKey(
                        name: "FK_PrisustvoTermin_Grupe_GrupaId",
                        column: x => x.GrupaId,
                        principalTable: "Grupe",
                        principalColumn: "GrupaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Prisustva",
                columns: table => new
                {
                    PrisustvoId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GrupaKandidatiId = table.Column<int>(nullable: false),
                    PrisustvoTerminId = table.Column<int>(nullable: false),
                    Prisutan = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prisustva", x => x.PrisustvoId);
                    table.ForeignKey(
                        name: "FK_Prisustva_Grupa_Kandidati_GrupaKandidatiId",
                        column: x => x.GrupaKandidatiId,
                        principalTable: "Grupa_Kandidati",
                        principalColumn: "GrupaKandidatiId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Prisustva_PrisustvoTermin_PrisustvoTerminId",
                        column: x => x.PrisustvoTerminId,
                        principalTable: "PrisustvoTermin",
                        principalColumn: "PrisustvoTerminId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Prisustva_GrupaKandidatiId",
                table: "Prisustva",
                column: "GrupaKandidatiId");

            migrationBuilder.CreateIndex(
                name: "IX_Prisustva_PrisustvoTerminId",
                table: "Prisustva",
                column: "PrisustvoTerminId");

            migrationBuilder.CreateIndex(
                name: "IX_PrisustvoTermin_GrupaId",
                table: "PrisustvoTermin",
                column: "GrupaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Prisustva");

            migrationBuilder.DropTable(
                name: "PrisustvoTermin");
        }
    }
}
