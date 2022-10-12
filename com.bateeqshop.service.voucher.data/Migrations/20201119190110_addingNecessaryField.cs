using Microsoft.EntityFrameworkCore.Migrations;

namespace com.bateeqshop.service.voucher.data.Migrations
{
    public partial class addingNecessaryField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CanMultiply",
                table: "VoucherType4s",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CSV_FreeProductId",
                table: "VoucherType1s",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxQty",
                table: "VoucherNominals",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxUsage",
                table: "VoucherNominals",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Nominal",
                table: "VoucherNominals",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanMultiply",
                table: "VoucherType4s");

            migrationBuilder.DropColumn(
                name: "CSV_FreeProductId",
                table: "VoucherType1s");

            migrationBuilder.DropColumn(
                name: "MaxQty",
                table: "VoucherNominals");

            migrationBuilder.DropColumn(
                name: "MaxUsage",
                table: "VoucherNominals");

            migrationBuilder.DropColumn(
                name: "Nominal",
                table: "VoucherNominals");
        }
    }
}
