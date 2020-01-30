using BookRental.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace BookRental.Controllers.Api
{
    public class UsersAPIController : ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        private ApplicationDbContext db;

        /// <summary>
        /// Ctr
        /// </summary>
        public UsersAPIController()
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
        /// <param name="typeArg"></param>
        /// <param name="queryArg"></param>
        /// <returns></returns>
        public IHttpActionResult Get(string typeArg, string queryArg = null)
        {
            if (typeArg.Equals("email", StringComparison.InvariantCultureIgnoreCase) && (null != queryArg))
            {
                var customerQuery = db.Users.Where(u => u.Email.ToLower().Contains(queryArg.ToLower()));

                return Ok(customerQuery.ToList());
            }

            if (typeArg.Equals("name", StringComparison.InvariantCultureIgnoreCase) && (null != queryArg))
            {
                var customerQuery = from u in db.Users
                                    where u.Email.ToLower().Contains(queryArg.ToLower())
                                    select new { u.FName, u.LName, u.BDay };

                var customerList = customerQuery.ToList();

                if (customerList.Count > 0)
                {
                    return Ok(customerList[0].FName + " " +
                              customerList[0].LName + ";" +
                              customerList[0].BDay);
                }

                return BadRequest();
            }

            return BadRequest();
        }
    }
}
