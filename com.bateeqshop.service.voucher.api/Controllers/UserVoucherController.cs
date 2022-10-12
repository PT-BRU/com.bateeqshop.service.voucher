using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using com.bateeqshop.service.voucher.api.Configuration;
using com.bateeqshop.service.voucher.api.CustomAttributes.AuthServices;
using com.bateeqshop.service.voucher.business.Service.UserVouchers;
using com.bateeqshop.service.voucher.business.ViewModel.Users;
using com.bateeqshop.service.voucher.business.ViewModel.UserVouchers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace com.bateeqshop.service.voucher.api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/user-voucher")]
    public class UserVoucherController : ControllerBase
    {
        private readonly ILogger<VoucherController> _logger;
        private readonly ServicesConfig _servicesConfig;
        private readonly IUserVoucherServices _userVoucherService;

        public UserVoucherController(ILogger<VoucherController> logger, IOptions<ServicesConfig> servicesConfig, IUserVoucherServices userVoucherService)
        {
            _logger = logger;
            _servicesConfig = servicesConfig.Value;
            _userVoucherService = userVoucherService;
        }
        /// <summary>
        /// using for add new Order
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizationServiceFactory]
        public async Task<IActionResult> CreateUserVoucherWithAddOrder([FromBody]AddVoucherListViewModel viewModel)
        {
            try
            {
                var user = GetMe();
                
                var result = _userVoucherService.InsertListVoucherGeneral(viewModel.VoucherIds,user.UserIds);
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        private ResponseUserMe GetMe()
        {
            var users = Request.Headers.Where(s => s.Key == "User-Auth").FirstOrDefault();
            var userValue = users.Value.ToString();
            if (userValue == null)
                return null;

            var userObject = JsonConvert.DeserializeObject<ResponseUserMe>(userValue);
            return userObject;
        }
    }
}
