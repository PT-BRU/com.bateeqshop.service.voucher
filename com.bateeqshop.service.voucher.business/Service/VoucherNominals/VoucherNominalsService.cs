using com.bateeqshop.service.voucher.data;
using com.bateeqshop.service.voucher.data.Model;
using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.bateeqshop.service.voucher.business.Service.VoucherNominals
{
    public class VoucherNominalsService : IVoucherNominalsService
    {
        private readonly VoucherDbContext _context;
        private string _userAgent = "BATEEQSHOP-VOUCHER";
        private string _userBy = "BATEEQSHOP-VOUCHER";

        public VoucherNominalsService(VoucherDbContext context)
        {
            _context = context;
        }

        public void Delete(int id)
        {
            try
            {
                var dataId = _context.VoucherNominals.Where(entity => entity.Id == id).FirstOrDefault();
                EntityExtension.FlagForDelete(dataId, _userBy, _userAgent);
                _context.Update(dataId);
                _context.SaveChanges();
                
            }catch(Exception ex)
            {
                throw ex;
            }
        }

        public List<VoucherNominal> Find()
        {
            return _context.VoucherNominals.ToList();
        }

        public Task<List<VoucherNominal>> FindAsync()
        {
            throw new NotImplementedException();
        }

        public List<VoucherNominal> FindById(int id)
        {
            return _context.VoucherNominals.Where(s => s.Id== id).ToList();
        }

        public Task<List<VoucherNominal>> FindByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public int Insert(VoucherNominal model)
        {
            EntityExtension.FlagForCreate(model, _userBy, _userAgent);
            _context.Add(model);
            return _context.SaveChanges();

        }

        public VoucherNominal InsertModel(VoucherNominal model)
        {
            EntityExtension.FlagForCreate(model, _userBy, _userAgent);
            var result = _context.Add(model);
            _context.SaveChanges();
            return result.Entity;
        }

        public void Update(VoucherNominal model)
        {
            EntityExtension.FlagForUpdate(model, _userBy, _userAgent);
            _context.VoucherNominals.Update(model);
            _context.SaveChanges();
        }
    }
}
