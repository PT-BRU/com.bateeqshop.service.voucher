using com.bateeqshop.service.voucher.business.Service.Helper.CallingServices;
using com.bateeqshop.service.voucher.business.ViewModel.Products;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace com.bateeqshop.service.voucher.business.Service.Products
{
    public class ProductService : CallingServiceState<ProductResponses, ProductRequest, ProductHeader>, IProductService
    {
        //private readonly string _defaultAPI = "http://bateeq-service-product-dev.azurewebsites.net/";
        //public string Host { get; set; }
        //public string Version { get; set; }
        //public string Path { get; set; }
        public string FullAddress { get; set; }
        public override ProductResponses Get(ProductRequest request, ProductHeader header)
        {
            try
            {
                //var schemaAuth = "Bearer ";
                var client = new HttpClient();
                //set header default
                foreach(var head in header.DefaultHeader)
                {
                    client.DefaultRequestHeaders.Add(head.Key, head.Value);
                }
                var queryString = string.Join('&', request.QueryString.Select(s => s.Key + '=' + s.Value));
                var fullLink = FullAddress;
                if (!string.IsNullOrEmpty(queryString))
                    fullLink = fullLink + "?" + queryString;

                // SEND
                var result = Task.Run(() => client.GetAsync(fullLink)).Result;

                //RESPONSE
                var resultGetString = result.Content.ReadAsStringAsync().Result; 
                var resultObject = JsonConvert.DeserializeObject<List<Product>>(resultGetString);
                var resultObjectCategory = JsonConvert.DeserializeObject<List<Category>>(resultGetString);


                var returnResponse = new ProductResponses
                {
                    GetById = resultObject,
                    CategoryByIds = resultObjectCategory
                };
                return returnResponse;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<Product> GetProductByListId(string apiLink, List<int> productIds)
        {
            try
            {
                FullAddress = apiLink;

                //header Default
                var defaultHeader = new ProductHeader
                {
                    DefaultHeader = new Dictionary<string, string>
                    {
                        {"Accept","application/json" }
                        //{"Content-Type","application/json" }
                    }
                };
                // build query string
                var querystring = new Dictionary<string, string>();
                //foreach (var id in productIds)
                //{
                //    querystring.Add("productIds", id.ToString());
                //}
                var resultGet = Get(
                    new ProductRequest { QueryString = querystring },
                    defaultHeader
                    );
                return resultGet.GetById;

            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public List<Category> GetCategoryByListId(string apiLink, List<int> categoryids)
        {
            try
            {
                FullAddress = apiLink;

                //header Default
                var defaultHeader = new ProductHeader
                {
                    DefaultHeader = new Dictionary<string, string>
                    {
                        {"Accept","application/json" }
                        //{"Content-Type","application/json" }
                    }
                };
                // build query string
                var querystring = new Dictionary<string, string>();
                //foreach (var id in productIds)
                //{
                //    querystring.Add("productIds", id.ToString());
                //}
                var resultGet = Get(
                    new ProductRequest { QueryString = querystring },
                    defaultHeader
                    );
                return resultGet.CategoryByIds;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
