using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthApi.Migrations
{
    public partial class HealthDB2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "BookContents",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookContents",
                table: "BookContents",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BookContents",
                table: "BookContents");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "BookContents");
        }
    }
}
