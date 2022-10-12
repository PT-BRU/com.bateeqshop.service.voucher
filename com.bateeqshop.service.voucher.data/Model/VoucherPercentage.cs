using com.bateeqshop.service.voucher.data.Model.Base;
using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace com.bateeqshop.service.voucher.data.Model
{
    public class VoucherPercentage : BaseGeneralVoucher
    {
        public decimal ExchangePoint { get; set; }
        public decimal Value { get; set; }
        public decimal MinSubtotal { get; set; }
        public decimal MaxDiscount { get; set; }

        // com.bateeqshop.service.core
        public int CurrencyId { get; set; }
    }
}
