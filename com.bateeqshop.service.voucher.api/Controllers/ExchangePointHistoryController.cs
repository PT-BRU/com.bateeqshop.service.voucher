using com.bateeqshop.service.voucher.business.Service.ExchangePoints;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace com.bateeqshop.service.voucher.api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/exchangePoint")]
    public class ExchangePointHistoryController : ControllerBase
    {
        private readonly IExchangePointServices _exchangePointService;

        public ExchangePointHistoryController(
            IExchangePointServices exchangePointService
            )
        {
            _exchangePointService = exchangePointService;
        }

        [HttpGet]
        [Route("GetEcxhangePointByUserId/{id}")]
        public async Task<IActionResult> GetByUserId (int id)
        {
            var xchangePoint = _exchangePointService.GetExchangePointByUser(id);

            return Ok(xchangePoint);
        }
    }
}
