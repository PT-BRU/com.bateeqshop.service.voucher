using com.bateeqshop.service.voucher.data;
using Com.Moonlay.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace com.bateeqshop.service.voucher.business.Service.Helper.Crud
{
    public class CrudService<IEntity> : ICrudService<IEntity>
    {
        private readonly VoucherDbContext _context;
        private readonly string _userAgent = "BATEEQSHOP_VOUCHER";
        private readonly string _userBy = "BATEEQSHOP_VOUCHER";
        private static string ConnectionString =
            "Server=(localdb)\\MSSQLLocalDB; Database=TestDb; Integrated Security=True;";
        public CrudService(VoucherDbContext context)
        {
            _context = context;
        }

        public void Delete(int id)
        {
            var context = BuildDynamic();
            //DbSet<IEntity> dbset = _context.Set<IEntity>();
            //context.Find<IEntity>(new { Id = id });

        }

        public List<IEntity> Find()
        {
            throw new NotImplementedException();
        }

        public Task<List<IEntity>> FindAsync()
        {
            throw new NotImplementedException();
        }

        public List<IEntity> FindById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<IEntity>> FindByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public int Insert(IEntity model)
        {
            throw new NotImplementedException();
        }

        public void Update(IEntity model)
        {
            throw new NotImplementedException();
        }

        private DbContext BuildDynamic()
        {
            var dynDbContextType = DynamicContextBuilder.BuildDynamicDbContextType();

            var builder = new DbContextOptionsBuilder();

            builder.UseSqlServer(
                ConnectionString);

            DbContext dynDbContext = (DbContext)Activator.CreateInstance(dynDbContextType, builder.Options);

            //dynDbContext.Add<Pet>(new Pet() { Name = "Daisy" });

            //dynDbContext.Add<Person>(new Person() { FirstName = "Dave" });

            //dynDbContext.SaveChanges();
            return dynDbContext;
        }

        public IEntity InsertModel(IEntity model)
        {
            throw new NotImplementedException();
        }
    }
}
