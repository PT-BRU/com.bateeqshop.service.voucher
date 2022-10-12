using com.bateeqshop.service.voucher.business.ViewModel.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace com.bateeqshop.service.voucher.business.ViewModel.Vouchers
{
    public class ResponseUseVoucherViewModel
    {
        public List<UseVoucherCodeViewModel> useVoucher { get; set; }
        public decimal discountPotential { get; set; }
        public List<Product> freeProduct { get; set; }
    }
}
