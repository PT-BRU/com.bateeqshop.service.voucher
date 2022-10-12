using com.bateeqshop.service.voucher.data;
using com.bateeqshop.service.voucher.data.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Com.Moonlay.Models;
using com.bateeqshop.service.voucher.business.Service.Vouchers;
using com.bateeqshop.service.voucher.business.ViewModel;
using System.Threading.Tasks;
using com.bateeqshop.service.voucher.business.Service.ProductCarts;
using com.bateeqshop.service.voucher.business.ViewModel.Users;

namespace com.bateeqshop.service.voucher.business.Service.UserVouchers
{
    public class UserVoucherServices : IUserVoucherServices
    {
        private readonly VoucherDbContext _context;
        private readonly string _userBy = "BATEEQSHOP-VOUCHER";
        private readonly string _userAgent = "BATEEQSHOP-VOUCHER";
        private readonly IVoucherServices _voucherService;
        private readonly IProductCartService _productCart;
        public UserVoucherServices(
            VoucherDbContext context,
            IVoucherServices voucherService,
            IProductCartService productCart
            )
        {
            _context = context;
            _voucherService = voucherService;
            _productCart = productCart;
        }

        public void Delete(int id, ResponseUserMe user, string uri)
        {
            try
            {
                var dataId = _context.UserVouchers.Where(entity => entity.Id == id).FirstOrDefault();
                var voucher = _context.Vouchers.Where(x => x.Id == dataId.VoucherId).FirstOrDefault();
                var isMembership = false;

                if (Voucher.VoucherType.Nominal == voucher.Type)
                {
                    if (_context.VoucherNominals.Any(x => x.Id == voucher.TypeId && x.MembershipId != null))
                        isMembership = true;
                }
                else if (Voucher.VoucherType.Product == voucher.Type)
                    isMembership = true;

                if (isMembership)
                {
                    dataId.IsRedeemed = false;
                    EntityExtension.FlagForUpdate(dataId, _userBy, _userAgent);
                }
                else
                    EntityExtension.FlagForDelete(dataId, _userBy, _userAgent);

                _context.Update(dataId);
                _context.SaveChanges();

                if (_context.UserVoucherRedeemProducts.Any(x => x.UserVoucherId == id))
                {
                    var dataRedeemProduct = _context.UserVoucherRedeemProducts.Where(x => x.UserVoucherId == id).FirstOrDefault();
                    EntityExtension.FlagForDelete(dataRedeemProduct, _userBy, _userAgent);
                    _context.Update(dataRedeemProduct);
                    _context.SaveChanges();

                    _productCart.DeleteProductGiftFromCart(dataRedeemProduct.ProductDetailId, uri, user.Token);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<UserVoucherVM> Find()
        {
            throw new NotImplementedException();
        }

        public Task<List<UserVoucherVM>> FindAsync()
        {
            throw new NotImplementedException();
        }

        public List<UserVoucherVM> FindById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserVoucherVM>> FindByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public UserVoucher Insert(UserVoucher model)
        {
            _context.Add(model);

            return model;
        }

        public int Insert(UserVoucherVM model)
        {
            throw new NotImplementedException();
        }

        public ICollection<UserVoucher> InsertListVoucherGeneral(ICollection<Voucher> voucherList, int userId)
        {

            var insertUserVouchers = voucherList.Select(s => new UserVoucher
            {
                VoucherId = s.Id,
                UserId = userId,
                IsRedeemed = true
            });
            List<UserVoucher> resultSaveList = new List<UserVoucher>();
            foreach (var userVoucher in insertUserVouchers)
            {
                var checkVoucherUser = _context.UserVouchers.Where(s => s.VoucherId == userVoucher.VoucherId && s.UserId == userId && !s.IsRedeemed);
                if (checkVoucherUser.Count() > 0)
                {
                    var getFirstVoucher = checkVoucherUser.OrderByDescending(s => s.EndDate).FirstOrDefault();
                    getFirstVoucher.IsRedeemed = true;
                    EntityExtension.FlagForUpdate(getFirstVoucher, _userBy, _userAgent);
                    _context.UserVouchers.Update(getFirstVoucher);
                    _context.SaveChanges();
                    resultSaveList.Add(getFirstVoucher);
                }
                else
                {
                    EntityExtension.FlagForCreate(userVoucher, _userBy, _userAgent);
                    var saveUserVoucher = _context.UserVouchers.Add(userVoucher);
                    resultSaveList.Add(saveUserVoucher.Entity);
                }
            }

            _context.SaveChanges();
            return resultSaveList;
        }

        public ICollection<UserVoucher> InsertListVoucherGeneral(List<int> voucherIds, int userId)
        {
            var voucherList = _voucherService.FindByIds(voucherIds);

            List<UserVoucher> resultSaveList = InsertListVoucherGeneral(voucherList, userId).ToList();
            return resultSaveList;
        }

        public UserVoucherVM InsertModel(UserVoucherVM model)
        {
            throw new NotImplementedException();
        }

        public void Update(UserVoucherVM model)
        {
            throw new NotImplementedException();
        }
    }
}
