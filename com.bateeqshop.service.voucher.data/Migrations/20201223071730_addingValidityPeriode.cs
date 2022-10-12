using Microsoft.EntityFrameworkCore.Migrations;

namespace com.bateeqshop.service.voucher.data.Migrations
{
    public partial class addingValidityPeriode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ValidityPeriod",
                table: "VoucherProducts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ValidityPeriod",
                table: "VoucherNominals",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValidityPeriod",
                table: "VoucherProducts");

            migrationBuilder.DropColumn(
                name: "ValidityPeriod",
                table: "VoucherNominals");
        }
    }
}
