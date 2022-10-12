using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace com.bateeqshop.service.voucher.api.Configuration
{
    public class ServicesConfig
    {
        public string ProductIds { get; set; }
        public string CategoryIds { get; set; }
        public string PostAddGiftToCart { get; set; }
        public string ProductServiceURI { get; set; }
        public string AuthToken { get; set; }
        public string GetUserDetail { get; set; }
        public string UpdatePointUser { get; set; }
    }
}
