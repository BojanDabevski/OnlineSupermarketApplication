using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineSupermarket.Repository.Migrations
{
    public partial class DbConKOmentiranoZaProdIOrd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductInOrder",
                table: "ProductInOrder");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductInOrder",
                table: "ProductInOrder",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ProductInOrder_ProductId",
                table: "ProductInOrder",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductInOrder",
                table: "ProductInOrder");

            migrationBuilder.DropIndex(
                name: "IX_ProductInOrder_ProductId",
                table: "ProductInOrder");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductInOrder",
                table: "ProductInOrder",
                columns: new[] { "ProductId", "OrderId" });
        }
    }
}
