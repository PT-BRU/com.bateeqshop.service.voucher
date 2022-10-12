using com.bateeqshop.service.voucher.data.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace com.bateeqshop.service.voucher.business.Service.GeneralSettings
{
    public interface IGeneralSetting
    {
        int Insert(GeneralSetting model);
        GeneralSetting GetById(int id);
        List<GeneralSetting> GetAll();
        int Update(GeneralSetting model);
        int Delete(int id);
    }
}
