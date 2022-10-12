using System;
using System.Collections.Generic;
using System.Text;

namespace com.bateeqshop.service.voucher.business.ViewModel.Products
{
    public class CartProduct
    {
        public int Id { get; set; }
        public int Qty { get; set; }
        public int CartId { get; set; }

        //[ForeignKey("CartId")]
        public Cart Cart { get; set; }
        public int? ProductDetailId { get; set; }
        public virtual ProductDetail ProductDetail { get; set; }
        public bool isCheckout { get; set; }
        public bool IsProductGift { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedUtc { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedAgent { get; set; }
        public DateTime LastModifiedUtc { get; set; }
        public string LastModifiedBy { get; set; }
        public string LastModifiedAgent { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DeletedUtc { get; set; }
        public string DeletedBy { get; set; }
        public string DeletedAgent { get; set; }
    }
}
