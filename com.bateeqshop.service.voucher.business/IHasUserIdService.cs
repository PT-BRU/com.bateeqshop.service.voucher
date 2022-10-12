using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace com.bateeqshop.service.voucher.business
{
    public interface IHasUserIdService<TModel>
    {
        List<TModel> FindByUserId(int userId);
        Task<List<TModel>> FindByUserIdAsync(int userId);
    }
}
