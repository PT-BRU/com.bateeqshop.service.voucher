using com.bateeqshop.service.voucher.data;
using com.bateeqshop.service.voucher.data.Model;
using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.bateeqshop.service.voucher.business.Service.VoucherType2Categorys
{
    public class VoucherType2CategorysService : IVoucherType2CategorysService
    {
        private readonly VoucherDbContext _context;
        private string _userAgent = "BATEEQSHOP-VOUCHER";
        private string _userBy = "BATEEQSHOP-VOUCHER";

        public VoucherType2CategorysService(VoucherDbContext context)
        {
            _context = context;
        }

        public void Delete(int id)
        {
            try
            {
                var dataId = _context.VoucherType2Categories.Where(entity => entity.Id == id).FirstOrDefault();
                EntityExtension.FlagForDelete(dataId, _userBy, _userAgent);
                _context.Update(dataId);
                _context.SaveChanges();
                
            }catch(Exception ex)
            {
                throw ex;
            }
        }

        public List<VoucherType2Category> Find()
        {
            return _context.VoucherType2Categories.ToList();
        }

        public Task<List<VoucherType2Category>> FindAsync()
        {
            throw new NotImplementedException();
        }

        public List<VoucherType2Category> FindById(int id)
        {
            return _context.VoucherType2Categories.Where(s => s.Id== id).ToList();
        }

        public Task<List<VoucherType2Category>> FindByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public int Insert(VoucherType2Category model)
        {
            EntityExtension.FlagForCreate(model, _userBy, _userAgent);
            _context.Add(model);
            return _context.SaveChanges();

        }

        public VoucherType2Category InsertModel(VoucherType2Category model)
        {
            EntityExtension.FlagForCreate(model, _userBy, _userAgent);
            var result = _context.Add(model);
            _context.SaveChanges();
            return result.Entity;
        }

        public void Update(VoucherType2Category model)
        {
            EntityExtension.FlagForUpdate(model, _userBy, _userAgent);
            _context.Update(model);
            _context.SaveChanges();
        }
    }
}
