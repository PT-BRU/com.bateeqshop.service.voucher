using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace com.bateeqshop.service.voucher.data.Model
{
    public class Voucher : StandardEntity
    {
        public string Code { get; set; }
        public int TypeId { get; set; }
        public VoucherType Type { get; set; }
        public string CSV_VoucherProductCombinationIds { get; set; }

        public enum VoucherType
        {
            Nominal,//0
            Percentage,//1
            Product,//2
            Type1,//3
            Type2Category,//4
            Type2Product,//5
            Type3,//6
            Type4,//7
            Undefined//8
        }
    }

    public static class VoucherTypeExtensions
    {
        public static string ToDescription(this Voucher.VoucherType voucherType)
        {
            return voucherType switch
            {
                Voucher.VoucherType.Nominal => "Nominal",
                Voucher.VoucherType.Percentage => "Percentage",
                Voucher.VoucherType.Product => "Product",
                Voucher.VoucherType.Type1 => "Type 1",
                Voucher.VoucherType.Type2Category => "Type 2 Category",
                Voucher.VoucherType.Type2Product => "Type 2 Product",
                Voucher.VoucherType.Type3 => "Type 3",
                Voucher.VoucherType.Type4 => "Type 4",
                _ => "Invalid Type",
            };
        }

        public static string ToForm(this Voucher.VoucherType voucherType)
        {
            return voucherType switch
            {
                Voucher.VoucherType.Nominal => "Nominal",
                Voucher.VoucherType.Percentage => "Percentage",
                Voucher.VoucherType.Product => "Product",
                Voucher.VoucherType.Type1 => "Buy n free m",
                Voucher.VoucherType.Type2Category => "Buy n discount m%",
                Voucher.VoucherType.Type2Product => "Buy n discount m%",
                Voucher.VoucherType.Type3 => "Buy n discount m% product (n)th",
                Voucher.VoucherType.Type4 => "Pay nominal Rp.xx, Free 1 product",
                _ => "Invalid Type",
            };
        }

        public static Voucher.VoucherType ToVoucherTypeEnum(this string description)
        {
            return description.ToLower() switch
            {
                "nominal" => Voucher.VoucherType.Nominal,
                "percentage" => Voucher.VoucherType.Percentage,
                "product" => Voucher.VoucherType.Product,
                "type 1" => Voucher.VoucherType.Type1,
                "type 2 category" => Voucher.VoucherType.Type2Category,
                "type 2 product" => Voucher.VoucherType.Type2Product,
                "type 3" => Voucher.VoucherType.Type3,
                "type 4" => Voucher.VoucherType.Type4,
                "buy n free m" => Voucher.VoucherType.Type1,
                "buy n discount m%"=> Voucher.VoucherType.Type2Product,
                "buy n discount m% product (n)th" => Voucher.VoucherType.Type3,
                "pay nominal rp.xx, free 1 product"=> Voucher.VoucherType.Type4,
                _ => Voucher.VoucherType.Undefined
            };
        }
    }

}
