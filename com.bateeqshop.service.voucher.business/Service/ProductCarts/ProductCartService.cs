using com.bateeqshop.service.voucher.business.ViewModel.Products;
using com.bateeqshop.service.voucher.data.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace com.bateeqshop.service.voucher.business.Service.ProductCarts
{
    public class ProductCartService : IProductCartService
    {
        public async Task<CartProduct> PostProductGiftToCart(string uri, CartProduct request,string token)
        {

            try
            {
                uri = $"{uri}product-cart/add-gift-to-cart";
                var client = new HttpClient();

                var body = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", token);

                var result = await client.PostAsync(uri, body);
                var statusCode = (int)result.StatusCode;
                string jsonContent = result.Content.ReadAsStringAsync().Result;
                CartProduct response = JsonConvert.DeserializeObject<CartProduct>(jsonContent);

                return response;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<CartProduct> DeleteProductGiftFromCart(int id, string uri, string token)
        {

            try
            {
                uri = $"{uri}product-cart/remove-by-voucher/{id}";
                var client = new HttpClient();

                //var body = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", token);

                var result = await client.DeleteAsync(uri);
                var statusCode = (int)result.StatusCode;
                string jsonContent = result.Content.ReadAsStringAsync().Result;
                CartProduct response = JsonConvert.DeserializeObject<CartProduct>(jsonContent);

                return response;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<CartProduct> DeleteProductGiftFromCart(UserVoucherRedeemProduct redeemProduct, string uri)
        {

            try
            {
                uri = $"{uri}/product-cart/delete-cartproduct-by-cart/{redeemProduct.CartProductId}?productId={redeemProduct.ProductDetailId}";
                var client = new HttpClient();

                //var body = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //client.DefaultRequestHeaders.Add("Authorization", token);

                var result = await client.DeleteAsync(uri);
                var statusCode = (int)result.StatusCode;
                string jsonContent = result.Content.ReadAsStringAsync().Result;
                CartProduct response = JsonConvert.DeserializeObject<CartProduct>(jsonContent);

                return response;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
