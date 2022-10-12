using com.bateeqshop.service.voucher.business.ViewModel.Products;
using com.bateeqshop.service.voucher.data.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace com.bateeqshop.service.voucher.business.Service.ProductCarts
{
    public interface IProductCartService
    {
        Task<CartProduct> PostProductGiftToCart(string uri, CartProduct request, string token);
        Task<CartProduct> DeleteProductGiftFromCart(int id, string uri, string token);
        Task<CartProduct> DeleteProductGiftFromCart(UserVoucherRedeemProduct redeemProduct, string uri);
    }
}
