using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookRental.Models
{
    public class BookRent
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int BookId { get; set; }



        public DateTime? StartDate { get; set; }

        public DateTime? ActualEndDate { get; set; }

        public DateTime? ScheduledEndDate { get; set; }

        public double? AdditionalCharge { get; set; }

        [Required]
        public double RentalPrice { get; set; }

        [Required]
        public string RentalDuratioin { get; set; }

        public StatusEnum Status { get; set; }

        public enum StatusEnum
        {
            Requested,
            Approved,
            Rejected,
            Rented,
            Closed,
        }

    }
}