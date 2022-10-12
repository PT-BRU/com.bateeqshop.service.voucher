using System;
using System.Collections.Generic;
using System.Text;

namespace com.bateeqshop.service.voucher.business.ViewModel.UserInfo
{
    public class UserMembership
    {
        public decimal Accumulation { get; set; }
        public DateTime AccumulationExpDate { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int MembershipId { get; set; }
        public virtual Membership Membership { get; set; }
    }
}
