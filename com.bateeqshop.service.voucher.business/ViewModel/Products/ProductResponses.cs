using System;
using System.Collections.Generic;
using System.Text;

namespace com.bateeqshop.service.voucher.business.ViewModel.Products
{
    public class ProductResponses
    {
        public List<Product> GetById { get; set; }
        public List<Category> CategoryByIds { get; set; }
    }
}
