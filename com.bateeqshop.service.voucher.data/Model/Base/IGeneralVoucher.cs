using System;
using System.Collections.Generic;
using System.Text;

namespace com.bateeqshop.service.voucher.data.Model.Base
{
    public interface IGeneralVoucher : IVoucher
    {
        public int MaxQty { get; set; }
        public int MaxUsage { get; set; }
    }
}
