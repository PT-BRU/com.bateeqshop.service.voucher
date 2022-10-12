using System;
using System.Collections.Generic;
using System.Text;

namespace com.bateeqshop.service.voucher.business.ViewModel.MyRewards
{
    public class MyRewardVM
    {
        public int VoucherId { get; set; }
        public int Count { get; set; }
        public string VoucherName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string VoucherType { get; set; }
        public string ProductGiftId { get; set; }
        public string ProductDetailGiftId { get; set; }
        public int UserVoucherId { get; set; }
        public string Member { get; set; }
    }
}
