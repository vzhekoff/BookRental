using BookRental.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BookRental.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        private ICollection<MembershipType> _membershipTypes;

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Phone")]
        public string Phone { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Birth Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [DateRange("01/01/1900")]
        public DateTime BDay { get; set; }

        public bool? Disable { get; set; }

        public ICollection<MembershipType> MembershipTypes
        {
            get
            {
                if (null == _membershipTypes)
                {
                    using (var db = ApplicationDbContext.Create())
                    {
                        _membershipTypes = db.MembershipTypes.Where(m => !(m.Name.ToLower().Equals(Utility.SD.AdminUserRole.ToLower()))).ToList();
                    }
                }

                return _membershipTypes;
            }
            set => _membershipTypes = value;
        }

        [Required]
        public int MembershipTypeId { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public bool Disable { get; set; }

        private ICollection<MembershipType> membershipTypes;

        public ICollection<MembershipType> MembershipTypes
        {
            get
            {
                if (null == membershipTypes)
                {
                    using (var db = ApplicationDbContext.Create())
                    {
                        membershipTypes = new List<MembershipType>();
                        foreach (var mt in db.MembershipTypes)
                        {
                            membershipTypes.Add(new MembershipType
                            {
                                Id = mt.Id,
                                Name = mt.Name,
                                SignUpFee = mt.SignUpFee,
                                ChargeRateOneMonth = mt.ChargeRateOneMonth,
                                ChargeRateSixMonth = mt.ChargeRateSixMonth
                            });
                        }
                    }
                }

                return membershipTypes;
            }
            set
            {
                membershipTypes = value;
            }
        }

        [Required]
        public int MembershipTypeId { get; set; }

        [Required]
        [Display(Name ="First Name")]
        public string FName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LName { get; set; }

        [Required]
        [Display(Name = "Phone")]
        public string Phone { get; set; }

        [Required]
        [DateRange("01/01/1900")]
        [Display(Name = "Birth Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString ="{0: dd mm yyyy}")]
        public string BDay { get; set; }

    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
