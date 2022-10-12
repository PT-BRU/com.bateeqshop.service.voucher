using com.bateeqshop.service.voucher.data;
using com.bateeqshop.service.voucher.data.Model;
using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.bateeqshop.service.voucher.business.Service.VoucherType3s
{
    public class VoucherType3sService : IVoucherType3sService
    {
        private readonly VoucherDbContext _context;
        private string _userAgent = "BATEEQSHOP-VOUCHER";
        private string _userBy = "BATEEQSHOP-VOUCHER";

        public VoucherType3sService(VoucherDbContext context)
        {
            _context = context;
        }

        public void Delete(int id)
        {
            try
            {
                var dataId = _context.VoucherType3s.Where(entity => entity.Id == id).FirstOrDefault();
                EntityExtension.FlagForDelete(dataId, _userBy, _userAgent);
                _context.Update(dataId);
                _context.SaveChanges();
                
            }catch(Exception ex)
            {
                throw ex;
            }
        }

        public List<VoucherType3> Find()
        {
            return _context.VoucherType3s.ToList();
        }

        public Task<List<VoucherType3>> FindAsync()
        {
            throw new NotImplementedException();
        }

        public List<VoucherType3> FindById(int id)
        {
            return _context.VoucherType3s.Where(s => s.Id== id).ToList();
        }

        public Task<List<VoucherType3>> FindByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public int Insert(VoucherType3 model)
        {
            EntityExtension.FlagForCreate(model, _userBy, _userAgent);
            _context.Add(model);
            return _context.SaveChanges();

        }

        public VoucherType3 InsertModel(VoucherType3 model)
        {
            EntityExtension.FlagForCreate(model, _userBy, _userAgent);
            var result = _context.Add(model);
            _context.SaveChanges();
            return result.Entity;
        }

        public void Update(VoucherType3 model)
        {
            EntityExtension.FlagForUpdate(model, _userBy, _userAgent);
            _context.Update(model);
            _context.SaveChanges();
        }
    }
}
