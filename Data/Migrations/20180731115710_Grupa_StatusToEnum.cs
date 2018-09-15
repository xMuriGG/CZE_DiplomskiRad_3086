using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CZE.Data.Migrations
{
    public partial class Grupa_StatusToEnum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AktivnaZaPrijave",
                table: "Grupe");

            migrationBuilder.DropColumn(
                name: "UToku",
                table: "Grupe");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Grupe",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Grupe");

            migrationBuilder.AddColumn<bool>(
                name: "AktivnaZaPrijave",
                table: "Grupe",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UToku",
                table: "Grupe",
                nullable: false,
                defaultValue: false);
        }
    }
}
