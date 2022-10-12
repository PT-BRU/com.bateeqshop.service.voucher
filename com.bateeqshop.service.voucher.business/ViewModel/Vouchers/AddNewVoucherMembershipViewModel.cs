using com.bateeqshop.service.voucher.business.ViewModel.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace com.bateeqshop.service.voucher.business.ViewModel.Vouchers
{
    public class AddNewVoucherMembershipViewModel
    {
        public string VoucherType { get; set; }
        public double Nominal { get; set; }
        public string VoucherName { get; set; }
        public string MinimumPurchase { get; set; }
        public string PointExchange { get; set; }
        public int ValidityPeriod { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Description { get; set; }
        public List<string> AssignToMember { get; set; }
        public List<string> ProductGift { get; set; }
        public int Id { get; set; }

    }
}
