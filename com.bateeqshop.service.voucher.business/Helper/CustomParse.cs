using System;
using System.Collections.Generic;
using System.Text;

namespace com.bateeqshop.service.voucher.business.Helper
{
    public static class CustomParse
    {
        public static int TryParseInt(string textNumber)
        {
            int val;
            bool canConvert = int.TryParse(textNumber, out val);
            if (canConvert)
                return val;
            else
                return 0;
        }
    }
}
