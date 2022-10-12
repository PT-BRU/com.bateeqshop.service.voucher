using com.bateeqshop.service.voucher.data.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace com.bateeqshop.service.voucher.business.ViewModel
{
    public class UserVoucherVM
    {
        public int Id { get; set; }
        public bool IsRedeemed { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public VoucherVM Voucher { get; set; }

        public static UserVoucherVM MapFrom(UserVoucher userVoucher)
        {
            return new UserVoucherVM()
            {
                Id = userVoucher.Id,
                IsRedeemed = userVoucher.IsRedeemed,
                StartDate = userVoucher.StartDate,
                EndDate = userVoucher.EndDate,
                Voucher = VoucherVM.MapFrom(userVoucher.Voucher)
            };
        }

        public static List<UserVoucherVM> MapFrom(List<UserVoucher> userVouchers)
        {
            var result = new List<UserVoucherVM>();

            foreach (var userVoucher in userVouchers)
            {
                result.Add(MapFrom(userVoucher));
            }

            return result;
        }
    }
}
