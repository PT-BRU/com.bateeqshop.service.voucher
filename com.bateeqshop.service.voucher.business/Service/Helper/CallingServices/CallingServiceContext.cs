using System;
using System.Collections.Generic;
using System.Text;

namespace com.bateeqshop.service.voucher.business.Service.Helper.CallingServices
{
    public class CallingServiceContext<TResponse, TRequest, THeader>
    {
        private CallingServiceState<TResponse, TRequest, THeader> _state;

        public CallingServiceContext(CallingServiceState<TResponse, TRequest, THeader> state)
        {
            _state = state;
        }
        public void SetState(CallingServiceState<TResponse,TRequest,THeader> state)
        {
            this._state = state;
            this._state.SetContext(this);
        }
    }
}
