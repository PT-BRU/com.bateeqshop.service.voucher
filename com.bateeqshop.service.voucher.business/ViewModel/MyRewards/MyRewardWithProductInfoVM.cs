using com.bateeqshop.service.voucher.business.ViewModel.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace com.bateeqshop.service.voucher.business.ViewModel.MyRewards
{
    public class MyRewardWithProductInfoVM
    {
        public int VoucherId { get; set; }
        public int UserVoucherId { get; set; }
        public int Count { get; set; }
        public string VoucherName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string VoucherType { get; set; }
        public List<Product> ProductGift { get; set; }
    }
}
