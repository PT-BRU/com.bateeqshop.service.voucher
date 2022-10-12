using com.bateeqshop.service.voucher.business.Service.ProductCarts;
using com.bateeqshop.service.voucher.business.Service.Products;
using com.bateeqshop.service.voucher.business.Service.VoucherNominals;
using com.bateeqshop.service.voucher.business.Service.VoucherPercentages;
using com.bateeqshop.service.voucher.business.Service.VoucherProducts;
using com.bateeqshop.service.voucher.business.Service.Vouchers;
using com.bateeqshop.service.voucher.business.Service.VoucherType1s;
using com.bateeqshop.service.voucher.business.Service.VoucherType2Categorys;
using com.bateeqshop.service.voucher.business.Service.VoucherType2Products;
using com.bateeqshop.service.voucher.business.Service.VoucherType3s;
using com.bateeqshop.service.voucher.business.Service.VoucherType4s;
using com.bateeqshop.service.voucher.business.ViewModel;
using com.bateeqshop.service.voucher.business.ViewModel.Products;
using com.bateeqshop.service.voucher.business.ViewModel.Users;
using com.bateeqshop.service.voucher.business.ViewModel.Vouchers;
using com.bateeqshop.service.voucher.data;
using com.bateeqshop.service.voucher.data.Model;
using Com.Moonlay.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.bateeqshop.service.voucher.business.Service.Vouchers
{
    public class VoucherServices : IVoucherServices
    {
        private string _userAgent = "BATEEQSHOP-VOUCHER";
        private string _userBy = "BATEEQSHOP-VOUCHER";
        private readonly VoucherDbContext _context;
        private IVoucherNominalsService _voucherNominal;
        private IVoucherPercentagesService _voucherPercentage;
        private IVoucherType1sService _voucherType1;
        private IVoucherType2CategorysService _voucherType2Category;
        private IVoucherType2ProductsService _voucherType2Product;
        private IVoucherType3sService _voucherType3;
        private IVoucherType4sService _voucherType4;
        private IProductService _productService;
        private IVoucherProductService _voucherProduct;
        private IProductCartService _productCart;
        public VoucherServices(VoucherDbContext context, IVoucherNominalsService voucherNominal, IVoucherPercentagesService voucherPercentage, IVoucherType1sService voucherType1, IVoucherType2CategorysService voucherType2Category, IVoucherType2ProductsService voucherType2Product, IVoucherType3sService voucherType3, IVoucherType4sService voucherType4, IProductService productService, IVoucherProductService voucherProduct, IProductCartService productCart)
        {
            _context = context;
            _voucherNominal = voucherNominal;
            _voucherPercentage = voucherPercentage;
            _voucherType1 = voucherType1;
            _voucherType2Category = voucherType2Category;
            _voucherType2Product = voucherType2Product;
            _voucherType3 = voucherType3;
            _voucherType4 = voucherType4;
            _productService = productService;
            _voucherProduct = voucherProduct;
            _productCart = productCart;
        }

        public void Delete(int id)
        {
            try
            {
                var voucherMaster = _context.Vouchers.Where(entity => entity.Id == id).FirstOrDefault();
                if (voucherMaster == null)
                    return;
                EntityExtension.FlagForDelete(voucherMaster, _userBy, _userAgent);
                switch (voucherMaster.Type)
                {
                    case Voucher.VoucherType.Percentage:
                        _voucherPercentage.Delete(voucherMaster.TypeId);
                        break;
                    case Voucher.VoucherType.Nominal:
                        _voucherNominal.Delete(voucherMaster.TypeId);
                        break;
                    case Voucher.VoucherType.Type1:
                        _voucherType1.Delete(voucherMaster.TypeId);
                        break;
                    case Voucher.VoucherType.Type2Category:
                        _voucherType2Category.Delete(voucherMaster.TypeId);
                        break;
                    case Voucher.VoucherType.Type2Product:
                        _voucherType2Product.Delete(voucherMaster.TypeId);
                        break;
                    case Voucher.VoucherType.Type3:
                        _voucherType3.Delete(voucherMaster.TypeId);
                        break;
                    case Voucher.VoucherType.Type4:
                        _voucherType4.Delete(voucherMaster.TypeId);
                        break;
                    case Voucher.VoucherType.Product:
                        //TODO : delete voucher product
                        break;
                    default:
                        break;
                }
                _context.Update(voucherMaster);
                _context.SaveChanges();
                return;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteUserVoucher(int id, ResponseUserMe user, string uri)
        {
            try
            {
                var dataId = _context.UserVouchers.Where(entity => entity.Id == id).FirstOrDefault();
                var voucher = _context.Vouchers.Where(x => x.Id == dataId.VoucherId).FirstOrDefault();
                var isMembership = false;

                if (Voucher.VoucherType.Nominal == voucher.Type)
                {
                    if (_context.VoucherNominals.Any(x => x.Id == voucher.TypeId && x.MembershipId != null))
                        isMembership = true;
                }
                else if (Voucher.VoucherType.Product == voucher.Type)
                    isMembership = true;

                if (isMembership)
                {
                    dataId.IsRedeemed = false;
                    EntityExtension.FlagForUpdate(dataId, _userBy, _userAgent);
                }
                else
                    EntityExtension.FlagForDelete(dataId, _userBy, _userAgent);

                _context.Update(dataId);
                _context.SaveChanges();

                if (_context.UserVoucherRedeemProducts.Any(x => x.UserVoucherId == id))
                {
                    var dataRedeemProduct = _context.UserVoucherRedeemProducts.Where(x => x.UserVoucherId == id).FirstOrDefault();
                    //EntityExtension.FlagForDelete(dataRedeemProduct, _userBy, _userAgent);
                    //_context.Update(dataRedeemProduct);
                    _context.SaveChanges();

                    _productCart.DeleteProductGiftFromCart(dataRedeemProduct.ProductDetailId, uri, user.Token);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteUserVoucher(int id, string uri)
        {
            try
            {
                var dataId = _context.UserVouchers.Where(entity => entity.Id == id).FirstOrDefault();
                var voucher = _context.Vouchers.Where(x => x.Id == dataId.VoucherId).FirstOrDefault();
                var isMembership = false;

                if(voucher != null)
                {
                    if (Voucher.VoucherType.Nominal == voucher.Type)
                    {
                        if (_context.VoucherNominals.Any(x => x.Id == voucher.TypeId && x.MembershipId != null))
                            isMembership = true;
                    }
                    else if (Voucher.VoucherType.Product == voucher.Type)
                        isMembership = true;
                }

                if (isMembership)
                {
                    dataId.IsRedeemed = false;
                    EntityExtension.FlagForUpdate(dataId, _userBy, _userAgent);
                }
                else
                    EntityExtension.FlagForDelete(dataId, _userBy, _userAgent);

                _context.Update(dataId);
                _context.SaveChanges();

                if (_context.UserVoucherRedeemProducts.Any(x => x.UserVoucherId == id))
                {
                    var dataRedeemProduct = _context.UserVoucherRedeemProducts.Where(x => x.UserVoucherId == id).FirstOrDefault();
                    EntityExtension.FlagForDelete(dataRedeemProduct, _userBy, _userAgent);
                    _context.Update(dataRedeemProduct);
                    _context.SaveChanges();

                    _productCart.DeleteProductGiftFromCart(dataRedeemProduct, uri);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<VoucherVM> FindById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<VoucherVM>> FindByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public int Insert(VoucherVM model)
        {
            throw new NotImplementedException();
        }

        public int UpdateModel(VoucherInsertPlainVM model)
        {
            try
            {
                //var checkSameVoucherCode = _context.Vouchers.Where(s => s.Code == model.DiscountCode);
                //if (checkSameVoucherCode.Count() > 0)
                //    throw new Exception("Voucher Code Duplicate");
                var TypeVoucher = VoucherTypeExtensions.ToVoucherTypeEnum(model.VoucherType);
                var voucher = _context.Vouchers.AsNoTracking().Where(s => s.Id == model.Id).FirstOrDefault();
                var typeId = 0;
                if (voucher != null)
                    typeId = voucher.TypeId;
                switch (TypeVoucher)
                {
                    case Voucher.VoucherType.Percentage:
                        var voucherPercentage = _context.VoucherPercentages.Where(s => s.Id == typeId).Select(s => new VoucherPercentage
                        {
                            Name = model.DiscountName,
                            Value = Convert.ToDecimal(model.Percentage),
                            CurrencyId = 0, //TODO : Is Currency is used?
                            MaxDiscount = Convert.ToDecimal(model.MaxDiscount),
                            Description = model.Description,
                            ExpiredDate = DateTime.ParseExact(model.EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            MinSubtotal = Convert.ToDecimal(model.MinimumPayment),
                            StartDate = DateTime.ParseExact(model.StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            MaxUsage = Convert.ToInt32(model.MaxUsagePerUser),
                            MaxQty = Convert.ToInt32(model.QuantityVoucher),
                            CreatedUtc = s.CreatedUtc,
                            CreatedAgent = s.CreatedAgent,
                            CreatedBy = s.CreatedBy,
                            Id = typeId
                        });
                        //voucherPercentage.Id = typeId;
                        _voucherPercentage.Update(voucherPercentage.FirstOrDefault());
                        break;
                    case Voucher.VoucherType.Nominal:
                        var voucherNominal = _context.VoucherNominals.Where(s => s.Id == typeId).Select(s => new VoucherNominal
                        {
                            Name = model.DiscountName,
                            Description = model.Description,
                            CurrencyId = 0, //TODO: Is Currency is used?
                            StartDate = DateTime.ParseExact(model.StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            ExpiredDate = DateTime.ParseExact(model.EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            Value = Convert.ToDecimal(model.Nominal),
                            MinSubtotal = Convert.ToDecimal(model.MinimumPayment),
                            MaxUsage = Convert.ToInt32(model.MaxUsagePerUser),
                            MaxQty = Convert.ToInt32(model.QuantityVoucher),
                            Nominal = Convert.ToDecimal(model.Nominal),
                            CreatedUtc = s.CreatedUtc,
                            CreatedAgent = s.CreatedAgent,
                            CreatedBy = s.CreatedBy,
                            Id = typeId
                        });
                        //voucherNominal.Id = typeId;
                        _voucherNominal.Update(voucherNominal.FirstOrDefault());
                        break;
                    case Voucher.VoucherType.Product:
                        var voucherProdcut = _context.VoucherProducts.Where(s => s.Id == typeId).Select(s => new VoucherProduct
                        {
                            Name = model.DiscountName,
                            Description = model.Description,
                            ProductId = model.ProductGift,
                            StartDate = DateTime.ParseExact(model.StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            ExpiredDate = DateTime.ParseExact(model.EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            ExchangePoint = model.ExchangePoint,
                            MembershipId = model.AssignToMembershipIds,
                            CreatedUtc = s.CreatedUtc,
                            CreatedAgent = s.CreatedAgent,
                            CreatedBy = s.CreatedBy,
                            Id = typeId
                        });
                        //voucherProdcut.Id = typeId;
                        _voucherProduct.Update(voucherProdcut.FirstOrDefault());
                        break;
                    case Voucher.VoucherType.Type1:
                        var voucherType1 = _context.VoucherType1s.Where(s => s.Id == typeId).Select(s => new VoucherType1
                        {
                            Name = model.DiscountName,
                            CSV_BuyProductIds = model.ProductPurchase,
                            Description = model.Description,
                            MaxUsage = Convert.ToInt32(model.MaxUsagePerUser),
                            MaxQty = Convert.ToInt32(model.QuantityVoucher),
                            StartDate = DateTime.ParseExact(model.StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            ExpiredDate = DateTime.ParseExact(model.EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            CSV_FreeProductId = model.ProductGift,
                            FreeQty = Convert.ToInt32(model.QtyItemGift),
                            MinBuyQtyProduct = Convert.ToInt32(model.QtyItemPurchase),
                            CreatedUtc = s.CreatedUtc,
                            CreatedAgent = s.CreatedAgent,
                            CreatedBy = s.CreatedBy,
                            Id = typeId
                        });
                        //voucherType1.Id = typeId;
                        _voucherType1.Update(voucherType1.FirstOrDefault());
                        break;
                    case Voucher.VoucherType.Type2Product:
                        if (model.AssignToCategory.ToLower() == "category")
                            TypeVoucher = Voucher.VoucherType.Type2Category;
                        if (TypeVoucher == Voucher.VoucherType.Type2Category)
                        {
                            var voucherType2Category = _context.VoucherType2Categories.Where(s => s.Id == typeId).Select(s => new VoucherType2Category
                            {
                                CSV_BuyCategoryIds = model.CategoryPurchase,
                                Description = model.Description,
                                MaxUsage = Convert.ToInt32(model.MaxUsagePerUser),
                                MaxQty = Convert.ToInt32(model.QuantityVoucher),
                                StartDate = DateTime.ParseExact(model.StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                                ExpiredDate = DateTime.ParseExact(model.EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                                PercentageDiscountValue = Convert.ToInt32(model.DiscountPercentage),
                                MinBuyQty = Convert.ToInt32(model.QtyItemPurchase),
                                CreatedUtc = s.CreatedUtc,
                                CreatedAgent = s.CreatedAgent,
                                CreatedBy = s.CreatedBy,
                                Id = typeId,
                                Name = model.DiscountName
                            });
                            //voucherType2Category.Id = typeId;
                            _voucherType2Category.Update(voucherType2Category.FirstOrDefault());

                        }
                        else
                        {
                            var voucherType2Product = _context.VoucherType2Products.Where(s => s.Id == typeId).Select(s => new VoucherType2Product
                            {
                                CSV_BuyProductIds = model.ProductPurchase,
                                Description = model.Description,
                                MaxUsage = Convert.ToInt32(model.MaxUsagePerUser),
                                MaxQty = Convert.ToInt32(model.QuantityVoucher),
                                StartDate = DateTime.ParseExact(model.StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                                ExpiredDate = DateTime.ParseExact(model.EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                                PercentageDiscountValue = Convert.ToInt32(model.DiscountPercentage),
                                MinBuyQty = Convert.ToInt32(model.QtyItemPurchase),
                                CreatedUtc = s.CreatedUtc,
                                CreatedAgent = s.CreatedAgent,
                                CreatedBy = s.CreatedBy,
                                Name = model.DiscountName,
                                Id = typeId
                            });
                            //voucherType2Product.Id = typeId;
                            _voucherType2Product.Update(voucherType2Product.FirstOrDefault());

                        }
                        break;
                    case Voucher.VoucherType.Type3:
                        var voucherType3 = _context.VoucherType3s.Where(s => s.Id == typeId).Select(s => new VoucherType3
                        {
                            Description = model.Description,
                            MaxUsage = Convert.ToInt32(model.MaxUsagePerUser),
                            MaxQty = Convert.ToInt32(model.QuantityVoucher),
                            StartDate = DateTime.ParseExact(model.StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            ExpiredDate = DateTime.ParseExact(model.EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            Name = model.DiscountName,
                            DiscountProductIndex = Convert.ToInt32(model.QtyItemPurchase),
                            PercentageDiscountValue = Convert.ToInt32(model.DiscountPercentage),
                            BuyProductId = Convert.ToInt32(model.ProductPurchase),
                            CreatedUtc = s.CreatedUtc,
                            CreatedAgent = s.CreatedAgent,
                            CreatedBy = s.CreatedBy,
                            Id = typeId
                        });
                        //voucherType3.Id = typeId;
                        _voucherType3.Update(voucherType3.FirstOrDefault());

                        break;
                    case Voucher.VoucherType.Type4:
                        var voucherType4 = _context.VoucherType4s.Where(s => s.Id == typeId).Select(s => new VoucherType4
                        {
                            Description = model.Description,
                            MaxUsage = Convert.ToInt32(model.MaxUsagePerUser),
                            MaxQty = Convert.ToInt32(model.QuantityVoucher),
                            StartDate = DateTime.ParseExact(model.StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            ExpiredDate = DateTime.ParseExact(model.EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            Name = model.DiscountName,
                            FreeProductId = model.ProductGift,
                            MinOrderValue = Convert.ToDecimal(model.MinimumPayment),
                            CreatedUtc = s.CreatedUtc,
                            CreatedAgent = s.CreatedAgent,
                            CreatedBy = s.CreatedBy,
                            CanMultiply = model.AppliesMultiply,
                            Id = typeId
                        });
                        //voucherType4.Id = typeId;
                        _voucherType4.Update(voucherType4.FirstOrDefault());

                        break;
                    default:
                        break;

                }

                var voucherCreate = new Voucher
                {
                    Code = model.DiscountCode,
                    Type = TypeVoucher,
                    TypeId = typeId,
                    Id = model.Id
                };

                EntityExtension.FlagForUpdate(voucherCreate, _userBy, _userAgent);
                var voucherModel = _context.Vouchers.Update(voucherCreate);
                var result = _context.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        public int InsertModel(VoucherInsertPlainVM model)
        {
            var checkSameVoucherCode = _context.Vouchers.Where(s => s.Code == model.DiscountCode);
            if (checkSameVoucherCode.Count() > 0)
                throw new Exception("Voucher Code Duplicate");
            try
            {
                var TypeVoucher = VoucherTypeExtensions.ToVoucherTypeEnum(model.VoucherType);
                var typeId = 0;
                switch (TypeVoucher)
                {
                    case Voucher.VoucherType.Percentage:
                        var voucherPercentage = new VoucherPercentage
                        {
                            Name = model.DiscountName,
                            Value = Convert.ToDecimal(model.Percentage),
                            CurrencyId = 0, //TODO : Is Currency is used?
                            MaxDiscount = Convert.ToDecimal(model.MaxDiscount),
                            Description = model.Description,
                            ExpiredDate = DateTime.ParseExact(model.EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            MinSubtotal = Convert.ToDecimal(model.MinimumPayment),
                            StartDate = DateTime.ParseExact(model.StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            MaxUsage = Convert.ToInt32(model.MaxUsagePerUser),
                            MaxQty = Convert.ToInt32(model.QuantityVoucher)
                        };
                        var resultPercentage = _voucherPercentage.InsertModel(voucherPercentage);
                        typeId = resultPercentage.Id;
                        break;
                    case Voucher.VoucherType.Nominal:
                        var voucherNominal = new VoucherNominal
                        {
                            Name = model.DiscountName,
                            Description = model.Description,
                            CurrencyId = 0, //TODO: Is Currency is used?
                            StartDate = DateTime.ParseExact(model.StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            ExpiredDate = DateTime.ParseExact(model.EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            Value = Convert.ToDecimal(model.Nominal),
                            MinSubtotal = Convert.ToDecimal(model.MinimumPayment),
                            MaxUsage = Convert.ToInt32(model.MaxUsagePerUser),
                            MaxQty = Convert.ToInt32(model.QuantityVoucher),
                            Nominal = Convert.ToDecimal(model.Nominal),
                            MembershipId = model.AssignToMembershipIds
                        };
                        var resultNominal = _voucherNominal.InsertModel(voucherNominal);
                        typeId = resultNominal.Id;
                        break;
                    case Voucher.VoucherType.Product:
                        var voucherProdcut = new VoucherProduct
                        {
                            Name = model.DiscountName,
                            Description = model.Description,
                            ProductId = model.ProductGift,
                            StartDate = DateTime.ParseExact(model.StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            ExpiredDate = DateTime.ParseExact(model.EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            ExchangePoint = model.ExchangePoint,
                            MembershipId = model.AssignToMembershipIds
                        };
                        var resultProduct = _voucherProduct.InsertModel(voucherProdcut);
                        typeId = resultProduct.Id;
                        break;
                    case Voucher.VoucherType.Type1:
                        var freeProductId = 0;
                        var tryConvertToFreeProduct = int.TryParse(model.ProductGift, out freeProductId);
                        var voucherType1 = new VoucherType1
                        {
                            Name = model.DiscountName,
                            CSV_BuyProductIds = model.ProductPurchase,
                            Description = model.Description,
                            MaxUsage = Convert.ToInt32(model.MaxUsagePerUser),
                            MaxQty = Convert.ToInt32(model.QuantityVoucher),
                            StartDate = DateTime.ParseExact(model.StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            ExpiredDate = DateTime.ParseExact(model.EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            CSV_FreeProductId = model.ProductGift,
                            FreeQty = Convert.ToInt32(model.QtyItemGift),
                            FreeProductId = freeProductId,
                            MinBuyQtyProduct = Convert.ToInt32(model.QtyItemPurchase)
                        };
                        var resultType1 = _voucherType1.InsertModel(voucherType1);
                        typeId = resultType1.Id;
                        break;
                    case Voucher.VoucherType.Type2Product:
                        if (model.AssignToCategory.ToLower() == "category")
                            TypeVoucher = Voucher.VoucherType.Type2Category;
                        if (TypeVoucher == Voucher.VoucherType.Type2Category)
                        {
                            var voucherType2Category = new VoucherType2Category
                            {
                                CSV_BuyCategoryIds = model.CategoryPurchase,
                                Description = model.Description,
                                MaxUsage = Convert.ToInt32(model.MaxUsagePerUser),
                                MaxQty = Convert.ToInt32(model.QuantityVoucher),
                                StartDate = DateTime.ParseExact(model.StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                                ExpiredDate = DateTime.ParseExact(model.EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                                PercentageDiscountValue = Convert.ToInt32(model.DiscountPercentage),
                                MinBuyQty = Convert.ToInt32(model.QtyItemPurchase),
                                Name = model.DiscountName
                            };
                            var resultVoucherType2Category = _voucherType2Category.InsertModel(voucherType2Category);
                            typeId = resultVoucherType2Category.Id;
                        }
                        else
                        {
                            var voucherType2Product = new VoucherType2Product
                            {
                                CSV_BuyProductIds = model.ProductPurchase,
                                Description = model.Description,
                                MaxUsage = Convert.ToInt32(model.MaxUsagePerUser),
                                MaxQty = Convert.ToInt32(model.QuantityVoucher),
                                StartDate = DateTime.ParseExact(model.StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                                ExpiredDate = DateTime.ParseExact(model.EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                                PercentageDiscountValue = Convert.ToInt32(model.DiscountPercentage),
                                MinBuyQty = Convert.ToInt32(model.QtyItemPurchase),
                                Name = model.DiscountName
                            };
                            var resultVoucherType2Product = _voucherType2Product.InsertModel(voucherType2Product);
                            typeId = resultVoucherType2Product.Id;
                        }
                        break;
                    case Voucher.VoucherType.Type3:
                        var voucherType3 = new VoucherType3
                        {
                            Description = model.Description,
                            MaxUsage = Convert.ToInt32(model.MaxUsagePerUser),
                            MaxQty = Convert.ToInt32(model.QuantityVoucher),
                            StartDate = DateTime.ParseExact(model.StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            ExpiredDate = DateTime.ParseExact(model.EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            Name = model.DiscountName,
                            DiscountProductIndex = Convert.ToInt32(model.QtyItemPurchase),
                            PercentageDiscountValue = Convert.ToInt32(model.DiscountPercentage),
                            BuyProductId = TryParseInt(model.ProductPurchase)
                        };
                        var resultVoucherType3 = _voucherType3.InsertModel(voucherType3);
                        typeId = resultVoucherType3.Id;
                        break;
                    case Voucher.VoucherType.Type4:
                        var voucherType4 = new VoucherType4
                        {
                            Description = model.Description,
                            MaxUsage = Convert.ToInt32(model.MaxUsagePerUser),
                            MaxQty = Convert.ToInt32(model.QuantityVoucher),
                            StartDate = DateTime.ParseExact(model.StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            ExpiredDate = DateTime.ParseExact(model.EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            Name = model.DiscountName,
                            FreeProductId = model.ProductGift,
                            MinOrderValue = Convert.ToDecimal(model.MinimumPayment),
                            CanMultiply = model.AppliesMultiply,
                        };
                        var resultVoucherType4 = _voucherType4.InsertModel(voucherType4);
                        typeId = resultVoucherType4.Id;
                        break;
                    default:
                        break;

                }

                var voucherCreate = new Voucher
                {
                    Code = model.DiscountCode,
                    Type = TypeVoucher,
                    TypeId = typeId
                };

                EntityExtension.FlagForCreate(voucherCreate, _userBy, _userAgent);
                var voucherModel = _context.Vouchers.Add(voucherCreate);
                var result = _context.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        public int InsertModelMembership(VoucherMembershipInsertPlainVM model)
        {
            try
            {
                var TypeVoucher = VoucherTypeExtensions.ToVoucherTypeEnum(model.VoucherType);
                var typeId = 0;
                switch (TypeVoucher)
                {
                    case Voucher.VoucherType.Percentage:
                        var voucherPercentage = new VoucherPercentage
                        {
                            Name = model.VoucherName,
                            Value = Convert.ToDecimal(model.Percentage),
                            CurrencyId = 0, //TODO : Is Currency is used?
                            MaxDiscount = Convert.ToDecimal(model.MaxDiscount),
                            Description = model.Description,
                            ExpiredDate = DateTime.ParseExact(model.EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            MinSubtotal = Convert.ToDecimal(model.MinimumPurchase),
                            StartDate = DateTime.ParseExact(model.StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            MaxUsage = Convert.ToInt32(model.MaxUsagePerUser),
                            MaxQty = Convert.ToInt32(model.QuantityVoucher)
                        };
                        var resultPercentage = _voucherPercentage.InsertModel(voucherPercentage);
                        typeId = resultPercentage.Id;
                        break;
                    case Voucher.VoucherType.Nominal:
                        var voucherNominal = new VoucherNominal
                        {
                            Name = model.VoucherName,
                            Description = model.Description,
                            CurrencyId = 0, //TODO: Is Currency is used?
                            StartDate = DateTime.ParseExact(model.StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            ExpiredDate = DateTime.ParseExact(model.EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            Value = Convert.ToDecimal(model.Nominal),
                            MinSubtotal = Convert.ToDecimal(model.MinimumPurchase),
                            ExchangePoint = model.PointExchange,
                            MaxUsage = Convert.ToInt32(model.MaxUsagePerUser),
                            MaxQty = Convert.ToInt32(model.QuantityVoucher),
                            Nominal = Convert.ToDecimal(model.Nominal),
                            MembershipId = model.AssignToMembershipIds,
                            ValidityPeriod = model.ValidityPeriod
                        };
                        var resultNominal = _voucherNominal.InsertModel(voucherNominal);
                        typeId = resultNominal.Id;
                        break;
                    case Voucher.VoucherType.Product:
                        var voucherProdcut = new VoucherProduct
                        {
                            Name = model.VoucherName,
                            Description = model.Description,
                            ProductId = model.ProductGift,
                            MinSubtotal = Convert.ToDecimal(model.MinimumPurchase),
                            StartDate = DateTime.ParseExact(model.StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            ExpiredDate = DateTime.ParseExact(model.EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            ExchangePoint = model.PointExchange,
                            MembershipId = model.AssignToMembershipIds,
                            ValidityPeriod = model.ValidityPeriod
                        };
                        var resultProduct = _voucherProduct.InsertModel(voucherProdcut);
                        typeId = resultProduct.Id;
                        break;
                    case Voucher.VoucherType.Type1:
                        var voucherType1 = new VoucherType1
                        {
                            Name = model.VoucherName,
                            CSV_BuyProductIds = model.ProductPurchase,
                            Description = model.Description,
                            MaxUsage = Convert.ToInt32(model.MaxUsagePerUser),
                            MaxQty = Convert.ToInt32(model.QuantityVoucher),
                            StartDate = DateTime.ParseExact(model.StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            ExpiredDate = DateTime.ParseExact(model.EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            CSV_FreeProductId = model.ProductGift,
                            FreeQty = Convert.ToInt32(model.QtyItemGift),
                            MinBuyQtyProduct = Convert.ToInt32(model.QtyItemGift)
                        };
                        var resultType1 = _voucherType1.InsertModel(voucherType1);
                        typeId = resultType1.Id;
                        break;
                    case Voucher.VoucherType.Type2Product:
                        if (model.AssignToCategory.ToLower() == "category")
                            TypeVoucher = Voucher.VoucherType.Type2Category;
                        if (TypeVoucher == Voucher.VoucherType.Type2Category)
                        {
                            var voucherType2Category = new VoucherType2Category
                            {
                                CSV_BuyCategoryIds = model.CategoryPurchase,
                                Description = model.Description,
                                MaxUsage = Convert.ToInt32(model.MaxUsagePerUser),
                                MaxQty = Convert.ToInt32(model.QuantityVoucher),
                                StartDate = DateTime.ParseExact(model.StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                                ExpiredDate = DateTime.ParseExact(model.EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                                PercentageDiscountValue = Convert.ToInt32(model.DiscountPercentage),
                                MinBuyQty = Convert.ToInt32(model.QtyItemPurchase)
                            };
                            var resultVoucherType2Category = _voucherType2Category.InsertModel(voucherType2Category);
                            typeId = resultVoucherType2Category.Id;
                        }
                        else
                        {
                            var voucherType2Product = new VoucherType2Product
                            {
                                CSV_BuyProductIds = model.ProductPurchase,
                                Description = model.Description,
                                MaxUsage = Convert.ToInt32(model.MaxUsagePerUser),
                                MaxQty = Convert.ToInt32(model.QuantityVoucher),
                                StartDate = DateTime.ParseExact(model.StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                                ExpiredDate = DateTime.ParseExact(model.EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                                PercentageDiscountValue = Convert.ToInt32(model.DiscountPercentage),
                                MinBuyQty = Convert.ToInt32(model.QtyItemPurchase)
                            };
                            var resultVoucherType2Product = _voucherType2Product.InsertModel(voucherType2Product);
                            typeId = resultVoucherType2Product.Id;
                        }
                        break;
                    case Voucher.VoucherType.Type3:
                        var voucherType3 = new VoucherType3
                        {
                            Description = model.Description,
                            MaxUsage = Convert.ToInt32(model.MaxUsagePerUser),
                            MaxQty = Convert.ToInt32(model.QuantityVoucher),
                            StartDate = DateTime.ParseExact(model.StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            ExpiredDate = DateTime.ParseExact(model.EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            Name = model.VoucherName,
                            DiscountProductIndex = Convert.ToInt32(model.QtyItemPurchase),
                            PercentageDiscountValue = Convert.ToInt32(model.DiscountPercentage)
                        };
                        var resultVoucherType3 = _voucherType3.InsertModel(voucherType3);
                        typeId = resultVoucherType3.Id;
                        break;
                    case Voucher.VoucherType.Type4:
                        var voucherType4 = new VoucherType4
                        {
                            Description = model.Description,
                            MaxUsage = Convert.ToInt32(model.MaxUsagePerUser),
                            MaxQty = Convert.ToInt32(model.QuantityVoucher),
                            StartDate = DateTime.ParseExact(model.StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            ExpiredDate = DateTime.ParseExact(model.EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            Name = model.VoucherName,
                            FreeProductId = model.ProductGift,
                            MinOrderValue = Convert.ToDecimal(model.MinimumPurchase),
                            CanMultiply = model.AppliesMultiply
                        };
                        var resultVoucherType4 = _voucherType4.InsertModel(voucherType4);
                        typeId = resultVoucherType4.Id;
                        break;
                    default:
                        break;

                }

                var voucherCreate = new Voucher
                {
                    Code = model.DiscountCode,
                    Type = TypeVoucher,
                    TypeId = typeId
                };

                EntityExtension.FlagForCreate(voucherCreate, _userBy, _userAgent);
                var voucherModel = _context.Vouchers.Add(voucherCreate);
                var result = _context.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        public async Task<int> UpdateModelMembership(VoucherMembershipInsertPlainVM model)
        {
            var transaction = _context.Database.BeginTransaction();
            try
            {
                var TypeVoucher = VoucherTypeExtensions.ToVoucherTypeEnum(model.VoucherType);
                var voucher = _context.Vouchers.AsNoTracking().Where(s => s.Id == model.Id).FirstOrDefault();
                var typeId = 0;
                if (voucher != null)
                    typeId = voucher.TypeId;
                switch (TypeVoucher)
                {
                    case Voucher.VoucherType.Nominal:
                        var voucherNominal = new VoucherNominal
                        {
                            Name = model.VoucherName,
                            Description = model.Description,
                            CurrencyId = 0, //TODO: Is Currency is used?
                            StartDate = DateTime.ParseExact(model.StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            ExpiredDate = DateTime.ParseExact(model.EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            Value = Convert.ToDecimal(model.Nominal),
                            MinSubtotal = Convert.ToDecimal(model.MinimumPurchase),
                            ExchangePoint = model.PointExchange,
                            MaxUsage = Convert.ToInt32(model.MaxUsagePerUser),
                            MaxQty = Convert.ToInt32(model.QuantityVoucher),
                            Nominal = Convert.ToDecimal(model.Nominal),
                            MembershipId = model.AssignToMembershipIds,
                            ValidityPeriod = model.ValidityPeriod,
                            Id = typeId
                        };
                        _voucherNominal.Update(voucherNominal);
                        break;
                    case Voucher.VoucherType.Product:
                        var voucherProdcut = new VoucherProduct
                        {
                            Name = model.VoucherName,
                            Description = model.Description,
                            ProductId = model.ProductGift,
                            MinSubtotal = Convert.ToDecimal(model.MinimumPurchase),
                            StartDate = DateTime.ParseExact(model.StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            ExpiredDate = DateTime.ParseExact(model.EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            ExchangePoint = model.PointExchange,
                            MembershipId = model.AssignToMembershipIds,
                            ValidityPeriod = model.ValidityPeriod,
                            Id = typeId
                        };
                        _voucherProduct.Update(voucherProdcut);
                        break;

                    default:
                        break;

                }

                var voucherCreate = new Voucher
                {
                    Code = model.DiscountCode,
                    Type = TypeVoucher,
                    TypeId = typeId,
                    Id = model.Id
                };

                EntityExtension.FlagForUpdate(voucherCreate, _userBy, _userAgent);
                _context.Vouchers.Update(voucherCreate);
                var result = _context.SaveChanges();
                transaction.Commit();
                return result;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
        }

        public VoucherVM InsertModel(VoucherVM model)
        {
            throw new NotImplementedException();
        }

        public void Update(VoucherVM model)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseUseVoucherViewModel> UseVoucher(UseVoucherViewModel model, string urlProductIds, ResponseUserMe user, string productServiceUri)
        {
            try
            {
                var voucherNominals = _context.VoucherNominals.AsQueryable();
                var voucherPercentages = _context.VoucherPercentages.AsQueryable();
                var voucherProducts = _context.VoucherProducts.AsQueryable();
                var voucherType1s = _context.VoucherType1s.AsQueryable();
                var voucherType2Products = _context.VoucherType2Products.AsQueryable();
                var voucherType2Categories = _context.VoucherType2Categories.AsQueryable();
                var voucherType3s = _context.VoucherType3s.AsQueryable();
                var voucherType4s = _context.VoucherType4s.AsQueryable();
                var dateNow = DateTime.Now.Date;

                //if (model.ProductList == null)
                //    throw new Exception("Please add product to your bag before use this voucher");

                var listProduct = new List<Product>();
                if (model.ProductList != null && model.ProductList.Count > 0)
                    listProduct = GetInfoProducts(model.ProductList.Select(s => s.Id).ToList(), urlProductIds);
                List<Voucher> modelVoucher;
                //if (model.IdVoucher != 0)
                //if (model.IdVoucher.FirstOrDefault() != 0)
                //    modelVoucher = _context.Vouchers.Where(s => model.IdVoucher.Contains(s.Id)).ToList();
                //else
                //    modelVoucher = _context.Vouchers.Where(s => model.VoucherCode.Contains(s.Code)).ToList();

                modelVoucher = _context.Vouchers.Where(s => model.Voucher.Select(x => x.VoucherId).Contains(s.Id)).ToList();

                if (modelVoucher == null)
                    throw new Exception("Voucher not Found");

                #region validation
                var checkVoucherStock = CheckStockVoucher(modelVoucher);
                var voucherNotAvails = checkVoucherStock.Where(s => !s.Validation);
                if (voucherNotAvails.Count() > 0)
                    foreach (var voucherNotAvail in voucherNotAvails)
                    {
                        var isNotCode = modelVoucher.Any(x => x.Id.Equals(voucherNotAvail.VoucherId));
                        int index = -1;

                        if (isNotCode)
                            index = modelVoucher.FindIndex(x => x.Id.Equals(voucherNotAvail.VoucherId));
                        else
                            index = modelVoucher.FindIndex(x => x.Code.Contains(voucherNotAvail.Code));

                        if (index > -1)
                        {
                            var voucherStockValidation = new UseVoucherCodeViewModel();
                            if (isNotCode)
                                voucherStockValidation = model.Voucher.Where(x => x.VoucherId.Equals(voucherNotAvail.VoucherId)).FirstOrDefault();
                            else
                                voucherStockValidation = model.Voucher.Where(x => x.VoucherCode.Contains(voucherNotAvail.Code)).FirstOrDefault();

                            if (voucherStockValidation.IsApplied)
                                continue;

                            voucherStockValidation.Message = "Voucher Out of Stock";

                            //if (isNotCode)
                            //model.Voucher.Where(x => x.VoucherId.Equals(voucherNotAvail.VoucherId)).FirstOrDefault().Message = "Voucher Out of Stock";
                            //else
                            //model.Voucher.Where(x => x.VoucherCode.Contains(voucherNotAvail.Code)).FirstOrDefault().Message = "Voucher Out of Stock";

                            //modelVoucher.RemoveAt(index);
                        }
                    }
                //throw new Exception("Voucher Out of Stock : " + string.Join(", ", voucherNotAvails.Select(s => s.Key).ToList()));

                var checkVoucherCombination = CheckDuplicateVoucher(modelVoucher, model);
                var voucherCombination = checkVoucherCombination.Where(s => !s.Validation);
                if (voucherCombination.Count() > 0)
                    foreach (var v in voucherCombination)
                    {
                        int index = modelVoucher.FindLastIndex(x => x.Id.Equals(v.VoucherId));
                        //int index = modelVoucher.FindIndex(x => x.Id.Equals(v.VoucherId));
                        if (index > -1)
                        {
                            var modelCombination = new UseVoucherCodeViewModel();
                            var isDuplicateVoucher = voucherCombination.Where(x => x.VoucherId.Equals(v.VoucherId)).Count() > 1;
                            if (isDuplicateVoucher)
                                modelCombination = model.Voucher.Where(x => x.VoucherId.Equals(v.VoucherId) && !x.IsApplied).FirstOrDefault();
                            else
                                modelCombination = model.Voucher.Where(x => x.VoucherId.Equals(v.VoucherId)).FirstOrDefault();

                            if (modelCombination.IsApplied)
                                continue;

                            modelCombination.Message = "Voucher can't be combine. Please check the t&c.";

                            //if (!isDuplicateVoucher)
                            //    modelVoucher.RemoveAt(index);
                            //validationResult.Add(new KeyValuePair<string, string>(v.Key, "Voucher Combination Can't Be Valid"));
                        }
                    }
                //throw new Exception("Voucher Combination Can't Be Valid : " + string.Join(", ", voucherNotAvails.Select(s => s.Key).ToList()));

                var checkAvailPerUser = CheckVoucherAvailPerUser(modelVoucher, user);
                var voucherNotAvailsPerUser = checkAvailPerUser.Where(s => !s.Value);
                if (voucherNotAvailsPerUser.Count() > 0)
                    foreach (var v in checkAvailPerUser)
                    {
                        if (v.Value)
                            continue;

                        if (model.Voucher.Where(x => x.VoucherId.Equals(v.Key)).FirstOrDefault().IsApplied)
                            continue;

                        int index = -1;
                        index = modelVoucher.FindIndex(x => x.Id.Equals(v.Key));

                        if (index > -1 && !v.Value)
                        {
                            model.Voucher.Where(x => x.VoucherId.Equals(v.Key)).FirstOrDefault().Message = "You have reached max voucher usage limit";
                            //validationResult.Add(new KeyValuePair<string, string>(v.Key, "You have reached max voucher usage limit"));
                            //modelVoucher.RemoveAt(index);
                        }
                    }
                //throw new Exception("Voucher Out of Stock For the Users : " + string.Join(", ", voucherNotAvailsPerUser.Select(s => s.Key).ToList()));
                //throw new Exception("You have reached max voucher usage limit.");

                //var checkVoucherExpires = CheckVoucherExpired(modelVoucher, dateNow, user);
                //var voucherExpires = checkVoucherExpires.Where(s => !s.Validation);
                //if (voucherExpires.Count() > 0)
                //    foreach (var voucherExpire in voucherExpires)
                //    {
                //        var isNotCode = modelVoucher.Any(x => x.Id.Equals(voucherExpire.VoucherId));
                //        int index = -1;

                //        if (isNotCode)
                //            index = modelVoucher.FindIndex(x => x.Id.Equals(voucherExpire.VoucherId));
                //        else
                //            index = modelVoucher.FindIndex(x => x.Code.Contains(voucherExpire.Code));

                //        if (index > -1)
                //        {
                //            if (isNotCode)
                //                model.Voucher.Where(x => x.VoucherId.Equals(voucherExpire.VoucherId)).FirstOrDefault().Message = "Voucher is Expired";
                //            else
                //                model.Voucher.Where(x => x.VoucherCode.Contains(voucherExpire.Code)).FirstOrDefault().Message = "Voucher is Expired";
                //            //validationResult.Add(new KeyValuePair<string, string>(voucherExpire.Key, "Voucher is Expired"));
                //            modelVoucher.RemoveAt(index);
                //        }
                //    }
                ////throw new Exception("Voucher is Expired : " + string.Join(", ", voucherExpires.Select(s => s.Key).ToList()));
                #endregion

                #region calculate discount by type voucher
                decimal calculateDiscount = 0;
                List<Product> freeProducts = new List<Product>();
                int voucherNominalCount = 0;
                int voucherType4Count = 0;
                foreach (var voucherVM in model.Voucher)
                {
                    var voucher = modelVoucher.Where(x => x.Id.Equals(voucherVM.VoucherId)).FirstOrDefault();

                    if (!string.IsNullOrEmpty(voucherVM.Message))
                        continue;

                    if (!voucherVM.IsApplied)
                    {
                        var checkVoucherExpires = CheckVoucherExpired(voucher, dateNow, user);
                        //var voucherExpires = checkVoucherExpires.Where(s => !s.Validation);
                        if (!checkVoucherExpires.Validation)
                        {
                            voucherVM.Message = "Voucher is Expired";
                            continue;
                        }
                    }

                    var expiredDateVoucher = new DateTime();
                    var startDateVoucher = new DateTime();
                    var isMembership = false;
                    //int idVoucher = 0;
                    switch (voucher.Type)
                    {
                        case Voucher.VoucherType.Nominal:
                            VoucherNominal voucherNominal = voucherNominals.Where(s => s.Id == voucher.TypeId).FirstOrDefault();
                            //idVoucher = modelVoucher.Where(s => s.TypeId.Equals(voucherNominal.Id)).FirstOrDefault().Id;
                            if (voucherNominal == null)
                                voucherVM.Message = "Voucher not Found";
                            //model.Voucher.Where(x => x.VoucherId.Equals(idVoucher))
                            //    .LastOrDefault().Message = "Voucher not Found";

                            var userVoucherNominalIfMembership = voucherNominal.MembershipId == null || voucherNominal.MembershipId == string.Empty ? null : _context.UserVouchers.FirstOrDefault(s => s.VoucherId == voucher.Id && s.UserId == user.UserIds && !s.IsRedeemed && (s.StartDate.Date <= dateNow && s.EndDate.Date >= dateNow));
                            expiredDateVoucher = userVoucherNominalIfMembership == null ? voucherNominal.ExpiredDate : userVoucherNominalIfMembership.EndDate;
                            startDateVoucher = userVoucherNominalIfMembership == null ? voucherNominal.ExpiredDate : userVoucherNominalIfMembership.StartDate;
                            isMembership = userVoucherNominalIfMembership != null;

                            var subTotalNominal = CalculatNominal(voucherNominal, listProduct, model.ProductList);
                            if (subTotalNominal > 0 && isMembership) voucherNominalCount++;

                            if (subTotalNominal >= voucherNominal.MinSubtotal * voucherNominalCount && isMembership)
                                calculateDiscount += voucherNominal.Nominal;
                            else if (subTotalNominal >= voucherNominal.MinSubtotal && !isMembership)
                                calculateDiscount += voucherNominal.Nominal;
                            else
                                voucherVM.Message = "Term and condition must be fulfilled";
                            //model.Voucher.Where(x => x.VoucherId.Equals(idVoucher))
                            //    .LastOrDefault().Message = "Term and condition must be fulfilled";

                            //expiredDateVoucher = voucherNominal.ExpiredDate;
                            //startDateVoucher = voucherNominal.StartDate;
                            break;

                        case Voucher.VoucherType.Percentage:
                            VoucherPercentage voucherPercentage = voucherPercentages.Where(s => s.Id == voucher.TypeId).FirstOrDefault();
                            //idVoucher = modelVoucher.Where(s => s.TypeId.Equals(voucherPercentage.Id)).FirstOrDefault().Id;
                            if (voucherPercentage == null)
                                voucherVM.Message = "Voucher not Found";
                            //model.Voucher.Where(x => x.VoucherId.Equals(idVoucher))
                            //    .FirstOrDefault().Message = "Voucher not Found";
                            //model.Voucher.Where(x => x.VoucherId.Equals(voucherPercentage.Id)).FirstOrDefault().Message = "Voucher not Found";
                            //throw new Exception("Voucher not Found");

                            var subTotalPercentage = CalculatPercentage(voucherPercentage, listProduct, model.ProductList);
                            //calculateDiscount += CalculatPercentage(voucherPercentage, listProduct, model.ProductList);

                            if (subTotalPercentage > 0)
                                calculateDiscount += subTotalPercentage;
                            else
                                voucherVM.Message = "Term and condition must be fulfilled";
                            //model.Voucher.Where(x => x.VoucherId.Equals(idVoucher))
                            //    .FirstOrDefault().Message = "Term and condition must be fulfilled";

                            expiredDateVoucher = voucherPercentage.ExpiredDate;
                            startDateVoucher = voucherPercentage.StartDate;
                            break;
                        case Voucher.VoucherType.Product:
                            VoucherProduct voucherProduct = voucherProducts.Where(s => s.Id == voucher.TypeId).FirstOrDefault();
                            //idVoucher = modelVoucher.Where(s => s.TypeId.Equals(voucherProduct.Id)).FirstOrDefault().Id;
                            if (voucherProduct == null)
                                voucherVM.Message = "Voucher not Found";
                            //model.Voucher.Where(x => x.VoucherId.Equals(idVoucher))
                            //    .FirstOrDefault().Message = "Voucher not Found";

                            var freeProductVoucher = CalculatProduct(voucherProduct, listProduct, model.ProductList,model.ProductGiftChoose, urlProductIds);
                            //calculateDiscount += freeProductVoucher.discountPotential;
                            //calculateDiscount += 0;
                            if (freeProductVoucher.freeProduct.Count > 0)
                                if (freeProducts.Count > 0)
                                    freeProducts = freeProductVoucher.freeProduct;
                                else
                                    freeProducts.AddRange(freeProductVoucher.freeProduct);
                            //else
                            //    voucherVM.Message = "Term and condition must be fulfilled";
                            //model.Voucher.Where(x => x.VoucherId.Equals(idVoucher))
                            //        .FirstOrDefault().Message = "Term and condition must be fulfilled";

                            var userVoucherIfMembership = voucherProduct.MembershipId == null || voucherProduct.MembershipId == string.Empty ? null : _context.UserVouchers.FirstOrDefault(s => s.VoucherId == voucher.Id && s.UserId == user.UserIds && !s.IsRedeemed && (s.StartDate.Date <= dateNow && s.EndDate.Date >= dateNow));
                            expiredDateVoucher = userVoucherIfMembership == null ? voucherProduct.ExpiredDate : userVoucherIfMembership.EndDate;
                            startDateVoucher = userVoucherIfMembership == null ? voucherProduct.ExpiredDate : userVoucherIfMembership.StartDate;
                            isMembership = userVoucherIfMembership != null;
                            break;
                        case Voucher.VoucherType.Type1:
                            VoucherType1 voucherType1 = voucherType1s.Where(s => s.Id == voucher.TypeId).FirstOrDefault();
                            //idVoucher = modelVoucher.Where(s => s.TypeId.Equals(voucherType1.Id)).FirstOrDefault().Id;
                            if (voucherType1 == null)
                                voucherVM.Message = "Voucher not Found";
                            //model.Voucher.Where(x => x.VoucherId.Equals(idVoucher))
                            //    .FirstOrDefault().Message = "Voucher not Found";

                            var freeProductType1 = CalculatType1(voucherType1, listProduct, model.ProductList, urlProductIds);
                            //calculateDiscount += freeProductType1.discountPotential;
                            if (freeProductType1.freeProduct.Count > 0)
                                freeProducts.AddRange(freeProductType1.freeProduct);
                            else
                                voucherVM.Message = "Term and condition must be fulfilled";
                            //model.Voucher.Where(x => x.VoucherId.Equals(idVoucher))
                            //        .FirstOrDefault().Message = "Term and condition must be fulfilled";

                            //freeProducts.AddRange(freeProductType1.freeProduct);
                            expiredDateVoucher = voucherType1.ExpiredDate;
                            startDateVoucher = voucherType1.StartDate;
                            break;
                        case Voucher.VoucherType.Type2Category:
                            VoucherType2Category voucherType2Categ = voucherType2Categories.Where(s => s.Id == voucher.TypeId).FirstOrDefault();
                            //idVoucher = modelVoucher.Where(s => s.TypeId.Equals(voucherType2Categ.Id)).FirstOrDefault().Id;
                            if (voucherType2Categ == null)
                                voucherVM.Message = "Voucher not Found";
                            //model.Voucher.Where(x => x.VoucherId.Equals(idVoucher))
                            //    .FirstOrDefault().Message = "Voucher not Found";

                            var discountType2Category = CalculatType2Category(voucherType2Categ, listProduct, model.ProductList, urlProductIds);
                            if (discountType2Category > 0)
                                calculateDiscount += discountType2Category;
                            else
                                voucherVM.Message = "Term and condition must be fulfilled";
                            //model.Voucher.Where(x => x.VoucherId.Equals(idVoucher))
                            //        .FirstOrDefault().Message = "Term and condition must be fulfilled";

                            expiredDateVoucher = voucherType2Categ.ExpiredDate;
                            startDateVoucher = voucherType2Categ.StartDate;
                            break;
                        case Voucher.VoucherType.Type2Product:
                            VoucherType2Product voucherType2Product = voucherType2Products.Where(s => s.Id == voucher.TypeId).FirstOrDefault();
                            //idVoucher = modelVoucher.Where(s => s.TypeId.Equals(voucherType2Product.Id)).FirstOrDefault().Id;
                            if (voucherType2Product == null)
                                voucherVM.Message = "Voucher not Found";
                            //model.Voucher.Where(x => x.VoucherId.Equals(idVoucher))
                            //    .FirstOrDefault().Message = "Voucher not Found";

                            var discountType2Product = CalculatType2Product(voucherType2Product, listProduct, model.ProductList, urlProductIds);
                            if (discountType2Product > 0)
                                calculateDiscount += discountType2Product;
                            else
                                voucherVM.Message = "Term and condition must be fulfilled";
                            //model.Voucher.Where(x => x.VoucherId.Equals(idVoucher))
                            //        .FirstOrDefault().Message = "Term and condition must be fulfilled";

                            expiredDateVoucher = voucherType2Product.ExpiredDate;
                            startDateVoucher = voucherType2Product.StartDate;
                            break;
                        case Voucher.VoucherType.Type3:
                            VoucherType3 voucherType3 = voucherType3s.Where(s => s.Id == voucher.TypeId).FirstOrDefault();
                            //idVoucher = modelVoucher.Where(s => s.TypeId.Equals(voucherType3.Id)).FirstOrDefault().Id;
                            if (voucherType3 == null)
                                voucherVM.Message = "Voucher not Found";
                            //model.Voucher.Where(x => x.VoucherId.Equals(idVoucher))
                            //    .FirstOrDefault().Message = "Voucher not Found";

                            var discountType3 = CalculatType3(voucherType3, listProduct, model.ProductList, urlProductIds);
                            if (discountType3 > 0)
                                calculateDiscount += discountType3;
                            else
                                voucherVM.Message = "Term and condition must be fulfilled";
                            //model.Voucher.Where(x => x.VoucherId.Equals(idVoucher))
                            //        .FirstOrDefault().Message = "Term and condition must be fulfilled";

                            expiredDateVoucher = voucherType3.ExpiredDate;
                            startDateVoucher = voucherType3.StartDate;
                            break;
                        case Voucher.VoucherType.Type4:
                            VoucherType4 voucherType4 = voucherType4s.Where(s => s.Id == voucher.TypeId).FirstOrDefault();
                            //idVoucher = modelVoucher.Where(s => s.TypeId.Equals(voucherType4.Id)).FirstOrDefault().Id;
                            if (voucherType4 == null)
                                voucherVM.Message = "Voucher not Found";
                            //model.Voucher.Where(x => x.VoucherId.Equals(idVoucher))
                            //    .LastOrDefault().Message = "Voucher not Found";

                            var freeProductType4 = CalculatType4(voucherType4, listProduct, model.ProductList, urlProductIds, voucherType4Count);
                            //calculateDiscount += freeProductType4.discountPotential;
                            //calculateDiscount += 0;
                            if (freeProductType4.freeProduct.Count > 0)
                            {
                                if (freeProducts.Count > 0)
                                    freeProducts = freeProductType4.freeProduct;
                                else
                                    freeProducts.AddRange(freeProductType4.freeProduct);

                                voucherType4Count++;
                            }
                            else
                                voucherVM.Message = "Term and condition must be fulfilled";

                            //model.Voucher.Where(x => x.VoucherId.Equals(idVoucher))
                            //        .LastOrDefault().Message = "Term and condition must be fulfilled";

                            expiredDateVoucher = voucherType4.ExpiredDate;
                            startDateVoucher = voucherType4.StartDate;
                            break;
                        default:
                            break;
                    }

                    //var singleVoucher = model.Voucher.Where(x => x.VoucherId.Equals(idVoucher)).FirstOrDefault();
                    var statusVoucher = voucherVM.Message == null;
                    bool isAnyUserVoucher = _context.UserVouchers.Any(x => x.IsRedeemed && x.Id == voucherVM.UserVoucherId);

                    #region addGiftToCart
                    List<Product> productGiftChoose = freeProducts.Join(model.ProductGiftChoose,
                                                        freeProduct => freeProduct.Id,
                                                        productGift => productGift.ProductId,
                                                        (freeProduct, productGift) => new Product
                                                        {
                                                            Active = freeProduct.Active,
                                                            Id = freeProduct.Id,
                                                            CreatedUtc = freeProduct.CreatedUtc,
                                                            CreatedBy = freeProduct.CreatedBy,
                                                            CreatedAgent = freeProduct.CreatedAgent,
                                                            LastModifiedUtc = freeProduct.LastModifiedUtc,
                                                            LastModifiedBy = freeProduct.LastModifiedBy,
                                                            LastModifiedAgent = freeProduct.LastModifiedAgent,
                                                            IsDeleted = freeProduct.IsDeleted,
                                                            DeletedUtc = freeProduct.DeletedUtc,
                                                            DeletedBy = freeProduct.DeletedBy,
                                                            DeletedAgent = freeProduct.DeletedAgent,
                                                            FreeQuantity = freeProduct.FreeQuantity,
                                                            RONumber = freeProduct.RONumber,
                                                            Name = freeProduct.Name,
                                                            DisplayName = freeProduct.DisplayName,
                                                            Description = freeProduct.Description,
                                                            NormalPrice = freeProduct.NormalPrice,
                                                            DiscountPrice = freeProduct.DiscountPrice,
                                                            IsPublished = freeProduct.IsPublished,
                                                            IsFeatured = freeProduct.IsFeatured,
                                                            Size_Guide = freeProduct.Size_Guide,
                                                            MotifId = freeProduct.MotifId,
                                                            IsPreOrder = freeProduct.IsPreOrder,
                                                            Motif = freeProduct.Motif,
                                                            ProductDetails = freeProduct.ProductDetails.Count() > 0 ? freeProduct.ProductDetails.Where(s => s.Id == productGift.ProductDetailId).ToList() : freeProduct.ProductDetails,
                                                            ProductReviews = freeProduct.ProductReviews,
                                                            ProductCategories = freeProduct.ProductCategories,
                                                            ProductImages = freeProduct.ProductImages,
                                                            ProductTags = freeProduct.ProductTags,
                                                            ProductLogos = freeProduct.ProductLogos,
                                                        }).ToList();

                    List<CartProduct> productsGiftCartAdded = new List<CartProduct>();
                    foreach (var productGift in productGiftChoose)
                    {
                        var qtyFree = productGift.FreeQuantity;
                        var productGiftAdd = productGift.ProductDetails.Select(s => new CartProduct
                        {
                            Qty = productGift.FreeQuantity,
                            IsProductGift = true,
                            //ProductDetail = s,
                            ProductDetailId = s.Id
                        });
                        foreach (var cartGift in productGiftAdd)
                        {
                            if (isAnyUserVoucher)
                                continue;

                            var giftAddToCart = await _productCart.PostProductGiftToCart(productServiceUri, cartGift, user.Token);
                            productsGiftCartAdded.Add(giftAddToCart);

                            #region insert toUserVoucher and UserVoucherProduct
                            //if Ismembership than it must be redeem first
                            if (isMembership)
                            {
                                var userVoucherExistingModel = _context.UserVouchers.FirstOrDefault(s => s.VoucherId == voucher.Id && s.UserId == user.UserIds && !s.IsRedeemed && (s.StartDate.Date <= dateNow && s.EndDate.Date >= dateNow));
                                //userVoucherExistingModel.IsRedeemed = true;
                                userVoucherExistingModel.IsRedeemed = statusVoucher;
                                EntityExtension.FlagForUpdate(userVoucherExistingModel, _userBy, _userAgent);
                                var userVoucherUpdate = _context.UserVouchers.Update(userVoucherExistingModel);
                                _context.SaveChanges();

                                //update user cart voucher redeem product
                                if (voucher.Type == Voucher.VoucherType.Product || voucher.Type == Voucher.VoucherType.Type4 || voucher.Type == Voucher.VoucherType.Type1)
                                {
                                    var userVoucherRedeemExisiting = _context.UserVoucherRedeemProducts.FirstOrDefault(s => s.UserVoucherId == userVoucherUpdate.Entity.Id);
                                    if (userVoucherExistingModel.IsRedeemed && userVoucherRedeemExisiting == null)
                                    {
                                        var userVoucherProduct = new UserVoucherRedeemProduct
                                        {
                                            ProductId = productGift.Id,
                                            ProductDetailId = cartGift.ProductDetailId.GetValueOrDefault(),
                                            CartProductId = giftAddToCart.Id,
                                            UserVoucherId = userVoucherUpdate.Entity.Id,
                                        };
                                        EntityExtension.FlagForCreate(userVoucherProduct, _userBy, _userAgent);
                                        var userVoucherProductInserted = _context.UserVoucherRedeemProducts.Add(userVoucherProduct);
                                        _context.SaveChanges();
                                    }
                                    else if (userVoucherExistingModel.IsRedeemed)
                                    {
                                        userVoucherRedeemExisiting.CartProductId = giftAddToCart.Id;
                                        EntityExtension.FlagForUpdate(userVoucherRedeemExisiting, _userBy, _userAgent);
                                        _context.Update(userVoucherRedeemExisiting);
                                        _context.SaveChanges();
                                    }
                                    else
                                    {
                                        EntityExtension.FlagForDelete(userVoucherRedeemExisiting, _userBy, _userAgent);
                                        _context.Update(userVoucherRedeemExisiting);
                                        _context.SaveChanges();
                                        await _productCart.DeleteProductGiftFromCart(cartGift.ProductDetailId.GetValueOrDefault(), productServiceUri, user.Token);
                                    }
                                }
                            }
                            else
                            {
                                var userVoucherNewModel = new UserVoucher()
                                {
                                    UserId = user.UserIds,
                                    VoucherId = voucher.Id,
                                    StartDate = startDateVoucher,
                                    EndDate = expiredDateVoucher,
                                    IsRedeemed = statusVoucher
                                };
                                EntityExtension.FlagForCreate(userVoucherNewModel, _userBy, _userAgent);
                                var userVoucherInserted = _context.UserVouchers.Add(userVoucherNewModel);
                                _context.SaveChanges();
                                if (voucher.Type == Voucher.VoucherType.Product || voucher.Type == Voucher.VoucherType.Type4 || voucher.Type == Voucher.VoucherType.Type1)
                                {
                                    var userVoucherProduct = new UserVoucherRedeemProduct
                                    {
                                        ProductId = productGift.Id,
                                        ProductDetailId = cartGift.ProductDetailId.GetValueOrDefault(),
                                        CartProductId = giftAddToCart.Id,
                                        UserVoucherId = userVoucherInserted.Entity.Id,
                                    };
                                    EntityExtension.FlagForCreate(userVoucherProduct, _userBy, _userAgent);
                                    var userVoucherProductInserted = _context.UserVoucherRedeemProducts.Add(userVoucherProduct);
                                    _context.SaveChanges();

                                    if (userVoucherInserted.Entity.Id > 0)
                                        voucherVM.UserVoucherId = userVoucherInserted.Entity.Id;
                                    //model.Voucher.Where(x => x.VoucherId.Equals(idVoucher))
                                    //        .FirstOrDefault().UserVoucherId = userVoucherProductInserted.Entity.Id;
                                }
                            }
                            #endregion
                        }
                    }
                    #endregion

                    #region Add UserVoucher not with ProductGift Voucher
                    if ((voucher.Type == Voucher.VoucherType.Nominal ||
                        voucher.Type == Voucher.VoucherType.Percentage ||
                        voucher.Type == Voucher.VoucherType.Type2Category ||
                        voucher.Type == Voucher.VoucherType.Type2Product ||
                        voucher.Type == Voucher.VoucherType.Type3) && !isAnyUserVoucher)
                    {
                        if (isMembership)
                        {
                            var userVoucherExistingModel = _context.UserVouchers.FirstOrDefault(s => s.VoucherId == voucher.Id && s.UserId == user.UserIds && !s.IsRedeemed && (s.StartDate.Date <= dateNow && s.EndDate.Date >= dateNow));
                            userVoucherExistingModel.IsRedeemed = statusVoucher;

                            EntityExtension.FlagForUpdate(userVoucherExistingModel, _userBy, _userAgent);
                            var userVoucherUpdate = _context.UserVouchers.Update(userVoucherExistingModel);

                            _context.SaveChanges();
                        }
                        else
                        {
                            if (voucherVM.Message == null)
                            {
                                var userVoucherNewModel = new UserVoucher()
                                {
                                    UserId = user.UserIds,
                                    VoucherId = voucher.Id,
                                    StartDate = startDateVoucher,
                                    EndDate = expiredDateVoucher,
                                    IsRedeemed = statusVoucher
                                };
                                EntityExtension.FlagForCreate(userVoucherNewModel, _userBy, _userAgent);
                                var userVoucherInserted = _context.UserVouchers.Add(userVoucherNewModel);
                                _context.SaveChanges();

                                if (userVoucherInserted.Entity.Id > 0)
                                    voucherVM.UserVoucherId = userVoucherInserted.Entity.Id;
                            }
                        }
                    }
                    #endregion

                    voucherVM.VoucherType = voucher.Type.ToDescription();

                    if (voucherVM.UserVoucherId != null && voucherVM.UserVoucherId > 0)
                        voucherVM.IsApplied = true;
                }
                #endregion

                var resultUseVoucher = new ResponseUseVoucherViewModel
                {
                    discountPotential = calculateDiscount,
                    freeProduct = freeProducts,
                };

                if (model.ProductGiftChoose.Count > 0)
                {
                    List<Product> productGiftChoose = freeProducts.Join(model.ProductGiftChoose,
                                    freeProduct => freeProduct.Id,
                                    productGift => productGift.ProductId,
                                    (freeProduct, productGift) => new Product
                                    {
                                        Active = freeProduct.Active,
                                        Id = freeProduct.Id,
                                        CreatedUtc = freeProduct.CreatedUtc,
                                        CreatedBy = freeProduct.CreatedBy,
                                        CreatedAgent = freeProduct.CreatedAgent,
                                        LastModifiedUtc = freeProduct.LastModifiedUtc,
                                        LastModifiedBy = freeProduct.LastModifiedBy,
                                        LastModifiedAgent = freeProduct.LastModifiedAgent,
                                        IsDeleted = freeProduct.IsDeleted,
                                        DeletedUtc = freeProduct.DeletedUtc,
                                        DeletedBy = freeProduct.DeletedBy,
                                        DeletedAgent = freeProduct.DeletedAgent,
                                        FreeQuantity = freeProduct.FreeQuantity,
                                        RONumber = freeProduct.RONumber,
                                        Name = freeProduct.Name,
                                        DisplayName = freeProduct.DisplayName,
                                        Description = freeProduct.Description,
                                        NormalPrice = freeProduct.NormalPrice,
                                        DiscountPrice = freeProduct.DiscountPrice,
                                        IsPublished = freeProduct.IsPublished,
                                        IsFeatured = freeProduct.IsFeatured,
                                        Size_Guide = freeProduct.Size_Guide,
                                        MotifId = freeProduct.MotifId,
                                        IsPreOrder = freeProduct.IsPreOrder,
                                        Motif = freeProduct.Motif,
                                        ProductDetails = freeProduct.ProductDetails.Count() > 0 ? freeProduct.ProductDetails.Where(s => s.Id == productGift.ProductDetailId).ToList() : freeProduct.ProductDetails,
                                        ProductReviews = freeProduct.ProductReviews,
                                        ProductCategories = freeProduct.ProductCategories,
                                        ProductImages = freeProduct.ProductImages,
                                        ProductTags = freeProduct.ProductTags,
                                        ProductLogos = freeProduct.ProductLogos,
                                    }).ToList();

                    resultUseVoucher = new ResponseUseVoucherViewModel
                    {
                        discountPotential = calculateDiscount,
                        freeProduct = productGiftChoose,
                    };
                }
                //else
                //{
                //    return resultUseVoucher;
                //}

                //if (resultUseVoucher.discountPotential == 0 && resultUseVoucher.freeProduct.Count <=0)
                //    throw new Exception("TnC must be fulfilled");

                #region AddValidationResult
                var validationResuts = model.Voucher.ToList();

                foreach (var validationResult in validationResuts.Select((v, i) => new { Value = v, Index = i }))
                {
                    if (validationResult.Value.UserVoucherId != null &&
                        validationResult.Value.UserVoucherId > 0 &&
                        validationResult.Value.Message != null)
                    {
                        DeleteUserVoucher(validationResult.Value.UserVoucherId, user, productServiceUri);
                        model.Voucher[validationResult.Index].UserVoucherId = 0;
                    }
                    //Delete(validationResult.UserVoucherId);
                }

                resultUseVoucher.useVoucher = model.Voucher;
                #endregion

                return resultUseVoucher;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Voucher> FindByIds(List<int> ids)
        {
            var vouchers = _context.Vouchers.Where(s => ids.Contains(s.Id));
            return vouchers.ToList();
        }
        public VoucherViewByIdViewModel ViewByIdWithProductInfo(int id, string apiProductByIds, string apiCategoryByIds)
        {
            var voucherById = ViewById(id);
            var productGiftInfo = GetInfoProducts(voucherById.ProductGift.Split(',').Select(s => TryParseInt(s)).ToList(), apiProductByIds);
            var productPurchaseInfo = GetInfoProducts(voucherById.ProductPurchase.Split(',').Select(s => TryParseInt(s)).ToList(), apiProductByIds);
            List<Category> categoryInfo = new List<Category>();
            if (voucherById.CategoryPurchase != null)
                categoryInfo = GetInfoCategories(voucherById.CategoryPurchase.Split(',').Select(s => TryParseInt(s)).ToList(), apiCategoryByIds);
            else categoryInfo = new List<Category>();

            var voucherWithProductInfo = new VoucherViewByIdViewModel(voucherById, productGiftInfo.Select(s => new Product { Id = s.Id, Name = s.Name }).ToList(), productPurchaseInfo.Select(s => new Product { Id = s.Id, Name = s.Name }).ToList(), categoryInfo.Select(s => new Category { Id = s.Id, Name = s.Name }).ToList());

            //var voucherWithProductInfo = new VoucherViewByIdViewModel
            //{
            //    AppliesMultiply = voucherById.AppliesMultiply,
            //    AssignToCategory = voucherById.AssignToCategory,
            //    AssignToMembershipIds = voucherById.AssignToMembershipIds,
            //    CategoryPurchase = voucherById.CategoryPurchase,
            //    Description = voucherById.Description,
            //    StartDate = voucherById.StartDate,
            //    VoucherType = voucherById.VoucherType,
            //    DiscountCode = voucherById.DiscountCode,
            //    DiscountName = voucherById.DiscountName,
            //    DiscountPercentage = voucherById.DiscountPercentage,
            //    VoucherTypeEnum = voucher
            //}
            return voucherWithProductInfo;
        }
        public List<VoucherInsertPlainVM> ViewByIds(List<int> ids)
        {
            var result = new List<VoucherSimplyViewModel>();
            var query = _context.Vouchers
                .AsQueryable();

            var queryPercentage = _context.VoucherPercentages.AsQueryable();
            var queryNominal = _context.VoucherNominals.AsQueryable();
            var queryProduct = _context.VoucherProducts.AsQueryable();
            var queryType1 = _context.VoucherType1s.AsQueryable();
            var queryType2Categ = _context.VoucherType2Categories.AsQueryable();
            var queryType2Product = _context.VoucherType2Products.AsQueryable();
            var queryType3 = _context.VoucherType3s.AsQueryable();
            var queryType4 = _context.VoucherType4s.AsQueryable();

            var userVoucher = _context.UserVouchers.AsQueryable();

            var resultVM = from q in query
                           join percentage1 in queryPercentage on q.TypeId equals percentage1.Id into percentages
                           from percentage in percentages.DefaultIfEmpty()
                           join nominal1 in queryNominal on q.TypeId equals nominal1.Id into nominals
                           from nominal in nominals.DefaultIfEmpty()
                           join product1 in queryProduct on q.TypeId equals product1.Id into products
                           from product in products.DefaultIfEmpty()
                           join type11 in queryType1 on q.TypeId equals type11.Id into type1s
                           from type1 in type1s.DefaultIfEmpty()
                           join type2categ1 in queryType2Categ on q.TypeId equals type2categ1.Id into type2categ1s
                           from type2categ in type2categ1s.DefaultIfEmpty()
                           join type2product1 in queryType2Product on q.TypeId equals type2product1.Id into type2products
                           from type2product in type2products.DefaultIfEmpty()
                           join type31 in queryType3 on q.TypeId equals type31.Id into type3s
                           from type3 in type3s.DefaultIfEmpty()
                           join type41 in queryType4 on q.TypeId equals type41.Id into type4s
                           from type4 in type4s.DefaultIfEmpty()
                           where ids.Contains(q.Id)
                           select new VoucherInsertVM
                           {
                               Code = q.Code,
                               Id = q.Id,
                               Type = q.Type.ToDescription(),
                               TypeId = (int)q.Type,
                               TypeEnum = q.Type,
                               VoucherPercentageData = q.Type == Voucher.VoucherType.Percentage ? percentage : null,
                               VoucherNominalData = q.Type == Voucher.VoucherType.Nominal ? nominal : null,
                               VoucherProductData = q.Type == Voucher.VoucherType.Product ? product : null,
                               VoucherType1Data = q.Type == Voucher.VoucherType.Type1 ? type1 : null,
                               VoucherType2CategoryData = q.Type == Voucher.VoucherType.Type2Category ? type2categ : null,
                               VoucherType2ProductData = q.Type == Voucher.VoucherType.Type2Product ? type2product : null,
                               VoucherType3Data = q.Type == Voucher.VoucherType.Type3 ? type3 : null,
                               VoucherType4Data = q.Type == Voucher.VoucherType.Type4 ? type4 : null,
                               UserVouchers = userVoucher.Where(s => s.VoucherId == q.Id).Count()
                           };

            var resultSimply = resultVM
                .Select(s => new VoucherInsertPlainVM
                {
                    Id = s.Id,
                    VoucherTypeEnum = (int)s.TypeEnum,
                    AssignToMembershipIds =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? string.Empty :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.MembershipId :
                    s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.MembershipId :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? string.Empty :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? string.Empty :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? string.Empty :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? string.Empty :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? string.Empty : string.Empty,
                    ExchangePoint =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? 0 :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.ExchangePoint :
                    s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.ExchangePoint :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? 0 :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? 0 :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? 0 :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? 0 :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? 0 : 0,
                    DiscountName =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? s.VoucherPercentageData.Name :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.Name :
                    s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.Name :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? s.VoucherType1Data.Name :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? s.VoucherType2CategoryData.Name :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? s.VoucherType2ProductData.Name :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? s.VoucherType3Data.Name :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? s.VoucherType4Data.Name : string.Empty,
                    StartDate =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? s.VoucherPercentageData.StartDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.StartDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.StartDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? s.VoucherType1Data.StartDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? s.VoucherType2CategoryData.StartDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? s.VoucherType2ProductData.StartDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? s.VoucherType3Data.StartDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? s.VoucherType4Data.StartDate.ToString("yyyy-MM-dd") : string.Empty,
                    AppliesMultiply =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? false :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? false :
                    s.TypeEnum == Voucher.VoucherType.Product ? false :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? false :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? false :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? false :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? false :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? s.VoucherType4Data.CanMultiply : false,
                    Description =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? s.VoucherPercentageData.Description :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.Description :
                    s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.Description :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? s.VoucherType1Data.Description :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? s.VoucherType2CategoryData.Description :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? s.VoucherType2ProductData.Description :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? s.VoucherType3Data.Description :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? s.VoucherType4Data.Description : string.Empty,
                    CategoryPurchase =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? string.Empty :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? string.Empty :
                    s.TypeEnum == Voucher.VoucherType.Product ? string.Empty :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? s.VoucherType1Data.CSV_BuyCategoryId :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? string.Empty :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? string.Empty :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? string.Empty :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? string.Empty : string.Empty,
                    DiscountCode = s.Code,
                    DiscountPercentage =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? s.VoucherPercentageData.Value.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? "0" :
                    s.TypeEnum == Voucher.VoucherType.Product ? "0" :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? "0" :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? s.VoucherType2CategoryData.PercentageDiscountValue.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? s.VoucherType2ProductData.PercentageDiscountValue.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? s.VoucherType3Data.PercentageDiscountValue.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? "0" : "0",
                    MaxDiscount =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? s.VoucherPercentageData.MaxDiscount.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? "0" :
                    s.TypeEnum == Voucher.VoucherType.Product ? "0" :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? "0" :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? "0" :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? "0" :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? "0" :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? "0" : "0",
                    EndDate =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? s.VoucherPercentageData.ExpiredDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.ExpiredDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.ExpiredDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? s.VoucherType1Data.ExpiredDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? s.VoucherType2CategoryData.ExpiredDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? s.VoucherType2ProductData.ExpiredDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? s.VoucherType3Data.ExpiredDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? s.VoucherType4Data.ExpiredDate.ToString("yyyy-MM-dd") : string.Empty,
                    AssignToCategory =
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? "Product" :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? "Category" : string.Empty,
                    MaxUsagePerUser =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? s.VoucherPercentageData.MaxUsage.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.MaxUsage.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) :
                    s.TypeEnum == Voucher.VoucherType.Product ? "0" :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? s.VoucherType1Data.MaxUsage.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? s.VoucherType2CategoryData.MaxUsage.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? s.VoucherType2ProductData.MaxUsage.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? s.VoucherType3Data.MaxUsage.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? s.VoucherType4Data.MaxUsage.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) : string.Empty,
                    MinimumPayment =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? s.VoucherPercentageData.MinSubtotal.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.MinSubtotal.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) :
                    s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.MinSubtotal.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? "0" :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? "0" :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? "0" :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? "0" :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? s.VoucherType4Data.MinOrderValue.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) : "0",
                    Nominal =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? 0 :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? Convert.ToDouble(s.VoucherNominalData.Nominal) :
                    s.TypeEnum == Voucher.VoucherType.Product ? 0 :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? 0 :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? 0 :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? 0 :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? 0 :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? 0 : 0,
                    ProductGift =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? string.Empty :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? string.Empty :
                    s.TypeEnum == Voucher.VoucherType.Product ? string.Empty :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? s.VoucherType1Data.CSV_FreeProductId :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? string.Empty :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? string.Empty :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? string.Empty :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? s.VoucherType4Data.FreeProductId.ToString() : string.Empty,
                    ProductPurchase =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? string.Empty :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? string.Empty :
                    s.TypeEnum == Voucher.VoucherType.Product ? string.Empty :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? s.VoucherType1Data.CSV_BuyProductIds :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? string.Empty :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? s.VoucherType2ProductData.CSV_BuyProductIds :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? string.Empty :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? string.Empty : string.Empty,
                    VoucherType = VoucherTypeExtensions.ToForm(s.TypeEnum),
                    Percentage =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? s.VoucherPercentageData.Value.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? "0" :
                    s.TypeEnum == Voucher.VoucherType.Product ? "0" :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? "0" :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? "0" :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? "0" :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? "0" :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? "0" : "0",
                    QuantityVoucher =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? s.VoucherPercentageData.MaxQty.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.MaxQty.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) :
                    s.TypeEnum == Voucher.VoucherType.Product ? "0" :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? s.VoucherType1Data.MaxQty.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? s.VoucherType2CategoryData.MaxQty.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? s.VoucherType2ProductData.MaxQty.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? s.VoucherType3Data.MaxQty.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? s.VoucherType4Data.MaxQty.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) : "0",

                });
            return resultSimply.ToList();

        }


        public VoucherInsertPlainVM ViewById(int id)
        {
            var result = new List<VoucherSimplyViewModel>();
            var query = _context.Vouchers
                .AsQueryable();

            var queryPercentage = _context.VoucherPercentages.AsQueryable();
            var queryNominal = _context.VoucherNominals.AsQueryable();
            var queryProduct = _context.VoucherProducts.AsQueryable();
            var queryType1 = _context.VoucherType1s.AsQueryable();
            var queryType2Categ = _context.VoucherType2Categories.AsQueryable();
            var queryType2Product = _context.VoucherType2Products.AsQueryable();
            var queryType3 = _context.VoucherType3s.AsQueryable();
            var queryType4 = _context.VoucherType4s.AsQueryable();

            var userVoucher = _context.UserVouchers.AsQueryable();

            var resultVM = from q in query
                           join percentage1 in queryPercentage on q.TypeId equals percentage1.Id into percentages
                           from percentage in percentages.DefaultIfEmpty()
                           join nominal1 in queryNominal on q.TypeId equals nominal1.Id into nominals
                           from nominal in nominals.DefaultIfEmpty()
                           join product1 in queryProduct on q.TypeId equals product1.Id into products
                           from product in products.DefaultIfEmpty()
                           join type11 in queryType1 on q.TypeId equals type11.Id into type1s
                           from type1 in type1s.DefaultIfEmpty()
                           join type2categ1 in queryType2Categ on q.TypeId equals type2categ1.Id into type2categ1s
                           from type2categ in type2categ1s.DefaultIfEmpty()
                           join type2product1 in queryType2Product on q.TypeId equals type2product1.Id into type2products
                           from type2product in type2products.DefaultIfEmpty()
                           join type31 in queryType3 on q.TypeId equals type31.Id into type3s
                           from type3 in type3s.DefaultIfEmpty()
                           join type41 in queryType4 on q.TypeId equals type41.Id into type4s
                           from type4 in type4s.DefaultIfEmpty()
                           where q.Id == id
                           select new VoucherInsertVM
                           {
                               Code = q.Code,
                               Id = q.Id,
                               Type = q.Type.ToDescription(),
                               TypeId = (int)q.Type,
                               TypeEnum = q.Type,
                               VoucherPercentageData = q.Type == Voucher.VoucherType.Percentage ? percentage : null,
                               VoucherNominalData = q.Type == Voucher.VoucherType.Nominal ? nominal : null,
                               VoucherProductData = q.Type == Voucher.VoucherType.Product ? product : null,
                               VoucherType1Data = q.Type == Voucher.VoucherType.Type1 ? type1 : null,
                               VoucherType2CategoryData = q.Type == Voucher.VoucherType.Type2Category ? type2categ : null,
                               VoucherType2ProductData = q.Type == Voucher.VoucherType.Type2Product ? type2product : null,
                               VoucherType3Data = q.Type == Voucher.VoucherType.Type3 ? type3 : null,
                               VoucherType4Data = q.Type == Voucher.VoucherType.Type4 ? type4 : null,
                               UserVouchers = userVoucher.Where(s => s.VoucherId == q.Id).Count()
                           };

            var resultSimply = resultVM
                .Select(s => new VoucherInsertPlainVM
                {
                    Id = s.Id,
                    VoucherTypeEnum = (int)s.TypeEnum,
                    AssignToMembershipIds =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? string.Empty :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.MembershipId :
                    s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.MembershipId :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? string.Empty :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? string.Empty :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? string.Empty :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? string.Empty :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? string.Empty : string.Empty,
                    ExchangePoint =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? 0 :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.ExchangePoint :
                    s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.ExchangePoint :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? 0 :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? 0 :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? 0 :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? 0 :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? 0 : 0,
                    DiscountName =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? s.VoucherPercentageData.Name :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.Name :
                    s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.Name :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? s.VoucherType1Data.Name :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? s.VoucherType2CategoryData.Name :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? s.VoucherType2ProductData.Name :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? s.VoucherType3Data.Name :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? s.VoucherType4Data.Name : string.Empty,
                    StartDate =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? s.VoucherPercentageData.StartDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.StartDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.StartDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? s.VoucherType1Data.StartDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? s.VoucherType2CategoryData.StartDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? s.VoucherType2ProductData.StartDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? s.VoucherType3Data.StartDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? s.VoucherType4Data.StartDate.ToString("yyyy-MM-dd") : string.Empty,
                    AppliesMultiply =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? false :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? false :
                    s.TypeEnum == Voucher.VoucherType.Product ? false :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? false :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? false :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? false :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? false :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? s.VoucherType4Data.CanMultiply : false,
                    Description =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? s.VoucherPercentageData.Description :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.Description :
                    s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.Description :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? s.VoucherType1Data.Description :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? s.VoucherType2CategoryData.Description :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? s.VoucherType2ProductData.Description :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? s.VoucherType3Data.Description :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? s.VoucherType4Data.Description : string.Empty,
                    CategoryPurchase =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? string.Empty :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? string.Empty :
                    s.TypeEnum == Voucher.VoucherType.Product ? string.Empty :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? s.VoucherType1Data.CSV_BuyCategoryId :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? s.VoucherType2CategoryData.CSV_BuyCategoryIds :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? string.Empty :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? string.Empty :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? string.Empty : string.Empty,
                    DiscountCode = s.Code,
                    DiscountPercentage =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? s.VoucherPercentageData.Value.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? "0" :
                    s.TypeEnum == Voucher.VoucherType.Product ? "0" :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? "0" :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? s.VoucherType2CategoryData.PercentageDiscountValue.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? s.VoucherType2ProductData.PercentageDiscountValue.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? s.VoucherType3Data.PercentageDiscountValue.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? "0" : "0",
                    MaxDiscount =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? s.VoucherPercentageData.MaxDiscount.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? "0" :
                    s.TypeEnum == Voucher.VoucherType.Product ? "0" :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? "0" :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? "0" :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? "0" :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? "0" :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? "0" : "0",
                    EndDate =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? s.VoucherPercentageData.ExpiredDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.ExpiredDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.ExpiredDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? s.VoucherType1Data.ExpiredDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? s.VoucherType2CategoryData.ExpiredDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? s.VoucherType2ProductData.ExpiredDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? s.VoucherType3Data.ExpiredDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? s.VoucherType4Data.ExpiredDate.ToString("yyyy-MM-dd") : string.Empty,
                    AssignToCategory =
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? "Product" :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? "Category" : string.Empty,
                    MaxUsagePerUser =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? s.VoucherPercentageData.MaxUsage.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.MaxUsage.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) :
                    s.TypeEnum == Voucher.VoucherType.Product ? "0" :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? s.VoucherType1Data.MaxUsage.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? s.VoucherType2CategoryData.MaxUsage.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? s.VoucherType2ProductData.MaxUsage.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? s.VoucherType3Data.MaxUsage.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? s.VoucherType4Data.MaxUsage.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) : string.Empty,
                    MinimumPayment =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? s.VoucherPercentageData.MinSubtotal.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.MinSubtotal.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) :
                    s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.MinSubtotal.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? "0" :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? "0" :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? "0" :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? "0" :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? s.VoucherType4Data.MinOrderValue.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) : "0",
                    Nominal =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? 0 :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? Convert.ToDouble(s.VoucherNominalData.Nominal) :
                    s.TypeEnum == Voucher.VoucherType.Product ? 0 :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? 0 :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? 0 :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? 0 :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? 0 :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? 0 : 0,
                    ProductGift =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? string.Empty :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? string.Empty :
                    s.TypeEnum == Voucher.VoucherType.Product ? string.Empty :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? s.VoucherType1Data.CSV_FreeProductId :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? string.Empty :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? string.Empty :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? string.Empty :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? s.VoucherType4Data.FreeProductId.ToString() : string.Empty,
                    ProductPurchase =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? string.Empty :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? string.Empty :
                    s.TypeEnum == Voucher.VoucherType.Product ? string.Empty :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? s.VoucherType1Data.CSV_BuyProductIds :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? string.Empty :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? s.VoucherType2ProductData.CSV_BuyProductIds :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? s.VoucherType3Data.BuyProductId.ToString() :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? string.Empty : string.Empty,
                    VoucherType = VoucherTypeExtensions.ToForm(s.TypeEnum),
                    Percentage =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? s.VoucherPercentageData.Value.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? "0" :
                    s.TypeEnum == Voucher.VoucherType.Product ? "0" :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? "0" :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? "0" :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? "0" :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? "0" :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? "0" : "0",
                    QuantityVoucher =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? s.VoucherPercentageData.MaxQty.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.MaxQty.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) :
                    s.TypeEnum == Voucher.VoucherType.Product ? "0" :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? s.VoucherType1Data.MaxQty.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? s.VoucherType2CategoryData.MaxQty.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? s.VoucherType2ProductData.MaxQty.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? s.VoucherType3Data.MaxQty.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? s.VoucherType4Data.MaxQty.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) : "0",
                    QtyItemPurchase =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? "0" :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? "0" :
                    s.TypeEnum == Voucher.VoucherType.Product ? "0" :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? s.VoucherType1Data.MinBuyQtyProduct.ToString() :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? s.VoucherType2CategoryData.MinBuyQty.ToString() :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? s.VoucherType2ProductData.MinBuyQty.ToString() :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? s.VoucherType3Data.DiscountProductIndex.ToString() :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? s.VoucherType4Data.MinOrderValue.ToString() : string.Empty,
                    QtyItemGift =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? "0" :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? "0" :
                    s.TypeEnum == Voucher.VoucherType.Product ? "0" :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? s.VoucherType1Data.FreeQty.ToString() :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? "0" :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? "0" :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? "0" :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? "0" : "0",
                });
            return resultSimply.FirstOrDefault();

        }

        public AddNewVoucherMembershipViewModel ViewByIdMembership(int id)
        {
            var result = new List<VoucherSimplyViewModel>();
            var query = _context.Vouchers
                .AsQueryable();

            var queryPercentage = _context.VoucherPercentages.AsQueryable();
            var queryNominal = _context.VoucherNominals.AsQueryable();
            var queryProduct = _context.VoucherProducts.AsQueryable();
            var queryType1 = _context.VoucherType1s.AsQueryable();
            var queryType2Categ = _context.VoucherType2Categories.AsQueryable();
            var queryType2Product = _context.VoucherType2Products.AsQueryable();
            var queryType3 = _context.VoucherType3s.AsQueryable();
            var queryType4 = _context.VoucherType4s.AsQueryable();

            var userVoucher = _context.UserVouchers.AsQueryable();

            var resultVM = from q in query
                           join percentage1 in queryPercentage on q.TypeId equals percentage1.Id into percentages
                           from percentage in percentages.DefaultIfEmpty()
                           join nominal1 in queryNominal on q.TypeId equals nominal1.Id into nominals
                           from nominal in nominals.DefaultIfEmpty()
                           join product1 in queryProduct on q.TypeId equals product1.Id into products
                           from product in products.DefaultIfEmpty()
                           join type11 in queryType1 on q.TypeId equals type11.Id into type1s
                           from type1 in type1s.DefaultIfEmpty()
                           join type2categ1 in queryType2Categ on q.TypeId equals type2categ1.Id into type2categ1s
                           from type2categ in type2categ1s.DefaultIfEmpty()
                           join type2product1 in queryType2Product on q.TypeId equals type2product1.Id into type2products
                           from type2product in type2products.DefaultIfEmpty()
                           join type31 in queryType3 on q.TypeId equals type31.Id into type3s
                           from type3 in type3s.DefaultIfEmpty()
                           join type41 in queryType4 on q.TypeId equals type41.Id into type4s
                           from type4 in type4s.DefaultIfEmpty()
                           where q.Id == id
                           select new VoucherInsertVM
                           {
                               Code = q.Code,
                               Id = q.Id,
                               Type = q.Type.ToDescription(),
                               TypeId = (int)q.Type,
                               TypeEnum = q.Type,
                               VoucherPercentageData = q.Type == Voucher.VoucherType.Percentage ? percentage : null,
                               VoucherNominalData = q.Type == Voucher.VoucherType.Nominal ? nominal : null,
                               VoucherProductData = q.Type == Voucher.VoucherType.Product ? product : null,
                               VoucherType1Data = q.Type == Voucher.VoucherType.Type1 ? type1 : null,
                               VoucherType2CategoryData = q.Type == Voucher.VoucherType.Type2Category ? type2categ : null,
                               VoucherType2ProductData = q.Type == Voucher.VoucherType.Type2Product ? type2product : null,
                               VoucherType3Data = q.Type == Voucher.VoucherType.Type3 ? type3 : null,
                               VoucherType4Data = q.Type == Voucher.VoucherType.Type4 ? type4 : null,
                               UserVouchers = userVoucher.Where(s => s.VoucherId == q.Id).Count()
                           };

            var resultSimply = resultVM
                .Select(s => new AddNewVoucherMembershipViewModel
                {
                    Id = s.Id,
                    VoucherType = VoucherTypeExtensions.ToForm(s.TypeEnum),
                    VoucherName =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? s.VoucherPercentageData.Name :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.Name :
                    s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.Name :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? s.VoucherType1Data.Name :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? s.VoucherType2CategoryData.Name :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? s.VoucherType2ProductData.Name :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? s.VoucherType3Data.Name :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? s.VoucherType4Data.Name : string.Empty,
                    StartDate =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? s.VoucherPercentageData.StartDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.StartDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.StartDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? s.VoucherType1Data.StartDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? s.VoucherType2CategoryData.StartDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? s.VoucherType2ProductData.StartDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? s.VoucherType3Data.StartDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? s.VoucherType4Data.StartDate.ToString("yyyy-MM-dd") : string.Empty,

                    Description =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? s.VoucherPercentageData.Description :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.Description :
                    s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.Description :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? s.VoucherType1Data.Description :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? s.VoucherType2CategoryData.Description :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? s.VoucherType2ProductData.Description :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? s.VoucherType3Data.Description :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? s.VoucherType4Data.Description : string.Empty,

                    EndDate =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? s.VoucherPercentageData.ExpiredDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.ExpiredDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.ExpiredDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? s.VoucherType1Data.ExpiredDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? s.VoucherType2CategoryData.ExpiredDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? s.VoucherType2ProductData.ExpiredDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? s.VoucherType3Data.ExpiredDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? s.VoucherType4Data.ExpiredDate.ToString("yyyy-MM-dd") : string.Empty,

                    MinimumPurchase =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? s.VoucherPercentageData.MinSubtotal.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.MinSubtotal.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) :
                    s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.MinSubtotal.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? "0" :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? "0" :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? "0" :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? "0" :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? s.VoucherType4Data.MinOrderValue.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) : "0",
                    Nominal =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? 0 :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? Convert.ToDouble(s.VoucherNominalData.Nominal) :
                    s.TypeEnum == Voucher.VoucherType.Product ? 0 :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? 0 :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? 0 :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? 0 :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? 0 :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? 0 : 0,
                    ProductGift =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? new List<string>() :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? new List<string>() :
                    s.TypeEnum == Voucher.VoucherType.Product ? new List<string>() { s.VoucherProductData.ProductId } :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? new List<string>() { s.VoucherType1Data.CSV_FreeProductId } :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? new List<string>() :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? new List<string>() :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? new List<string>() :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? new List<string>() { s.VoucherType4Data.FreeProductId.ToString() } : new List<string>(),

                    PointExchange =
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.ExchangePoint.ToString() :
                    s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.ExchangePoint.ToString() : "0",

                    AssignToMember =
                    s.TypeEnum == Voucher.VoucherType.Nominal ? new List<string>() { s.VoucherNominalData.MembershipId.ToString() } :
                    s.TypeEnum == Voucher.VoucherType.Product ? new List<string>() { s.VoucherProductData.MembershipId.ToString() } : new List<string>(),

                    ValidityPeriod =
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.ValidityPeriod :
                    s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.ValidityPeriod : 0,
                });
            return resultSimply.FirstOrDefault();

        }
        public AddNewVoucherMembershipWithProductInfoViewModel ViewByIdMembershipWithProductInfo(int id, string urlProductIds)
        {
            var voucherGetById = ViewByIdMembership(id);
            List<string> listFreeProductString = new List<string>();
            foreach (var productString in voucherGetById.ProductGift)
            {
                listFreeProductString.AddRange(productString.Split(','));
            }
            var listFreeProduct = listFreeProductString.Select(s => TryParseInt(s)).ToList();

            var listFreeProductInfo = GetInfoProducts(listFreeProduct, urlProductIds);
            var result = new AddNewVoucherMembershipWithProductInfoViewModel
            {
                VoucherType = voucherGetById.VoucherType,
                Nominal = voucherGetById.Nominal,
                VoucherName = voucherGetById.VoucherName,
                MinimumPurchase = voucherGetById.MinimumPurchase,
                PointExchange = voucherGetById.PointExchange,
                ValidityPeriod = voucherGetById.ValidityPeriod,
                StartDate = voucherGetById.StartDate,
                EndDate = voucherGetById.EndDate,
                Description = voucherGetById.Description,
                AssignToMember = voucherGetById.AssignToMember,
                ProductGift = listFreeProductInfo,
                Id = voucherGetById.Id
            };
            return result;
        }
        public ICollection<VoucherSimplyViewModel> ViewSummary()
        {
            throw new NotImplementedException();
        }

        public ICollection<VoucherSimplyViewModel> ViewSummarySearch(DateTime startDate, DateTime endDate, Voucher.VoucherType voucherType, string code, string name)
        {
            var result = new List<VoucherSimplyViewModel>();
            var query = _context.Vouchers
                .AsQueryable();

            var queryPercentage = _context.VoucherPercentages.AsQueryable();
            var queryNominal = _context.VoucherNominals.AsQueryable();
            var queryProduct = _context.VoucherProducts.AsQueryable();
            var queryType1 = _context.VoucherType1s.AsQueryable();
            var queryType2Categ = _context.VoucherType2Categories.AsQueryable();
            var queryType2Product = _context.VoucherType2Products.AsQueryable();
            var queryType3 = _context.VoucherType3s.AsQueryable();
            var queryType4 = _context.VoucherType4s.AsQueryable();

            var userVoucher = _context.UserVouchers.AsQueryable();

            if (startDate != null && startDate.Year != 1)
            {
                queryPercentage = queryPercentage.Where(s => s.StartDate <= startDate);
                queryNominal = queryNominal.Where(s => s.StartDate <= startDate);
                queryProduct = queryProduct.Where(s => s.StartDate <= startDate);
                queryType1 = queryType1.Where(s => s.StartDate <= startDate);
                queryType2Categ = queryType2Categ.Where(s => s.StartDate <= startDate);
                queryType2Product = queryType2Product.Where(s => s.StartDate <= startDate);
                queryType3 = queryType3.Where(s => s.StartDate <= startDate);
                queryType4 = queryType4.Where(s => s.StartDate <= startDate);
            }

            if (endDate != null && endDate.Year != 1)
            {
                queryPercentage = queryPercentage.Where(s => s.ExpiredDate >= endDate);
                queryNominal = queryNominal.Where(s => s.ExpiredDate >= endDate);
                queryProduct = queryProduct.Where(s => s.ExpiredDate >= endDate);
                queryType1 = queryType1.Where(s => s.ExpiredDate >= endDate);
                queryType2Categ = queryType2Categ.Where(s => s.ExpiredDate >= endDate);
                queryType2Product = queryType2Product.Where(s => s.ExpiredDate >= endDate);
                queryType3 = queryType3.Where(s => s.ExpiredDate >= endDate);
                queryType4 = queryType4.Where(s => s.ExpiredDate >= endDate);
            }

            if (!string.IsNullOrEmpty(name))
            {
                queryPercentage = queryPercentage.Where(s => s.Name.ToLower().Contains(name.ToLower()));
                queryNominal = queryNominal.Where(s => s.Name.ToLower().Contains(name.ToLower()));
                queryProduct = queryProduct.Where(s => s.Name.ToLower().Contains(name.ToLower()));
                queryType1 = queryType1.Where(s => s.Name.ToLower().Contains(name.ToLower()));
                queryType2Categ = queryType2Categ.Where(s => s.Name.ToLower().Contains(name.ToLower()));
                queryType2Product = queryType2Product.Where(s => s.Name.ToLower().Contains(name.ToLower()));
                queryType3 = queryType3.Where(s => s.Name.ToLower().Contains(name.ToLower()));
                queryType4 = queryType4.Where(s => s.Name.ToLower().Contains(name.ToLower()));
            }

            if (!string.IsNullOrEmpty(code))
            {
                query = query.Where(s => s.Code.ToLower().Contains(code.ToLower()));
            }

            var resultVM = from q in query
                           join percentage1 in queryPercentage on q.TypeId equals percentage1.Id into percentages
                           from percentage in percentages.DefaultIfEmpty()
                           join nominal1 in queryNominal on q.TypeId equals nominal1.Id into nominals
                           from nominal in nominals.DefaultIfEmpty()
                           join product1 in queryProduct on q.TypeId equals product1.Id into products
                           from product in products.DefaultIfEmpty()
                           join type11 in queryType1 on q.TypeId equals type11.Id into type1s
                           from type1 in type1s.DefaultIfEmpty()
                           join type2categ1 in queryType2Categ on q.TypeId equals type2categ1.Id into type2categ1s
                           from type2categ in type2categ1s.DefaultIfEmpty()
                           join type2product1 in queryType2Product on q.TypeId equals type2product1.Id into type2products
                           from type2product in type2products.DefaultIfEmpty()
                           join type31 in queryType3 on q.TypeId equals type31.Id into type3s
                           from type3 in type3s.DefaultIfEmpty()
                           join type41 in queryType4 on q.TypeId equals type41.Id into type4s
                           from type4 in type4s.DefaultIfEmpty()
                               //join useVoucher in userVoucher on q.Id equals useVoucher.VoucherId into qUseVoucher
                               //from voucherUses in qUseVoucher.DefaultIfEmpty()
                           select new VoucherInsertVM
                           {
                               Code = q.Code,
                               Id = q.Id,
                               Type = q.Type.ToForm(),
                               TypeId = (int)q.Type,
                               TypeEnum = q.Type,
                               VoucherPercentageData = q.Type == Voucher.VoucherType.Percentage ?
                               percentage
                               //new VoucherPercentage
                               //{
                               //    CurrencyId = percentage.CurrencyId,
                               //    Description = percentage.Description,
                               //    ExchangePoint = percentage.ExchangePoint,
                               //    ExpiredDate = percentage.ExpiredDate.ToString("yyyy-MM-dd"),
                               //    Id = percentage.Id,
                               //    ImageUrl = percentage.ImageUrl,
                               //    IsDeleted = percentage.IsDeleted,
                               //    MaxDiscount = percentage.MaxDiscount,
                               //    MaxQty = percentage.MaxQty,
                               //    MaxUsage = percentage.MaxUsage,
                               //    MinSubtotal = percentage.MinSubtotal,
                               //    Name = percentage.Name,
                               //    StartDate = percentage.StartDate.ToString("yyyy-MM-dd")
                               //}

                               : null,

                               VoucherNominalData = q.Type == Voucher.VoucherType.Nominal ? nominal : null,
                               VoucherProductData = q.Type == Voucher.VoucherType.Product ? product : null,
                               VoucherType1Data = q.Type == Voucher.VoucherType.Type1 ? type1 : null,
                               VoucherType2CategoryData = q.Type == Voucher.VoucherType.Type2Category ? type2categ : null,
                               VoucherType2ProductData = q.Type == Voucher.VoucherType.Type2Product ? type2product : null,
                               VoucherType3Data = q.Type == Voucher.VoucherType.Type3 ? type3 : null,
                               VoucherType4Data = q.Type == Voucher.VoucherType.Type4 ? type4 : null,
                               UserVouchers = userVoucher.Where(s => s.VoucherId == q.Id).Count(),
                               CreatedDate = q.CreatedUtc,
                               ModifiedDate = q.Type == Voucher.VoucherType.Nominal ? nominal.LastModifiedUtc :
                               q.Type == Voucher.VoucherType.Product ? product.LastModifiedUtc :
                               q.Type == Voucher.VoucherType.Type1 ? type1.LastModifiedUtc :
                               q.Type == Voucher.VoucherType.Type2Category ? type2categ.LastModifiedUtc :
                               q.Type == Voucher.VoucherType.Type2Product ? type2product.LastModifiedUtc :
                               q.Type == Voucher.VoucherType.Type3 ? type3.LastModifiedUtc :
                               q.Type == Voucher.VoucherType.Type4 ? type4.LastModifiedUtc : DateTime.MinValue,
                           };

            if (voucherType != Voucher.VoucherType.Undefined)
            {
                resultVM = resultVM.Where(s => s.TypeId == (int)voucherType);
            }

            var resultSimply = resultVM
            //    .Where(s=> 
            //(s.VoucherProductData!= null ||
            //s.VoucherNominalData!= null ||
            //s.VoucherPercentageData != null ||
            //s.VoucherType1Data!= null ||
            //s.VoucherType2CategoryData != null ||
            //s.VoucherType2ProductData != null ||
            //s.VoucherType3Data != null ||
            //s.VoucherType4Data != null)
            //&& !string.IsNullOrEmpty(s.Type)
            //)
                .Select(s => new VoucherSimplyViewModel
                {
                    id = s.Id,
                    DiscountType = s.Type,
                    DiscountName =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? s.VoucherPercentageData.Name :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.Name :
                    s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.Name :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? s.VoucherType1Data.Name :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? s.VoucherType2CategoryData.Name :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? s.VoucherType2ProductData.Name :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? s.VoucherType3Data.Name :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? s.VoucherType4Data.Name : string.Empty,
                    StartDate =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? s.VoucherPercentageData.StartDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.StartDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.StartDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? s.VoucherType1Data.StartDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? s.VoucherType2CategoryData.StartDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? s.VoucherType2ProductData.StartDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? s.VoucherType3Data.StartDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? s.VoucherType4Data.StartDate.ToString("yyyy-MM-dd") : string.Empty,
                    EndDate =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? s.VoucherPercentageData.ExpiredDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.ExpiredDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.ExpiredDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? s.VoucherType1Data.ExpiredDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? s.VoucherType2CategoryData.ExpiredDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? s.VoucherType2ProductData.ExpiredDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? s.VoucherType3Data.ExpiredDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? s.VoucherType4Data.ExpiredDate.ToString("yyyy-MM-dd") : string.Empty,
                    TotalUse = s.UserVouchers,
                    Status =
                    (s.TypeEnum == Voucher.VoucherType.Percentage ? s.VoucherPercentageData.ExpiredDate.Date >= DateTime.Now.Date && s.VoucherPercentageData.StartDate.Date <= DateTime.Now.Date :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.ExpiredDate.Date >= DateTime.Now.Date && s.VoucherNominalData.StartDate.Date <= DateTime.Now.Date :
                    s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.ExpiredDate.Date >= DateTime.Now.Date && s.VoucherProductData.StartDate.Date <= DateTime.Now.Date :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? s.VoucherType1Data.ExpiredDate.Date >= DateTime.Now.Date && s.VoucherType1Data.StartDate.Date <= DateTime.Now.Date :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? s.VoucherType2CategoryData.ExpiredDate.Date >= DateTime.Now.Date && s.VoucherType2CategoryData.StartDate.Date <= DateTime.Now.Date :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? s.VoucherType2ProductData.ExpiredDate.Date >= DateTime.Now.Date && s.VoucherType2ProductData.StartDate.Date <= DateTime.Now.Date :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? s.VoucherType3Data.ExpiredDate.Date >= DateTime.Now.Date && s.VoucherType3Data.StartDate.Date <= DateTime.Now.Date :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? s.VoucherType4Data.ExpiredDate.Date >= DateTime.Now.Date && s.VoucherType4Data.StartDate.Date <= DateTime.Now.Date : false)
                    &&
                    (s.TypeEnum == Voucher.VoucherType.Percentage ? s.VoucherPercentageData.MaxQty >= s.UserVouchers :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.MaxQty >= s.UserVouchers :
                    s.TypeEnum == Voucher.VoucherType.Product ? false :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? s.VoucherType1Data.MaxQty >= s.UserVouchers :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? s.VoucherType2CategoryData.MaxQty >= s.UserVouchers :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? s.VoucherType2ProductData.MaxQty >= s.UserVouchers :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? s.VoucherType3Data.MaxQty >= s.UserVouchers :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? s.VoucherType4Data.MaxQty >= s.UserVouchers : false) ?
                    "Active" : "NotActive",
                    Description =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? s.VoucherPercentageData.Description :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.Description :
                    s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.Description :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? s.VoucherType1Data.Description :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? s.VoucherType2CategoryData.Description :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? s.VoucherType2ProductData.Description :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? s.VoucherType3Data.Description :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? s.VoucherType4Data.Description : string.Empty,
                    DiscountCode = s.Code,
                    Membership =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? string.Empty :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.MembershipId :
                    s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.MembershipId :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? string.Empty :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? string.Empty :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? string.Empty :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? string.Empty :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? string.Empty : string.Empty,
                    CreatedDate = s.CreatedDate,
                    ModifiedDate = s.ModifiedDate
                });

            return resultSimply.ToList().Where(s => s.Membership == string.Empty || s.Membership == null).OrderByDescending(s => s.ModifiedDate).ToList();
        }

        List<VoucherVM> IService<VoucherVM>.Find()
        {
            var vouchers = _context.Vouchers.ToList();
            return VoucherVM.MapFrom(vouchers);
        }

        async Task<List<VoucherVM>> IService<VoucherVM>.FindAsync()
        {
            var vouchers = await _context.Vouchers.ToListAsync();
            return VoucherVM.MapFrom(vouchers);
        }

        public int InsertVoucherMembership(AddNewVoucherMembershipViewModel model)
        {
            try
            {
                var TypeVoucher = VoucherTypeExtensions.ToVoucherTypeEnum(model.VoucherType);
                var typeId = 0;
                switch (TypeVoucher)
                {
                    case Voucher.VoucherType.Nominal:
                        var voucherNominal = new VoucherNominal
                        {
                            Name = model.VoucherName,
                            Description = model.Description,
                            CurrencyId = 0, //TODO: Is Currency is used?
                            StartDate = DateTime.ParseExact(model.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                            ExpiredDate = DateTime.ParseExact(model.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                            Value = Convert.ToDecimal(model.Nominal),
                            MinSubtotal = Convert.ToDecimal(model.MinimumPurchase),
                            Nominal = Convert.ToDecimal(model.Nominal),
                            ExchangePoint = Convert.ToDecimal(model.PointExchange),
                            MembershipId = string.Join(',', model.AssignToMember)
                        };
                        var resultNominal = _voucherNominal.InsertModel(voucherNominal);
                        typeId = resultNominal.Id;
                        break;
                    case Voucher.VoucherType.Product:
                        var voucherProduct = new VoucherProduct
                        {
                            Name = model.VoucherName,
                            Description = model.Description,
                            ProductId = string.Join(',', model.ProductGift),
                            StartDate = DateTime.ParseExact(model.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                            ExpiredDate = DateTime.ParseExact(model.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                            ExchangePoint = Convert.ToDecimal(model.PointExchange),
                            MembershipId = string.Join(',', model.AssignToMember)

                        };
                        var resultProduct = _voucherProduct.InsertModel(voucherProduct);
                        typeId = resultProduct.Id;
                        break;
                    default:
                        break;

                }

                var voucherCreate = new Voucher
                {
                    Code = $"MEMPROM-{typeId.ToString()}-{Guid.NewGuid().ToString()}",
                    Type = TypeVoucher,
                    TypeId = typeId
                };

                EntityExtension.FlagForCreate(voucherCreate, _userBy, _userAgent);
                var voucherModel = _context.Vouchers.Add(voucherCreate);
                var result = _context.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                throw ex;

            }
            return 0;
        }


        public ICollection<VoucherSimplyViewModel> ViewSummaryMembershipSearch(DateTime startDate, DateTime endDate, Voucher.VoucherType voucherType, string code, string name, int membershipsId)
        {
            var result = new List<VoucherSimplyViewModel>();
            var query = _context.Vouchers
                .AsQueryable();


            var queryNominal = _context.VoucherNominals.AsQueryable().Where(s => s.MembershipId != null && !string.IsNullOrEmpty(s.MembershipId) && s.MembershipId != "0");
            var queryProduct = _context.VoucherProducts.AsQueryable().Where(s => s.MembershipId != null && !string.IsNullOrEmpty(s.MembershipId) && s.MembershipId != "0");

            var userVoucher = _context.UserVouchers.AsQueryable();

            if (startDate != null && startDate.Year != 1)
            {
                queryNominal = queryNominal.Where(s => s.StartDate <= startDate);
                queryProduct = queryProduct.Where(s => s.StartDate <= startDate);

            }

            if (endDate != null && endDate.Year != 1)
            {
                queryNominal = queryNominal.Where(s => s.ExpiredDate >= endDate);
                queryProduct = queryProduct.Where(s => s.ExpiredDate >= endDate);

            }

            if (!string.IsNullOrEmpty(name))
            {
                queryNominal = queryNominal.Where(s => s.Name.ToLower().Contains(name.ToLower()) && (s.MembershipId != "0" && !string.IsNullOrEmpty(s.MembershipId)));
                queryProduct = queryProduct.Where(s => s.Name.ToLower().Contains(name.ToLower()) && (s.MembershipId != "0" && !string.IsNullOrEmpty(s.MembershipId)));

            }

            if (membershipsId != 0)
            {
                queryNominal = queryNominal.Where(s => s.MembershipId.Contains(membershipsId.ToString()));
                queryProduct = queryProduct.Where(s => s.MembershipId.Contains(membershipsId.ToString()));
            }

            if (!string.IsNullOrEmpty(code))
            {
                query = query.Where(s => s.Code.ToLower().Contains(code.ToLower()));
            }


            var resultVM = from a in (from q in query
                                      join nominal1 in queryNominal on q.TypeId equals nominal1.Id into nominals
                                      from nominal in nominals.DefaultIfEmpty()
                                      join product1 in queryProduct on q.TypeId equals product1.Id into products
                                      from product in products.DefaultIfEmpty()

                                      select new VoucherInsertVM
                                      {
                                          Code = q.Code,
                                          Id = q.Id,
                                          Type = q.Type.ToDescription(),
                                          TypeId = (int)q.Type,
                                          TypeEnum = q.Type,
                                          VoucherNominalData = q.Type == Voucher.VoucherType.Nominal ? nominal : null,
                                          VoucherProductData = q.Type == Voucher.VoucherType.Product ? product : null,
                                          UserVouchers = userVoucher.Where(s => s.VoucherId == q.Id).Count()
                                      }).ToList()
                           where (a.VoucherNominalData != null || a.VoucherProductData != null)
                           select new VoucherInsertVM
                           {
                               Code = a.Code,
                               Id = a.Id,
                               Type = a.Type,
                               TypeId = a.TypeId,
                               TypeEnum = a.TypeEnum,
                               VoucherNominalData = a.VoucherNominalData,
                               VoucherProductData = a.VoucherProductData,
                               UserVouchers = a.UserVouchers
                           }
                           ;
            //resultVM = resultVM.Where(s => s.VoucherNominalData != null || s.VoucherProductData != null);

            if (voucherType != Voucher.VoucherType.Undefined)
            {
                resultVM = resultVM.Where(s => s.TypeId == (int)voucherType);
            }
            //TODO : View Membership for Total Claim and total Used
            var resultSimply = resultVM
                .Select(s => new VoucherSimplyViewModel
                {
                    id = s.Id,
                    DiscountType = s.Type,
                    DiscountName =
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.Name :
                    s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.Name :
                     string.Empty,
                    StartDate =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? s.VoucherPercentageData.StartDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.StartDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.StartDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? s.VoucherType1Data.StartDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? s.VoucherType2CategoryData.StartDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? s.VoucherType2ProductData.StartDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? s.VoucherType3Data.StartDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? s.VoucherType4Data.StartDate.ToString("yyyy-MM-dd") : string.Empty,
                    EndDate =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? s.VoucherPercentageData.ExpiredDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.ExpiredDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.ExpiredDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? s.VoucherType1Data.ExpiredDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? s.VoucherType2CategoryData.ExpiredDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? s.VoucherType2ProductData.ExpiredDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? s.VoucherType3Data.ExpiredDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? s.VoucherType4Data.ExpiredDate.ToString("yyyy-MM-dd") : string.Empty,
                    TotalUse = s.UserVouchers,
                    Status =
                    (s.TypeEnum == Voucher.VoucherType.Percentage ? s.VoucherPercentageData.ExpiredDate >= DateTime.Now :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.ExpiredDate >= DateTime.Now :
                    s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.ExpiredDate >= DateTime.Now :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? s.VoucherType1Data.ExpiredDate >= DateTime.Now :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? s.VoucherType2CategoryData.ExpiredDate >= DateTime.Now :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? s.VoucherType2ProductData.ExpiredDate >= DateTime.Now :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? s.VoucherType3Data.ExpiredDate >= DateTime.Now :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? s.VoucherType4Data.ExpiredDate >= DateTime.Now : false)
                    ||
                    (s.TypeEnum == Voucher.VoucherType.Percentage ? s.VoucherPercentageData.MaxQty <= s.UserVouchers :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.MaxQty <= s.UserVouchers :
                    s.TypeEnum == Voucher.VoucherType.Product ? false :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? s.VoucherType1Data.MaxQty <= s.UserVouchers :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? s.VoucherType2CategoryData.MaxQty <= s.UserVouchers :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? s.VoucherType2ProductData.MaxQty <= s.UserVouchers :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? s.VoucherType3Data.MaxQty <= s.UserVouchers :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? s.VoucherType4Data.MaxQty <= s.UserVouchers : false) ?
                    "NotActive" : "Active",
                    Description =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? s.VoucherPercentageData.Description :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.Description :
                    s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.Description :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? s.VoucherType1Data.Description :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? s.VoucherType2CategoryData.Description :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? s.VoucherType2ProductData.Description :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? s.VoucherType3Data.Description :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? s.VoucherType4Data.Description : string.Empty,
                    DiscountCode = s.Code,
                    ProductId =
                    s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.ProductId : string.Empty,
                    ExchangePoint =
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.ExchangePoint.ToString() :
                    s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.ExchangePoint.ToString() : "0"
                }).ToList();

            //var resultExchange = resultSimply.Where(s => s.ExchangePoint == "0");
            if (resultSimply == null)
                return new List<VoucherSimplyViewModel>();
            return resultSimply.ToList();
        }
        public ICollection<VoucherSimplyViewModel> ViewSummaryMembershipActiveSearch(DateTime startDate, DateTime endDate, Voucher.VoucherType voucherType, string code, string name, int membershipsId)
        {
            var result = new List<VoucherSimplyViewModel>();
            var query = _context.Vouchers
                .AsQueryable();


            var queryNominal = _context.VoucherNominals.AsQueryable().Where(s => s.MembershipId != null && !string.IsNullOrEmpty(s.MembershipId) && s.MembershipId != "0");
            var queryProduct = _context.VoucherProducts.AsQueryable().Where(s => s.MembershipId != null && !string.IsNullOrEmpty(s.MembershipId) && s.MembershipId != "0");

            var userVoucher = _context.UserVouchers.AsQueryable();

            if (startDate != null && startDate.Year != 1)
            {
                queryNominal = queryNominal.Where(s => s.StartDate <= startDate);
                queryProduct = queryProduct.Where(s => s.StartDate <= startDate);

            }

            if (endDate != null && endDate.Year != 1)
            {
                queryNominal = queryNominal.Where(s => s.ExpiredDate >= endDate);
                queryProduct = queryProduct.Where(s => s.ExpiredDate >= endDate);

            }

            if (!string.IsNullOrEmpty(name))
            {
                queryNominal = queryNominal.Where(s => s.Name.ToLower().Contains(name.ToLower()) && (s.MembershipId != "0" && !string.IsNullOrEmpty(s.MembershipId)));
                queryProduct = queryProduct.Where(s => s.Name.ToLower().Contains(name.ToLower()) && (s.MembershipId != "0" && !string.IsNullOrEmpty(s.MembershipId)));

            }

            if (membershipsId != 0)
            {
                queryNominal = queryNominal.Where(s => s.MembershipId.Contains(membershipsId.ToString()));
                queryProduct = queryProduct.Where(s => s.MembershipId.Contains(membershipsId.ToString()));
            }

            if (!string.IsNullOrEmpty(code))
            {
                query = query.Where(s => s.Code.ToLower().Contains(code.ToLower()));
            }


            var resultVM = from a in (from q in query
                                      join nominal1 in queryNominal on q.TypeId equals nominal1.Id into nominals
                                      from nominal in nominals.DefaultIfEmpty()
                                      join product1 in queryProduct on q.TypeId equals product1.Id into products
                                      from product in products.DefaultIfEmpty()

                                      select new VoucherInsertVM
                                      {
                                          Code = q.Code,
                                          Id = q.Id,
                                          Type = q.Type.ToDescription(),
                                          TypeId = (int)q.Type,
                                          TypeEnum = q.Type,
                                          VoucherNominalData = q.Type == Voucher.VoucherType.Nominal ? nominal : null,
                                          VoucherProductData = q.Type == Voucher.VoucherType.Product ? product : null,
                                          UserVouchers = userVoucher.Where(s => s.VoucherId == q.Id).Count()
                                      }).ToList()
                           where (a.VoucherNominalData != null || a.VoucherProductData != null)
                           select new VoucherInsertVM
                           {
                               Code = a.Code,
                               Id = a.Id,
                               Type = a.Type,
                               TypeId = a.TypeId,
                               TypeEnum = a.TypeEnum,
                               VoucherNominalData = a.VoucherNominalData,
                               VoucherProductData = a.VoucherProductData,
                               UserVouchers = a.UserVouchers
                           }
                           ;
            //resultVM = resultVM.Where(s => s.VoucherNominalData != null || s.VoucherProductData != null);

            if (voucherType != Voucher.VoucherType.Undefined)
            {
                resultVM = resultVM.Where(s => s.TypeId == (int)voucherType);
            }
            //TODO : View Membership for Total Claim and total Used
            var resultSimply = resultVM
                .Select(s => new VoucherSimplyViewModel
                {
                    id = s.Id,
                    DiscountType = s.Type,
                    DiscountName =
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.Name :
                    s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.Name :
                     string.Empty,
                    StartDate =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? s.VoucherPercentageData.StartDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.StartDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.StartDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? s.VoucherType1Data.StartDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? s.VoucherType2CategoryData.StartDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? s.VoucherType2ProductData.StartDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? s.VoucherType3Data.StartDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? s.VoucherType4Data.StartDate.ToString("yyyy-MM-dd") : string.Empty,
                    EndDate =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? s.VoucherPercentageData.ExpiredDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.ExpiredDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.ExpiredDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? s.VoucherType1Data.ExpiredDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? s.VoucherType2CategoryData.ExpiredDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? s.VoucherType2ProductData.ExpiredDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? s.VoucherType3Data.ExpiredDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? s.VoucherType4Data.ExpiredDate.ToString("yyyy-MM-dd") : string.Empty,
                    TotalUse = s.UserVouchers,
                    Status =
                    (s.TypeEnum == Voucher.VoucherType.Percentage ? s.VoucherPercentageData.ExpiredDate.Date >= DateTime.Now.Date && s.VoucherPercentageData.StartDate.Date <= DateTime.Now.Date :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.ExpiredDate.Date >= DateTime.Now.Date && s.VoucherNominalData.StartDate.Date <= DateTime.Now.Date :
                    s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.ExpiredDate.Date >= DateTime.Now.Date && s.VoucherProductData.StartDate.Date <= DateTime.Now.Date :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? s.VoucherType1Data.ExpiredDate.Date >= DateTime.Now.Date && s.VoucherType1Data.StartDate.Date <= DateTime.Now.Date :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? s.VoucherType2CategoryData.ExpiredDate.Date >= DateTime.Now.Date && s.VoucherType2CategoryData.StartDate.Date <= DateTime.Now.Date :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? s.VoucherType2ProductData.ExpiredDate.Date >= DateTime.Now.Date && s.VoucherType2ProductData.StartDate.Date <= DateTime.Now.Date :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? s.VoucherType3Data.ExpiredDate.Date >= DateTime.Now.Date && s.VoucherType3Data.StartDate.Date <= DateTime.Now.Date :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? s.VoucherType4Data.ExpiredDate.Date >= DateTime.Now.Date && s.VoucherType4Data.StartDate.Date <= DateTime.Now.Date : false)
                    ?
                    "Active" : "NotActive",
                    Description =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? s.VoucherPercentageData.Description :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.Description :
                    s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.Description :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? s.VoucherType1Data.Description :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? s.VoucherType2CategoryData.Description :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? s.VoucherType2ProductData.Description :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? s.VoucherType3Data.Description :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? s.VoucherType4Data.Description : string.Empty,
                    DiscountCode = s.Code,
                    ProductId =
                    s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.ProductId : string.Empty,
                    ExchangePoint =
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.ExchangePoint.ToString() :
                    s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.ExchangePoint.ToString() : "0"
                }).ToList();

            //var resultExchange = resultSimply.Where(s => s.ExchangePoint == "0");
            if (resultSimply == null)
                return new List<VoucherSimplyViewModel>();
            return resultSimply.Where(s => s.Status == "Active").ToList();
        }

        public VoucherIndexViewModel ViewSummaryMembershipSearchIndex(DateTime startDate, DateTime endDate, Voucher.VoucherType voucherType, string code, string name, int page, int limit, int membershipId)
        {
            var result = new List<VoucherSimplyViewModel>();
            var query = _context.Vouchers
                .AsQueryable();


            var queryNominal = _context.VoucherNominals.AsQueryable().Where(s => s.MembershipId != null && !string.IsNullOrEmpty(s.MembershipId) && s.MembershipId != "0");
            var queryProduct = _context.VoucherProducts.AsQueryable().Where(s => s.MembershipId != null && !string.IsNullOrEmpty(s.MembershipId) && s.MembershipId != "0");

            var userVoucher = _context.UserVouchers.AsQueryable();

            if (startDate != null && startDate.Year != 1)
            {
                queryNominal = queryNominal.Where(s => s.StartDate >= startDate || s.ExpiredDate >= startDate);
                queryProduct = queryProduct.Where(s => s.StartDate >= startDate || s.ExpiredDate >= startDate);
            }

            if (endDate != null && endDate.Year != 1)
            {
                queryNominal = queryNominal.Where(s => s.StartDate <= endDate || s.ExpiredDate <= endDate);
                queryProduct = queryProduct.Where(s => s.StartDate <= endDate || s.ExpiredDate <= endDate);
            }

            if (!string.IsNullOrEmpty(name))
            {
                queryNominal = queryNominal.Where(s => s.Name.ToLower().Contains(name.ToLower()));
                queryProduct = queryProduct.Where(s => s.Name.ToLower().Contains(name.ToLower()));
            }

            if (!string.IsNullOrEmpty(code))
            {
                query = query.Where(s => s.Code.ToLower().Contains(code.ToLower()));
            }

            var resultVM = from a in (from q in query
                                      join nominal1 in queryNominal on q.TypeId equals nominal1.Id into nominals
                                      from nominal in nominals.DefaultIfEmpty()
                                      join product1 in queryProduct on q.TypeId equals product1.Id into products
                                      from product in products.DefaultIfEmpty()

                                      select new VoucherInsertVM
                                      {
                                          Code = q.Code,
                                          Id = q.Id,
                                          Type = q.Type.ToDescription(),
                                          TypeId = (int)q.Type,
                                          TypeEnum = q.Type,
                                          VoucherNominalData = q.Type == Voucher.VoucherType.Nominal ? nominal : null,
                                          VoucherProductData = q.Type == Voucher.VoucherType.Product ? product : null,
                                          UserVouchers = userVoucher.Where(s => s.VoucherId == q.Id).Count(),
                                          UserVoucherRedeemed = userVoucher.Where(s => s.VoucherId == q.Id && s.IsRedeemed).Count()
                                      }).ToList()
                           where (a.VoucherNominalData != null || a.VoucherProductData != null)
                           select new VoucherInsertVM
                           {
                               Code = a.Code,
                               Id = a.Id,
                               Type = a.Type,
                               TypeId = a.TypeId,
                               TypeEnum = a.TypeEnum,
                               VoucherNominalData = a.VoucherNominalData,
                               VoucherProductData = a.VoucherProductData,
                               UserVouchers = a.UserVouchers
                           };

            //resultVM = resultVM.Where(s => s.VoucherNominalData != null || s.VoucherProductData != null);

            if (voucherType != Voucher.VoucherType.Undefined)
            {
                resultVM = resultVM.Where(s => s.TypeId == (int)voucherType);
            }
            //TODO : View Membership for Total Claim and total Used
            var resultSimply = resultVM
                .Select(s => new VoucherSimplyViewModel
                {
                    id = s.Id,
                    DiscountType = s.Type,
                    DiscountName =
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.Name :
                    s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.Name :
                     string.Empty,
                    StartDate =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? s.VoucherPercentageData.StartDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.StartDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.StartDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? s.VoucherType1Data.StartDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? s.VoucherType2CategoryData.StartDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? s.VoucherType2ProductData.StartDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? s.VoucherType3Data.StartDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? s.VoucherType4Data.StartDate.ToString("yyyy-MM-dd") : string.Empty,
                    EndDate =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? s.VoucherPercentageData.ExpiredDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.ExpiredDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.ExpiredDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? s.VoucherType1Data.ExpiredDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? s.VoucherType2CategoryData.ExpiredDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? s.VoucherType2ProductData.ExpiredDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? s.VoucherType3Data.ExpiredDate.ToString("yyyy-MM-dd") :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? s.VoucherType4Data.ExpiredDate.ToString("yyyy-MM-dd") : string.Empty,
                    TotalUse = s.UserVouchers,
                    TotalClaimed = s.UserVoucherRedeemed,
                    Status =
                    (s.TypeEnum == Voucher.VoucherType.Percentage ? s.VoucherPercentageData.ExpiredDate >= DateTime.Now :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.ExpiredDate >= DateTime.Now :
                    s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.ExpiredDate >= DateTime.Now :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? s.VoucherType1Data.ExpiredDate >= DateTime.Now :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? s.VoucherType2CategoryData.ExpiredDate >= DateTime.Now :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? s.VoucherType2ProductData.ExpiredDate >= DateTime.Now :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? s.VoucherType3Data.ExpiredDate >= DateTime.Now :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? s.VoucherType4Data.ExpiredDate >= DateTime.Now : false)
                    ||
                    (s.TypeEnum == Voucher.VoucherType.Percentage ? s.VoucherPercentageData.MaxQty <= s.UserVouchers :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.MaxQty <= s.UserVouchers :
                    s.TypeEnum == Voucher.VoucherType.Product ? false :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? s.VoucherType1Data.MaxQty <= s.UserVouchers :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? s.VoucherType2CategoryData.MaxQty <= s.UserVouchers :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? s.VoucherType2ProductData.MaxQty <= s.UserVouchers :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? s.VoucherType3Data.MaxQty <= s.UserVouchers :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? s.VoucherType4Data.MaxQty <= s.UserVouchers : false) ?
                    "NotActive" : "Active",
                    Description =
                    s.TypeEnum == Voucher.VoucherType.Percentage ? s.VoucherPercentageData.Description :
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.Description :
                    s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.Description :
                    s.TypeEnum == Voucher.VoucherType.Type1 ? s.VoucherType1Data.Description :
                    s.TypeEnum == Voucher.VoucherType.Type2Category ? s.VoucherType2CategoryData.Description :
                    s.TypeEnum == Voucher.VoucherType.Type2Product ? s.VoucherType2ProductData.Description :
                    s.TypeEnum == Voucher.VoucherType.Type3 ? s.VoucherType3Data.Description :
                    s.TypeEnum == Voucher.VoucherType.Type4 ? s.VoucherType4Data.Description : string.Empty,
                    DiscountCode = s.Code,
                    ProductId =
                    s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.ProductId : string.Empty,
                    ExchangePoint =
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.ExchangePoint.ToString() :
                    s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.ExchangePoint.ToString() : "0",
                    Membership =
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.MembershipId.ToString() :
                    s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.MembershipId.ToString() : "0",
                    Nominal =
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.Nominal.ToString() : "0",
                    ModifiedDate =
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.LastModifiedUtc :
                    s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.LastModifiedUtc : DateTime.MinValue,
                    CreatedDate =
                    s.TypeEnum == Voucher.VoucherType.Nominal ? s.VoucherNominalData.CreatedUtc :
                    s.TypeEnum == Voucher.VoucherType.Product ? s.VoucherProductData.CreatedUtc : DateTime.MinValue
                }).ToList();

            //var resultExchange = resultSimply.Where(s => s.ExchangePoint == "0");
            if (resultSimply == null)
                //return new List<VoucherSimplyViewModel>();
                return new VoucherIndexViewModel
                {
                    Data = new List<VoucherSimplyViewModel>(),
                    Page = page,
                    Total = 0
                };

            if (membershipId != 0)
                resultSimply = resultSimply.Where(s => s.Membership.Split(',').Contains(membershipId.ToString())).ToList();

            return new VoucherIndexViewModel
            {
                Data = resultSimply.OrderByDescending(x => x.ModifiedDate)
                .Skip((page - 1) * limit).Take(limit)
                .ToList(),
                Page = page,
                Total = resultSimply.Count
            };
        }

        public List<UserVoucher> GetExpiredRedeemedVoucher()
        {
            return _context.UserVouchers.Where(x => x.CreatedUtc.AddDays(1) < DateTime.UtcNow && x.IsRedeemed && !x.IsCheckout).ToList();
        }

        #region private method
        private decimal CalculatNominal(VoucherNominal voucher, List<Product> listProduct, List<ProductPurchaseViewModel> itemPurchase)
        {
            if (listProduct.Count() <= 0)
                return 0;

            var sumTotalProduct = listProduct.Join(itemPurchase, prod => prod.Id, purchase => purchase.Id, (prod, purchase) => new { prod.DiscountPrice, prod.NormalPrice, purchase.QtyPurchase }).Sum(s => s.DiscountPrice != 0 ? 0 : s.NormalPrice * s.QtyPurchase);
            //if (sumTotalProduct >= voucher.MinSubtotal)
            //    return voucher.Nominal;

            return sumTotalProduct;
        }
        private decimal CalculatPercentage(VoucherPercentage voucher, List<Product> listProduct, List<ProductPurchaseViewModel> itemPurchase)
        {
            if (listProduct.Count() <= 0)
                return 0;

            //var sumTotalProduct = listProduct.Sum(s => s.DiscountPrice != 0 ? 0 : s.NormalPrice);
            var sumTotalProduct = listProduct.Join(itemPurchase, prod => prod.Id, purchase => purchase.Id, (prod, purchase) => new { prod.DiscountPrice, prod.NormalPrice, purchase.QtyPurchase }).Sum(s => s.DiscountPrice != 0 ? 0 : s.NormalPrice * s.QtyPurchase);

            if (sumTotalProduct >= voucher.MinSubtotal)
            {
                var discountAll = sumTotalProduct * (voucher.Value / Convert.ToDecimal(100));
                return voucher.MaxDiscount >= discountAll ? discountAll : voucher.MaxDiscount;
            }

            return 0;
        }

        private ResponseUseVoucherViewModel CalculatProduct(VoucherProduct voucher, List<Product> listProduct, List<ProductPurchaseViewModel> itemPurchase,List<ProductGiftChooseViewModel> productGift, string urlProductIds)
        {

            //if (listProduct.Count() <= 0)
            //    return new ResponseUseVoucherViewModel
            //    {
            //        discountPotential = 0,
            //        freeProduct = new List<Product>()
            //    };

            //var freeProductIds = voucher.ProductId.Split(',').Select(s => TryParseInt(s)).ToList();
            var freeProductIds = productGift.Select(s=> s.ProductId).ToList();
            if (freeProductIds.Count > 0)
            {
                var freeProductInfo = GetInfoProducts(freeProductIds, urlProductIds);
                var freeProductInfoFirst = freeProductInfo.FirstOrDefault();
                freeProductInfoFirst.FreeQuantity = 1;
                return new ResponseUseVoucherViewModel
                {
                    //discountPotential = freeProductInfoFirst.DiscountPrice == 0 ? freeProductInfoFirst.NormalPrice : freeProductInfoFirst.DiscountPrice,
                    freeProduct = new List<Product> { freeProductInfoFirst }
                };
            }
            else
            {
                return new ResponseUseVoucherViewModel
                {
                    //discountPotential = freeProductInfoFirst.DiscountPrice == 0 ? freeProductInfoFirst.NormalPrice : freeProductInfoFirst.DiscountPrice,
                    freeProduct = new List<Product>()
                };
            }
            //var priceFreeProduct = freeProductInfoFirst.DiscountPrice == 0 ? freeProductInfoFirst.NormalPrice : freeProductInfoFirst.DiscountPrice;

            //var productAndQuantity = listProduct.Join(itemPurchase,
            //                    product => product.Id,
            //                    itemPrch => itemPrch.Id,
            //                    (product, itemPrch) => new
            //                    {
            //                        price = product.DiscountPrice == 0 ? product.NormalPrice : product.DiscountPrice,
            //                        qty = itemPrch.QtyPurchase
            //                    }
            //                  );

            //var sumPurchase = productAndQuantity.Sum(s => s.price * s.qty);

            //if (sumPurchase >= voucher.MinSubtotal)
            //return new ResponseUseVoucherViewModel
            //{
            //    //discountPotential = freeProductInfoFirst.DiscountPrice == 0 ? freeProductInfoFirst.NormalPrice : freeProductInfoFirst.DiscountPrice,
            //    freeProduct = new List<Product> { freeProductInfoFirst }
            //};
            //else
            //    return new ResponseUseVoucherViewModel
            //    {
            //        //discountPotential = 0,
            //        //freeProduct = new List<Product> { freeProductInfoFirst }
            //        freeProduct = new List<Product>()
            //    };
        }

        private ResponseUseVoucherViewModel CalculatType1(VoucherType1 voucher, List<Product> listProduct, List<ProductPurchaseViewModel> itemPurchase, string urlProductIds)
        {

            var voucherBuyProductIds = voucher.CSV_BuyProductIds.Split(',').Select(s => TryParseInt(s)).ToList();

            if (listProduct.Count() <= 0)
                return new ResponseUseVoucherViewModel
                {
                    discountPotential = 0,
                    freeProduct = new List<Product>()
                };

            var isProductRelated = itemPurchase.Where(s => voucherBuyProductIds.Contains(s.Id));
            if (isProductRelated.Count() <= 0)
                //throw new Exception("Product related not found in your Bag. Please check t&c!");
                return new ResponseUseVoucherViewModel
                {
                    discountPotential = 0,
                    freeProduct = new List<Product>()
                };

            if (itemPurchase.Where(s => voucherBuyProductIds.Contains(s.Id) && (s.QtyPurchase >= voucher.MinBuyQtyProduct)).Count() <= 0)
                //return new ResponseUseVoucherViewModel
                //{
                //    discountPotential = 0,
                //    freeProduct = null
                //};
                //throw new Exception("Quantity product is not fulfilled.Please check T & C");
                return new ResponseUseVoucherViewModel
                {
                    discountPotential = 0,
                    freeProduct = new List<Product>()
                };

            if (itemPurchase.Sum(s => s.QtyPurchase) >= voucher.MinBuyQtyProduct)
            {
                var freeProductIds = voucher.CSV_FreeProductId.Split(',').Select(s => TryParseInt(s)).ToList();
                var freeProductInfo = GetInfoProducts(freeProductIds, urlProductIds);
                var freeProductInfoFirst = freeProductInfo.FirstOrDefault();
                var priceFreeProduct = freeProductInfoFirst.DiscountPrice == 0 ? freeProductInfoFirst.NormalPrice : freeProductInfoFirst.DiscountPrice;

                var freeProductInsertQuantity = freeProductInfoFirst;
                freeProductInfoFirst.FreeQuantity = voucher.FreeQty;

                return new ResponseUseVoucherViewModel
                {
                    discountPotential = priceFreeProduct * voucher.FreeQty,
                    freeProduct = new List<Product> { freeProductInfoFirst }
                };
            }

            return new ResponseUseVoucherViewModel
            {
                discountPotential = 0,
                freeProduct = new List<Product>()
            };
        }

        private decimal CalculatType2Product(VoucherType2Product voucher, List<Product> listProduct, List<ProductPurchaseViewModel> itemPurchase, string urlProductIds)
        {
            var voucherBuyProductIds = voucher.CSV_BuyProductIds.Split(',').Select(s => TryParseInt(s)).ToList();
            var voucherMinBuyProduct = voucher.CSV_BuyProductIds.Split(',').Select(s => new { Id = TryParseInt(s), voucher.MinBuyQty }).ToList();


            if (listProduct.Count() <= 0)
                return 0;

            if (itemPurchase.Where(s => voucherBuyProductIds.Contains(s.Id) && s.QtyPurchase >= voucher.MinBuyQty).Count() <= 0)
                return 0;
            //throw new Exception("Quantity product is not fulfilled.Please check T & C");

            var buyProductIds = itemPurchase.Select(s => s.Id).ToList();
            var buyProductInfo = GetInfoProducts(buyProductIds, urlProductIds);

            var buyProductWillDiscount = buyProductInfo.Where(s => s.DiscountPrice == 0).Join(itemPurchase,
                                        prodInfo => prodInfo.Id,
                                        purchase => purchase.Id,
                                        (prodInfo, purchase) =>
                                        new
                                        {
                                            prodId = prodInfo.Id,
                                            discountPotential = purchase.QtyPurchase >= voucher.MinBuyQty ?
                                                                ((prodInfo.DiscountPrice == 0 ? prodInfo.NormalPrice : prodInfo.DiscountPrice) * purchase.QtyPurchase) * (voucher.PercentageDiscountValue / 100) : 0
                                        });

            var sumDiscount = buyProductWillDiscount.Sum(s => s.discountPotential);
            return sumDiscount;
        }

        private decimal CalculatType2Category(VoucherType2Category voucher, List<Product> listProduct, List<ProductPurchaseViewModel> itemPurchase, string urlProductIds)
        {
            var voucherBuyProductIds = voucher.CSV_BuyCategoryIds.Split(',').Select(s => TryParseInt(s)).ToList();
            var voucherMinBuyProduct = voucher.CSV_BuyCategoryIds.Split(',').Select(s => new { Id = TryParseInt(s), voucher.MinBuyQty }).ToList();

            if (listProduct.Count() <= 0)
                return 0;

            var buyProductIds = itemPurchase.Select(s => s.Id).ToList();
            var buyProductInfo = GetInfoProducts(buyProductIds, urlProductIds);

            var validationResult = buyProductInfo.Join(itemPurchase,
                                        prodInfo => prodInfo.Id,
                                        purchase => purchase.Id,
                                        (prodInfo, purchase) =>
                                        new
                                        {
                                            Id = prodInfo.Id,
                                            QtyPurchase = purchase.QtyPurchase,
                                            PotentionalDiscount = purchase.QtyPurchase * (prodInfo.NormalPrice * (voucher.PercentageDiscountValue / 100)),
                                            Validation = prodInfo.ProductCategories.Any(x => voucherBuyProductIds.Contains(x.CategoryId.GetValueOrDefault())) && prodInfo.DiscountPrice == 0
                                        });

            var qtyItemValid = validationResult.Where(x => x.Validation).Sum(x => x.QtyPurchase);

            if (qtyItemValid < voucher.MinBuyQty)
                return 0;

            //var buyProductWillDiscount = buyProductInfo.Where(s => s.DiscountPrice == 0).Join(itemPurchase,
            //                            prodInfo => prodInfo.Id,
            //                            purchase => purchase.Id,
            //                            (prodInfo, purchase) =>
            //                            new
            //                            {
            //                                prodId = prodInfo.Id,
            //                                discountPotential = purchase.QtyPurchase >= voucher.MinBuyQty ?
            //                                                    (prodInfo.DiscountPrice == 0 ? prodInfo.NormalPrice : prodInfo.DiscountPrice) * (voucher.PercentageDiscountValue / 100) : 0
            //                            });

            //var sumDiscount = buyProductWillDiscount.Sum(s => s.discountPotential);
            //return sumDiscount;

            return validationResult.Where(x => x.Validation).Sum(x => x.PotentionalDiscount);
        }

        private decimal CalculatType3(VoucherType3 voucher, List<Product> listProduct, List<ProductPurchaseViewModel> itemPurchase, string urlProductIds)
        {

            if (listProduct.Count() <= 0)
                return 0;

            if (itemPurchase.Where(s => s.Id == voucher.BuyProductId && s.QtyPurchase >= voucher.DiscountProductIndex).Count() <= 0)
                return 0;
            //throw new Exception("Quantity product is not fulfilled.Please check T & C");

            var buyProductIds = itemPurchase.Select(s => s.Id).ToList();
            var buyProductInfo = GetInfoProducts(buyProductIds, urlProductIds);

            var buyProductWillDiscount = buyProductInfo.Where(s => s.DiscountPrice == 0).FirstOrDefault(s => s.Id == voucher.BuyProductId);
            if (buyProductWillDiscount == null)
                return 0;

            var sumDiscount = (buyProductWillDiscount.DiscountPrice == 0 ? buyProductWillDiscount.NormalPrice : buyProductWillDiscount.DiscountPrice) * (voucher.PercentageDiscountValue / 100);

            return sumDiscount;
        }

        private ResponseUseVoucherViewModel CalculatType4(VoucherType4 voucher, List<Product> listProduct, List<ProductPurchaseViewModel> itemPurchase, string urlProductIds, int counter)
        {

            if (listProduct.Count() <= 0)
                return new ResponseUseVoucherViewModel
                {
                    discountPotential = 0,
                    freeProduct = new List<Product>()
                };

            var freeProductInfo = GetInfoProducts(voucher.FreeProductId.Split(',').Select(s => TryParseInt(s)).ToList(), urlProductIds);
            var freeProductInfoFirst = freeProductInfo.FirstOrDefault();
            freeProductInfoFirst.FreeQuantity = 1; // hardcode 1 quantity
            //var priceFreeProduct = freeProductInfoFirst.DiscountPrice == 0 ? freeProductInfoFirst.NormalPrice : freeProductInfoFirst.DiscountPrice;

            var productAndQuantity = listProduct.Join(itemPurchase,
                                product => product.Id,
                                itemPrch => itemPrch.Id,
                                (product, itemPrch) => new
                                {
                                    price = product.DiscountPrice == 0 ? product.NormalPrice : 0,
                                    qty = itemPrch.QtyPurchase
                                }
                              );

            var sumPurchase = productAndQuantity.Sum(s => s.price * s.qty);
            counter = counter > 0 ? ++counter : 1;

            if (sumPurchase >= voucher.MinOrderValue * counter)
                return new ResponseUseVoucherViewModel
                {
                    discountPotential = freeProductInfoFirst.DiscountPrice == 0 ? freeProductInfoFirst.NormalPrice : 0,
                    freeProduct = new List<Product> { freeProductInfoFirst }
                };
            else
                return new ResponseUseVoucherViewModel
                {
                    discountPotential = 0,
                    freeProduct = new List<Product>()
                };
            //throw new Exception("Quantity product is not fulfilled.Please check T & C");

        }

        private Dictionary<int, bool> CheckVoucherAvailPerUser(List<Voucher> voucherList, ResponseUserMe users)
        {
            var voucherIds = voucherList.Select(s => s.Id);
            var userVouchers = _context.UserVouchers.Where(s => s.UserId == users.UserIds && voucherIds.Contains(s.Voucher.Id)).ToList();
            if (userVouchers.Count() <= 0)
                return voucherList.Select(s => new { s.Id, s.Code }).ToDictionary(s => s.Id, s => true);

            var userVoucherGroupVoucherId = userVouchers.GroupBy(
                key => key.VoucherId,
                value => value,
                (key, value) => new
                {
                    Id = key,
                    VouchersGroup = value
                }
                );

            var checkUseableUsers = userVoucherGroupVoucherId.Select(s => new
            {
                VoucherId = s.VouchersGroup.FirstOrDefault().Voucher.Id,
                Code = s.VouchersGroup.FirstOrDefault().Voucher.Code,
                MaxUserUsage =
                    s.VouchersGroup.FirstOrDefault().Voucher.Type == Voucher.VoucherType.Nominal ? _context.VoucherNominals.FirstOrDefault(t => t.Id == s.VouchersGroup.FirstOrDefault().Voucher.TypeId).MaxUsage :
                    s.VouchersGroup.FirstOrDefault().Voucher.Type == Voucher.VoucherType.Percentage ? _context.VoucherPercentages.FirstOrDefault(t => t.Id == s.VouchersGroup.FirstOrDefault().Voucher.TypeId).MaxUsage :
                    s.VouchersGroup.FirstOrDefault().Voucher.Type == Voucher.VoucherType.Type1 ? _context.VoucherType1s.FirstOrDefault(t => t.Id == s.VouchersGroup.FirstOrDefault().Voucher.TypeId).MaxUsage :
                    s.VouchersGroup.FirstOrDefault().Voucher.Type == Voucher.VoucherType.Type2Category ? _context.VoucherType2Categories.FirstOrDefault(t => t.Id == s.VouchersGroup.FirstOrDefault().Voucher.TypeId).MaxUsage :
                    s.VouchersGroup.FirstOrDefault().Voucher.Type == Voucher.VoucherType.Type2Product ? _context.VoucherType2Products.FirstOrDefault(t => t.Id == s.VouchersGroup.FirstOrDefault().Voucher.TypeId).MaxUsage :
                    s.VouchersGroup.FirstOrDefault().Voucher.Type == Voucher.VoucherType.Type3 ? _context.VoucherType3s.FirstOrDefault(t => t.Id == s.VouchersGroup.FirstOrDefault().Voucher.TypeId).MaxUsage :
                    s.VouchersGroup.FirstOrDefault().Voucher.Type == Voucher.VoucherType.Type4 ? _context.VoucherType4s.FirstOrDefault(t => t.Id == s.VouchersGroup.FirstOrDefault().Voucher.TypeId).MaxUsage :
                    s.VouchersGroup.FirstOrDefault().Voucher.Type == Voucher.VoucherType.Product ? s.VouchersGroup.Count() + 1 : ///product always true
                    0,
                TotalUseUser = s.VouchersGroup.Count(),
                IsMembership =
                    s.VouchersGroup.FirstOrDefault().Voucher.Type == Voucher.VoucherType.Nominal ? _context.VoucherNominals.FirstOrDefault(t => t.Id == s.VouchersGroup.FirstOrDefault().Voucher.TypeId).MembershipId :
                    s.VouchersGroup.FirstOrDefault().Voucher.Type == Voucher.VoucherType.Percentage ? string.Empty :
                    s.VouchersGroup.FirstOrDefault().Voucher.Type == Voucher.VoucherType.Type1 ? string.Empty :
                    s.VouchersGroup.FirstOrDefault().Voucher.Type == Voucher.VoucherType.Type2Category ? string.Empty :
                    s.VouchersGroup.FirstOrDefault().Voucher.Type == Voucher.VoucherType.Type2Product ? string.Empty :
                    s.VouchersGroup.FirstOrDefault().Voucher.Type == Voucher.VoucherType.Type3 ? string.Empty :
                    s.VouchersGroup.FirstOrDefault().Voucher.Type == Voucher.VoucherType.Type4 ? string.Empty :
                    s.VouchersGroup.FirstOrDefault().Voucher.Type == Voucher.VoucherType.Product ? _context.VoucherProducts.FirstOrDefault(t => t.Id == s.VouchersGroup.FirstOrDefault().Voucher.TypeId).MembershipId :
                    string.Empty,
            });

            var resultVoucherAvail = checkUseableUsers.Select(s => new
            {
                voucherId = s.VoucherId,
                IsAvail = s.IsMembership == string.Empty || s.IsMembership == null ? s.MaxUserUsage > s.TotalUseUser :
                            _context.UserVouchers.Where(t => t.UserId == users.UserIds && t.VoucherId == s.VoucherId && !t.IsRedeemed).Count() > 0
            }
            ).ToDictionary(s => s.voucherId, s => s.IsAvail);

            return resultVoucherAvail;
        }

        private List<VoucherValidationVM> CheckStockVoucher(List<Voucher> voucherList)
        {
            var voucherListIds = voucherList.Select(s => s.Id);
            var voucherAvails = _context.UserVouchers.Where(s => voucherListIds.Contains(s.VoucherId)).ToList();
            if (voucherAvails.Count() <= 0)
                return voucherList.Select(s => new VoucherValidationVM { VoucherId = s.Id, Code = s.Code, Validation = true }).ToList();
            //return voucherList.Select(s => new { s.Id, s.Code }).ToDictionary(s => s.Code == null ? s.Id.ToString() : s.Code, s => true);

            var voucherGroupAvails = voucherAvails.GroupBy(
                key => key.VoucherId,
                value => value,
                (key, value) => new
                {
                    Id = key,
                    VouchersGroup = value
                }
                );

            var checkUseableUsers = voucherGroupAvails.Select(s => new
            {
                Id = s.VouchersGroup.FirstOrDefault().Voucher.Id,
                Code = s.VouchersGroup.FirstOrDefault().Voucher.Code,
                MaxUserUsage =
                    s.VouchersGroup.FirstOrDefault().Voucher.Type == Voucher.VoucherType.Nominal ? _context.VoucherNominals.FirstOrDefault(t => t.Id == s.VouchersGroup.FirstOrDefault().Voucher.TypeId).MaxQty :
                    s.VouchersGroup.FirstOrDefault().Voucher.Type == Voucher.VoucherType.Percentage ? _context.VoucherPercentages.FirstOrDefault(t => t.Id == s.VouchersGroup.FirstOrDefault().Voucher.TypeId).MaxQty :
                    s.VouchersGroup.FirstOrDefault().Voucher.Type == Voucher.VoucherType.Type1 ? _context.VoucherType1s.FirstOrDefault(t => t.Id == s.VouchersGroup.FirstOrDefault().Voucher.TypeId).MaxQty :
                    s.VouchersGroup.FirstOrDefault().Voucher.Type == Voucher.VoucherType.Type2Category ? _context.VoucherType2Categories.FirstOrDefault(t => t.Id == s.VouchersGroup.FirstOrDefault().Voucher.TypeId).MaxQty :
                    s.VouchersGroup.FirstOrDefault().Voucher.Type == Voucher.VoucherType.Type2Product ? _context.VoucherType2Products.FirstOrDefault(t => t.Id == s.VouchersGroup.FirstOrDefault().Voucher.TypeId).MaxQty :
                    s.VouchersGroup.FirstOrDefault().Voucher.Type == Voucher.VoucherType.Type3 ? _context.VoucherType3s.FirstOrDefault(t => t.Id == s.VouchersGroup.FirstOrDefault().Voucher.TypeId).MaxQty :
                    s.VouchersGroup.FirstOrDefault().Voucher.Type == Voucher.VoucherType.Type4 ? _context.VoucherType4s.FirstOrDefault(t => t.Id == s.VouchersGroup.FirstOrDefault().Voucher.TypeId).MaxQty :
                    s.VouchersGroup.FirstOrDefault().Voucher.Type == Voucher.VoucherType.Product ? s.VouchersGroup.Count() + 1 : ///product always true
                    0,
                TotalUseUser = s.VouchersGroup.Count(),
                IsMembership =
                    s.VouchersGroup.FirstOrDefault().Voucher.Type == Voucher.VoucherType.Nominal ? _context.VoucherNominals.FirstOrDefault(t => t.Id == s.VouchersGroup.FirstOrDefault().Voucher.TypeId).MembershipId :
                    s.VouchersGroup.FirstOrDefault().Voucher.Type == Voucher.VoucherType.Percentage ? string.Empty :
                    s.VouchersGroup.FirstOrDefault().Voucher.Type == Voucher.VoucherType.Type1 ? string.Empty :
                    s.VouchersGroup.FirstOrDefault().Voucher.Type == Voucher.VoucherType.Type2Category ? string.Empty :
                    s.VouchersGroup.FirstOrDefault().Voucher.Type == Voucher.VoucherType.Type2Product ? string.Empty :
                    s.VouchersGroup.FirstOrDefault().Voucher.Type == Voucher.VoucherType.Type3 ? string.Empty :
                    s.VouchersGroup.FirstOrDefault().Voucher.Type == Voucher.VoucherType.Type4 ? string.Empty :
                    s.VouchersGroup.FirstOrDefault().Voucher.Type == Voucher.VoucherType.Product ? _context.VoucherProducts.FirstOrDefault(t => t.Id == s.VouchersGroup.FirstOrDefault().Voucher.TypeId).MembershipId :
                    string.Empty,
            });

            var resultVoucherAvail = checkUseableUsers.Select(s => new VoucherValidationVM
            {
                VoucherId = s.Id,
                Code = s.Code,
                Validation = s.IsMembership == string.Empty || s.IsMembership == null ? s.MaxUserUsage > s.TotalUseUser : true
            }
            ).ToList();

            return resultVoucherAvail;
        }

        private VoucherValidationVM CheckVoucherExpired(Voucher voucher, DateTime dateNow, ResponseUserMe user)
        {
            var s = new
            {
                Id = voucher.Id,
                TypeId = voucher.TypeId,
                VoucherType = voucher.Type,
                Code = voucher.Code,
                StartDate =
                    voucher.Type == Voucher.VoucherType.Nominal ? _context.VoucherNominals.FirstOrDefault(t => t.Id == voucher.TypeId).StartDate :
                    voucher.Type == Voucher.VoucherType.Percentage ? _context.VoucherPercentages.FirstOrDefault(t => t.Id == voucher.TypeId).StartDate :
                    voucher.Type == Voucher.VoucherType.Type1 ? _context.VoucherType1s.FirstOrDefault(t => t.Id == voucher.TypeId).StartDate :
                    voucher.Type == Voucher.VoucherType.Type2Category ? _context.VoucherType2Categories.FirstOrDefault(t => t.Id == voucher.TypeId).StartDate :
                    voucher.Type == Voucher.VoucherType.Type2Product ? _context.VoucherType2Products.FirstOrDefault(t => t.Id == voucher.TypeId).StartDate :
                    voucher.Type == Voucher.VoucherType.Type3 ? _context.VoucherType3s.FirstOrDefault(t => t.Id == voucher.TypeId).StartDate :
                    voucher.Type == Voucher.VoucherType.Type4 ? _context.VoucherType4s.FirstOrDefault(t => t.Id == voucher.TypeId).StartDate :
                    voucher.Type == Voucher.VoucherType.Product ? _context.VoucherProducts.FirstOrDefault(t => t.Id == voucher.TypeId).StartDate :
                    DateTime.Now,
                ExpiredDate =
                    voucher.Type == Voucher.VoucherType.Nominal ? _context.VoucherNominals.FirstOrDefault(t => t.Id == voucher.TypeId).ExpiredDate :
                    voucher.Type == Voucher.VoucherType.Percentage ? _context.VoucherPercentages.FirstOrDefault(t => t.Id == voucher.TypeId).ExpiredDate :
                    voucher.Type == Voucher.VoucherType.Type1 ? _context.VoucherType1s.FirstOrDefault(t => t.Id == voucher.TypeId).ExpiredDate :
                    voucher.Type == Voucher.VoucherType.Type2Category ? _context.VoucherType2Categories.FirstOrDefault(t => t.Id == voucher.TypeId).ExpiredDate :
                    voucher.Type == Voucher.VoucherType.Type2Product ? _context.VoucherType2Products.FirstOrDefault(t => t.Id == voucher.TypeId).ExpiredDate :
                    voucher.Type == Voucher.VoucherType.Type3 ? _context.VoucherType3s.FirstOrDefault(t => t.Id == voucher.TypeId).ExpiredDate :
                    voucher.Type == Voucher.VoucherType.Type4 ? _context.VoucherType4s.FirstOrDefault(t => t.Id == voucher.TypeId).ExpiredDate :
                    voucher.Type == Voucher.VoucherType.Product ? _context.VoucherProducts.FirstOrDefault(t => t.Id == voucher.TypeId).ExpiredDate :
                    DateTime.Now,
                IsMembership =
                    voucher.Type == Voucher.VoucherType.Nominal ? !string.IsNullOrEmpty(_context.VoucherNominals.FirstOrDefault(t => t.Id == voucher.TypeId).MembershipId) :
                    voucher.Type == Voucher.VoucherType.Product ? !string.IsNullOrEmpty(_context.VoucherProducts.FirstOrDefault(t => t.Id == voucher.TypeId).MembershipId) : false
            };

            var validityPeriodMembership = s.IsMembership && s.VoucherType == Voucher.VoucherType.Nominal ? _context.VoucherNominals.FirstOrDefault(t => t.Id == s.TypeId).ValidityPeriod :
                                           s.IsMembership && s.VoucherType == Voucher.VoucherType.Product ? _context.VoucherProducts.FirstOrDefault(t => t.Id == s.TypeId).ValidityPeriod : 0;

            var startDate = s.IsMembership ? _context.UserVouchers.FirstOrDefault(t => t.UserId == user.UserIds && t.VoucherId == s.Id && !t.IsRedeemed).CreatedUtc.Date
                            : s.StartDate.Date;
            var endDate = s.IsMembership ? _context.UserVouchers.FirstOrDefault(t => t.UserId == user.UserIds && t.VoucherId == s.Id && !t.IsRedeemed).CreatedUtc.AddDays(30 * validityPeriodMembership).Date
                          : s.ExpiredDate.Date;

            var resultVoucherAvail = new VoucherValidationVM
            {
                VoucherId = s.Id,
                Code = s.Code,
                Validation = startDate <= dateNow && endDate >= dateNow
                //!s.IsMembership ? (s.StartDate <= dateNow && s.ExpiredDate >= dateNow) : 
                //_context.UserVouchers.Where(t => t.StartDate.Date <= dateNow && t.EndDate.Date >= dateNow && t.UserId == user.UserIds && t.VoucherId == s.Id && !t.IsRedeemed).Count() > 0
            };

            return resultVoucherAvail;
        }

        private List<VoucherValidationVM> CheckDuplicateVoucher(List<Voucher> voucherList, UseVoucherViewModel voucherListVM)
        {
            var voucherDataByType = voucherList.Select(s => new
            {
                Code = s.Code == null ? s.Id.ToString() : s.Code,
                VoucherId = s.Id == null ? 0 : s.Id,
                VoucherType = s.Type,
                VoucherNominalData =
                    s.Type == Voucher.VoucherType.Nominal ? _context.VoucherNominals.FirstOrDefault(t => t.Id == s.TypeId) : null,
                VoucherPercentageData =
                    s.Type == Voucher.VoucherType.Percentage ? _context.VoucherPercentages.FirstOrDefault(t => t.Id == s.TypeId) : null,
                VoucherProductData =
                    s.Type == Voucher.VoucherType.Product ? _context.VoucherProducts.FirstOrDefault(t => t.Id == s.TypeId) : null,
                VoucherType1Data =
                    s.Type == Voucher.VoucherType.Type1 ? _context.VoucherType1s.FirstOrDefault(t => t.Id == s.TypeId) : null,
                VoucherType2CategoryData =
                    s.Type == Voucher.VoucherType.Type2Category ? _context.VoucherType2Categories.FirstOrDefault(t => t.Id == s.TypeId) : null,
                VoucherType2ProductData =
                    s.Type == Voucher.VoucherType.Type2Product ? _context.VoucherType2Products.FirstOrDefault(t => t.Id == s.TypeId) : null,
                VoucherType3Data =
                    s.Type == Voucher.VoucherType.Type3 ? _context.VoucherType3s.FirstOrDefault(t => t.Id == s.TypeId) : null,
                VoucherType4Data =
                    s.Type == Voucher.VoucherType.Type4 ? _context.VoucherType4s.FirstOrDefault(t => t.Id == s.TypeId) : null,
            }); ;

            var voucherDataGroupSameId = voucherDataByType.GroupBy(
                key => new { key.VoucherId, key.VoucherType },
                item => item,
                (key, item) => new { key, item });

            //var test = voucherDataGroupSameId.Select(s => s.item.Select(x => x.VoucherNominalData).Where(x => string.IsNullOrEmpty(x.MembershipId))).ToList();
            var resultDuplicateVoucher = new List<VoucherValidationVM>();

            foreach (var v in voucherListVM.Voucher)
            {
                var voucher = voucherDataGroupSameId.Where(x => x.key.VoucherId.Equals(v.VoucherId)).FirstOrDefault();

                resultDuplicateVoucher.Add(new VoucherValidationVM
                {
                    VoucherId = voucher.key.VoucherId,
                    VoucherType = voucher.key.VoucherType,
                    Validation = false,
                    IsMembership =
                    voucher.key.VoucherType == Voucher.VoucherType.Nominal ? voucher.item.Any(t => !string.IsNullOrEmpty(t.VoucherNominalData.MembershipId)) :
                    voucher.key.VoucherType == Voucher.VoucherType.Product ? true : false,
                    IsMultiply =
                    voucher.key.VoucherType == Voucher.VoucherType.Type4 ? voucher.item.Select(x => x.VoucherType4Data.CanMultiply).FirstOrDefault() :
                    voucher.key.VoucherType == Voucher.VoucherType.Product ? true : false
                }); ;
            }

            //var test = resultDuplicateVoucher.Where(s => s.VoucherType.Equals(Voucher.VoucherType.Type4) && !s.IsMultiply).Count();

            var result = resultDuplicateVoucher
                .Select(x => new VoucherValidationVM
                {
                    VoucherId = x.VoucherId,
                    VoucherType = x.VoucherType,
                    Validation =
                    x.VoucherType == Voucher.VoucherType.Nominal ? resultDuplicateVoucher.Where(s => x.IsMembership ? s.VoucherType.Equals(x.VoucherType) && !s.IsMembership : s.VoucherType.Equals(x.VoucherType)).Count() <= 1 :
                    //s.item.Where(t => string.IsNullOrEmpty(t.VoucherNominalData.MembershipId)).Count() <= 1 :
                    x.VoucherType == Voucher.VoucherType.Percentage ? resultDuplicateVoucher.Where(s => s.VoucherType.Equals(x.VoucherType)).Count() <= 1 :
                    x.VoucherType == Voucher.VoucherType.Product ? true :
                    x.VoucherType == Voucher.VoucherType.Type1 ? resultDuplicateVoucher.Where(s => s.VoucherType.Equals(x.VoucherType)).Count() <= 1 :
                    x.VoucherType == Voucher.VoucherType.Type2Category ? resultDuplicateVoucher.Where(s => s.VoucherType.Equals(x.VoucherType)).Count() <= 1 :
                    x.VoucherType == Voucher.VoucherType.Type2Product ? resultDuplicateVoucher.Where(s => s.VoucherType.Equals(x.VoucherType)).Count() <= 1 :
                    x.VoucherType == Voucher.VoucherType.Type3 ? resultDuplicateVoucher.Where(s => s.VoucherType.Equals(x.VoucherType)).Count() <= 1 :
                    x.VoucherType == Voucher.VoucherType.Type4 ? resultDuplicateVoucher.Where(s => s.VoucherType.Equals(x.VoucherType) && s.IsMultiply == false).Count() <= 1 : false,
                    IsMembership = x.IsMembership
                }).ToList();

            //var result = voucherDataGroupSameId
            //    .Select(s =>
            //    new VoucherValidationVM
            //    {
            //        VoucherId = Int32.Parse(s.key.VoucherId),
            //        VoucherType = s.key.VoucherType,
            //        Validation =
            //        s.key.VoucherType == Voucher.VoucherType.Nominal ? voucherDataGroupSameId.Select(s => s.item.Select(x => x.VoucherNominalData).Where(x => string.IsNullOrEmpty(x.MembershipId))).Count() <= 1 :
            //        //s.item.Where(t => string.IsNullOrEmpty(t.VoucherNominalData.MembershipId)).Count() <= 1 :
            //        s.key.VoucherType == Voucher.VoucherType.Percentage ? s.item.Count() <= 1 :
            //        s.key.VoucherType == Voucher.VoucherType.Product ? true :
            //        s.key.VoucherType == Voucher.VoucherType.Type1 ? s.item.Count() <= 1 :
            //        s.key.VoucherType == Voucher.VoucherType.Type2Category ? s.item.Count() <= 1 :
            //        s.key.VoucherType == Voucher.VoucherType.Type2Product ? s.item.Count() <= 1 :
            //        s.key.VoucherType == Voucher.VoucherType.Type3 ? s.item.Count() <= 1 :
            //        s.key.VoucherType == Voucher.VoucherType.Type4 ? s.item.Where(t => !t.VoucherType4Data.CanMultiply).Count() <= 1 : false,
            //        IsMembership =
            //        s.key.VoucherType == Voucher.VoucherType.Nominal ? s.item.Any(t => !string.IsNullOrEmpty(t.VoucherNominalData.MembershipId)) :
            //        s.key.VoucherType == Voucher.VoucherType.Product ? true : false
            //    }).ToList();
            //var resultAsDictionary = resultVoucherDouble.ToDictionary(s => s.VoucherId, s => s.CanCombination);

            var resultValidation = result.Select(x => new VoucherValidationVM
            {
                VoucherId = x.VoucherId,
                VoucherType = x.VoucherType,
                IsMembership = x.IsMembership,
                Validation =
                    x.VoucherType == Voucher.VoucherType.Nominal ? !result.Any(s => ((x.IsMembership ? s.VoucherType == x.VoucherType && !s.IsMembership : s.VoucherType == x.VoucherType && s.IsMembership) || s.VoucherType != x.VoucherType) && s.VoucherType != Voucher.VoucherType.Product && s.Validation == true) && x.Validation :
                    x.VoucherType == Voucher.VoucherType.Percentage ? !result.Any(s => s.VoucherType != x.VoucherType && s.VoucherType != Voucher.VoucherType.Product && s.Validation == true) && x.Validation :
                    x.VoucherType == Voucher.VoucherType.Product ? true :
                    x.VoucherType == Voucher.VoucherType.Type1 ? !result.Any(s => s.VoucherType != x.VoucherType && s.VoucherType != Voucher.VoucherType.Product && s.Validation == true) && x.Validation :
                    x.VoucherType == Voucher.VoucherType.Type2Category ? !result.Any(s => s.VoucherType != x.VoucherType && s.VoucherType != Voucher.VoucherType.Product && s.Validation == true) && x.Validation :
                    x.VoucherType == Voucher.VoucherType.Type2Product ? !result.Any(s => s.VoucherType != x.VoucherType && s.VoucherType != Voucher.VoucherType.Product && s.Validation == true) && x.Validation :
                    x.VoucherType == Voucher.VoucherType.Type3 ? !result.Any(s => s.VoucherType != x.VoucherType && s.VoucherType != Voucher.VoucherType.Product && s.Validation == true) && x.Validation :
                    x.VoucherType == Voucher.VoucherType.Type4 ? !result.Any(s => s.VoucherType != x.VoucherType && s.VoucherType != Voucher.VoucherType.Product && s.Validation == true) && x.Validation : false
                //Validation = !result.Any(s => (s.VoucherType != x.VoucherType || (s.VoucherType == x.VoucherType && x.IsMembership)) &&
                //                           s.VoucherType != Voucher.VoucherType.Product &&
                //                           s.Validation == true)
            }).ToList();

            return resultValidation;
        }

        private List<Product> GetInfoProducts(List<int> productIds, string urlProductIds)
        {
            var productUrl = urlProductIds;
            List<string> listString = new List<string>();
            foreach (var str in productIds)
            {
                listString.Add("productIds=" + str.ToString());
            }
            var querystring = string.Join("&", listString);
            productUrl = productUrl + querystring;
            var listProductInfo = _productService.GetProductByListId(productUrl, productIds);
            return listProductInfo;
        }

        private List<Category> GetInfoCategories(List<int> categoryIds, string urlCategoryIds)
        {
            var categoriesUrl = urlCategoryIds;
            List<string> listString = new List<string>();
            foreach (var str in categoryIds)
            {
                listString.Add("categoryIds=" + str.ToString());
            }
            var querystring = string.Join("&", listString);
            categoriesUrl = categoriesUrl + querystring;
            var listCategoriesInfo = _productService.GetCategoryByListId(categoriesUrl, categoryIds);
            return listCategoriesInfo;
        }

        private int TryParseInt(string textNumber)
        {
            int val;
            bool canConvert = int.TryParse(textNumber, out val);
            if (canConvert)
                return val;
            else
                return 0;
        }
        #endregion
    }
}
