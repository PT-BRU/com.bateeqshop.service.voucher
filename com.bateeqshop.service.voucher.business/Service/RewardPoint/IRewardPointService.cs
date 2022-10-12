using com.bateeqshop.service.voucher.data.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace com.bateeqshop.service.voucher.business.Service.RewardPoint
{
    public interface IRewardPointService
    {
        RewardPoints FindById(int id);
    }
}
