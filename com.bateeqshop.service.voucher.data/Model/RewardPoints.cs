using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace com.bateeqshop.service.voucher.data.Model
{
    public class RewardPoints : StandardEntity
    {
        public int ShoppingTotal { get; set; }
        public int PointsEarned { get; set; }
    }
}
