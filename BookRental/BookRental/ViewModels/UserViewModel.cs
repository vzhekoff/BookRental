using BookRental.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookRental.ViewModels
{
    public class UserViewModel
    {
        /// <summary>
        /// Ctr
        /// </summary>
        /// <param name="user"></param>
        public UserViewModel(ApplicationUser user)
        {
            Id = user.Id;
            FName = user.FName;
            LName = user.LName;
            BDay = user.BDay;
            EMail = user.Email;
            Phone = user.Phone;
            MembershipTypeId = user.MembershipTypeId;
            Disable = user.Disable;
        }

        /// <summary>
        /// Ctr
        /// </summary>
        public UserViewModel()
        {

        }

        [Required]
        public string Id { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string EMail { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public ICollection<MembershipType> MembershipTypes { get; set; }

        [Required]
        public int MembershipTypeId { get; set; }

        [Required]
        public string FName { get; set; }

        [Required]
        public string LName { get; set; }

        [Required]
        public string Phone { get; set; }

        //[Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:dd MMM yyyy}")]
        public DateTime BDay { get; set; }

        public bool Disable { get; set; }

    }
}