using com.bateeqshop.service.voucher.api.Configuration;
using com.bateeqshop.service.voucher.business.Service.Vouchers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace com.bateeqshop.service.voucher.api.CronJob
{
    [DisallowConcurrentExecution]
    public class AutoRemoveUseVoucherJob : IJob
    {
        private readonly ILogger<AutoRemoveUseVoucherJob> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly ServicesConfig _servicesConfig;

        public AutoRemoveUseVoucherJob(IServiceProvider serviceProvider, ILogger<AutoRemoveUseVoucherJob> logger, IOptions<ServicesConfig> servicesConfig)
        {
            _serviceProvider = serviceProvider;
            _servicesConfig = servicesConfig.Value;
            _logger = logger;
        }

        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var _voucherService = scope.ServiceProvider.GetService<IVoucherServices>();

                    var userVoucherRedeemed = _voucherService.GetExpiredRedeemedVoucher();

                    foreach (var userVoucher in userVoucherRedeemed)
                    {
                        _voucherService.DeleteUserVoucher(userVoucher.Id, _servicesConfig.ProductServiceURI);
                    }
                }

                return Task.CompletedTask;
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                return Task.CompletedTask;
            }
        }
    }
}
