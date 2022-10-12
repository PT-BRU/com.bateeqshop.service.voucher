using com.bateeqshop.service.voucher.data;
using com.bateeqshop.service.voucher.data.Model;
using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.bateeqshop.service.voucher.business.Service.VoucherProducts
{
    public class VoucherProductService : IVoucherProductService
    {
        private readonly VoucherDbContext _context;
        private string _userAgent = "BATEEQSHOP-VOUCHER";
        private string _userBy = "BATEEQSHOP-VOUCHER";

        public VoucherProductService(VoucherDbContext context)
        {
            _context = context;
        }

        public void Delete(int id)
        {
            try
            {
                var dataId = _context.VoucherProducts.Where(entity => entity.Id == id).FirstOrDefault();
                EntityExtension.FlagForDelete(dataId, _userBy, _userAgent);
                _context.Update(dataId);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<VoucherProduct> Find()
        {
            return _context.VoucherProducts.ToList();
        }

        public Task<List<VoucherProduct>> FindAsync()
        {
            throw new NotImplementedException();
        }

        public List<VoucherProduct> FindById(int id)
        {
            return _context.VoucherProducts.Where(s => s.Id == id).ToList();
        }

        public Task<List<VoucherProduct>> FindByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public int Insert(VoucherProduct model)
        {
            EntityExtension.FlagForCreate(model, _userBy, _userAgent);
            _context.Add(model);
            return _context.SaveChanges();

        }

        public VoucherProduct InsertModel(VoucherProduct model)
        {
            EntityExtension.FlagForCreate(model, _userBy, _userAgent);
            var result = _context.Add(model);
            _context.SaveChanges();
            return result.Entity;
        }

        public void Update(VoucherProduct model)
        {
            EntityExtension.FlagForUpdate(model, _userBy, _userAgent);
            _context.VoucherProducts.Update(model);
            _context.SaveChanges();
        }
    }
}
