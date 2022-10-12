using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace com.bateeqshop.service.voucher.data.Model
{
    public class UserVoucherRedeemProduct: StandardEntity
    {
        public int ProductId { get; set; }
        public int ProductDetailId { get; set; }
        public int CartProductId { get; set; }
        public int UserVoucherId { get; set; }
        [ForeignKey("UserVoucherId")]
        public UserVoucher UserVoucher { get; set; }
    }
}
