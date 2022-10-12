using System;
using System.Collections.Generic;
using System.Text;

namespace com.bateeqshop.service.voucher.data.Model.Base
{
    public class BaseGeneralVoucher : BaseVoucher, IGeneralVoucher
    {
        public int MaxQty { get; set; }
        public int MaxUsage { get; set; }
    }
}
