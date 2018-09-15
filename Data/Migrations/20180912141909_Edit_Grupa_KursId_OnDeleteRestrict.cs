using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CZE.Data.Migrations
{
    public partial class Edit_Grupa_KursId_OnDeleteRestrict : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grupe_Kursevi_KursId",
                table: "Grupe");

            migrationBuilder.AddForeignKey(
                name: "FK_Grupe_Kursevi_KursId",
                table: "Grupe",
                column: "KursId",
                principalTable: "Kursevi",
                principalColumn: "KursId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grupe_Kursevi_KursId",
                table: "Grupe");

            migrationBuilder.AddForeignKey(
                name: "FK_Grupe_Kursevi_KursId",
                table: "Grupe",
                column: "KursId",
                principalTable: "Kursevi",
                principalColumn: "KursId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
