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
    public class RewardPointService : IService<RewardPoints>
    {
        private readonly VoucherDbContext _context;

        public RewardPointService(
            VoucherDbContext context
            )
        {
            _context = context;
        }

        public void Delete(int id)
        {
            var rewards = _context.RewardPoints.SingleOrDefault(x => x.Id == id);

            if (rewards == null)
            {
                throw new ArgumentNullException("Not Found");
            }

            _context.Remove(rewards);

            _context.SaveChanges();
        }

        public List<RewardPoints> Find()
        {
            var rewards = _context.RewardPoints.ToList();

            return rewards;
        }

        public Task<List<RewardPoints>> FindAsync()
        {
            var rewards = _context.RewardPoints.ToListAsync();

            return rewards;
        }

        public List<RewardPoints> FindById(int id)
        {
            var rewards = _context.RewardPoints.Where(x => x.Id == id).ToList();

            return rewards;
        }

        public Task<List<RewardPoints>> FindByIdAsync(int id)
        {
            var rewards = _context.RewardPoints.Where(x => x.Id == id).ToListAsync();

            return rewards;
        }

        public int Insert(RewardPoints model)
        {
            _context.RewardPoints.Add(model);
            return _context.SaveChanges();
        }

        public RewardPoints InsertModel(RewardPoints model)
        {
            throw new NotImplementedException();
        }

        public void Update(RewardPoints model)
        {
            if(model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            _context.RewardPoints.Update(model);
            _context.SaveChanges();
        }
    }
}
