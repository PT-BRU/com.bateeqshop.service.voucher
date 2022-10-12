using com.bateeqshop.service.voucher.data;
using com.bateeqshop.service.voucher.data.Model;
using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.bateeqshop.service.voucher.business.Service.UserVoucherRedeemProducts
{
    public class UserVoucherRedeemProductService : IUserVoucherRedeemProductService
    {
        private readonly VoucherDbContext _context;
        private string _userAgent = "BATEEQSHOP-VOUCHER";
        private string _userBy = "BATEEQSHOP-VOUCHER";

        public UserVoucherRedeemProductService(VoucherDbContext context)
        {
            _context = context;
        }

        public void Delete(int id)
        {
            try
            {
                var dataId = _context.UserVoucherRedeemProducts.Where(entity => entity.Id == id).FirstOrDefault();
                EntityExtension.FlagForDelete(dataId, _userBy, _userAgent);
                _context.Update(dataId);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<UserVoucherRedeemProduct> Find()
        {
            return _context.UserVoucherRedeemProducts.ToList();
        }

        public Task<List<UserVoucherRedeemProduct>> FindAsync()
        {
            throw new NotImplementedException();
        }

        public List<UserVoucherRedeemProduct> FindById(int id)
        {
            return _context.UserVoucherRedeemProducts.Where(s => s.Id == id).ToList();
        }

        public Task<List<UserVoucherRedeemProduct>> FindByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public int Insert(UserVoucherRedeemProduct model)
        {
            EntityExtension.FlagForCreate(model, _userBy, _userAgent);
            _context.Add(model);
            return _context.SaveChanges();

        }

        public UserVoucherRedeemProduct InsertModel(UserVoucherRedeemProduct model)
        {
            EntityExtension.FlagForCreate(model, _userBy, _userAgent);
            var result = _context.Add(model);
            _context.SaveChanges();
            return result.Entity;
        }

        public void Update(UserVoucherRedeemProduct model)
        {
            EntityExtension.FlagForUpdate(model, _userBy, _userAgent);
            _context.Update(model);
            _context.SaveChanges();
        }
    }
}
