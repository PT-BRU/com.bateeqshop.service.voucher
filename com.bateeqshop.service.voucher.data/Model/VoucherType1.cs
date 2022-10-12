using com.bateeqshop.service.voucher.data.Model.Base;
using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace com.bateeqshop.service.voucher.data.Model
{
    public class VoucherType1 : BaseGeneralVoucher
    {
        public int MinBuyQtyProduct { get; set; }
        public int MinBuyQtyCategory { get; set; }
        /// <summary>
        /// if u using free product one id only
        /// </summary>
        public int FreeProductId { get; set; }
        public int FreeQty { get; set; }

        // com.bateeqshop.service.product
        public string CSV_BuyProductIds { get; set; }
        public string CSV_BuyCategoryId { get; set; }
        /// <summary>
        /// if You want Free Product multiple id
        /// </summary>
        public string CSV_FreeProductId { get; set; }
    }
}
