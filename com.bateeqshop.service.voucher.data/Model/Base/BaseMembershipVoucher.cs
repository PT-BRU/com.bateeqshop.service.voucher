using System;
using System.Collections.Generic;
using System.Text;

namespace com.bateeqshop.service.voucher.data.Model.Base
{
    public class BaseMembershipVoucher : BaseVoucher, IMembershipVoucher
    {
        public string MembershipId { get; set; }
    }
}
