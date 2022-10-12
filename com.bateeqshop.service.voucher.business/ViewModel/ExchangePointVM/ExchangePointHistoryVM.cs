using com.bateeqshop.service.voucher.business.ViewModel.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace com.bateeqshop.service.voucher.business.ViewModel.ExchangePointVM
{
    public class ExchangePointHistoryVM
    {
        public string RedeemedDate { get; set; }
        public string VoucherName { get; set; }
        public int RedeemedPoint { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int Id { get; set; }
        public string MembershipId { get; set; }
        public string ValidityPeriod { get; set; }
        public string VoucherType { get; set; }
        public int UserVoucherId { get; set; }
        public string ProductGift { get; set; }
        public string ProductDetailGiftId { get; set; }
        public string Member { get; set; }
    }
}
