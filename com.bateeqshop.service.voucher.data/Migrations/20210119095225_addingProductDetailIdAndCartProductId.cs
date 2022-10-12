using Microsoft.EntityFrameworkCore.Migrations;

namespace com.bateeqshop.service.voucher.data.Migrations
{
    public partial class addingProductDetailIdAndCartProductId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CartProductId",
                table: "UserVoucherRedeemProducts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductDetailId",
                table: "UserVoucherRedeemProducts",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CartProductId",
                table: "UserVoucherRedeemProducts");

            migrationBuilder.DropColumn(
                name: "ProductDetailId",
                table: "UserVoucherRedeemProducts");
        }
    }
}
