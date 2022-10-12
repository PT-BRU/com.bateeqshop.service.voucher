using com.bateeqshop.service.voucher.data.Config;
using com.bateeqshop.service.voucher.data.Model;
using Com.Moonlay.Data.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Cryptography.X509Certificates;

namespace com.bateeqshop.service.voucher.data
{
    public class VoucherDbContext : StandardDbContext
    {
        public VoucherDbContext(DbContextOptions<VoucherDbContext> options) : base(options)
        {
            
        }

        public DbSet<ExchangedPointHistory> ExchangedPointHistories { get; set; }
        public DbSet<UserVoucher> UserVouchers { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<VoucherNominal> VoucherNominals { get; set; }
        public DbSet<VoucherPercentage> VoucherPercentages { get; set; }
        public DbSet<VoucherProduct> VoucherProducts { get; set; }
        public DbSet<VoucherType1> VoucherType1s { get; set; }
        public DbSet<VoucherType2Category> VoucherType2Categories { get; set; }
        public DbSet<VoucherType2Product> VoucherType2Products { get; set; }
        public DbSet<VoucherType3> VoucherType3s { get; set; }
        public DbSet<VoucherType4> VoucherType4s { get; set; }
        public DbSet<RewardPoints> RewardPoints { get; set; }
        public DbSet<GeneralSetting> GeneralSettings { get; set; }
        public DbSet<UserVoucherRedeemProduct> UserVoucherRedeemProducts { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new VoucherNominalConfig());
            modelBuilder.ApplyConfiguration(new VoucherPercentageConfig());
            modelBuilder.ApplyConfiguration(new VoucherProductConfig());
            modelBuilder.ApplyConfiguration(new VoucherType1Config());
            modelBuilder.ApplyConfiguration(new VoucherType2CategoryConfig());
            modelBuilder.ApplyConfiguration(new VoucherType2ProductConfig());
            modelBuilder.ApplyConfiguration(new VoucherType3Config());
            modelBuilder.ApplyConfiguration(new VoucherType4Config());


            modelBuilder.Entity<Voucher>().HasQueryFilter(entity => entity.IsDeleted == false);
            modelBuilder.Entity<VoucherType1>().HasQueryFilter(entity => entity.IsDeleted == false);
            modelBuilder.Entity<VoucherType2Category>().HasQueryFilter(entity => entity.IsDeleted == false);
            modelBuilder.Entity<VoucherType2Product>().HasQueryFilter(entity => entity.IsDeleted == false);
            modelBuilder.Entity<VoucherNominal>().HasQueryFilter(entity => entity.IsDeleted == false);
            modelBuilder.Entity<VoucherPercentage>().HasQueryFilter(entity => entity.IsDeleted == false);
            modelBuilder.Entity<VoucherProduct>().HasQueryFilter(entity => entity.IsDeleted == false);
            modelBuilder.Entity<VoucherType3>().HasQueryFilter(entity => entity.IsDeleted == false);
            modelBuilder.Entity<VoucherType4>().HasQueryFilter(entity => entity.IsDeleted == false);
            modelBuilder.Entity<UserVoucherRedeemProduct>().HasQueryFilter(entity => !entity.IsDeleted);
            modelBuilder.Entity<UserVoucher>().HasQueryFilter(entity => entity.IsDeleted == false);
        }

        public DbSet<T> GetDbSet<T>() where T :class
        {
            return this.Set<T>();
        }
    }
}
