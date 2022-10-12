using com.bateeqshop.service.voucher.business;
using com.bateeqshop.service.voucher.business.Service.RewardPoint;
using com.bateeqshop.service.voucher.data.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace com.bateeqshop.service.voucher.api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    public class RewardPointsController : ControllerBase
    {
        #region Field
        private readonly IService<RewardPoints> _rewardPointService;
        private readonly IRewardPointService _service;
        #endregion

        #region Ctor
        public RewardPointsController(
            IService<RewardPoints> rewardPointService,
           IRewardPointService service
            )
        {
            _rewardPointService = rewardPointService;
            _service = service;
        }
        #endregion

        #region Method
        [HttpPost]

        [Route("v{version:apiVersion}/api/createNewRewardPoint")]
        public async Task<IActionResult> CreateNewRewardPoints([FromBody] RewardPoints model)
        {
            try
            {
                _rewardPointService.Insert(model);
            }
            catch(Exception e)
            {
                throw e;
            }

            return Ok(model);
        }

        [HttpPut]

        [Route("v{version:apiVersion}/api/updateRewardPoint")]
        public async Task<IActionResult> UpdateRewardPoint([FromBody] RewardPoints model)
        {
            try
            {
                _rewardPointService.Update(model);
            }
            catch(Exception e)
            {
                throw e;
            }
                return Ok(model);
        }

        [HttpGet]

        [Route("v{version:apiVersion}/api/getAllRewardPoints")]
        public async Task<IActionResult> GetAllRewardPoints()
        {
           var result = _rewardPointService.Find();

           return Ok(result);
        }

        [HttpGet]

        [Route("v{version:apiVersion}/api/getRewardPoints/{id}")]
        public async Task<IActionResult> GetRewardPointById(int id)
        {
            var result = _service.FindById(id);

            return Ok(result);
        }

        [HttpDelete]

        [Route("v{version:apiVersion}/api/deleteRewardPoint/{id}")]
        public async Task<IActionResult> DeleteRewardPoint(int id)
        {
            try
            {
                _rewardPointService.Delete(id);
            }
            catch (Exception e)
            {
                throw e;
            }

            return Ok("Succesfully Deleted");
        }

        #endregion

    }
    }
