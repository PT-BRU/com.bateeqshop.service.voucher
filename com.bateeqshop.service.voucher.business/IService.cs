using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.bateeqshop.service.voucher.business
{
    public interface IService<TModel>
    {
        List<TModel> Find();
        Task<List<TModel>> FindAsync();
        int Insert(TModel model);
        Task<List<TModel>> FindByIdAsync(int id);
        List<TModel> FindById(int id);
        void Update(TModel model);
        void Delete(int id);
        TModel InsertModel(TModel model);
    }
}
