using com.bateeqshop.service.voucher.business.ViewModel.ExchangePointVM;
using com.bateeqshop.service.voucher.business.ViewModel.UserInfo;
using com.bateeqshop.service.voucher.business.ViewModel.Vouchers;
using com.bateeqshop.service.voucher.data;
using com.bateeqshop.service.voucher.data.Model;
using Com.Moonlay.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace com.bateeqshop.service.voucher.business.Service.RedeemVoucher
{
    public class RedeemVoucherServices : IRedeemVoucherService
    {
        private readonly VoucherDbContext _context;
        private string _userBy = "BATEEQSHOP-VOUCHER";
        private string _userAgent = "BATEEQSHOP-VOUCHER";
        public RedeemVoucherServices(
            VoucherDbContext context
            )
        {
            _context = context;
        }

        //public int RedeemVoucherNominel(RedeemVoucherNominal model)
        //{
        //    #region
        //    var client = new HttpClient();
        //    var apiurl = "https://bateeq-api-auth.azurewebsites.net/v1/api/getUserDetail/";
        //    var url = apiurl + model.UserId;

        //    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        //    var resClient = Task.Run(() => client.GetAsync(url));

        //    string jsonContent = resClient.Result.Content.ReadAsStringAsync().Result;
        //    User user = JsonConvert.DeserializeObject<User>(jsonContent);
        //    #endregion

        //    #region
        //    var query = _context.Vouchers.Where(entity => entity.Id == model.VoucherId).AsQueryable();

        //    var queryPercentage = _context.VoucherPercentages.AsQueryable();
        //    var queryNominal = _context.VoucherNominals.AsQueryable();
        //    var queryProduct = _context.VoucherProducts.AsQueryable();
        //    var queryType1 = _context.VoucherType1s.AsQueryable();
        //    var queryType2Categ = _context.VoucherType2Categories.AsQueryable();
        //    var queryType2Product = _context.VoucherType2Products.AsQueryable();
        //    var queryType3 = _context.VoucherType3s.AsQueryable();
        //    var queryType4 = _context.VoucherType4s.AsQueryable();

        //    var userVoucher = _context.UserVouchers.AsQueryable();

        //    var resultVM = from q in query
        //                   join percentage1 in queryPercentage on q.TypeId equals percentage1.Id into percentages
        //                   from percentage in percentages.DefaultIfEmpty()
        //                   join nominal1 in queryNominal on q.TypeId equals nominal1.Id into nominals
        //                   from nominal in nominals.DefaultIfEmpty()
        //                   join product1 in queryProduct on q.TypeId equals product1.Id into products
        //                   from product in products.DefaultIfEmpty()
        //                   join type11 in queryType1 on q.TypeId equals type11.Id into type1s
        //                   from type1 in type1s.DefaultIfEmpty()
        //                   join type2categ1 in queryType2Categ on q.TypeId equals type2categ1.Id into type2categ1s
        //                   from type2categ in type2categ1s.DefaultIfEmpty()
        //                   join type2product1 in queryType2Product on q.TypeId equals type2product1.Id into type2products
        //                   from type2product in type2products.DefaultIfEmpty()
        //                   join type31 in queryType3 on q.TypeId equals type31.Id into type3s
        //                   from type3 in type3s.DefaultIfEmpty()
        //                   join type41 in queryType4 on q.TypeId equals type41.Id into type4s
        //                   from type4 in type4s.DefaultIfEmpty()

        //                   select new VoucherInsertVM
        //                   {
        //                       Code = q.Code,
        //                       Id = q.Id,
        //                       Type = q.Type.ToDescription(),
        //                       TypeId = (int)q.Type,
        //                       TypeEnum = q.Type,
        //                       VoucherPercentageData = q.Type == Voucher.VoucherType.Percentage ? percentage : null,
        //                       VoucherNominalData = q.Type == Voucher.VoucherType.Nominal ? nominal : null,
        //                       VoucherProductData = q.Type == Voucher.VoucherType.Product ? product : null,
        //                       VoucherType1Data = q.Type == Voucher.VoucherType.Type1 ? type1 : null,
        //                       VoucherType2CategoryData = q.Type == Voucher.VoucherType.Type2Category ? type2categ : null,
        //                       VoucherType2ProductData = q.Type == Voucher.VoucherType.Type2Product ? type2product : null,
        //                       VoucherType3Data = q.Type == Voucher.VoucherType.Type3 ? type3 : null,
        //                       VoucherType4Data = q.Type == Voucher.VoucherType.Type4 ? type4 : null,
        //                       UserVouchers = userVoucher.Where(s => s.VoucherId == q.Id).Count()
        //                   };

        //    var resultSimply = resultVM
        //        .Select(s => new ExchangePointHistoryVM
        //        {
        //            Id = s.Id,
        //            RedeemedDate =
        //            s.TypeEnum == Voucher.VoucherType.Percentage ? s.VoucherPercentageData.StartDate.ToString("dd/MM/yyyy") :
        //            s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.StartDate.ToString("dd/MM/yyyy") :
        //            s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.StartDate.ToString("dd/MM/yyyy") :
        //            s.TypeEnum == Voucher.VoucherType.Type1 ? s.VoucherType1Data.StartDate.ToString("dd/MM/yyyy") :
        //            s.TypeEnum == Voucher.VoucherType.Type2Category ? s.VoucherType2CategoryData.StartDate.ToString("dd/MM/yyyy") :
        //            s.TypeEnum == Voucher.VoucherType.Type2Product ? s.VoucherType2ProductData.StartDate.ToString("dd/MM/yyyy") :
        //            s.TypeEnum == Voucher.VoucherType.Type3 ? s.VoucherType3Data.StartDate.ToString("dd/MM/yyyy") :
        //            s.TypeEnum == Voucher.VoucherType.Type4 ? s.VoucherType4Data.StartDate.ToString("dd/MM/yyyy") : string.Empty,
        //            VoucherName =
        //            s.TypeEnum == Voucher.VoucherType.Percentage ? s.VoucherPercentageData.Name :
        //            s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.Name :
        //            s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.Name :
        //            s.TypeEnum == Voucher.VoucherType.Type1 ? s.VoucherType1Data.Name :
        //            s.TypeEnum == Voucher.VoucherType.Type2Category ? s.VoucherType2CategoryData.Name :
        //            s.TypeEnum == Voucher.VoucherType.Type2Product ? s.VoucherType2ProductData.Name :
        //            s.TypeEnum == Voucher.VoucherType.Type3 ? s.VoucherType3Data.Name :
        //            s.TypeEnum == Voucher.VoucherType.Type4 ? s.VoucherType4Data.Name : string.Empty,
        //            RedeemedPoint =
        //            (int)(s.TypeEnum == Voucher.VoucherType.Percentage ? s.VoucherPercentageData.ExchangePoint :
        //            s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.ExchangePoint :
        //            s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.ExchangePoint : decimal.Zero),
        //            StartDate =
        //            s.TypeEnum == Voucher.VoucherType.Percentage ? s.VoucherPercentageData.StartDate.ToString("yyyy-MM-dd") :
        //            s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.StartDate.ToString("yyyy-MM-dd") :
        //            s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.StartDate.ToString("yyyy-MM-dd") :
        //            s.TypeEnum == Voucher.VoucherType.Type1 ? s.VoucherType1Data.StartDate.ToString("yyyy-MM-dd") :
        //            s.TypeEnum == Voucher.VoucherType.Type2Category ? s.VoucherType2CategoryData.StartDate.ToString("yyyy-MM-dd") :
        //            s.TypeEnum == Voucher.VoucherType.Type2Product ? s.VoucherType2ProductData.StartDate.ToString("yyyy-MM-dd") :
        //            s.TypeEnum == Voucher.VoucherType.Type3 ? s.VoucherType3Data.StartDate.ToString("yyyy-MM-dd") :
        //            s.TypeEnum == Voucher.VoucherType.Type4 ? s.VoucherType4Data.StartDate.ToString("yyyy-MM-dd") : string.Empty,
        //            EndDate =
        //            s.TypeEnum == Voucher.VoucherType.Percentage ? s.VoucherPercentageData.ExpiredDate.ToString("yyyy-MM-dd") :
        //            s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.ExpiredDate.ToString("yyyy-MM-dd") :
        //            s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.ExpiredDate.ToString("yyyy-MM-dd") :
        //            s.TypeEnum == Voucher.VoucherType.Type1 ? s.VoucherType1Data.ExpiredDate.ToString("yyyy-MM-dd") :
        //            s.TypeEnum == Voucher.VoucherType.Type2Category ? s.VoucherType2CategoryData.ExpiredDate.ToString("yyyy-MM-dd") :
        //            s.TypeEnum == Voucher.VoucherType.Type2Product ? s.VoucherType2ProductData.ExpiredDate.ToString("yyyy-MM-dd") :
        //            s.TypeEnum == Voucher.VoucherType.Type3 ? s.VoucherType3Data.ExpiredDate.ToString("yyyy-MM-dd") :
        //            s.TypeEnum == Voucher.VoucherType.Type4 ? s.VoucherType4Data.ExpiredDate.ToString("yyyy-MM-dd") : string.Empty,
        //        });

        //    var resultVoucher = resultSimply.FirstOrDefault();
        //    #endregion

        //    #region Validation & Update User Point
        //    if (user.TotalPoint < resultVoucher.RedeemedPoint)
        //    {
        //        throw new Exception("Not enough point");
        //    }
        //    else
        //    {
        //        UserVoucher userVoucher1 = new UserVoucher
        //        {
        //            Active = true,
        //            CreatedAgent = "BATEEQ-VOUCHER",
        //            CreatedUtc = DateTime.Now,
        //            CreatedBy = "BATEEQ-VOUCHER",
        //            IsDeleted = false,
        //            IsRedeemed = false,
        //            StartDate = DateTime.Parse(resultVoucher.StartDate),
        //            EndDate = DateTime.Parse(resultVoucher.EndDate),
        //            VoucherId = model.VoucherId,
        //            UserId = model.UserId
        //        };
        //        _context.UserVouchers.Add(userVoucher1).GetDatabaseValues();
        //        _context.SaveChanges();

        //        ExchangedPointHistory epHistory = new ExchangedPointHistory
        //        {
        //            Active = true,
        //            CreatedAgent = "BATEEQ-VOUCHER",
        //            CreatedBy = "BATEEQ-VOUCHER",
        //            CreatedUtc = DateTime.Now,
        //            IsDeleted = false,
        //            UserVoucherId = userVoucher1.Id,
        //            ExchangedPoint = resultVoucher.RedeemedPoint,
        //        };

        //        _context.ExchangedPointHistories.Add(epHistory).GetDatabaseValues();
        //        _context.SaveChanges();

        //        PointUser pointUser = new PointUser
        //        {
        //            UpdatePoint = user.TotalPoint - resultVoucher.RedeemedPoint,
        //            UserId = model.UserId
        //        };

        //        var requestBody = JsonConvert.SerializeObject(pointUser);
        //        HttpContent request = new StringContent(requestBody);

        //        request.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

        //        var updateUrl = "https://bateeq-api-auth.azurewebsites.net/v1/api/updatePointUser";
        //        var result = client.PutAsync(updateUrl, request);
        //    }

        //    #endregion

        //    return _context.SaveChanges();
        //}

        public int RedeemVoucherProduct(RedeemVoucherBody model,string apiGetUserDetail,string apiUpdatePointUser)
        {
            #region Get User
            var client = new HttpClient();

            //var apiurl = "https://bateeq-api-auth.azurewebsites.net/v1/api/getUserDetail/";
            var apiurl = apiGetUserDetail;


            var url = apiurl + model.UserId; 

            
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var resClient = Task.Run(() => client.GetAsync(url));

            string jsonContent = resClient.Result.Content.ReadAsStringAsync().Result;
            User user = JsonConvert.DeserializeObject<User>(jsonContent);

            #endregion

            #region Get Voucher Info
            var query = _context.Vouchers.Where(entity =>entity.Id == model.VoucherId).AsQueryable();

            var queryPercentage = _context.VoucherPercentages.AsQueryable();
            var queryNominal = _context.VoucherNominals.AsQueryable();
            var queryProduct = _context.VoucherProducts.AsQueryable();
            var queryType1 = _context.VoucherType1s.AsQueryable();
            var queryType2Categ = _context.VoucherType2Categories.AsQueryable();
            var queryType2Product = _context.VoucherType2Products.AsQueryable();
            var queryType3 = _context.VoucherType3s.AsQueryable();
            var queryType4 = _context.VoucherType4s.AsQueryable();

            var userVoucher = _context.UserVouchers.AsQueryable();

            var resultVM = from q in query
                           join percentage1 in queryPercentage on q.TypeId equals percentage1.Id into percentages
                           from percentage in percentages.DefaultIfEmpty()
                           join nominal1 in queryNominal on q.TypeId equals nominal1.Id into nominals
                           from nominal in nominals.DefaultIfEmpty()
                           join product1 in queryProduct on q.TypeId equals product1.Id into products
                           from product in products.DefaultIfEmpty()
                           join type11 in queryType1 on q.TypeId equals type11.Id into type1s
                           from type1 in type1s.DefaultIfEmpty()
                           join type2categ1 in queryType2Categ on q.TypeId equals type2categ1.Id into type2categ1s
                           from type2categ in type2categ1s.DefaultIfEmpty()
                           join type2product1 in queryType2Product on q.TypeId equals type2product1.Id into type2products
                           from type2product in type2products.DefaultIfEmpty()
                           join type31 in queryType3 on q.TypeId equals type31.Id into type3s
                           from type3 in type3s.DefaultIfEmpty()
                           join type41 in queryType4 on q.TypeId equals type41.Id into type4s
                           from type4 in type4s.DefaultIfEmpty()

                           select new VoucherInsertVM
                           {
                               Code = q.Code,
                               Id = q.Id,
                               Type = q.Type.ToDescription(),
                               TypeId = (int)q.Type,
                               TypeEnum = q.Type,
                               VoucherPercentageData = q.Type == Voucher.VoucherType.Percentage ? percentage : null,
                               VoucherNominalData = q.Type == Voucher.VoucherType.Nominal ? nominal : null,
                               VoucherProductData = q.Type == Voucher.VoucherType.Product ? product : null,
                               VoucherType1Data = q.Type == Voucher.VoucherType.Type1 ? type1 : null,
                               VoucherType2CategoryData = q.Type == Voucher.VoucherType.Type2Category ? type2categ : null,
                               VoucherType2ProductData = q.Type == Voucher.VoucherType.Type2Product ? type2product : null,
                               VoucherType3Data = q.Type == Voucher.VoucherType.Type3 ? type3 : null,
                               VoucherType4Data = q.Type == Voucher.VoucherType.Type4 ? type4 : null,
                               UserVouchers = userVoucher.Where(s => s.VoucherId == q.Id).Count()
                           };

            var resultSimply = resultVM
                .Select(s => new ExchangePointHistoryVM
                {
                    Id = s.Id,
                    RedeemedDate =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? s.VoucherPercentageData.StartDate.ToString("dd/MM/yyyy") :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.StartDate.ToString("dd/MM/yyyy") :
                    s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.StartDate.ToString("dd/MM/yyyy") :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? s.VoucherType1Data.StartDate.ToString("dd/MM/yyyy") :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? s.VoucherType2CategoryData.StartDate.ToString("dd/MM/yyyy") :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? s.VoucherType2ProductData.StartDate.ToString("dd/MM/yyyy") :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? s.VoucherType3Data.StartDate.ToString("dd/MM/yyyy") :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? s.VoucherType4Data.StartDate.ToString("dd/MM/yyyy") : string.Empty,
                    VoucherName =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? s.VoucherPercentageData.Name :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.Name :
                    s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.Name :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? s.VoucherType1Data.Name :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? s.VoucherType2CategoryData.Name :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? s.VoucherType2ProductData.Name :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? s.VoucherType3Data.Name :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? s.VoucherType4Data.Name : string.Empty,
                    RedeemedPoint =
                    (int)(s.TypeEnum == Voucher.VoucherType.Percentage ? s.VoucherPercentageData.ExchangePoint : 
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.ExchangePoint : 
                    s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.ExchangePoint : decimal.Zero),
                    MembershipId =
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.MembershipId :
                    s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.MembershipId : string.Empty,
                    ValidityPeriod =
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.ValidityPeriod.ToString() :
                    s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.ValidityPeriod.ToString() : string.Empty,
                    StartDate =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? s.VoucherPercentageData.StartDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.StartDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.StartDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? s.VoucherType1Data.StartDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? s.VoucherType2CategoryData.StartDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? s.VoucherType2ProductData.StartDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? s.VoucherType3Data.StartDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? s.VoucherType4Data.StartDate.ToString("yyyy-MM-dd") : string.Empty,
                    EndDate =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? s.VoucherPercentageData.ExpiredDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.ExpiredDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.ExpiredDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? s.VoucherType1Data.ExpiredDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? s.VoucherType2CategoryData.ExpiredDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? s.VoucherType2ProductData.ExpiredDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? s.VoucherType3Data.ExpiredDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? s.VoucherType4Data.ExpiredDate.ToString("yyyy-MM-dd") : string.Empty,
                });

            var resultVoucher = resultSimply.FirstOrDefault();
            #endregion

            #region Validation & Update User Point
            if (user.TotalPoint < resultVoucher.RedeemedPoint)
            {
                throw new Exception("Not enough point");
            }
            else
            {

                UserVoucher userVoucher1 = new UserVoucher
                {
                    Active = true,
                    CreatedAgent = "BATEEQ-VOUCHER",
                    CreatedUtc = DateTime.Now,
                    CreatedBy = "BATEEQ-VOUCHER",
                    IsDeleted = false, 
                    IsRedeemed = false,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddMonths(Convert.ToInt32(resultVoucher.ValidityPeriod)),
                    VoucherId = model.VoucherId,
                    UserId = model.UserId
                };

                _context.UserVouchers.Add(userVoucher1).GetDatabaseValues();
                _context.SaveChanges();

                //if (Convert.ToDateTime(userVoucher1.EndDate).Date >= Convert.ToDateTime(resultVoucher.EndDate).Date)
                //{
                //    userVoucher1.EndDate =Convert.ToDateTime(resultVoucher.EndDate);
                //    EntityExtension.FlagForUpdate(userVoucher1, "BATEEQSHOP-VOUCHER", "BATEEQSHOP-VOUCHER");
                //    _context.SaveChanges();
                //}

                //TODO : Redeem Voucher must save product
                if (model.ProductId != 0)
                {
                    UserVoucherRedeemProduct redeemProduct = new UserVoucherRedeemProduct
                    {
                        ProductId = model.ProductId,
                        UserVoucherId = userVoucher1.Id,
                        ProductDetailId = model.ProductDetailId
                    };
                    EntityExtension.FlagForCreate(redeemProduct, _userBy, _userAgent);

                    var userVoucherRedeemProductSave = _context.UserVoucherRedeemProducts.Add(redeemProduct);
                    _context.SaveChanges();

                }
                ExchangedPointHistory epHistory = new ExchangedPointHistory
                {
                    Active = true,
                    CreatedAgent = "BATEEQ-VOUCHER",
                    CreatedBy = "BATEEQ-VOUCHER",
                    CreatedUtc = DateTime.Now,
                    IsDeleted = false,
                    UserVoucherId = userVoucher1.Id,
                    ExchangedPoint = resultVoucher.RedeemedPoint,
                };

                _context.ExchangedPointHistories.Add(epHistory).GetDatabaseValues();
                _context.SaveChanges();

                PointUser pointUser = new PointUser
                {   
                    UpdatePoint = user.TotalPoint - resultVoucher.RedeemedPoint,
                    UserId = model.UserId
                };

                var requestBody = JsonConvert.SerializeObject(pointUser);
                HttpContent request = new StringContent(requestBody);

                request.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                //var updateUrl = "https://bateeq-api-auth.azurewebsites.net/v1/api/updatePointUser";
                var updateUrl = apiUpdatePointUser;

                var result = client.PutAsync(updateUrl, request);
            }

            #endregion

            return _context.SaveChanges();
        }

    }
}
