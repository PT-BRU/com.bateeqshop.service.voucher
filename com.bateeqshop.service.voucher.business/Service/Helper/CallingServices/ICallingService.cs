using System;
using System.Collections.Generic;
using System.Text;

namespace com.bateeqshop.service.voucher.business.Service.Helper.CallingServices
{
    public interface ICallingService<TModel>
    {
        TModel Get(string apiLink,Dictionary<string,string> header);
    }
}
