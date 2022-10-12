using System;
using System.Collections.Generic;
using System.Text;

namespace com.bateeqshop.service.voucher.business.ViewModel.Users
{
    public class ResponseUserMe
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public int UserIds { get; set; }
        public int StatusCode { get; set; }
        public string Token { get; set; }
    }
}
