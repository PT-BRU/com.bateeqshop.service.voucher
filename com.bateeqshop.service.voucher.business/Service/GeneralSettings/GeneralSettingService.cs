using com.bateeqshop.service.voucher.data;
using com.bateeqshop.service.voucher.data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.bateeqshop.service.voucher.business.Service.GeneralSettings
{
    public class GeneralSettingService : IGeneralSetting
    {
        private readonly VoucherDbContext _context;

        public GeneralSettingService(
            VoucherDbContext context
            )
        {
            _context = context;
        }
        public int Delete(int id)
        {
            var general = _context.GeneralSettings.Where(entity => entity.id == id).FirstOrDefault();
            if (general != null)
            {
                _context.Remove(general);
            }

            return _context.SaveChanges();
        }

        public List<GeneralSetting> GetAll()
        {
            var general = _context.GeneralSettings.ToList();

            return general;
        }

        public GeneralSetting GetById(int id)
        {
            var general = _context.GeneralSettings.Where(entity => entity.id == id).FirstOrDefault();
            
            return general;
        }

        public int Insert(GeneralSetting model)
        {
            var general = _context.GeneralSettings.Where(entity => entity.ConfigurationAttribute == model.ConfigurationAttribute).FirstOrDefault();
            if (general != null)
            {
                throw new ArgumentNullException("General Settings with that name is already define");
            }else
            {
                model.ConfigurationAttribute = "RewardPointExpired";
                model.Description = "Use to determine the validity period of points collected from purchases.";
                model.Value = model.Value;
                _context.Add(model);
            }

            return _context.SaveChanges();
        }

        public int Update(GeneralSetting model)
        {
            var general = _context.GeneralSettings.Where(entity => entity.id == model.id).FirstOrDefault();
            if(general == null)
            {
                throw new ArgumentNullException("General Settings is not found");
            }
            else
            {
                general.Value = model.Value;
                _context.GeneralSettings.Update(general);
            }

            return _context.SaveChanges();
        }
    }
}
