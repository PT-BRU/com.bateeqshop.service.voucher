using com.bateeqshop.service.voucher.business.ViewModel;
using com.bateeqshop.service.voucher.business.ViewModel.Users;
using com.bateeqshop.service.voucher.data.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace com.bateeqshop.service.voucher.business.Service.UserVouchers
{
    public interface IUserVoucherServices: IService<UserVoucherVM>
    {
        UserVoucher Insert(UserVoucher model);
        ICollection<UserVoucher> InsertListVoucherGeneral(ICollection<Voucher> voucherList, int userId);
        ICollection<UserVoucher> InsertListVoucherGeneral(List<int> voucherIds, int userId);
        void Delete(int id, ResponseUserMe user, string uri);
    }
}
