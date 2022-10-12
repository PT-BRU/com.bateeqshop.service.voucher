using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace com.bateeqshop.service.voucher.data.Migrations
{
    public partial class AddingNewUserVoucherRedeemProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserVoucherRedeemProducts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: false),
                    CreatedUtc = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedAgent = table.Column<string>(nullable: true),
                    LastModifiedUtc = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModifiedAgent = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedUtc = table.Column<DateTime>(nullable: false),
                    DeletedBy = table.Column<string>(nullable: true),
                    DeletedAgent = table.Column<string>(nullable: true),
                    ProductId = table.Column<int>(nullable: false),
                    UserVoucherId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserVoucherRedeemProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserVoucherRedeemProducts_UserVouchers_UserVoucherId",
                        column: x => x.UserVoucherId,
                        principalTable: "UserVouchers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserVoucherRedeemProducts_UserVoucherId",
                table: "UserVoucherRedeemProducts",
                column: "UserVoucherId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserVoucherRedeemProducts");
        }
    }
}
