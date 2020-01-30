using BookRental.Models;
using BookRental.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BookRental.Controllers.Api
{
    public class BookAPIController : ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        private ApplicationDbContext db;

        /// <summary>
        /// Ctr
        /// </summary>
        public BookAPIController()
        {
            db = new ApplicationDbContext();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult Get(string queryArg = null)
        {
            var bookQuery = db.Books.Where(b => b.Title.ToLower().Contains(queryArg.ToLower()));

            return Ok(bookQuery.ToList());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult Get(string typeArg, string isbnArg = null, string emailArg = null, string rentalArg = null)
        {
            var bookQuery = db.Books.Where(b => b.ISBN.Equals(isbnArg, StringComparison.InvariantCultureIgnoreCase)).SingleOrDefault();

            if (typeArg.Equals("price", StringComparison.InvariantCultureIgnoreCase) && (null != emailArg))
            {
                var chargeRate = from u in db.Users
                                 join m in db.MembershipTypes on u.MembershipTypeId equals m.Id
                                 where u.Email.Equals(emailArg, StringComparison.InvariantCultureIgnoreCase)
                                 select new { m.ChargeRateOneMonth, m.ChargeRateSixMonth };

                if (chargeRate.ToList().Count > 0)
                {
                    var priceOne = Convert.ToDouble(bookQuery.Price) * Convert.ToDouble(chargeRate.ToList()[0].ChargeRateOneMonth) / 100;
                    var priceSix = Convert.ToDouble(bookQuery.Price) * Convert.ToDouble(chargeRate.ToList()[0].ChargeRateSixMonth) / 100;

                    if (SD.SixMonthCount.Equals(rentalArg, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return Ok(priceSix);
                    }
                    else
                    {
                        return Ok(priceOne);
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return Ok(bookQuery.Availability);
            }
        }


    }
}
