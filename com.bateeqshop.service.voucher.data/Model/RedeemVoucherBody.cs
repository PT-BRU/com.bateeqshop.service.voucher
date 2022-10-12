using System;
using System.Collections.Generic;
using System.Text;

namespace com.bateeqshop.service.voucher.data.Model
{
    public class RedeemVoucherBody
    {
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public int VoucherId { get; set; }
        public int ProductDetailId { get; set; }
    }
}
