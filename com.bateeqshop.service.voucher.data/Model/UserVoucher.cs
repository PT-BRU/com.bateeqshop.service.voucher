using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace com.bateeqshop.service.voucher.data.Model
{
    public class UserVoucher : StandardEntity
    {
        public bool IsRedeemed { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [Required]
        public int VoucherId { get; set; }
        public virtual Voucher Voucher { get; set; }

        // com.bateeqshop.service.auth
        public int UserId { get; set; }
        public bool IsCheckout { get; set; }
        
        public virtual List<UserVoucherRedeemProduct> UserVoucherRedeemProduct { get; set; }
    }
}
