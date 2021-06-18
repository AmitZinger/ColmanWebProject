using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ColmanWebProject.Migrations
{
    public partial class onetoone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageByte",
                table: "Image");

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Image",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Url",
                table: "Image");

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageByte",
                table: "Image",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
