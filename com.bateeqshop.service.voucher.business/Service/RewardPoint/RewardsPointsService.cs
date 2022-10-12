using com.bateeqshop.service.voucher.data;
using com.bateeqshop.service.voucher.data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.bateeqshop.service.voucher.business.Service.RewardPoint
{
    public class RewardsPointsService : IRewardPointService
    {
        private readonly VoucherDbContext _context;

        public RewardsPointsService(
            VoucherDbContext context
            )
        {
            _context = context;
        }

        public RewardPoints FindById(int id)
        {
            var result = _context.RewardPoints.Where(entity => entity.Id == id).FirstOrDefault();

            return result;
        }
    }
}
