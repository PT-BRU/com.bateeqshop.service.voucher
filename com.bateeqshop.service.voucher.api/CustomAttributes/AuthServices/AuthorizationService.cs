using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml.Schema;
using com.bateeqshop.service.voucher.api.Configuration;
using com.bateeqshop.service.voucher.business.ViewModel.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace com.bateeqshop.service.voucher.api.CustomAttributes.AuthServices
{

    public class AuthorizationService : ActionFilterAttribute
    {
        private readonly IConfiguration _config;


        public string Url { get; set; }
        public AuthorizationService(IConfiguration config)
        {
            _config = config;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            #region getMe
            try
            {
                var urlAuth = _config.GetSection("Services").Get<ServicesConfig>();

                var schemaAuth = "Bearer ";
                //var apiAuth = "http://com-bateeqshop-service-auth.azurewebsites.net//v1/token";
                var apiAuth = urlAuth.AuthToken;
                var client = new HttpClient();
                var authorToken = context.HttpContext.Request.Headers.Where(s=> s.Key.ToLower() == "authorization").FirstOrDefault();
                //var token = authorToken != null ? authorToken.Value.ToString() : schemaAuth;
                var token = authorToken.Value.ToString();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (string.IsNullOrEmpty(token)) {

                    ContentResult content = new ContentResult();
                    content.ContentType = "application/json";
                    content.StatusCode = 401;
                    content.Content = JsonConvert.SerializeObject(new { Message = "Bearer Token Not Exist" });

                    context.Result = content;
                    base.OnActionExecuting(context);

                    return;
                }
                else
                    client.DefaultRequestHeaders.Add("Authorization", token);


                var result = Task.Run(()=> client.GetAsync(apiAuth)).Result;
                var statusCode = (int)result.StatusCode;
                if(statusCode == 401)
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized));
                else if (statusCode == 400)
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized));


                string jsonContent = result.Content.ReadAsStringAsync().Result;
                ResponseUserMe response = JsonConvert.DeserializeObject<ResponseUserMe>(jsonContent);
                response.Token = token;
                response.StatusCode = statusCode;

                context.HttpContext.Request.Headers.Add("User-Auth", JsonConvert.SerializeObject(response));
                base.OnActionExecuting(context);
                //return;
            }
            catch (HttpResponseException e)
            {
                //throw e;
                ContentResult content = new ContentResult();
                content.ContentType = "application/json";
                content.StatusCode = 401;
                content.Content = JsonConvert.SerializeObject(new { Message = "Unauthorized"});

                context.Result = content;
                base.OnActionExecuting(context);

                return;
            }
            #endregion
            //base.OnActionExecuting(context);
        }
    }
}
