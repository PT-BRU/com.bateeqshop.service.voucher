using com.bateeqshop.service.voucher.data.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace com.bateeqshop.service.voucher.business.Service.BaseRepositories
{
    public interface IUnitOfWork
    {
        IBaseRepository<Voucher> voucher { get; }
    }
}
