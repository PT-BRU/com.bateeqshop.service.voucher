using com.bateeqshop.service.voucher.data;
using Com.Moonlay.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace com.bateeqshop.service.voucher.business.Service.BaseRepositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : StandardEntity
    {
        private VoucherDbContext _context;
        private DbSet<TEntity> _dbset;
        private string _userAgent = "BATEEQSHOP-VOUCHER";
        private string _userBy = "BATEEQSHOP-VOUCHER";

        public BaseRepository(VoucherDbContext context, DbSet<TEntity> dbset)
        {
            _context = context;
            _dbset = _context.Set<TEntity>();
        }

        public virtual int Delete(int id)
        {
            try
            {
                var modelToDelete = _dbset.Find(id);
                if (modelToDelete == null)
                    return 0;
                FlagForDelete(modelToDelete);
                _context.Update(modelToDelete);
                return _context.SaveChanges();
            }catch(Exception ex)
            {
                throw ex;
            }
        }

        public virtual IEnumerable<TEntity> Get(
       Expression<Func<TEntity, bool>> filter = null,
       Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
       string includeProperties = "")
        {
            IQueryable<TEntity> query = _dbset;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }


            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public TEntity FindById(int id)
        {
            return _dbset.Find(id);
        }

        public int Insert(TEntity model)
        {
            try
            {
                FlagForUpdate(model);
                _context.Add(model);
                return _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Update(TEntity model)
        {
            try
            {
                var modelToUpdate = _dbset.Find(model.Id);
                if (modelToUpdate == null)
                    return 0;
                FlagForUpdate(modelToUpdate);
                _context.Update(modelToUpdate);
                return _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void FlagForDelete(TEntity model, bool isDeleted =true)
        {
            model.DeletedAgent = _userAgent;
            model.DeletedBy = _userBy;
            model.DeletedUtc = DateTime.UtcNow;
            model.IsDeleted = isDeleted;
        }

        public void FlagForCreate(TEntity model)
        {
            model.CreatedAgent = _userAgent;
            model.CreatedBy = _userBy;
            model.CreatedUtc = DateTime.UtcNow;
        }
        public void FlagForUpdate(TEntity model)
        {
            model.LastModifiedAgent = _userAgent;
            model.LastModifiedBy = _userBy;
            model.LastModifiedUtc = DateTime.UtcNow;
        }
    }
}
