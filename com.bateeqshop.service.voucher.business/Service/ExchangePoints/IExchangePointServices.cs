using com.bateeqshop.service.voucher.business.ViewModel.ExchangePointVM;
using com.bateeqshop.service.voucher.data.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace com.bateeqshop.service.voucher.business.Service.ExchangePoints
{
    public interface IExchangePointServices
    {
        List<ExchangePointHistoryVM> GetExchangePointByUser(int id);
    }
}
