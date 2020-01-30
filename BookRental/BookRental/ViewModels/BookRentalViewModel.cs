using BookRental.Models;
using BookRental.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static BookRental.Models.BookRent;

namespace BookRental.ViewModels
{
    public class BookRentalViewModel
    {
        public int Id { get; set; }

        #region "BookDetails"

        public int BookId { get; set; }

        public string ISBN { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public string Description { get; set; }

        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }

        [Range(0, 1000)]
        public int Availability { get; set; }

        [DataType(DataType.Currency)]
        public double Price { get; set; }

        [DisplayName("Date Added")]
        [DisplayFormat(DataFormatString = "{0: dd MMM yyyy}")]
        public DateTime? DateAdded { get; set; }

        public int GenreId { get; set; }

        public Genre Genre { get; set; }

        [DisplayName("Publication Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0: dd MMM yyyy}")]
        public DateTime PublicationDate { get; set; }

        public int Pages { get; set; }

        [DisplayName("Product Dimensions")]
        public string ProductDimensions { get; set; }

        public string Publisher { get; set; }

        #endregion

        #region "Rental Details"

        [DisplayName("Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MMM/yyyy}")]
        public DateTime? StartDate { get; set; }

        [DisplayName("Actual End Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MMM/yyyy}")]
        public DateTime? ActualEndDate { get; set; }

        [DisplayName("Scheduled Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MMM/yyyy}")]
        public DateTime? ScheduledEndDate { get; set; }

        [DisplayName("Aditional Charge")]
        public double? AdditionalCharge { get; set; }

        [DisplayName("Rental Price")]
        public double RentalPrice { get; set; }

        public string RentalDuratioin { get; set; }

        public string Status { get; set; }

        public double RPOneMonth { get; set; }
        public double RPSixMonth { get; set; }
        #endregion

        #region "User Details"

        public string UserId { get; set; }

        [DisplayName("E-Mail")]
        [DataType(DataType.EmailAddress)]
        public string EMail { get; set; }

        [DisplayName("First Name")]
        public string FName { get; set; }

        [DisplayName("Last Name")]
        public string LName { get; set; }

        [DisplayName("Name")]
        public string Name { get { return FName + " " + LName; } }

        [DisplayName("Birth Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0: dd MMM yyy")]
        public DateTime BDay { get; set; }

        public string ActionName
        {
            get
            {
                if (Status.ToLower().Contains(SD.RequestedLower))
                {
                    return "Approve";
                }
                if (Status.ToLower().Contains(SD.ApprovedLower))
                {
                    return "PickUp";
                }
                if (Status.ToLower().Contains(SD.RentedLower))
                {
                    return "Return";
                }

                return null;
            }
        }

        #endregion

    }
}