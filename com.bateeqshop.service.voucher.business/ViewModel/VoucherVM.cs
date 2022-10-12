using com.bateeqshop.service.voucher.data.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace com.bateeqshop.service.voucher.business.ViewModel
{
    public class VoucherVM
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int TypeId { get; set; }
        public string Type { get; set; }
        public ICollection<int> VoucherProductCombinationIds { get; set; }

        public static VoucherVM MapFrom(Voucher voucher)
        {
            var voucherProductCombinationIds = new HashSet<int>();
            var strVoucherProductCombinationIds = voucher.CSV_VoucherProductCombinationIds.Split(',');
            foreach (var strVoucherProductId in strVoucherProductCombinationIds)
            {
                if (int.TryParse(strVoucherProductId, out int voucherProductId))
                    voucherProductCombinationIds.Add(voucherProductId);
                else
                    continue;
            }

            return new VoucherVM()
            {
                Id = voucher.Id,
                Code = voucher.Code,
                TypeId = voucher.TypeId,
                Type = voucher.Type.ToForm(),
                VoucherProductCombinationIds = voucherProductCombinationIds
            };
        }

        public static List<VoucherVM> MapFrom(List<Voucher> vouchers)
        {
            var result = new List<VoucherVM>();
            foreach (var voucher in vouchers)
            {
                result.Add(MapFrom(voucher));
            }

            return result;
        }
    }
}
