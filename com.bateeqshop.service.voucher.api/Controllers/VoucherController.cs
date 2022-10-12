using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using com.bateeqshop.service.voucher.api.Configuration;
using com.bateeqshop.service.voucher.api.CustomAttributes.AuthServices;
using com.bateeqshop.service.voucher.business;
using com.bateeqshop.service.voucher.business.Service.RedeemVoucher;
using com.bateeqshop.service.voucher.business.Service.UserVouchers;
using com.bateeqshop.service.voucher.business.Service.Vouchers;
using com.bateeqshop.service.voucher.business.ViewModel;
using com.bateeqshop.service.voucher.business.ViewModel.Users;
using com.bateeqshop.service.voucher.business.ViewModel.Vouchers;
using com.bateeqshop.service.voucher.data.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace com.bateeqshop.service.voucher.api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/voucher")]
    public class VoucherController : ControllerBase
    {
        private readonly ILogger<VoucherController> _logger;
        private readonly IService<VoucherVM> _voucherService;
        private readonly IService<UserVoucherVM> _userVoucherCrudService;
        private readonly IHasUserIdService<UserVoucherVM> _userVoucherService;
        private readonly IVoucherServices _voucherServices;
        private readonly IRedeemVoucherService _redeemVoucherService;
        private readonly ServicesConfig _servicesConfig;

        public VoucherController(IService<VoucherVM> voucherService,
            IHasUserIdService<UserVoucherVM> userVoucherService,
            ILogger<VoucherController> logger,
            IVoucherServices voucherServices,
            IRedeemVoucherService redeemVoucherService,
            IOptions<ServicesConfig> servicesConfig,
            IService<UserVoucherVM> userVoucherCrudService
            )
        {
            _logger = logger;
            _voucherService = voucherService;
            _userVoucherService = userVoucherService;
            _voucherServices = voucherServices;
            _redeemVoucherService = redeemVoucherService;
            _servicesConfig = servicesConfig.Value;
            _userVoucherCrudService = userVoucherCrudService;
        }

        [HttpGet]
        [Route("search")]
        public async Task<IActionResult> FindAsync([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] int voucherType, [FromQuery] string code, [FromQuery] string name, [FromQuery] int page = 1, [FromQuery] int pageSize = 25)
        {
            try
            {
                var voucherTypeEnum = (Voucher.VoucherType)(voucherType == 0 ? 8 : voucherType);
                var result = _voucherServices.ViewSummarySearch(startDate, endDate, voucherTypeEnum, code, name);

                return Ok(new
                {
                    data = result.Skip((page - 1) * pageSize).Take(pageSize),
                    total = result.Count
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Route("active")]
        public async Task<IActionResult> FindActiveAsync([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] int voucherType, [FromQuery] string code, [FromQuery] string name, [FromQuery] int page = 1, [FromQuery] int pageSize = 25)
        {
            try
            {
                var voucherTypeEnum = (Voucher.VoucherType)(voucherType == 0 ? 8 : voucherType);
                var result = _voucherServices.ViewSummarySearch(startDate, endDate, voucherTypeEnum, code, name).Where(s => s.Status == "Active");
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> FindAsyncID([FromRoute] int id)
        {
            try
            {
                var result = _voucherServices.ViewByIdWithProductInfo(id, _servicesConfig.ProductIds, _servicesConfig.CategoryIds);
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }


        [HttpGet]
        [Route("membership/search")]
        public async Task<IActionResult> FindMembershipAsync([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] int voucherType, [FromQuery] string code, [FromQuery] string name, [FromQuery] int membershipId)
        {
            try
            {
                var voucherTypeEnum = (Voucher.VoucherType)(voucherType == 0 ? 8 : voucherType);
                voucherTypeEnum = (Voucher.VoucherType)(voucherType == 9 ? 0 : voucherTypeEnum);
                var result = _voucherServices.ViewSummaryMembershipSearch(startDate, endDate, voucherTypeEnum, code, name, membershipId);
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Route("membership/active")]
        public async Task<IActionResult> FindMembershipActiveAsync([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] int voucherType, [FromQuery] string code, [FromQuery] string name, [FromQuery] int membershipId)
        {
            try
            {
                var voucherTypeEnum = (Voucher.VoucherType)(voucherType == 0 ? 8 : voucherType);
                voucherTypeEnum = (Voucher.VoucherType)(voucherType == 9 ? 0 : voucherTypeEnum);
                var result = _voucherServices.ViewSummaryMembershipActiveSearch(startDate, endDate, voucherTypeEnum, code, name, membershipId);
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }


        [HttpGet]
        [Route("membership")]
        public async Task<IActionResult> FindMembershipIndexViewAsync([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] int voucherType, [FromQuery] string code, [FromQuery] string name, [FromQuery] int page, [FromQuery] int limit, [FromQuery] int membershipId)
        {
            try
            {
                if (page == 0 || page == null)
                    page = 1;
                if (limit == 0 || limit == null)
                    limit = 10;
                var voucherTypeEnum = (Voucher.VoucherType)(voucherType == 0 ? 8 : voucherType);
                voucherTypeEnum = (Voucher.VoucherType)(voucherType == 9 ? 0 : voucherTypeEnum);

                var result = _voucherServices.ViewSummaryMembershipSearchIndex(startDate, endDate, voucherTypeEnum, code, name, page, limit, membershipId);
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Route("membership/{id}")]
        public async Task<IActionResult> FindAsyncIDMembership([FromRoute] int id)
        {
            try
            {
                var result = _voucherServices.ViewByIdMembership(id);
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Route("membership-product-info/{id}")]
        public async Task<IActionResult> FindAsyncIDMembershipWithProductInfo([FromRoute] int id)
        {
            try
            {
                var result = _voucherServices.ViewByIdMembershipWithProductInfo(id, _servicesConfig.ProductIds);
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPost]
        [Route("membership")]
        public async Task<IActionResult> PostMembership(VoucherMembershipInsertPlainVM model)
        {

            try
            {
                if (!ModelState.IsValid)
                    return new BadRequestObjectResult(new { Message = "Validation not valid", ValidationResult = ModelState.Values.SelectMany(state => state.Errors) });

                var result = _voucherServices.InsertModelMembership(model);
                return Ok(new { Message = "Data Saved", ValidationResult = ModelState.Values.SelectMany(state => state.Errors) });

            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { Message = "Cannot Save Data", Exception = ex });

            }
        }

        [HttpPut]
        [Route("membership")]
        public async Task<IActionResult> PutMembership(VoucherMembershipInsertPlainVM model)
        {

            try
            {
                if (!ModelState.IsValid)
                    return new BadRequestObjectResult(new { Message = "Validation not valid", ValidationResult = ModelState.Values.SelectMany(state => state.Errors) });

                var result = await _voucherServices.UpdateModelMembership(model);
                return Ok(new { Message = "Data Saved", ValidationResult = ModelState.Values.SelectMany(state => state.Errors) });

            }
            catch (Exception e)
            {
                //return new BadRequestObjectResult(new { Message = "Cannot Save Data", Exception = ex });
                _logger.LogError(e, e.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet("me")]
        public async Task<IActionResult> FindByUserIdAsync()
        {
            try
            {
                // Get userId by decoding token HERE 
                var userId = 1;

                var result = await _userVoucherService.FindByUserIdAsync(userId);
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(VoucherInsertPlainVM model)
        {

            try
            {
                if (!ModelState.IsValid)
                    return new BadRequestObjectResult(new { Message = "Validation not valid", ValidationResult = ModelState.Values.SelectMany(state => state.Errors) });

                var result = _voucherServices.InsertModel(model);
                return Ok(new { Message = "Data Saved", ValidationResult = ModelState.Values.SelectMany(state => state.Errors) });

            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { Message = "Cannot Save Data", Exception = ex });

            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(VoucherInsertPlainVM model)
        {

            try
            {
                if (!ModelState.IsValid)
                    return new BadRequestObjectResult(new { Message = "Validation not valid", ValidationResult = ModelState.Values.SelectMany(state => state.Errors) });

                var result = _voucherServices.UpdateModel(model);
                return Ok(new { Message = "Data Saved", ValidationResult = ModelState.Values.SelectMany(state => state.Errors) });

            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { Message = "Cannot Save Data", Exception = ex });

            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new BadRequestObjectResult(new { Message = "Validation not valid", ValidationResult = ModelState.Values.SelectMany(state => state.Errors) });

                _voucherServices.Delete(id);
                return Ok(new { Message = "Data Saved", ValidationResult = ModelState.Values.SelectMany(state => state.Errors) });

            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { Message = "Cannot Save Data", Exception = ex });

            }
        }

        [HttpPost]
        [Route("use-voucher")]
        [AuthorizationServiceFactory]
        public async Task<IActionResult> UseVoucher([FromBody] UseVoucherViewModel viewModel)
        {
            try
            {
                //override null value
                if (viewModel.ProductGiftChoose == null)
                    viewModel.ProductGiftChoose = new List<ProductGiftChooseViewModel>();
                
                var user = GetMe();
                if (!ModelState.IsValid)
                    return new BadRequestObjectResult(new { Message = "Validation not valid", ValidationResult = ModelState.Values.SelectMany(state => state.Errors) });

                //override null
                if (viewModel.ProductGiftChoose == null)
                    viewModel.ProductGiftChoose = new List<ProductGiftChooseViewModel>();

                var resultDiscountPotential = await _voucherServices.UseVoucher(viewModel, _servicesConfig.ProductIds, user, _servicesConfig.ProductServiceURI);
                //return Ok(new { discountPotential = resultDiscountPotential });
                //return Ok(new ResponseUseVoucherViewModel
                //{
                //    discountPotential = resultDiscountPotential.discountPotential,
                //    freeProduct = resultDiscountPotential.freeProduct
                //});
                return Ok(resultDiscountPotential);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { Message = "Cannot Save Data", Exception = ex.Message });

            }
        }

        [HttpDelete]
        [Route("remove-voucher/{userVoucherId}")]
        [AuthorizationServiceFactory]
        public async Task<IActionResult> RemoveVoucher([FromRoute] int userVoucherId)
        {
            try
            {
                var user = GetMe();
                if (!ModelState.IsValid)
                    return new BadRequestObjectResult(new { Message = "Validation not valid", ValidationResult = ModelState.Values.SelectMany(state => state.Errors) });

                _voucherServices.DeleteUserVoucher(userVoucherId, user, _servicesConfig.ProductServiceURI);
                //return Ok(new { discountPotential = resultDiscountPotential });
                //return Ok(new ResponseUseVoucherViewModel
                //{
                //    discountPotential = resultDiscountPotential.discountPotential,
                //    freeProduct = resultDiscountPotential.freeProduct
                //});
                return Ok(new { Message = "Data Deleted" });


            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { Message = "Cannot Save Data", Exception = ex.Message });

            }
        }

        [HttpPost]
        [Route("redeem-voucher")]
        //[AuthorizationService]
        public async Task<IActionResult> RedeemVoucher([FromBody] RedeemVoucherBody model)
        {
            var result = _redeemVoucherService.RedeemVoucherProduct(model,_servicesConfig.GetUserDetail,_servicesConfig.UpdatePointUser);

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
