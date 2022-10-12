using com.bateeqshop.service.voucher.business.ViewModel;
using com.bateeqshop.service.voucher.data;
using com.bateeqshop.service.voucher.data.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.bateeqshop.service.voucher.business.Service
{
    public class VoucherService : IService<VoucherVM>
    {
        private readonly VoucherDbContext _context;
        public VoucherService(VoucherDbContext context)
        {
            _context = context;
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<VoucherVM> FindById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<VoucherVM>> FindByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public int Insert(VoucherVM model)
        {
            try
            {
                return 0;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public VoucherVM InsertModel(VoucherVM model)
        {
            throw new NotImplementedException();
        }

        public void Update(VoucherVM model)
        {
            throw new NotImplementedException();
        }

        List<VoucherVM> IService<VoucherVM>.Find()
        {
            var vouchers = _context.Vouchers.ToList();
            return VoucherVM.MapFrom(vouchers);
        }

        async Task<List<VoucherVM>> IService<VoucherVM>.FindAsync()
        {
            var vouchers = await _context.Vouchers.ToListAsync();
            return VoucherVM.MapFrom(vouchers);
        }
    }
}
