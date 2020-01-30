using BookRental.Models;
using BookRental.Utility;
using BookRental.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Net;

namespace BookRental.Controllers
{
    [Authorize]
    public class BookRentController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        private ApplicationDbContext db;

        /// <summary>
        /// Ctr
        /// </summary>
        public BookRentController()
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
        /// <param name="titleArg"></param>
        /// <param name="isbnArg"></param>
        /// <returns></returns>
        public ActionResult Create(string titleArg = null, string isbnArg = null)
        {
            if ((null != titleArg) && (null != isbnArg))
            {
                BookRentalViewModel model = new BookRentalViewModel
                {
                    Title = titleArg,
                    ISBN = isbnArg
                };

                return View(model);
            }

            return View(new BookRentalViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookRentalViewModel modelArg)
        {
            if (ModelState.IsValid)
            {
                var email = modelArg.EMail;

                var userDetails = from u in db.Users
                                  where u.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase)
                                  select new { u.Id, u.FName, u.LName, u.BDay };

                var isbn = modelArg.ISBN;

                Book bookSelected = db.Books.Where(b => b.ISBN.Equals(isbn, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

                var rentalDuration = modelArg.RentalDuratioin;

                var chargeRate = from u in db.Users
                                 join m in db.MembershipTypes on u.MembershipTypeId equals m.Id
                                 where u.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase)
                                 select new { m.ChargeRateOneMonth, m.ChargeRateSixMonth };

                var priceOne = Convert.ToDouble(bookSelected.Price) * Convert.ToDouble(chargeRate.ToList()[0].ChargeRateOneMonth) / 100;
                var priceSix = Convert.ToDouble(bookSelected.Price) * Convert.ToDouble(chargeRate.ToList()[0].ChargeRateSixMonth) / 100;

                double rentalPr = 0;

                if (SD.SixMonthCount.Equals(modelArg.RentalDuratioin, StringComparison.InvariantCultureIgnoreCase))
                {
                    rentalPr = priceSix;
                }
                else
                {
                    rentalPr = priceOne;
                }

                BookRent model2DB = new BookRent
                {
                    BookId = bookSelected.Id,
                    RentalPrice = rentalPr,
                    ScheduledEndDate = modelArg.ScheduledEndDate,
                    RentalDuratioin = modelArg.RentalDuratioin,
                    Status = BookRent.StatusEnum.Requested,
                    UserId = userDetails.ToList()[0].Id
                };

                bookSelected.Availability--;
                db.BookRental.Add(model2DB);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View();
        }

        // GET: BookRent
        public ActionResult Index(int? pageNumber, string option = null, string search = null)
        {
            string userId = User.Identity.GetUserId();

            var model = from br in db.BookRental
                        join bk in db.Books on br.BookId equals bk.Id
                        join us in db.Users on br.UserId equals us.Id

                        select new BookRentalViewModel
                        {
                            BookId = bk.Id,
                            RentalPrice = br.RentalPrice,
                            Price = bk.Price,
                            Pages = bk.Pages,
                            FName = us.FName,
                            LName = us.LName,
                            BDay = us.BDay,
                            ScheduledEndDate = br.ScheduledEndDate,
                            Author = bk.Author,
                            Availability = bk.Availability,
                            DateAdded = bk.DateAdded,
                            Description = bk.Description,
                            EMail = us.Email,
                            GenreId = bk.GenreId,
                            Genre = db.Genres.Where(g => g.GenreId.Equals(bk.GenreId)).FirstOrDefault(),
                            ISBN = bk.ISBN,
                            ImageUrl = bk.ImageUrl,
                            ProductDimensions = bk.ProductDimensions,
                            PublicationDate = bk.PublicationDate,
                            Publisher = bk.Publisher,
                            RentalDuratioin = br.RentalDuratioin,
                            Status = br.Status.ToString(),
                            Title = bk.Title,
                            UserId = us.Id,
                            Id = br.Id,
                            StartDate = br.StartDate
                        };

            if ((null != option) && option.Equals("email", StringComparison.InvariantCultureIgnoreCase) &&
                (null != search) && (search.Length > 0))
            {
                model = model.Where(u=>u.EMail.Contains(search));
            }

            if ((null != option) && option.Equals("name", StringComparison.InvariantCultureIgnoreCase) &&
                (null != search) && (search.Length > 0))
            {
                model = model.Where(u => u.FName.Contains(search) || u.LName.Contains(search));
            }

            if ((null != option) && option.Equals("status", StringComparison.InvariantCultureIgnoreCase) &&
                (null != search) && (search.Length > 0))
            {
                model = model.Where(u => u.Status.Contains(search));
            }

            if (!User.IsInRole(SD.AdminUserRole))
            {
                model = model.Where(u => u.UserId.Equals(userId));
            }

            return View(model.ToList().ToPagedList(pageNumber?? 1, 2));
        }

        [HttpPost]
        public ActionResult Reserve(BookRentalViewModel book)
        {
            var userId = User.Identity.GetUserId();

            Book book2Rent = db.Books.Find(book.BookId);

            double rentalPr = 0;

            if (null != userId)
            {
                var chargeRate = from u in db.Users
                                 join m in db.MembershipTypes on u.MembershipTypeId equals m.Id
                                 where u.Id.Equals(userId, StringComparison.InvariantCultureIgnoreCase)
                                 select new { m.ChargeRateOneMonth, m.ChargeRateSixMonth };

                if (SD.SixMonthCount == book.RentalDuratioin)
                {
                    rentalPr = Convert.ToDouble(book2Rent.Price) * Convert.ToDouble(chargeRate.ToList()[0].ChargeRateSixMonth) / 100;
                }
                if (SD.OneMonthCount == book.RentalDuratioin)
                {
                    rentalPr = Convert.ToDouble(book2Rent.Price) * Convert.ToDouble(chargeRate.ToList()[0].ChargeRateOneMonth) / 100;
                }

                BookRent bookRent = new BookRent
                {
                    BookId = book2Rent.Id,
                    UserId = userId,
                    RentalDuratioin = book.RentalDuratioin,
                    RentalPrice = rentalPr,
                    Status = BookRent.StatusEnum.Requested
                };

                db.BookRental.Add(bookRent);

                var bookInDb = db.Books.SingleOrDefault(c => c.Id == book.BookId);

                bookInDb.Availability--;
                db.SaveChanges();

                return RedirectToAction("Index", "BookRent");
            }


            return View();
        }

        public ActionResult Details(int? id)
        {
            if (null == id)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BookRent bookRent = db.BookRental.Find(id);

            if (null == bookRent)
            {
                return new HttpNotFoundResult();
            }

            var model = getVMfromRent(bookRent);

            if (null == model)
            {
                return new HttpNotFoundResult();
            }

            return View(model);
        }

        public ActionResult Decline(int? id)
        {
            if (null == id)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BookRent bookRent = db.BookRental.Find(id);

            if (null == bookRent)
            {
                return new HttpNotFoundResult();
            }

            var model = getVMfromRent(bookRent);

            if (null == model)
            {
                return new HttpNotFoundResult();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Decline(BookRentalViewModel model)
        {
            if (0 == model.Id)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BookRent bookRent = db.BookRental.Find(model.Id);

            if (null == bookRent)
            {
                return new HttpNotFoundResult();
            }

            bookRent.Status = BookRent.StatusEnum.Rejected;

            Book book = db.Books.Find(bookRent.BookId);

            if (null == book)
            {
                return new HttpNotFoundResult();
            }

            book.Availability++;

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Approve(int? id)
        {
            if (null == id)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BookRent bookRent = db.BookRental.Find(id);

            if (null == bookRent)
            {
                return new HttpNotFoundResult();
            }

            var model = getVMfromRent(bookRent);

            if (null == model)
            {
                return new HttpNotFoundResult();
            }

            return View("Approve", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Approve(BookRentalViewModel model)
        {
            if (0 == model.Id)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BookRent bookRent = db.BookRental.Find(model.Id);

            if (null == bookRent)
            {
                return new HttpNotFoundResult();
            }

            bookRent.Status = BookRent.StatusEnum.Approved;

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult PickUp(int? id)
        {
            if (null == id)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BookRent bookRent = db.BookRental.Find(id);

            if (null == bookRent)
            {
                return new HttpNotFoundResult();
            }

            var model = getVMfromRent(bookRent);

            if (null == model)
            {
                return new HttpNotFoundResult();
            }

            return View("Approve", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PickUp(BookRentalViewModel model)
        {
            if (0 == model.Id)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BookRent bookRent = db.BookRental.Find(model.Id);

            if (null == bookRent)
            {
                return new HttpNotFoundResult();
            }

            bookRent.Status = BookRent.StatusEnum.Rented;
            bookRent.StartDate = DateTime.Now;

            if (bookRent.RentalDuratioin.Equals(SD.SixMonthCount))
            {
                bookRent.ScheduledEndDate = bookRent.StartDate.Value.AddMonths(Convert.ToInt32(SD.SixMonthCount));
            }
            else if (bookRent.RentalDuratioin.Equals(SD.OneMonthCount))
            {
                bookRent.ScheduledEndDate = bookRent.StartDate.Value.AddMonths(Convert.ToInt32(SD.OneMonthCount));
            }

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Return(int? id)
        {
            if (null == id)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BookRent bookRent = db.BookRental.Find(id);

            if (null == bookRent)
            {
                return new HttpNotFoundResult();
            }

            var model = getVMfromRent(bookRent);

            if (null == model)
            {
                return new HttpNotFoundResult();
            }

            return View("Approve", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Return(BookRentalViewModel model)
        {
            if (0 == model.Id)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BookRent bookRent = db.BookRental.Find(model.Id);

            if (null == bookRent)
            {
                return new HttpNotFoundResult();
            }

            bookRent.Status = BookRent.StatusEnum.Closed;
            bookRent.ActualEndDate = DateTime.Now;
            bookRent.AdditionalCharge = model.AdditionalCharge;

            Book book = db.Books.Find(bookRent.BookId);

            if (null == book)
            {
                return new HttpNotFoundResult();
            }

            book.Availability++;

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int? id)
        {
            if (null == id)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BookRent bookRent = db.BookRental.Find(id);

            if (null == bookRent)
            {
                return new HttpNotFoundResult();
            }

            var model = getVMfromRent(bookRent);

            if (null == model)
            {
                return new HttpNotFoundResult();
            }

            return View(model);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirm(int? id)
        {
            if (0 == id)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BookRent bookRent = db.BookRental.Find(id);

            if (null == bookRent)
            {
                return new HttpNotFoundResult();
            }

            Book book = db.Books.Where(b=>b.Id.Equals(bookRent.BookId)).FirstOrDefault();

            if (null == book)
            {
                return new HttpNotFoundResult();
            }

            if (!bookRent.Status.Equals("Rented"))
            {
                book.Availability++;
            }

            db.BookRental.Remove(bookRent);
            db.SaveChanges();

            return RedirectToAction("Index");
        }


        private BookRentalViewModel getVMfromRent(BookRent brArg)
        {
            Book bookSelected = db.Books.Where(b => b.Id == brArg.BookId).FirstOrDefault();

            var userDetails = from u in db.Users
                              where u.Id.Equals(brArg.UserId)
                              select new { u.Id, u.FName, u.LName, u.BDay, u.Email };

            var ud = userDetails.ToList()[0];

            BookRentalViewModel model = new BookRentalViewModel
            {
                Id = brArg.Id,
                BookId = bookSelected.Id,
                RentalPrice = brArg.RentalPrice,
                Price = bookSelected.Price,
                Pages = bookSelected.Pages,
                FName = ud.FName,
                LName = ud.LName,
                EMail = ud.Email,
                BDay = ud.BDay,
                UserId = ud.Id,
                ScheduledEndDate = brArg.ScheduledEndDate,
                Author = bookSelected.Author,
                StartDate = brArg.StartDate,
                Availability = bookSelected.Availability,
                DateAdded = bookSelected.DateAdded,
                Description = bookSelected.Description,
                GenreId = bookSelected.GenreId,
                ISBN = bookSelected.ISBN,
                ImageUrl = bookSelected.ImageUrl,
                ProductDimensions = bookSelected.ProductDimensions,
                PublicationDate = bookSelected.PublicationDate,
                Publisher = bookSelected.Publisher,
                RentalDuratioin = brArg.RentalDuratioin,
                Status = brArg.Status.ToString(),
                Title = bookSelected.Title,
                AdditionalCharge = brArg.AdditionalCharge
            };

            model.Genre = db.Genres.FirstOrDefault(g => g.GenreId.Equals(bookSelected.GenreId));

            return model;
        }
    }
}