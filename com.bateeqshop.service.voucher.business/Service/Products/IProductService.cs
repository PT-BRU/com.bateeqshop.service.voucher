using com.bateeqshop.service.voucher.business.Service.Helper.CallingServices;
using com.bateeqshop.service.voucher.business.ViewModel.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace com.bateeqshop.service.voucher.business.Service.Products
{
    public interface IProductService
    { 
        List<Product> GetProductByListId(string apiLink, List<int> productIds);
        List<Category> GetCategoryByListId(string apiLink, List<int> categoryids);
    }
}
