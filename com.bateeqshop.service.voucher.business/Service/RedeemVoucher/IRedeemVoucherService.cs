using com.bateeqshop.service.voucher.data.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace com.bateeqshop.service.voucher.business.Service.RedeemVoucher
{
    public interface IRedeemVoucherService
    {
        int RedeemVoucherProduct(RedeemVoucherBody model, string apiGetUserDetail, string apiUpdatePointUser);
        //int RedeemVoucherNominel(RedeemVoucherNominal model);
    }
}
