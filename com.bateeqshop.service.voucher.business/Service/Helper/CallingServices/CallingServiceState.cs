using System;
using System.Collections.Generic;
using System.Text;

namespace com.bateeqshop.service.voucher.business.Service.Helper.CallingServices
{
    public abstract class CallingServiceState<TResponse,TRequest,THeader>
    {
        protected CallingServiceContext<TResponse, TRequest, THeader> _context;
        public void SetContext(CallingServiceContext<TResponse, TRequest, THeader> context)
        {
            this._context = context;
        }

        public abstract TResponse Get(TRequest request,THeader header);
    }
}
