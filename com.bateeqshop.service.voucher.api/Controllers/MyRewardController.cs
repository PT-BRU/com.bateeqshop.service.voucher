using com.bateeqshop.service.voucher.api.Configuration;
using com.bateeqshop.service.voucher.api.CustomAttributes.AuthServices;
using com.bateeqshop.service.voucher.business.Service.MyRewards;
using com.bateeqshop.service.voucher.business.ViewModel.Users;
using com.bateeqshop.service.voucher.data.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace com.bateeqshop.service.voucher.api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/reward")]
    public class MyRewardController : ControllerBase
    {
        private readonly IRewardService _rewardService;
        private readonly ServicesConfig _serviceConfig;

        public MyRewardController(
            IRewardService rewardService,
            IOptions<ServicesConfig> servicesConfig
            )
        {
            _rewardService = rewardService;
            _serviceConfig = servicesConfig.Value;
        }

        [HttpGet]
        [Route("myReward/{id}")]
        public async Task<IActionResult> GetMyReward(int id)
        {
            var result = _rewardService.GetMyRewardByUserId(id);

            return Ok(result);
        }

        [HttpGet]
        [Route("myReward-with-product/{id}")]
        [AuthorizationServiceFactory]
        public async Task<IActionResult> GetMyRewardWitProductInfo(int id)

        {
            var users = GetMe();
            var result = _rewardService.GetMyRewardByUserId(users, _serviceConfig.ProductIds);

            return Ok(result);
        }

        [HttpPut]
        [Route("updateStatusCheckout")]
        [AuthorizationServiceFactory]
        public async Task<IActionResult> UpdateStatusCheckout([FromBody] List<int> userVoucherId )
        {
            var users = GetMe();
            var result = _rewardService.UpdateStatusCheckout(users, userVoucherId);

            return Ok(result);
        }

        [HttpGet]
        [Route("myReward-with-redeem")]
        [AuthorizationServiceFactory]
        public async Task<IActionResult> GetMyRewardWithStatusRedeem([FromQuery]bool statusRedeem,[FromQuery]bool isCheckout)
        {
            var users = GetMe();
            var result = _rewardService.GetMyRewardByUserIdWithRedeemStatus(users,statusRedeem, isCheckout, _serviceConfig.ProductIds );

            return Ok(result);
        }

        #region Private
        private ResponseUserMe GetMe()
        {
            var users = Request.Headers.Where(s => s.Key == "User-Auth").FirstOrDefault();
            var userValue = users.Value.ToString();
            if (userValue == null)
                return null;

            var userObject = JsonConvert.DeserializeObject<ResponseUserMe>(userValue);
            return userObject;
        }
        #endregion
    }
}
