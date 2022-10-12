using com.bateeqshop.service.voucher.business.Enum;
using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace com.bateeqshop.service.voucher.business.ViewModel.Products
{
    public class Product 
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
        public int FreeQuantity { get; set; }
        public string RONumber { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public decimal NormalPrice { get; set; }
        public decimal DiscountPrice { get; set; }
        public bool IsPublished { get; set; }
        public bool IsFeatured { get; set; }
        public SizeGuide Size_Guide { get; set; }
        public int? MotifId { get; set; }
        public bool IsPreOrder { get; set; }
        public virtual Motif Motif { get; set; }
        public virtual ICollection<ProductDetail> ProductDetails { get; set; }
        public virtual ICollection<ProductReview> ProductReviews { get; set; }
        public virtual ICollection<ProductCategory> ProductCategories { get; set; }
        public virtual ICollection<ProductImage> ProductImages { get; set; }
        public virtual ICollection<ProductTag> ProductTags { get; set; }
        public virtual ICollection<ProductLogo> ProductLogos { get; set; }

    }
}
