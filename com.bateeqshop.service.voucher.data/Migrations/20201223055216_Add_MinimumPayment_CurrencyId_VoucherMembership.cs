using Microsoft.EntityFrameworkCore.Migrations;

namespace com.bateeqshop.service.voucher.data.Migrations
{
    public partial class Add_MinimumPayment_CurrencyId_VoucherMembership : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrencyId",
                table: "VoucherProducts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "MinSubtotal",
                table: "VoucherProducts",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "VoucherProducts");

            migrationBuilder.DropColumn(
                name: "MinSubtotal",
                table: "VoucherProducts");
        }
    }
}
