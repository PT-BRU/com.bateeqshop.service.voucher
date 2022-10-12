using com.bateeqshop.service.voucher.business.Service.GeneralSettings;
using com.bateeqshop.service.voucher.data.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace com.bateeqshop.service.voucher.api.Controllers
{
    [ApiController]
    //[Route("wishlist")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/generalSetting")]
    public class GeneralSettingController : ControllerBase
    {
        private readonly IGeneralSetting _service;

        public GeneralSettingController(
            IGeneralSetting service
            )
        {
            _service = service;
        }

        [HttpPost]
        [Route("AddGeneralSetting")]
        public async Task<IActionResult> AddGeneralSetting([FromBody] GeneralSetting model)
        {
            _service.Insert(model);

            return Ok(model);
        }

        [HttpGet]
        [Route("GetGeneralSettingById/{id}")]
        public async Task<IActionResult> GetGeneralSetting(int id)
        {
            var general = _service.GetById(id);

            return Ok(general);
        }

        [HttpGet]
        [Route("GetAllGeneralSetting")]
        public async Task<IActionResult> GetAll()
        {
            var general = _service.GetAll();

            return Ok(general);
        }

        [HttpPut]
        [Route("UpdateGeneralSetting")]
        public async Task<IActionResult> UpdateGeneralSetting([FromBody]GeneralSetting model)
        {
            var general = _service.Update(model);

            return Ok(general);
        }

        [HttpDelete]
        [Route("DeleteGeneralSetting/{id}")]
        public async Task<IActionResult> DeleteGeneralSetting(int id)
        {
            _service.Delete(id);

            return Ok("General Setting Sucesfully Deleted");
        }

    }
}
