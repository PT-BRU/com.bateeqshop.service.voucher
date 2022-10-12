using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using com.bateeqshop.service.voucher.api.Configuration;
using com.bateeqshop.service.voucher.api.CronJob;
using com.bateeqshop.service.voucher.business.CronJob;
using com.bateeqshop.service.voucher.business;
using com.bateeqshop.service.voucher.business.Service;
using com.bateeqshop.service.voucher.business.Service.ExchangePoints;
using com.bateeqshop.service.voucher.business.Service.GeneralSettings;
using com.bateeqshop.service.voucher.business.Service.MyRewards;
using com.bateeqshop.service.voucher.business.Service.ProductCarts;
using com.bateeqshop.service.voucher.business.Service.Products;
using com.bateeqshop.service.voucher.business.Service.RedeemVoucher;
using com.bateeqshop.service.voucher.business.Service.RewardPoint;
using com.bateeqshop.service.voucher.business.Service.UserVouchers;
using com.bateeqshop.service.voucher.business.Service.VoucherNominals;
using com.bateeqshop.service.voucher.business.Service.VoucherPercentages;
using com.bateeqshop.service.voucher.business.Service.VoucherProducts;
using com.bateeqshop.service.voucher.business.Service.Vouchers;
using com.bateeqshop.service.voucher.business.Service.VoucherType1s;
using com.bateeqshop.service.voucher.business.Service.VoucherType2Categorys;
using com.bateeqshop.service.voucher.business.Service.VoucherType2Products;
using com.bateeqshop.service.voucher.business.Service.VoucherType3s;
using com.bateeqshop.service.voucher.business.Service.VoucherType4s;
using com.bateeqshop.service.voucher.business.ViewModel;
using com.bateeqshop.service.voucher.data;
using com.bateeqshop.service.voucher.data.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using com.bateeqshop.service.voucher.api.CustomAttributes.AuthServices;

namespace com.bateeqshop.service.voucher.api
{
    public class Startup
    {

        private readonly string[] EXPOSED_HEADERS = new string[] { "Content-Disposition", "api-version", "content-length", "content-md5", "content-type", "date", "request-id", "response-time" };
        private readonly string AUTH_POLICY = "AuthPolicy";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddApiVersioning(options => options.DefaultApiVersion = new ApiVersion(1, 0));
            services.AddControllers();
            services.AddDbContext<VoucherDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddTransient<IService<VoucherVM>, VoucherService>();
            services.AddTransient<IHasUserIdService<UserVoucherVM>, UserVoucherService>();
            services.AddTransient<IService<RewardPoints>, RewardPointService>();
            services.AddTransient<IRewardPointService, RewardsPointsService>();
            services.AddTransient<IGeneralSetting, GeneralSettingService>();
            services.AddTransient<IVoucherServices, VoucherServices>();
            services.AddTransient<IService<VoucherVM>, VoucherService>();
            services.AddTransient<IVoucherServices, VoucherServices>();
            services.AddTransient<IVoucherPercentagesService, VoucherPercentagesService>();
            services.AddTransient<IVoucherNominalsService, VoucherNominalsService>();
            services.AddTransient<IVoucherType1sService, VoucherType1sService>();
            services.AddTransient<IVoucherType2CategorysService, VoucherType2CategorysService>();
            services.AddTransient<IVoucherType2ProductsService, VoucherType2ProductsService>();
            services.AddTransient<IVoucherType3sService, VoucherType3sService>();
            services.AddTransient<IVoucherType4sService, VoucherType4sService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IExchangePointServices, ExchangePointServices>();
            services.AddTransient<IVoucherProductService, VoucherProductService>();
            services.AddTransient<IRedeemVoucherService, RedeemVoucherServices>();
            services.AddTransient<IRewardService, RewardService>();
            services.AddTransient<IUserVoucherServices, UserVoucherServices>();
            services.AddTransient<IProductCartService, ProductCartService>();
            services.AddTransient<IService<UserVoucherVM>, UserVoucherServices>();
            services.AddScoped<AuthorizationService>();
            //Newtonsoft Json
            services.AddControllersWithViews().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            #region CronJob
            services.AddHostedService<QuartzHostedService>();
            services.AddTransient<IJobFactory, SingletonJobFactory>();
            services.AddTransient<ISchedulerFactory, StdSchedulerFactory>();

            // Add our job
            services.AddTransient<AutoRemoveUseVoucherJob>();
            services.AddSingleton(new CronJobSchedule(
                jobType: typeof(AutoRemoveUseVoucherJob),
                cronExpression: "0 * * * * ?"));
            #endregion

            #region Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo() { Title = "Voucher API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter into field the word 'Bearer' following by space and JWT",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement());
                c.CustomSchemaIds(i => i.FullName);
            });
            #endregion

            #region CORS

            //services.AddCors(options => options.AddPolicy(AUTH_POLICY, builder =>
            //{
            //    builder.AllowAnyOrigin()
            //           .AllowAnyMethod()
            //           .AllowAnyHeader()
            //           .WithExposedHeaders(EXPOSED_HEADERS);
            //}));

            services.AddCors(options => options.AddPolicy("AllowOrigin", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .WithMethods("POST","GET","DELETE","PUT","OPTIONS");
            }));

            #endregion

            #region Configuration
            var servicesConfig = Configuration.GetSection("Services");
            services.Configure<ServicesConfig>(servicesConfig);
            #endregion


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            #region AutoMigrate
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<VoucherDbContext>();
                context.Database.Migrate();
            }
            #endregion
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Voucher API");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("AllowOrigin");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
