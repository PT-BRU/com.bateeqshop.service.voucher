using com.bateeqshop.service.voucher.business.Enum;
using com.bateeqshop.service.voucher.business.Helper;
using com.bateeqshop.service.voucher.business.Service.Products;
using com.bateeqshop.service.voucher.business.ViewModel.ExchangePointVM;
using com.bateeqshop.service.voucher.business.ViewModel.MyRewards;
using com.bateeqshop.service.voucher.business.ViewModel.Products;
using com.bateeqshop.service.voucher.business.ViewModel.Users;
using com.bateeqshop.service.voucher.business.ViewModel.Vouchers;
using com.bateeqshop.service.voucher.data;
using com.bateeqshop.service.voucher.data.Model;
using Com.Moonlay.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.bateeqshop.service.voucher.business.Service.MyRewards
{
    public class RewardService : IRewardService
    {
        private readonly VoucherDbContext _context;
        private readonly IProductService _productService;

        public RewardService(
            VoucherDbContext context,
            IProductService productService
            )
        {
            _context = context;
            _productService = productService;
        }

        public int UpdateStatusCheckout(ResponseUserMe users, List<int> userVoucherId)
        {
            var rewards = _context.UserVouchers
                .Where(s => userVoucherId.Contains(s.Id))
                .ToList();
            
            foreach(UserVoucher i in rewards)
            {
                var vouchers = _context.UserVouchers
                    .Where(s => s.Id == i.Id).FirstOrDefault();

                vouchers.IsCheckout = true;
                _context.SaveChanges();
                //EntityExtension.FlagForUpdate(i, "BATEEQ-VOUCHER", "BATEEQ-VOUCHER");
            }

            return _context.SaveChanges();
        }
        public List<MyRewardVM> GetMyRewardByUserId(int id)
        {
            var VoucherId = _context.UserVouchers
                .Where(entity => entity.UserId == id && entity.IsRedeemed == false)
                .Select(entity => entity.VoucherId)
                //.Distinct()
                .ToList();

            var query = _context.Vouchers.Where(entity => VoucherId.Contains(entity.Id)).AsQueryable();


            #region get voucher info
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
                    VoucherType = s.Type,
                    ProductGift =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? string.Empty :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? string.Empty :
                    s.TypeEnum == Voucher.VoucherType.Product ? _context.UserVoucherRedeemProducts.Where(t => t.UserVoucher.VoucherId == s.Id && t.UserVoucher.UserId == id && !t.UserVoucher.IsRedeemed).FirstOrDefault().ProductId.ToString() :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? _context.UserVoucherRedeemProducts.Where(t => t.UserVoucher.VoucherId == s.Id && t.UserVoucher.UserId == id && !t.UserVoucher.IsRedeemed).FirstOrDefault().ProductId.ToString() :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? string.Empty :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? string.Empty :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? string.Empty :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? _context.UserVoucherRedeemProducts.Where(t => t.UserVoucher.VoucherId == s.Id && t.UserVoucher.UserId == id && !t.UserVoucher.IsRedeemed).FirstOrDefault().ProductId.ToString() : string.Empty,
                    ProductDetailGiftId =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? string.Empty :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? string.Empty :
                    s.TypeEnum == Voucher.VoucherType.Product ? _context.UserVoucherRedeemProducts.Where(t => t.UserVoucher.VoucherId == s.Id && t.UserVoucher.UserId == id && !t.UserVoucher.IsRedeemed).FirstOrDefault().ProductDetailId.ToString() :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? _context.UserVoucherRedeemProducts.Where(t => t.UserVoucher.VoucherId == s.Id && t.UserVoucher.UserId == id && !t.UserVoucher.IsRedeemed).FirstOrDefault().ProductDetailId.ToString() :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? string.Empty :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? string.Empty :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? string.Empty :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? _context.UserVoucherRedeemProducts.Where(t => t.UserVoucher.VoucherId == s.Id && t.UserVoucher.UserId == id && !t.UserVoucher.IsRedeemed).FirstOrDefault().ProductDetailId.ToString() : string.Empty,
                });

            var test = resultSimply.ToList();
            #endregion

            var result = _context.UserVouchers
                .Where(entity => entity.UserId == id && entity.IsRedeemed == false)
                .Join(resultSimply,
                exPoint => exPoint.VoucherId,
                voucher => voucher.Id,
                (exPoint, voucher) => new MyRewardVM
                {
                    EndDate = exPoint.EndDate.ToString("yyyy-MM-dd"),
                    StartDate = exPoint.StartDate.ToString("yyyy-MM-dd"),
                    VoucherName = voucher.VoucherName,
                    VoucherId = voucher.Id,
                    VoucherType = voucher.VoucherType,
                    ProductGiftId = voucher.ProductGift,
                    ProductDetailGiftId = voucher.ProductDetailGiftId
                })
                .ToList();

            var results = result.OrderByDescending(entity => entity.StartDate).ToList();

            var resultGroupby = result.GroupBy(
                    key => key.VoucherId,
                    value => value,
                    (key, value) => new MyRewardVM
                    {
                        VoucherId = key,
                        VoucherName = value.FirstOrDefault().VoucherName,
                        StartDate = value.FirstOrDefault().StartDate,
                        EndDate = value.FirstOrDefault().EndDate,
                        Count = value.Count(),
                        ProductDetailGiftId = value.FirstOrDefault().ProductDetailGiftId,
                        ProductGiftId = value.FirstOrDefault().ProductGiftId
                    }
                ).ToList();

            return results;
        }

        public List<MyRewardVM> GetMyRewardByUserIdWithRedeemStatus(int id, bool statusRedeem, bool statusCheckout)
        {
            var VoucherId = _context.UserVouchers
                .Where(entity => entity.UserId == id && entity.IsRedeemed == statusRedeem && entity.IsCheckout == statusCheckout && entity.IsDeleted == false)
                .Select(entity => entity.VoucherId)
                //.Distinct()
                .ToList();

            var query = _context.Vouchers.Where(entity => VoucherId.Contains(entity.Id)).AsQueryable();


            #region get voucher info
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
                    VoucherType = s.Type,
                    ProductGift = string.Empty,
                    //s.TypeEnum == Voucher.VoucherType.Percentage ? string.Empty :
                    //s.TypeEnum == Voucher.VoucherType.Nominal ? string.Empty :
                    ////s.TypeEnum == Voucher.VoucherType.Product ? _context.UserVoucherRedeemProducts.Where(t => t.UserVoucher.VoucherId == s.Id && t.UserVoucher.UserId == id && !t.UserVoucher.IsRedeemed).FirstOrDefault().ProductId.ToString() :
                    ////s.TypeEnum == Voucher.VoucherType.Type1 ? _context.UserVoucherRedeemProducts.Where(t => t.UserVoucher.VoucherId == s.Id && t.UserVoucher.UserId == id && !t.UserVoucher.IsRedeemed).FirstOrDefault().ProductId.ToString() :
                    //s.TypeEnum == Voucher.VoucherType.Product ? _context.UserVoucherRedeemProducts.Where(t => t.UserVoucherId == s.Id && !t.UserVoucher.IsRedeemed).FirstOrDefault().ProductId.ToString() :
                    //s.TypeEnum == Voucher.VoucherType.Type1 ? _context.UserVoucherRedeemProducts.Where(t => t.UserVoucherId == s.Id && !t.UserVoucher.IsRedeemed).FirstOrDefault().ProductId.ToString() :
                    //s.TypeEnum == Voucher.VoucherType.Type2Category ? string.Empty :
                    //s.TypeEnum == Voucher.VoucherType.Type2Product ? string.Empty :
                    //s.TypeEnum == Voucher.VoucherType.Type3 ? string.Empty :
                    //s.TypeEnum == Voucher.VoucherType.Type4 ? _context.UserVoucherRedeemProducts.Where(t => t.UserVoucherId == s.Id && !t.UserVoucher.IsRedeemed).FirstOrDefault().ProductId.ToString() : string.Empty,
                    ProductDetailGiftId = string.Empty,
                    //s.TypeEnum == Voucher.VoucherType.Percentage ? string.Empty :
                    //s.TypeEnum == Voucher.VoucherType.Nominal ? string.Empty :
                    //s.TypeEnum == Voucher.VoucherType.Product ? _context.UserVoucherRedeemProducts.Where(t => t.UserVoucherId == s.Id && !t.UserVoucher.IsRedeemed).FirstOrDefault().ProductDetailId.ToString() :
                    //s.TypeEnum == Voucher.VoucherType.Type1 ? _context.UserVoucherRedeemProducts.Where(t => t.UserVoucherId == s.Id && !t.UserVoucher.IsRedeemed).FirstOrDefault().ProductDetailId.ToString() :
                    //s.TypeEnum == Voucher.VoucherType.Type2Category ? string.Empty :
                    //s.TypeEnum == Voucher.VoucherType.Type2Product ? string.Empty :
                    //s.TypeEnum == Voucher.VoucherType.Type3 ? string.Empty :
                    //s.TypeEnum == Voucher.VoucherType.Type4 ? _context.UserVoucherRedeemProducts.Where(t => t.UserVoucherId == s.Id && !t.UserVoucher.IsRedeemed).FirstOrDefault().ProductDetailId.ToString() : string.Empty,
                    Member = 
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.MembershipId : 
                    s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.MembershipId : null,
                });

            //var test = resultSimply.ToList();
            #endregion

            if (statusRedeem == false)
            {
                var result1 = _context.UserVouchers.Include(s => s.UserVoucherRedeemProduct)
                .Where(entity => entity.UserId == id && entity.IsRedeemed == statusRedeem && entity.IsCheckout == statusCheckout && entity.Voucher.Type == Voucher.VoucherType.Product || entity.UserId == id && entity.IsRedeemed == statusRedeem && entity.IsCheckout == statusCheckout && entity.Voucher.Type == Voucher.VoucherType.Nominal)
                .Join(resultSimply,
                exPoint => exPoint.VoucherId,
                voucher => voucher.Id,
                (exPoint, voucher) => new MyRewardVM
                {
                    EndDate = exPoint.EndDate.ToString("yyyy-MM-dd"),
                    StartDate = exPoint.StartDate.ToString("yyyy-MM-dd"),
                    VoucherName = voucher.VoucherName,
                    VoucherId = voucher.Id,
                    VoucherType = voucher.VoucherType,
                    ProductGiftId = exPoint.UserVoucherRedeemProduct == null? string.Empty: exPoint.UserVoucherRedeemProduct.FirstOrDefault().ProductId.ToString(),
                    ProductDetailGiftId = exPoint.UserVoucherRedeemProduct == null? string.Empty:exPoint.UserVoucherRedeemProduct.FirstOrDefault().ProductDetailId.ToString(),
                    UserVoucherId = exPoint.Id, 
                    Member = voucher.Member
                })
                .ToList();

                var resultss = result1.Where(s => s.Member != null).OrderByDescending(entity => entity.StartDate).ToList();

                return resultss;
            }

            var result = _context.UserVouchers.Include(s=> s.UserVoucherRedeemProduct)
                .Where(entity => entity.UserId == id && entity.IsRedeemed == statusRedeem && entity.IsCheckout == statusCheckout)
                .Join(resultSimply,
                exPoint => exPoint.VoucherId,
                voucher => voucher.Id,
                (exPoint, voucher) => new MyRewardVM
                {
                    EndDate = exPoint.EndDate.ToString("yyyy-MM-dd"),
                    StartDate = exPoint.StartDate.ToString("yyyy-MM-dd"),
                    VoucherName = voucher.VoucherName,
                    VoucherId = voucher.Id,
                    VoucherType = voucher.VoucherType,
                    ProductGiftId = exPoint.UserVoucherRedeemProduct == null ? string.Empty : exPoint.UserVoucherRedeemProduct.FirstOrDefault().ProductId.ToString(),
                    ProductDetailGiftId = exPoint.UserVoucherRedeemProduct == null ? string.Empty : exPoint.UserVoucherRedeemProduct.FirstOrDefault().ProductDetailId.ToString(),
                    UserVoucherId = exPoint.Id
                })
                .ToList();

            var results = result.OrderByDescending(entity => entity.StartDate).ToList();

            return results;
        }

        public List<MyRewardWithProductInfoVM> GetMyRewardByUserId(ResponseUserMe users, string urlProductIds)
        {
            var getRewardByUserId = GetMyRewardByUserId(users.UserIds);

            var result = new List<MyRewardWithProductInfoVM>();
            var getRewardWithProductInfo = getRewardByUserId.Select(s => new MyRewardWithProductInfoVM
            {
                Count = s.Count,
                StartDate = s.StartDate,
                EndDate = s.EndDate,
                VoucherId = s.VoucherId,
                VoucherName = s.VoucherName,
                VoucherType = s.VoucherType,
                ProductGift = GetInfoProducts(SplitString(s.ProductGiftId== null ? string.Empty: s.ProductGiftId, ',').Select(t => CustomParse.TryParseInt(t)).ToList(), urlProductIds).Select(t => new Product
                {
                    Id = t.Id,
                    Active = t.Active,
                    CreatedUtc = t.CreatedUtc,
                    CreatedBy = t.CreatedBy,
                    CreatedAgent = t.CreatedAgent,
                    LastModifiedUtc = t.LastModifiedUtc,
                    LastModifiedBy = t.LastModifiedBy,
                    LastModifiedAgent = t.LastModifiedAgent,
                    IsDeleted = t.IsDeleted,
                    DeletedUtc = t.DeletedUtc,
                    DeletedBy = t.DeletedBy,
                    DeletedAgent = t.DeletedAgent,
                    FreeQuantity = t.FreeQuantity,
                    RONumber = t.RONumber,
                    Name = t.Name,
                    DisplayName = t.DisplayName,
                    Description = t.Description,
                    NormalPrice = t.NormalPrice,
                    DiscountPrice = t.DiscountPrice,
                    IsPublished = t.IsPublished,
                    IsFeatured = t.IsFeatured,
                    Size_Guide = t.Size_Guide,
                    IsPreOrder = t.IsPreOrder,
                    Motif = t.Motif,
                    ProductReviews = t.ProductReviews,
                    ProductCategories = t.ProductCategories,
                    ProductImages = t.ProductImages,
                    ProductTags = t.ProductTags,
                    ProductLogos = t.ProductLogos,
                    ProductDetails = t.ProductDetails.Where(d => SplitString(s.ProductDetailGiftId == null? string.Empty:s.ProductDetailGiftId, ',').Contains(d.Id.ToString())).ToList()
                }
                ).ToList()

            });
            return getRewardWithProductInfo.ToList();

        }

        public List<MyRewardWithProductInfoVM> GetMyRewardByUserIdWithRedeemStatus(ResponseUserMe users, bool statusRedeem, bool statusCheckout, string urlProductIds)
        {
            var getRewardByUserId = GetMyRewardByUserIdWithRedeemStatus(users.UserIds,statusRedeem, statusCheckout);

            var result = new List<MyRewardWithProductInfoVM>();
            var getRewardWithProductInfo = getRewardByUserId.Select(s => new MyRewardWithProductInfoVM
            {
                Count = s.Count,
                StartDate = s.StartDate,
                EndDate = s.EndDate,
                VoucherId = s.VoucherId,
                VoucherName = s.VoucherName,
                VoucherType = s.VoucherType,
                UserVoucherId = s.UserVoucherId,
                ProductGift = GetInfoProducts(SplitString(s.ProductGiftId == null ? string.Empty : s.ProductGiftId, ',').Select(t => CustomParse.TryParseInt(t)).ToList(), urlProductIds).Select(t => new Product
                {
                    Id = t.Id,
                    Active = t.Active,
                    CreatedUtc = t.CreatedUtc,
                    CreatedBy = t.CreatedBy,
                    CreatedAgent = t.CreatedAgent,
                    LastModifiedUtc = t.LastModifiedUtc,
                    LastModifiedBy = t.LastModifiedBy,
                    LastModifiedAgent = t.LastModifiedAgent,
                    IsDeleted = t.IsDeleted,
                    DeletedUtc = t.DeletedUtc,
                    DeletedBy = t.DeletedBy,
                    DeletedAgent = t.DeletedAgent,
                    FreeQuantity = t.FreeQuantity,
                    RONumber = t.RONumber,
                    Name = t.Name,
                    DisplayName = t.DisplayName,
                    Description = t.Description,
                    NormalPrice = t.NormalPrice,
                    DiscountPrice = t.DiscountPrice,
                    IsPublished = t.IsPublished,
                    IsFeatured = t.IsFeatured,
                    Size_Guide = t.Size_Guide,
                    IsPreOrder = t.IsPreOrder,
                    Motif = t.Motif,
                    ProductReviews = t.ProductReviews,
                    ProductCategories = t.ProductCategories,
                    ProductImages = t.ProductImages,
                    ProductTags = t.ProductTags,
                    ProductLogos = t.ProductLogos,
                    ProductDetails = t.ProductDetails.Where(d => SplitString(s.ProductDetailGiftId == null ? string.Empty : s.ProductDetailGiftId, ',').Contains(d.Id.ToString())).ToList()
                }
                ).ToList()

            });
            return getRewardWithProductInfo.ToList();

        }


        private List<Product> GetInfoProducts(List<int> productIds, string urlProductIds)
        {
            var productUrl = urlProductIds;
            List<string> listString = new List<string>();
            foreach (var str in productIds)
            {
                listString.Add("productIds=" + str.ToString());
            }
            var querystring = string.Join("&", listString);
            productUrl = productUrl + querystring;
            var listProductInfo = _productService.GetProductByListId(productUrl, productIds);
            return listProductInfo;
        }

        private List<string> SplitString(string text, char separator) => text.Split(separator).ToList();

    }
}
