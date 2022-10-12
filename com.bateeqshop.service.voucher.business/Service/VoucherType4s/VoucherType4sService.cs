using com.bateeqshop.service.voucher.data;
using com.bateeqshop.service.voucher.data.Model;
using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.bateeqshop.service.voucher.business.Service.VoucherType4s
{
    public class VoucherType4sService : IVoucherType4sService
    {
        private readonly VoucherDbContext _context;
        private string _userAgent = "BATEEQSHOP-VOUCHER";
        private string _userBy = "BATEEQSHOP-VOUCHER";

        public VoucherType4sService(VoucherDbContext context)
        {
            _context = context;
        }

        public void Delete(int id)
        {
            try
            {
                var dataId = _context.VoucherType4s.Where(entity => entity.Id == id).FirstOrDefault();
                EntityExtension.FlagForDelete(dataId, _userBy, _userAgent);
                _context.Update(dataId);
                _context.SaveChanges();
                
            }catch(Exception ex)
            {
                throw ex;
            }
        }

        public List<VoucherType4> Find()
        {
            return _context.VoucherType4s.ToList();
        }

        public Task<List<VoucherType4>> FindAsync()
        {
            throw new NotImplementedException();
        }

        public List<VoucherType4> FindById(int id)
        {
            return _context.VoucherType4s.Where(s => s.Id== id).ToList();
        }

        public Task<List<VoucherType4>> FindByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public int Insert(VoucherType4 model)
        {
            EntityExtension.FlagForCreate(model, _userBy, _userAgent);
            _context.Add(model);
            return _context.SaveChanges();

        }

        public VoucherType4 InsertModel(VoucherType4 model)
        {
            EntityExtension.FlagForCreate(model, _userBy, _userAgent);
            var result = _context.Add(model);
            _context.SaveChanges();
            return result.Entity;
        }

        public void Update(VoucherType4 model)
        {
            EntityExtension.FlagForUpdate(model, _userBy, _userAgent);
            _context.Update(model);
            _context.SaveChanges();
        }
    }
}
