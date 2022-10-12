using System;
using System.Collections.Generic;
using System.Text;

namespace com.bateeqshop.service.voucher.business.ViewModel.UserInfo
{
    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int PhoneNumberCountry { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime DBO { get; set; }
        public string Gender { get; set; }
        public string ImageUrl { get; set; }
        public bool IsEmailVerified { get; set; }
        public bool IsPhoneNumberVerified { get; set; }
        public decimal TotalPoint { get; set; }
        public string OTP { get; set; }
        public bool IsFirstLogin { get; set; }
        public int UserMembershipId { get; set; }
        public bool IsBateeqExisting { get; set; }
        public DateTime OtpExpired { get; set; }
        //public virtual ICollection<UserMembership> UserMemberships { get; set; }
        //public virtual ICollection<UserExternalLogin> UserExternalLogins { get; set; }
        //public virtual ICollection<AddressBook> Addresses { get; set; }
    }
}
