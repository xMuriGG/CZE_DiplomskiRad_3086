using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CZE.Data.Migrations
{
    public partial class Slika : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Slika",
                table: "Grupe");

            migrationBuilder.DropColumn(
                name: "SlikaThumb",
                table: "Grupe");

            migrationBuilder.DropColumn(
                name: "SlikaThumbUrl",
                table: "Grupe");

            migrationBuilder.DropColumn(
                name: "SlikaUrl",
                table: "Grupe");

            migrationBuilder.AddColumn<int>(
                name: "SlikaId",
                table: "Grupe",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Slike",
                columns: table => new
                {
                    SlikaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Naziv = table.Column<string>(maxLength: 100, nullable: true),
                    SlikaFile = table.Column<byte[]>(maxLength: 1048576, nullable: true),
                    SlikaThumb = table.Column<byte[]>(maxLength: 524288, nullable: true),
                    SlikaThumbUrl = table.Column<string>(maxLength: 2100, nullable: true),
                    SlikaUrl = table.Column<string>(maxLength: 2100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Slike", x => x.SlikaId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Grupe_SlikaId",
                table: "Grupe",
                column: "SlikaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Grupe_Slike_SlikaId",
                table: "Grupe",
                column: "SlikaId",
                principalTable: "Slike",
                principalColumn: "SlikaId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grupe_Slike_SlikaId",
                table: "Grupe");

            migrationBuilder.DropTable(
                name: "Slike");

            migrationBuilder.DropIndex(
                name: "IX_Grupe_SlikaId",
                table: "Grupe");

            migrationBuilder.DropColumn(
                name: "SlikaId",
                table: "Grupe");

            migrationBuilder.AddColumn<byte[]>(
                name: "Slika",
                table: "Grupe",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "SlikaThumb",
                table: "Grupe",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SlikaThumbUrl",
                table: "Grupe",
                maxLength: 2100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SlikaUrl",
                table: "Grupe",
                maxLength: 2100,
                nullable: true);
        }
    }
}
