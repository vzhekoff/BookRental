using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using BookRental.Models;
using BookRental.Utility;

namespace BookRental.Controllers
{
    [Authorize(Roles = SD.AdminUserRole)]
    public class GenreController : Controller
    {
        private ApplicationDbContext db;

        public GenreController()
        {
            db = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
        }

        // GET: Genre
        public ActionResult Index()
        {
            return View(db.Genres.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Details(int? id)
        {
            if (null == id)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Genre genre = db.Genres.Find(id);

            if (null == genre)
            {
                return HttpNotFound();
            }

            return View(genre);
        }

        public ActionResult Edit(int? id)
        {
            if (null == id)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Genre genre = db.Genres.Find(id);

            if (null == genre)
            {
                return HttpNotFound();
            }

            return View(genre);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Genre genre)
        {
            if (ModelState.IsValid)
            {
                db.Entry(genre).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View();
        }

        public ActionResult Delete(int? id)
        {
            if (null == id)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Genre genre = db.Genres.Find(id);

            if (null == genre)
            {
                return HttpNotFound();
            }

            return View(genre);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            Genre genre = db.Genres.Find(id);

            db.Genres.Remove(genre);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Genre genre)
        {
            //var genreInDB = db.Genres.FirstOrDefault(g => g.GenreId.Equals(genre.GenreId));

            //genreInDB.Name = genre.Name;

            if (ModelState.IsValid)
            {
                db.Genres.Add(genre);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View();
        }
    }
}