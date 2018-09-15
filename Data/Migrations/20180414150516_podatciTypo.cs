using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CZE.Data.Migrations
{
    public partial class podatciTypo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PodaciOFirmi",
                table: "Osobe",
                newName: "PodatciOFirmi");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PodatciOFirmi",
                table: "Osobe",
                newName: "PodaciOFirmi");
        }
    }
}
