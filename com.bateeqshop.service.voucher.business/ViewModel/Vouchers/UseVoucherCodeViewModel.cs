using com.bateeqshop.service.voucher.data.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace com.bateeqshop.service.voucher.business.ViewModel.Vouchers
{
    public class UseVoucherCodeViewModel
    {
        public int UserVoucherId { get; set; }
        public int VoucherId { get; set; }
        public string VoucherCode { get; set; }
        public string VoucherType { get; set; }
        public string Message { get; set; }
        public bool IsApplied { get; set; }
    }
}
