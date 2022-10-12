using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace com.bateeqshop.service.voucher.business.ViewModel.Products
{
    public class Logo 
    {
        public int Id { get; set; }
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
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<ProductLogo> ProductLogos { get; set; }
    }
}
