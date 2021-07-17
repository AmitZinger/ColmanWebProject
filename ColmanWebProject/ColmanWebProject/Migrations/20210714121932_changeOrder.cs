using Microsoft.EntityFrameworkCore.Migrations;

namespace ColmanWebProject.Migrations
{
    public partial class changeOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShippingAddress",
                table: "Order",
                newName: "ShippingAddressStreet");

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddressCity",
                table: "Order",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddressHomeNum",
                table: "Order",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShippingAddressCity",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ShippingAddressHomeNum",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "ShippingAddressStreet",
                table: "Order",
                newName: "ShippingAddress");
        }
    }
}
