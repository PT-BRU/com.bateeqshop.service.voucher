using System;
using System.Collections.Generic;
using System.Text;

namespace com.bateeqshop.service.voucher.business.ViewModel.UserInfo
{
    public class Membership
    {
        public string Name { get; set; }
        public decimal MinAccumulation { get; set; }
        public int PercentageDiscValue { get; set; }
        public string Description { get; set; }
        public int MinPoint { get; set; }
        public virtual ICollection<UserMembership> UserMemberships { get; set; }
    }
}
