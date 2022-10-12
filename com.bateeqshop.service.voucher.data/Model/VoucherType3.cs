using com.bateeqshop.service.voucher.data.Model.Base;
using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace com.bateeqshop.service.voucher.data.Model
{
    public class VoucherType3 : BaseGeneralVoucher
    {
        public int DiscountProductIndex { get; set; }
        public decimal PercentageDiscountValue { get; set; }

        // com.bateeqshop.service.product
        public int BuyProductId { get; set; }
    }
}
