using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace com.bateeqshop.service.voucher.business.ViewModel.Products
{
    public class ProductReview 
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
        public float Rate { get; set; }
        public string Description { get; set; }
        public bool IsPublished { get; set; }

        public int? ProductId { get; set; }

        public virtual Product Product { get; set; }

        public int UserId { get; set; }
    }
}
