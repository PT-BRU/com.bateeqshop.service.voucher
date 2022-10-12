using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace com.bateeqshop.service.voucher.data.Model
{
    public class ExchangedPointHistory : StandardEntity
    {
        public decimal ExchangedPoint { get; set; }
        public int UserVoucherId { get; set; }
        public UserVoucher UserVoucher { get; set; }
    }
}
