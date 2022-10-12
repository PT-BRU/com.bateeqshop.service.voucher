using com.bateeqshop.service.voucher.data.Model.Base;
using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace com.bateeqshop.service.voucher.data.Model
{
    public class VoucherProduct : BaseMembershipVoucher
    {
        public decimal ExchangePoint { get; set; }
        public string ProductId { get; set; }
        public decimal MinSubtotal { get; set; }
        public int CurrencyId { get; set; }
        public int ValidityPeriod { get; set; }
    }
}
