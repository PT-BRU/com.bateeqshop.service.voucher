using System;
using System.Collections.Generic;
using System.Text;

namespace com.bateeqshop.service.voucher.data.Model
{
    public class RedeemVoucherNominal
    {
        public int UserId { get; set; }
        public int VoucherId { get; set; }
        public int Qty { get; set; }
    }
}
