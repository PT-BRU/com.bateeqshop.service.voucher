using com.bateeqshop.service.voucher.data.Model.Base;
using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace com.bateeqshop.service.voucher.data.Model
{
    public class VoucherNominal : BaseMembershipVoucher
    {
        public decimal ExchangePoint { get; set; }
        public decimal Value { get; set; }
        public decimal MinSubtotal { get; set; }
        public int MaxQty { get; set; }
        public int MaxUsage { get; set; }
        public decimal Nominal { get; set; }

        // com.bateeqshop.service.core
        public int CurrencyId { get; set; }
        public int ValidityPeriod { get; set; }

    }
}
