using System;
using System.Collections.Generic;
using System.Text;

namespace com.bateeqshop.service.voucher.business.ViewModel.Vouchers
{
    public class VoucherIndexViewModel
    {
        public int Page { get; set; }
        public int Total { get; set; }
        public ICollection<VoucherSimplyViewModel> Data { get; set; }
    }
}
