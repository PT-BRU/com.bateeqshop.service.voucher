using System;
using System.Collections.Generic;
using System.Text;

namespace com.bateeqshop.service.voucher.business.ViewModel.Vouchers
{
    public class VoucherSimplyViewModel
    {
        public int id { get; set; }
        public string DiscountName { get; set; }
        public string DiscountType { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int TotalUse { get; set; }
        public int TotalClaimed { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public string DiscountCode { get; set; }
        public string ExchangePoint { get; set; }
        public string Membership { get; set; }
        public string ProductId { get; set; }
        public string Nominal { get; set; }
        public DateTime CreatedDate{ get; set; }
        public DateTime ModifiedDate{ get; set; }
    }
}
