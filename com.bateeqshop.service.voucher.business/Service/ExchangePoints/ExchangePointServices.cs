using com.bateeqshop.service.voucher.business.ViewModel.ExchangePointVM;
using com.bateeqshop.service.voucher.business.ViewModel.Vouchers;
using com.bateeqshop.service.voucher.data;
using com.bateeqshop.service.voucher.data.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.bateeqshop.service.voucher.business.Service.ExchangePoints
{
    public class ExchangePointServices : IExchangePointServices
    {
        private readonly VoucherDbContext _context;

        public ExchangePointServices(
            VoucherDbContext context
            )
        {
            _context = context;
        }

        public List<ExchangePointHistoryVM> GetExchangePointByUser(int id)
        {
            var epHistory = _context.ExchangedPointHistories
                .Include(entity => entity.UserVoucher)
                .ThenInclude(entity => entity.Voucher)
                .Where(entity => entity.UserVoucher.UserId == id)
                .IgnoreQueryFilters()
                .Select(entity =>  entity.UserVoucher.Voucher.Id )
                .Distinct()
                .ToList();

            var query = _context.Vouchers.Where(entity => epHistory.Contains(entity.Id)).AsQueryable();

            var queryPercentage = _context.VoucherPercentages.AsQueryable();
            var queryNominal = _context.VoucherNominals.AsQueryable();
            var queryProduct = _context.VoucherProducts.AsQueryable();
            var queryType1 = _context.VoucherType1s.AsQueryable();
            var queryType2Categ = _context.VoucherType2Categories.AsQueryable();
            var queryType2Product = _context.VoucherType2Products.AsQueryable();
            var queryType3 = _context.VoucherType3s.AsQueryable();
            var queryType4 = _context.VoucherType4s.AsQueryable();

            var userVoucher = _context.UserVouchers.Where(s=> s.UserId == id).AsQueryable();

            var resultVM = from uVoucher in userVoucher
                           join q in query on uVoucher.VoucherId equals q.Id 
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
                               UserVouchers = userVoucher.Where(s => s.VoucherId == q.Id).Count(),
                               //RedeemDate = userVoucher.Where(s => s.VoucherId == q.Id && s.UserId == id).Count() > 0 ? userVoucher.FirstOrDefault(s => s.VoucherId == q.Id ).StartDate : new DateTime()
                               //RedeemDate = userVoucher.Where(s => s.VoucherId == q.Id && s.UserId == id).Count() > 0 ? userVoucher.FirstOrDefault(s => s.VoucherId == q.Id).StartDate : new DateTime()
                               RedeemDate = uVoucher.StartDate,
                               UserIdVoucher = uVoucher.Id
                           };

            var resultSimply = resultVM
                .Select(s => new ExchangePointHistoryVM
                {
                    Id = s.Id,
                    //RedeemedDate = 
                    //s.TypeEnum == Voucher.VoucherType.Percentage ? s.VoucherPercentageData.StartDate.ToString("dd/MM/yyyy") :
                    //s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.StartDate.ToString("dd/MM/yyyy") :
                    //s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.StartDate.ToString("dd/MM/yyyy") :
                    //s.TypeEnum == Voucher.VoucherType.Type1 ? s.VoucherType1Data.StartDate.ToString("dd/MM/yyyy") :
                    //s.TypeEnum == Voucher.VoucherType.Type2Category ? s.VoucherType2CategoryData.StartDate.ToString("dd/MM/yyyy") :
                    //s.TypeEnum == Voucher.VoucherType.Type2Product ? s.VoucherType2ProductData.StartDate.ToString("dd/MM/yyyy") :
                    //s.TypeEnum == Voucher.VoucherType.Type3 ? s.VoucherType3Data.StartDate.ToString("dd/MM/yyyy") :
                    //s.TypeEnum == Voucher.VoucherType.Type4 ? s.VoucherType4Data.StartDate.ToString("dd/MM/yyyy") : string.Empty,
                    RedeemedDate = s.RedeemDate.HasValue ?s.RedeemDate.GetValueOrDefault().ToString("dd/MM/yyyy"): "", 
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
                    UserVoucherId = s.UserIdVoucher
                });
            
            var test = resultSimply.ToList();

             var res = _context.ExchangedPointHistories
                .Include(entity => entity.UserVoucher)
                .ThenInclude(entity => entity.Voucher)
                .Where(entity => entity.UserVoucher.UserId == id)
                .IgnoreQueryFilters()
                .Distinct()
                .ToList();

            var result = _context.ExchangedPointHistories
                .Include(entity => entity.UserVoucher)
                .Where(entity => entity.UserVoucher.UserId == id)
                .Join(resultSimply,
                exPoint => exPoint.UserVoucher.Id,
                voucher => voucher.UserVoucherId,
                (exPoint, voucher) => new ExchangePointHistoryVM
                {
                    RedeemedPoint = (int)exPoint.ExchangedPoint,
                    RedeemedDate = voucher.RedeemedDate,
                    VoucherName = voucher.VoucherName,
                    Id = exPoint.Id
                }
                //{
                //    RedeemPoint = exPoint.ExchangedPoint,
                //    RedeemDate = voucher.RedeemedDate,
                //    VoucherName = voucher.VoucherName
                //}
                ).Distinct().ToList().OrderByDescending(s => DateTime.ParseExact(s.RedeemedDate,"dd/MM/yyyy",null)).ToList();

            return result;
        }
    }
}
