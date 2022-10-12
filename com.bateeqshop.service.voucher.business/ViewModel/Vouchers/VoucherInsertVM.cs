using com.bateeqshop.service.voucher.data.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace com.bateeqshop.service.voucher.business.ViewModel.Vouchers
{
    public class VoucherInsertVM
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int TypeId { get; set; }
        public string Type { get; set; }
        public int UserVouchers { get; set; }
        public int UserVoucherRedeemed { get; set; }
        public int UserIdVoucher { get; set; }
        public DateTime? RedeemDate { get; set; }
        public Voucher.VoucherType TypeEnum { get; set; }
        public VoucherNominal VoucherNominalData { get; set; }
        public VoucherPercentage VoucherPercentageData { get; set; }
        public VoucherProduct VoucherProductData { get; set; }
        public VoucherType1 VoucherType1Data { get; set; }
        public VoucherType2Category VoucherType2CategoryData { get; set; }
        public VoucherType2Product VoucherType2ProductData { get; set; }
        public VoucherType3 VoucherType3Data { get; set; }
        public VoucherType4 VoucherType4Data { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
