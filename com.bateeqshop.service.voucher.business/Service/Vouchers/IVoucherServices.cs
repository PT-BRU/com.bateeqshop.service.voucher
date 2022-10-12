using com.bateeqshop.service.voucher.business.ViewModel;
using com.bateeqshop.service.voucher.business.ViewModel.Users;
using com.bateeqshop.service.voucher.business.ViewModel.Vouchers;
using com.bateeqshop.service.voucher.data.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace com.bateeqshop.service.voucher.business.Service.Vouchers
{ 
    public interface IVoucherServices : IService<VoucherVM>
    {
        int InsertModel(VoucherInsertPlainVM model);
        int InsertModelMembership(VoucherMembershipInsertPlainVM model);
        
        Task<int> UpdateModelMembership(VoucherMembershipInsertPlainVM model);

        public int UpdateModel(VoucherInsertPlainVM model);
        public ICollection<VoucherSimplyViewModel> ViewSummary();
        public ICollection<VoucherSimplyViewModel> ViewSummarySearch(DateTime startDate, DateTime endDate, Voucher.VoucherType voucherType, string code, string name);
        public ICollection<VoucherSimplyViewModel> ViewSummaryMembershipSearch(DateTime startDate, DateTime endDate, Voucher.VoucherType voucherType, string code, string name,int membershipId);
        public ICollection<VoucherSimplyViewModel> ViewSummaryMembershipActiveSearch(DateTime startDate, DateTime endDate, Voucher.VoucherType voucherType, string code, string name, int membershipId);
        public VoucherInsertPlainVM ViewById(int id);
        public VoucherViewByIdViewModel ViewByIdWithProductInfo(int id, string apiProductByIds,string apiCategoryByIds);
        List<Voucher> FindByIds(List<int> ids);
        List<VoucherInsertPlainVM> ViewByIds(List<int> ids);
        public Task<ResponseUseVoucherViewModel> UseVoucher(UseVoucherViewModel model,string urlProductIds,ResponseUserMe user,string urlAddGiftToCart);
        AddNewVoucherMembershipViewModel ViewByIdMembership(int id);
        AddNewVoucherMembershipWithProductInfoViewModel ViewByIdMembershipWithProductInfo(int id,string urlProductIds);
        VoucherIndexViewModel ViewSummaryMembershipSearchIndex(DateTime startDate, DateTime endDate, Voucher.VoucherType voucherType, string code, string name, int page, int limit, int membershipId);
        void DeleteUserVoucher(int id, ResponseUserMe user, string uri);
        void DeleteUserVoucher(int id, string uri);
        List<UserVoucher> GetExpiredRedeemedVoucher();
    }
}
