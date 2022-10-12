using System;
using System.Collections.Generic;
using System.Text;

namespace com.bateeqshop.service.voucher.business.ViewModel.Vouchers
{
    public class VoucherInsertPlainMembership
    {
        public string VoucherType { get; set; }
        public string VoucherName { get; set; }
        public string ValidityPeriode { get; set; }
        public string StartDate { get; set; }
        public string Nominal { get; set; }
        public string MinimumPurchases { get; set; }
        public string EndDate { get; set; }
        public string AssignToMember { get; set; }
        public string Description { get; set; }
    }
}
