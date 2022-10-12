using com.bateeqshop.service.voucher.business.ViewModel;
using com.bateeqshop.service.voucher.data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.bateeqshop.service.voucher.business.Service
{
    public class UserVoucherService : IHasUserIdService<UserVoucherVM>
    {
        private readonly VoucherDbContext _context;
        public UserVoucherService(VoucherDbContext context)
        {
            _context = context;
        }

        public List<UserVoucherVM> FindByUserId(int userId)
        {
            var userVouchers = _context.UserVouchers
                                 .Include(x => x.Voucher)
                                 .Where(x => x.UserId == userId)
                                 .ToList();

            return UserVoucherVM.MapFrom(userVouchers);
        }

        public async Task<List<UserVoucherVM>> FindByUserIdAsync(int userId)
        {
            var userVouchers = await _context.UserVouchers
                                 .Include(x => x.Voucher)
                                 .Where(x => x.UserId == userId)
                                 .ToListAsync();

            return UserVoucherVM.MapFrom(userVouchers);
        }
    }
}
