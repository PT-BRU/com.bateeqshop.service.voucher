using com.bateeqshop.service.voucher.api.Configuration;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;


namespace com.bateeqshop.service.voucher.api.CustomAttributes.AuthServices
{
    public class AuthorizationServiceFactory : ActionFilterAttribute, IFilterFactory
    {
        public string Url { get; set; }
        public bool IsReusable => false;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var serviceAuth = serviceProvider.GetService<AuthorizationService>();
            serviceAuth.Url = Url;
            return serviceAuth;
        }
    }
}
