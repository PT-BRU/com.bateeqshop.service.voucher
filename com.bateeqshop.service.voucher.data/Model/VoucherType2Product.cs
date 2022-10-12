using com.bateeqshop.service.voucher.data.Model.Base;
using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace com.bateeqshop.service.voucher.data.Model
{
    public class VoucherType2Product : BaseGeneralVoucher
    {
        public int MinBuyQty { get; set; }
        public decimal PercentageDiscountValue { get; set; }

        // com.bateeqshop.service.product
        public string CSV_BuyProductIds { get; set; }
    }
}
