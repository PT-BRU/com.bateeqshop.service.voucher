using com.bateeqshop.service.voucher.data.Model.Base;
using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace com.bateeqshop.service.voucher.data.Model
{
    public class VoucherType4 : BaseGeneralVoucher
    {
        public decimal MinOrderValue { get; set; }
        public bool CanMultiply { get; set; }

        // com.bateeqshop.service.product
        public string FreeProductId { get; set; }
        // com.bateeqshop.service.core
        public int CurrencyId { get; set; }
        
    }
}
