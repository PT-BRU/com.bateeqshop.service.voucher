using System;
using System.Collections.Generic;
using System.Text;

namespace com.bateeqshop.service.voucher.business.ViewModel.Vouchers
{
    public class UseVoucherViewModel
    {
        public List<UseVoucherCodeViewModel> Voucher { get; set; }
        //public List<int> IdVoucher { get; set; }
        //public List<int> ProductList { get; set; }
        public List<ProductPurchaseViewModel> ProductList { get; set; }
        public List<ProductGiftChooseViewModel> ProductGiftChoose { get; set; }

    }
}
