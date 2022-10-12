using com.bateeqshop.service.voucher.business.ViewModel.MyRewards;
using com.bateeqshop.service.voucher.business.ViewModel.Users;
using com.bateeqshop.service.voucher.data.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace com.bateeqshop.service.voucher.business.Service.MyRewards
{
    public interface IRewardService
    {
       List<MyRewardVM> GetMyRewardByUserId(int id);
       List<MyRewardVM> GetMyRewardByUserIdWithRedeemStatus(int id, bool statusRedeem, bool statusCheckout);
       List<MyRewardWithProductInfoVM> GetMyRewardByUserId(ResponseUserMe users, string urlProductIds);
       List<MyRewardWithProductInfoVM> GetMyRewardByUserIdWithRedeemStatus(ResponseUserMe users, bool statusRedeem, bool statusCheckout, string urlProductIds);
       int UpdateStatusCheckout(ResponseUserMe users, List<int> userVoucherId);
    }
}
