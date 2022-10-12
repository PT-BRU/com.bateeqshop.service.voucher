using com.bateeqshop.service.voucher.data.Model.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace com.bateeqshop.service.voucher.data.Config.Base
{
    public class BaseConfig<TModel> : IEntityTypeConfiguration<TModel> where TModel : BaseVoucher
    {
        public void Configure(EntityTypeBuilder<TModel> builder)
        {
            builder.Property(p => p.Name).HasMaxLength(255);
        }
    }
}
