using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CZE.Data.Migrations
{
    public partial class Edit_CentarLatLong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UrlLokacija",
                table: "Centri");

            migrationBuilder.AddColumn<string>(
                name: "Latitude",
                table: "Centri",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Longitude",
                table: "Centri",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Centri");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Centri");

            migrationBuilder.AddColumn<string>(
                name: "UrlLokacija",
                table: "Centri",
                maxLength: 2100,
                nullable: true);
        }
    }
}
