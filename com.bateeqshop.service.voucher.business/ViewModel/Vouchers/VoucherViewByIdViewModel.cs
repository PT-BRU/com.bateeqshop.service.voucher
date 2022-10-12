using com.bateeqshop.service.voucher.business.ViewModel.Products;
using com.bateeqshop.service.voucher.data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace com.bateeqshop.service.voucher.business.ViewModel.Vouchers
{
    public class VoucherViewByIdViewModel : IValidatableObject
    {
        public int Id { get; set; }
        public bool AppliesMultiply { get; set; }
        public string AssignToCategory { get; set; }
        public Category CategoryPurchase { get; set; }
        public List<Category> CategoryPurchaseMultiple { get; set; }
        public string DiscountCode { get; set; }
        public string DiscountName { get; set; }
        public string DiscountPercentage { get; set; }
        public string EndDate { get; set; }
        public string MaxDiscount { get; set; }
        public string MaxUsagePerUser { get; set; }
        public string MinimumPayment { get; set; }
        public double Nominal { get; set; }
        public string Percentage { get; set; }
        public Product ProductGift { get; set; }
        public List<Product> ProductGiftMultiple { get; set; }
        public Product ProductPurchase { get; set; }
        public List<Product> ProductPurchaseMultiple { get; set; }
        public string QtyItemGift { get; set; }
        public string QtyItemPurchase { get; set; }
        public string QuantityVoucher { get; set; }
        public string StartDate { get; set; }
        public string VoucherType { get; set; }
        public int VoucherTypeEnum { get; set; }
        public string Description { get; set; }
        public string AssignToMembershipIds { get; set; }
        public decimal ExchangePoint { get; set; }


        static VoucherViewByIdViewModel()
        {
            //this.Id = Id;
            //this.AppliesMultiply = AppliesMultiply;
            //this.AssignToCategory = AssignToCategory;
            //this.CategoryPurchase = CategoryPurchase;
            //this.DiscountCode = DiscountCode;
            //this.DiscountName = DiscountName;
            //this.DiscountPercentage = DiscountPercentage;
            //this.EndDate = EndDate;
            //this.MaxDiscount = MaxDiscount;
            //this.MaxUsagePerUser = MaxUsagePerUser;
            //this.MinimumPayment = MinimumPayment;
            //this.Nominal = Nominal;
            //this.Percentage = Percentage;
            //this.ProductGift = ProductGift;
            //this.ProductPurchase = ProductPurchase;
            //this.QtyItemGift = QtyItemGift;
            //this.QtyItemPurchase = QtyItemPurchase;
            //this.QuantityVoucher = QuantityVoucher;
            //this.StartDate = StartDate;
            //this.VoucherType = VoucherType;
            //this.VoucherTypeEnum = VoucherTypeEnum;
            //this.Description = Description;
            //this.AssignToMembershipIds = AssignToMembershipIds;
            //this.ExchangePoint = ExchangePoint;
        }

        public VoucherViewByIdViewModel(VoucherInsertPlainVM plainVoucher)
        {
            Id = plainVoucher.Id;
            AppliesMultiply = plainVoucher.AppliesMultiply;
            AssignToCategory = plainVoucher.AssignToCategory;
            //CategoryPurchase = plainVoucher.CategoryPurchase;
            DiscountCode = plainVoucher.DiscountCode;
            DiscountName = plainVoucher.DiscountName;
            DiscountPercentage = plainVoucher.DiscountPercentage;
            EndDate = plainVoucher.EndDate;
            MaxDiscount = plainVoucher.MaxDiscount;
            MaxUsagePerUser = plainVoucher.MaxUsagePerUser;
            MinimumPayment = plainVoucher.MinimumPayment;
            Nominal = plainVoucher.Nominal;
            Percentage = plainVoucher.Percentage;
            //ProductGift = plainVoucher.ProductGift;
            //ProductPurchase = plainVoucher.ProductPurchase;
            QtyItemGift = plainVoucher.QtyItemGift;
            QtyItemPurchase = plainVoucher.QtyItemPurchase;
            QuantityVoucher = plainVoucher.QuantityVoucher;
            StartDate = plainVoucher.StartDate;
            VoucherType = plainVoucher.VoucherType;
            VoucherTypeEnum = plainVoucher.VoucherTypeEnum;
            Description = plainVoucher.Description;
            AssignToMembershipIds = plainVoucher.AssignToMembershipIds;
            ExchangePoint = plainVoucher.ExchangePoint;
        }

        public VoucherViewByIdViewModel(VoucherInsertPlainVM plainVoucher,Product productFreeInfo,Product productPurchaseInfo,Category categoryInfo)
        {
            Id = plainVoucher.Id;
            AppliesMultiply = plainVoucher.AppliesMultiply;
            AssignToCategory = plainVoucher.AssignToCategory;
            CategoryPurchase = categoryInfo;
            DiscountCode = plainVoucher.DiscountCode;
            DiscountName = plainVoucher.DiscountName;
            DiscountPercentage = plainVoucher.DiscountPercentage;
            EndDate = plainVoucher.EndDate;
            MaxDiscount = plainVoucher.MaxDiscount;
            MaxUsagePerUser = plainVoucher.MaxUsagePerUser;
            MinimumPayment = plainVoucher.MinimumPayment;
            Nominal = plainVoucher.Nominal;
            Percentage = plainVoucher.Percentage;
            ProductGift = productFreeInfo;
            ProductPurchase = productPurchaseInfo;
            QtyItemGift = plainVoucher.QtyItemGift;
            QtyItemPurchase = plainVoucher.QtyItemPurchase;
            QuantityVoucher = plainVoucher.QuantityVoucher;
            StartDate = plainVoucher.StartDate;
            VoucherType = plainVoucher.VoucherType;
            VoucherTypeEnum = plainVoucher.VoucherTypeEnum;
            Description = plainVoucher.Description;
            AssignToMembershipIds = plainVoucher.AssignToMembershipIds;
            ExchangePoint = plainVoucher.ExchangePoint;
        }

        public VoucherViewByIdViewModel(VoucherInsertPlainVM plainVoucher, List<Product> productFreeInfo, List<Product> productPurchaseInfo, List<Category> categoryInfo)
        {
            Id = plainVoucher.Id;
            AppliesMultiply = plainVoucher.AppliesMultiply;
            AssignToCategory = plainVoucher.AssignToCategory;
            CategoryPurchase = categoryInfo.FirstOrDefault();
            CategoryPurchaseMultiple = categoryInfo;
            DiscountCode = plainVoucher.DiscountCode;
            DiscountName = plainVoucher.DiscountName;
            DiscountPercentage = plainVoucher.DiscountPercentage;
            EndDate = plainVoucher.EndDate;
            MaxDiscount = plainVoucher.MaxDiscount;
            MaxUsagePerUser = plainVoucher.MaxUsagePerUser;
            MinimumPayment = plainVoucher.MinimumPayment;
            Nominal = plainVoucher.Nominal;
            Percentage = plainVoucher.Percentage;
            ProductGift = productFreeInfo.FirstOrDefault();
            ProductGiftMultiple = productFreeInfo;
            ProductPurchase = productPurchaseInfo.FirstOrDefault();
            ProductPurchaseMultiple = productPurchaseInfo;
            QtyItemGift = plainVoucher.QtyItemGift;
            QtyItemPurchase = plainVoucher.QtyItemPurchase;
            QuantityVoucher = plainVoucher.QuantityVoucher;
            StartDate = plainVoucher.StartDate;
            VoucherType = plainVoucher.VoucherType;
            VoucherTypeEnum = plainVoucher.VoucherTypeEnum;
            Description = plainVoucher.Description;
            AssignToMembershipIds = plainVoucher.AssignToMembershipIds;
            ExchangePoint = plainVoucher.ExchangePoint;
        }
        //static void SetProductPurchase (Product productInfo)
        //{
        //    ProductPurchase = productInfo;
        //}
        //static void SetProductGift (Product productInfo)
        //{
        //    ProductGift = productInfo;
        //}
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            Voucher.VoucherType typeVoucher = VoucherTypeExtensions.ToVoucherTypeEnum(VoucherType.ToLower());

            switch (typeVoucher)
            {
                case Voucher.VoucherType.Nominal:


                    if (Nominal == 0)
                    {
                        yield return new ValidationResult(
                            $"Nominal Must be Provide",
                            new[] { nameof(Nominal) });
                    }
                    if (string.IsNullOrEmpty(MinimumPayment) || string.IsNullOrWhiteSpace(MinimumPayment) || MinimumPayment == "0")
                    {
                        yield return new ValidationResult(
                            $"MinimumPayment Must be Provide",
                            new[] { nameof(MinimumPayment) });
                    }
                    if (string.IsNullOrEmpty(QuantityVoucher) || string.IsNullOrWhiteSpace(QuantityVoucher) || QuantityVoucher == "0")
                    {
                        yield return new ValidationResult(
                            $"QuantityVoucher Must be Provide",
                            new[] { nameof(QuantityVoucher) });
                    }
                    if (string.IsNullOrEmpty(MaxUsagePerUser) || string.IsNullOrWhiteSpace(MaxUsagePerUser) || MaxUsagePerUser == "0")
                    {
                        yield return new ValidationResult(
                            $"MaxUsagePerUser Must be Provide",
                            new[] { nameof(MaxUsagePerUser) });
                    }
                    if (string.IsNullOrEmpty(DiscountName) || string.IsNullOrWhiteSpace(DiscountName))
                    {
                        yield return new ValidationResult(
                            $"DiscountName Must be Provide",
                            new[] { nameof(DiscountName) });
                    }
                    if (string.IsNullOrEmpty(DiscountCode) || string.IsNullOrWhiteSpace(DiscountCode))
                    {
                        yield return new ValidationResult(
                            $"DiscountCode Must be Provide",
                            new[] { nameof(DiscountCode) });
                    }
                    if (string.IsNullOrEmpty(StartDate) || string.IsNullOrWhiteSpace(StartDate) || StartDate == "NaN/NaN/NaN")
                    {
                        yield return new ValidationResult(
                            $"StartDate Must be Provide",
                            new[] { nameof(StartDate) });
                    }
                    if (string.IsNullOrEmpty(EndDate) || string.IsNullOrWhiteSpace(EndDate) || EndDate == "NaN/NaN/NaN")
                    {
                        yield return new ValidationResult(
                            $"EndDate Must be Provide",
                            new[] { nameof(EndDate) });
                    }
                    if (string.IsNullOrEmpty(Description) || string.IsNullOrWhiteSpace(Description))
                    {
                        yield return new ValidationResult(
                            $"Description Must be Provide",
                            new[] { nameof(Description) });
                    }
                    break;
                case Voucher.VoucherType.Percentage:


                    if (Percentage == "0" || string.IsNullOrEmpty(Percentage) || string.IsNullOrWhiteSpace(Percentage))
                    {
                        yield return new ValidationResult(
                            $"Percentage Must be Provide",
                            new[] { nameof(Percentage) });
                    }
                    if (MaxDiscount == "0" || string.IsNullOrEmpty(MaxDiscount) || string.IsNullOrWhiteSpace(MaxDiscount))
                    {
                        yield return new ValidationResult(
                            $"MaxDiscount Must be Provide",
                            new[] { nameof(MaxDiscount) });
                    }
                    if (string.IsNullOrEmpty(MinimumPayment) || string.IsNullOrWhiteSpace(MinimumPayment))
                    {
                        yield return new ValidationResult(
                            $"MinimumPayment Must be Provide",
                            new[] { nameof(MinimumPayment) });
                    }
                    if (string.IsNullOrEmpty(DiscountName) || string.IsNullOrWhiteSpace(DiscountName))
                    {
                        yield return new ValidationResult(
                            $"DiscountName Must be Provide",
                            new[] { nameof(DiscountName) });
                    }
                    if (string.IsNullOrEmpty(DiscountCode) || string.IsNullOrWhiteSpace(DiscountCode))
                    {
                        yield return new ValidationResult(
                            $"DiscountCode Must be Provide",
                            new[] { nameof(DiscountCode) });
                    }
                    if (string.IsNullOrEmpty(QuantityVoucher) || string.IsNullOrWhiteSpace(QuantityVoucher) || QuantityVoucher == "0")
                    {
                        yield return new ValidationResult(
                            $"QuantityVoucher Must be Provide",
                            new[] { nameof(QuantityVoucher) });
                    }
                    if (string.IsNullOrEmpty(MaxUsagePerUser) || string.IsNullOrWhiteSpace(MaxUsagePerUser) || MaxUsagePerUser == "0")
                    {
                        yield return new ValidationResult(
                            $"MaxUsagePerUser Must be Provide",
                            new[] { nameof(MaxUsagePerUser) });
                    }
                    if (string.IsNullOrEmpty(StartDate) || string.IsNullOrWhiteSpace(StartDate) || StartDate == "NaN/NaN/NaN")
                    {
                        yield return new ValidationResult(
                            $"StartDate Must be Provide",
                            new[] { nameof(StartDate) });
                    }
                    if (string.IsNullOrEmpty(EndDate) || string.IsNullOrWhiteSpace(EndDate) || EndDate == "NaN/NaN/NaN")
                    {
                        yield return new ValidationResult(
                            $"EndDate Must be Provide",
                            new[] { nameof(EndDate) });
                    }
                    if (string.IsNullOrEmpty(Description) || string.IsNullOrWhiteSpace(Description))
                    {
                        yield return new ValidationResult(
                            $"Description Must be Provide",
                            new[] { nameof(Description) });
                    }
                    break;
                case Voucher.VoucherType.Product:
                    if (string.IsNullOrEmpty(QuantityVoucher) || string.IsNullOrWhiteSpace(QuantityVoucher) || QuantityVoucher == "0")
                    {
                        yield return new ValidationResult(
                            $"QuantityVoucher Must be Provide",
                            new[] { nameof(QuantityVoucher) });
                    }
                    if (string.IsNullOrEmpty(MaxUsagePerUser) || string.IsNullOrWhiteSpace(MaxUsagePerUser) || MaxUsagePerUser == "0")
                    {
                        yield return new ValidationResult(
                            $"MaxUsagePerUser Must be Provide",
                            new[] { nameof(MaxUsagePerUser) });
                    }
                    if (string.IsNullOrEmpty(DiscountName) || string.IsNullOrWhiteSpace(DiscountName))
                    {
                        yield return new ValidationResult(
                            $"DiscountName Must be Provide",
                            new[] { nameof(DiscountName) });
                    }
                    if (string.IsNullOrEmpty(DiscountCode) || string.IsNullOrWhiteSpace(DiscountCode))
                    {
                        yield return new ValidationResult(
                            $"DiscountCode Must be Provide",
                            new[] { nameof(DiscountCode) });
                    }
                    if (string.IsNullOrEmpty(StartDate) || string.IsNullOrWhiteSpace(StartDate) || StartDate == "NaN/NaN/NaN")
                    {
                        yield return new ValidationResult(
                            $"StartDate Must be Provide",
                            new[] { nameof(StartDate) });
                    }
                    if (string.IsNullOrEmpty(EndDate) || string.IsNullOrWhiteSpace(EndDate) || EndDate == "NaN/NaN/NaN")
                    {
                        yield return new ValidationResult(
                            $"EndDate Must be Provide",
                            new[] { nameof(EndDate) });
                    }
                    if (string.IsNullOrEmpty(Description) || string.IsNullOrWhiteSpace(Description) || Description == "NaN/NaN/NaN")
                    {
                        yield return new ValidationResult(
                            $"Description Must be Provide",
                            new[] { nameof(Description) });
                    }
                    break;
                case Voucher.VoucherType.Type1:
                    if (string.IsNullOrEmpty(QuantityVoucher) || string.IsNullOrWhiteSpace(QuantityVoucher) || QuantityVoucher == "0")
                    {
                        yield return new ValidationResult(
                            $"QuantityVoucher Must be Provide",
                            new[] { nameof(QuantityVoucher) });
                    }
                    if (string.IsNullOrEmpty(MaxUsagePerUser) || string.IsNullOrWhiteSpace(MaxUsagePerUser) || MaxUsagePerUser == "0")
                    {
                        yield return new ValidationResult(
                            $"MaxUsagePerUser Must be Provide",
                            new[] { nameof(MaxUsagePerUser) });
                    }
                    if (string.IsNullOrEmpty(DiscountName) || string.IsNullOrWhiteSpace(DiscountName))
                    {
                        yield return new ValidationResult(
                            $"DiscountName Must be Provide",
                            new[] { nameof(DiscountName) });
                    }
                    if (string.IsNullOrEmpty(DiscountCode) || string.IsNullOrWhiteSpace(DiscountCode))
                    {
                        yield return new ValidationResult(
                            $"DiscountCode Must be Provide",
                            new[] { nameof(DiscountCode) });
                    }
                    if (string.IsNullOrEmpty(StartDate) || string.IsNullOrWhiteSpace(StartDate) || StartDate == "NaN/NaN/NaN")
                    {
                        yield return new ValidationResult(
                            $"StartDate Must be Provide",
                            new[] { nameof(StartDate) });
                    }
                    if (string.IsNullOrEmpty(EndDate) || string.IsNullOrWhiteSpace(EndDate) || EndDate == "NaN/NaN/NaN")
                    {
                        yield return new ValidationResult(
                            $"EndDate Must be Provide",
                            new[] { nameof(EndDate) });
                    }
                    if (string.IsNullOrEmpty(Description) || string.IsNullOrWhiteSpace(Description) || Description == "NaN/NaN/NaN")
                    {
                        yield return new ValidationResult(
                            $"Description Must be Provide",
                            new[] { nameof(Description) });
                    }
                    break;
                case Voucher.VoucherType.Type2Category:
                    if (string.IsNullOrEmpty(QuantityVoucher) || string.IsNullOrWhiteSpace(QuantityVoucher) || QuantityVoucher == "0")
                    {
                        yield return new ValidationResult(
                            $"QuantityVoucher Must be Provide",
                            new[] { nameof(QuantityVoucher) });
                    }
                    if (string.IsNullOrEmpty(MaxUsagePerUser) || string.IsNullOrWhiteSpace(MaxUsagePerUser) || MaxUsagePerUser == "0")
                    {
                        yield return new ValidationResult(
                            $"MaxUsagePerUser Must be Provide",
                            new[] { nameof(MaxUsagePerUser) });
                    }
                    if (string.IsNullOrEmpty(DiscountName) || string.IsNullOrWhiteSpace(DiscountName))
                    {
                        yield return new ValidationResult(
                            $"DiscountName Must be Provide",
                            new[] { nameof(DiscountName) });
                    }
                    if (string.IsNullOrEmpty(DiscountCode) || string.IsNullOrWhiteSpace(DiscountCode))
                    {
                        yield return new ValidationResult(
                            $"DiscountCode Must be Provide",
                            new[] { nameof(DiscountCode) });
                    }
                    if (string.IsNullOrEmpty(StartDate) || string.IsNullOrWhiteSpace(StartDate) || StartDate == "NaN/NaN/NaN")
                    {
                        yield return new ValidationResult(
                            $"StartDate Must be Provide",
                            new[] { nameof(StartDate) });
                    }
                    if (string.IsNullOrEmpty(EndDate) || string.IsNullOrWhiteSpace(EndDate) || EndDate == "NaN/NaN/NaN")
                    {
                        yield return new ValidationResult(
                            $"EndDate Must be Provide",
                            new[] { nameof(EndDate) });
                    }
                    if (string.IsNullOrEmpty(Description) || string.IsNullOrWhiteSpace(Description) || Description == "NaN/NaN/NaN")
                    {
                        yield return new ValidationResult(
                            $"Description Must be Provide",
                            new[] { nameof(Description) });
                    }
                    break;
                case Voucher.VoucherType.Type2Product:
                    if (string.IsNullOrEmpty(QuantityVoucher) || string.IsNullOrWhiteSpace(QuantityVoucher) || QuantityVoucher == "0")
                    {
                        yield return new ValidationResult(
                            $"QuantityVoucher Must be Provide",
                            new[] { nameof(QuantityVoucher) });
                    }
                    if (string.IsNullOrEmpty(MaxUsagePerUser) || string.IsNullOrWhiteSpace(MaxUsagePerUser) || MaxUsagePerUser == "0")
                    {
                        yield return new ValidationResult(
                            $"MaxUsagePerUser Must be Provide",
                            new[] { nameof(MaxUsagePerUser) });
                    }
                    if (string.IsNullOrEmpty(DiscountName) || string.IsNullOrWhiteSpace(DiscountName))
                        if (string.IsNullOrEmpty(DiscountName) || string.IsNullOrWhiteSpace(DiscountName))
                        {
                            yield return new ValidationResult(
                                $"DiscountName Must be Provide",
                                new[] { nameof(DiscountName) });
                        }
                    if (string.IsNullOrEmpty(DiscountCode) || string.IsNullOrWhiteSpace(DiscountCode))
                    {
                        yield return new ValidationResult(
                            $"DiscountCode Must be Provide",
                            new[] { nameof(DiscountCode) });
                    }
                    if (string.IsNullOrEmpty(StartDate) || string.IsNullOrWhiteSpace(StartDate) || StartDate == "NaN/NaN/NaN")
                    {
                        yield return new ValidationResult(
                            $"StartDate Must be Provide",
                            new[] { nameof(StartDate) });
                    }
                    if (string.IsNullOrEmpty(EndDate) || string.IsNullOrWhiteSpace(EndDate) || EndDate == "NaN/NaN/NaN")
                    {
                        yield return new ValidationResult(
                            $"EndDate Must be Provide",
                            new[] { nameof(EndDate) });
                    }
                    if (string.IsNullOrEmpty(Description) || string.IsNullOrWhiteSpace(Description) || Description == "NaN/NaN/NaN")
                    {
                        yield return new ValidationResult(
                            $"Description Must be Provide",
                            new[] { nameof(Description) });
                    }
                    break;
                case Voucher.VoucherType.Type3:
                    if (string.IsNullOrEmpty(QuantityVoucher) || string.IsNullOrWhiteSpace(QuantityVoucher) || QuantityVoucher == "0")
                    {
                        yield return new ValidationResult(
                            $"QuantityVoucher Must be Provide",
                            new[] { nameof(QuantityVoucher) });
                    }
                    if (string.IsNullOrEmpty(MaxUsagePerUser) || string.IsNullOrWhiteSpace(MaxUsagePerUser) || MaxUsagePerUser == "0")
                    {
                        yield return new ValidationResult(
                            $"MaxUsagePerUser Must be Provide",
                            new[] { nameof(MaxUsagePerUser) });
                    }
                    if (string.IsNullOrEmpty(DiscountName) || string.IsNullOrWhiteSpace(DiscountName))
                    {
                        yield return new ValidationResult(
                            $"DiscountName Must be Provide",
                            new[] { nameof(DiscountName) });
                    }
                    if (string.IsNullOrEmpty(DiscountCode) || string.IsNullOrWhiteSpace(DiscountCode))
                    {
                        yield return new ValidationResult(
                            $"DiscountCode Must be Provide",
                            new[] { nameof(DiscountCode) });
                    }
                    if (string.IsNullOrEmpty(StartDate) || string.IsNullOrWhiteSpace(StartDate) || StartDate == "NaN/NaN/NaN")
                    {
                        yield return new ValidationResult(
                            $"StartDate Must be Provide",
                            new[] { nameof(StartDate) });
                    }
                    if (string.IsNullOrEmpty(EndDate) || string.IsNullOrWhiteSpace(EndDate) || EndDate == "NaN/NaN/NaN")
                    {
                        yield return new ValidationResult(
                            $"EndDate Must be Provide",
                            new[] { nameof(EndDate) });
                    }
                    if (string.IsNullOrEmpty(Description) || string.IsNullOrWhiteSpace(Description) || Description == "NaN/NaN/NaN")
                    {
                        yield return new ValidationResult(
                            $"Description Must be Provide",
                            new[] { nameof(Description) });
                    }
                    break;
                case Voucher.VoucherType.Type4:
                    if (string.IsNullOrEmpty(QuantityVoucher) || string.IsNullOrWhiteSpace(QuantityVoucher) || QuantityVoucher == "0")
                    {
                        yield return new ValidationResult(
                            $"QuantityVoucher Must be Provide",
                            new[] { nameof(QuantityVoucher) });
                    }
                    if (string.IsNullOrEmpty(MaxUsagePerUser) || string.IsNullOrWhiteSpace(MaxUsagePerUser) || MaxUsagePerUser == "0")
                    {
                        yield return new ValidationResult(
                            $"MaxUsagePerUser Must be Provide",
                            new[] { nameof(MaxUsagePerUser) });
                    }
                    if (string.IsNullOrEmpty(DiscountName) || string.IsNullOrWhiteSpace(DiscountName))
                    {
                        yield return new ValidationResult(
                            $"DiscountName Must be Provide",
                            new[] { nameof(DiscountName) });
                    }
                    if (string.IsNullOrEmpty(DiscountCode) || string.IsNullOrWhiteSpace(DiscountCode))
                    {
                        yield return new ValidationResult(
                            $"DiscountCode Must be Provide",
                            new[] { nameof(DiscountCode) });
                    }
                    if (string.IsNullOrEmpty(StartDate) || string.IsNullOrWhiteSpace(StartDate) || StartDate == "NaN/NaN/NaN")
                    {
                        yield return new ValidationResult(
                            $"StartDate Must be Provide",
                            new[] { nameof(StartDate) });
                    }
                    if (string.IsNullOrEmpty(EndDate) || string.IsNullOrWhiteSpace(EndDate) || EndDate == "NaN/NaN/NaN")
                    {
                        yield return new ValidationResult(
                            $"EndDate Must be Provide",
                            new[] { nameof(EndDate) });
                    }
                    if (string.IsNullOrEmpty(Description) || string.IsNullOrWhiteSpace(Description) || Description == "NaN/NaN/NaN")
                    {
                        yield return new ValidationResult(
                            $"Description Must be Provide",
                            new[] { nameof(Description) });
                    }
                    break;
                default:
                    break;

            }

        }
    }
}
