using com.bateeqshop.service.voucher.data.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace com.bateeqshop.service.voucher.business.ViewModel.Vouchers
{
    public class VoucherValidationVM
    {
        public int VoucherId { get; set; }
        public string Code { get; set; }
        public Voucher.VoucherType VoucherType { get; set; }
        public bool Validation { get; set; }
        public bool IsMembership { get; set; }
        public bool IsMultiply { get; set; }
    }
}
