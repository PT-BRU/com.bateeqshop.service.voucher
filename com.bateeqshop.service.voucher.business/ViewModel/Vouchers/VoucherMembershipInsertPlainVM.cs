using com.bateeqshop.service.voucher.data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace com.bateeqshop.service.voucher.business.ViewModel.Vouchers
{

    public class VoucherMembershipInsertPlainVM : IValidatableObject
    {
        public int Id { get; set; }
        public bool AppliesMultiply { get; set; }
        public string AssignToCategory { get; set; }
        public string CategoryPurchase { get; set; }
        public string DiscountCode { get; set; }
        public string VoucherName { get; set; }
        public string DiscountPercentage { get; set; }
        public string EndDate { get; set; }
        public string MaxDiscount { get; set; }
        public string MaxUsagePerUser { get; set; }
        public string MinimumPurchase { get; set; }
        public double Nominal { get; set; }
        public string Percentage { get; set; }
        public string ProductGift { get; set; }
        public string ProductPurchase { get; set; }
        public string QtyItemGift { get; set; }
        public string QtyItemPurchase { get; set; }
        public string QuantityVoucher { get; set; }
        public string StartDate { get; set; }
        public string VoucherType { get; set; }
        public int VoucherTypeEnum { get; set; }
        public string Description { get; set; }
        public string AssignToMembershipIds { get; set; }
        public decimal PointExchange { get; set; }
        public int ValidityPeriod { get; set; }

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
                    if (string.IsNullOrEmpty(MinimumPurchase) || string.IsNullOrWhiteSpace(MinimumPurchase))
                    {
                        yield return new ValidationResult(
                            $"MinimumPayment Must be Provide",
                            new[] { nameof(MinimumPurchase) });
                    }
                    if (string.IsNullOrEmpty(VoucherName) || string.IsNullOrWhiteSpace(VoucherName))
                    {
                        yield return new ValidationResult(
                            $"DiscountName Must be Provide",
                            new[] { nameof(VoucherName) });
                    }
                    if (ValidityPeriod <= 0)
                    {
                        yield return new ValidationResult(
                            $"Validity Period Must be Provide",
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
                    if (string.IsNullOrEmpty(AssignToMembershipIds) || string.IsNullOrWhiteSpace(AssignToMembershipIds))
                    {
                        yield return new ValidationResult(
                            $"AssignToMembershipIds Must be Provide",
                            new[] { nameof(AssignToMembershipIds) });
                    }
                    break;

                case Voucher.VoucherType.Product:

                    if (string.IsNullOrEmpty(MinimumPurchase) || string.IsNullOrWhiteSpace(MinimumPurchase))
                    {
                        yield return new ValidationResult(
                            $"MinimumPayment Must be Provide",
                            new[] { nameof(MinimumPurchase) });
                    }
                    if (string.IsNullOrEmpty(VoucherName) || string.IsNullOrWhiteSpace(VoucherName))
                    {
                        yield return new ValidationResult(
                            $"DiscountName Must be Provide",
                            new[] { nameof(VoucherName) });
                    }
                    if (ValidityPeriod <= 0)
                    {
                        yield return new ValidationResult(
                            $"Validity Period Must be Provide",
                            new[] { nameof(DiscountCode) });
                    }
                    if (string.IsNullOrEmpty(ProductGift) || string.IsNullOrWhiteSpace(ProductGift))
                    {
                        yield return new ValidationResult(
                            $"DiscountName Must be Provide",
                            new[] { nameof(VoucherName) });
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
                    if (string.IsNullOrEmpty(AssignToMembershipIds) || string.IsNullOrWhiteSpace(AssignToMembershipIds))
                    {
                        yield return new ValidationResult(
                            $"AssignToMembershipIds Must be Provide",
                            new[] { nameof(AssignToMembershipIds) });
                    }
                    break;


                case Voucher.VoucherType.Percentage:


                    //if (Nominal == 0)
                    //{
                    //    yield return new ValidationResult(
                    //        $"Nominal Must be Provide",
                    //        new[] { nameof(Nominal) });
                    //}
                    if (string.IsNullOrEmpty(MinimumPurchase) || string.IsNullOrWhiteSpace(MinimumPurchase))
                    {
                        yield return new ValidationResult(
                            $"MinimumPayment Must be Provide",
                            new[] { nameof(MinimumPurchase) });
                    }
                    if (string.IsNullOrEmpty(VoucherName) || string.IsNullOrWhiteSpace(VoucherName))
                    {
                        yield return new ValidationResult(
                            $"DiscountName Must be Provide",
                            new[] { nameof(VoucherName) });
                    }
                    //if (string.IsNullOrEmpty(DiscountCode) || string.IsNullOrWhiteSpace(DiscountCode))
                    //{
                    //    yield return new ValidationResult(
                    //        $"DiscountCode Must be Provide",
                    //        new[] { nameof(DiscountCode) });
                    //}
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
                case Voucher.VoucherType.Type2Category:
                    if (string.IsNullOrEmpty(VoucherName) || string.IsNullOrWhiteSpace(VoucherName))
                    {
                        yield return new ValidationResult(
                            $"DiscountName Must be Provide",
                            new[] { nameof(VoucherName) });
                    }
                    //if (string.IsNullOrEmpty(DiscountCode) || string.IsNullOrWhiteSpace(DiscountCode))
                    //{
                    //    yield return new ValidationResult(
                    //        $"DiscountCode Must be Provide",
                    //        new[] { nameof(DiscountCode) });
                    //}
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
                    if (string.IsNullOrEmpty(VoucherName) || string.IsNullOrWhiteSpace(VoucherName))
                        if (string.IsNullOrEmpty(VoucherName) || string.IsNullOrWhiteSpace(VoucherName))
                        {
                            yield return new ValidationResult(
                                $"DiscountName Must be Provide",
                                new[] { nameof(VoucherName) });
                        }
                    //if (string.IsNullOrEmpty(DiscountCode) || string.IsNullOrWhiteSpace(DiscountCode))
                    //{
                    //    yield return new ValidationResult(
                    //        $"DiscountCode Must be Provide",
                    //        new[] { nameof(DiscountCode) });
                    //}
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
                    if (string.IsNullOrEmpty(VoucherName) || string.IsNullOrWhiteSpace(VoucherName))
                    {
                        yield return new ValidationResult(
                            $"DiscountName Must be Provide",
                            new[] { nameof(VoucherName) });
                    }
                    //if (string.IsNullOrEmpty(DiscountCode) || string.IsNullOrWhiteSpace(DiscountCode))
                    //{
                    //    yield return new ValidationResult(
                    //        $"DiscountCode Must be Provide",
                    //        new[] { nameof(DiscountCode) });
                    //}
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
                    if (string.IsNullOrEmpty(VoucherName) || string.IsNullOrWhiteSpace(VoucherName))
                    {
                        yield return new ValidationResult(
                            $"DiscountName Must be Provide",
                            new[] { nameof(VoucherName) });
                    }
                    //if (string.IsNullOrEmpty(DiscountCode) || string.IsNullOrWhiteSpace(DiscountCode))
                    //{
                    //    yield return new ValidationResult(
                    //        $"DiscountCode Must be Provide",
                    //        new[] { nameof(DiscountCode) });
                    //}
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
