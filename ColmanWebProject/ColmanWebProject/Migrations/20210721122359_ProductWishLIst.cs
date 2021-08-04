using Microsoft.EntityFrameworkCore.Migrations;

namespace ColmanWebProject.Migrations
{
    public partial class ProductWishLIst : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductWishList");

            migrationBuilder.CreateTable(
                name: "ProductsWishList",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    WishListId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsWishList", x => new { x.ProductId, x.WishListId });
                    table.ForeignKey(
                        name: "FK_ProductsWishList_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductsWishList_WishList_WishListId",
                        column: x => x.WishListId,
                        principalTable: "WishList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductsWishList_WishListId",
                table: "ProductsWishList",
                column: "WishListId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductsWishList");

            migrationBuilder.CreateTable(
                name: "ProductWishList",
                columns: table => new
                {
                    ProductsId = table.Column<int>(type: "int", nullable: false),
                    WishListsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductWishList", x => new { x.ProductsId, x.WishListsId });
                    table.ForeignKey(
                        name: "FK_ProductWishList_Product_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductWishList_WishList_WishListsId",
                        column: x => x.WishListsId,
                        principalTable: "WishList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductWishList_WishListsId",
                table: "ProductWishList",
                column: "WishListsId");
        }
    }
}
