using BookRental.Models;
using BookRental.Utility;
using BookRental.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BookRental.Controllers
{
    [Authorize(Roles = SD.AdminUserRole)]
    public class UserController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        private ApplicationDbContext db;

        public UserController()
        {
            db = ApplicationDbContext.Create();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (null != db))
            {
                db.Dispose();
            }

            base.Dispose(disposing);
        }


        // GET: User
        public ActionResult Index()
        {
            var user = from u in db.Users
                       join m in db.MembershipTypes on u.MembershipTypeId equals m.Id
                       select new UserViewModel
                       {
                           Id = u.Id,
                           FName = u.FName,
                           LName = u.LName,
                           EMail = u.Email,
                           Phone = u.Phone,
                           BDay = u.BDay,
                           MembershipTypeId = u.MembershipTypeId,
                           MembershipTypes = (ICollection<MembershipType>)db.MembershipTypes.ToList().Where(n => n.Id.Equals(u.MembershipTypeId)),
                           Disable = u.Disable
                       };

            var usersList = user.ToList();

            return View(usersList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserViewModel u)
        {
            if (!ModelState.IsValid)
            {
                UserViewModel model = new UserViewModel
                {
                    Id = u.Id,
                    FName = u.FName,
                    LName = u.LName,
                    EMail = u.EMail,
                    Phone = u.Phone,
                    BDay = u.BDay,
                    MembershipTypeId = u.MembershipTypeId,
                    //MembershipTypes = (ICollection<MembershipType>)db.MembershipTypes.ToList().Where(n => n.Id.Equals(u.MembershipTypeId)),
                    MembershipTypes = db.MembershipTypes.ToList(),
                    Disable = u.Disable

                };

                return View("Edit", model);
            }
            else
            {
                var userDB = db.Users.Single(us => us.Id == u.Id);

                userDB.Id = u.Id;
                userDB.FName = u.FName;
                userDB.LName = u.LName;
                userDB.Email = u.EMail;
                userDB.Phone = u.Phone;
                userDB.BDay = u.BDay;
                userDB.MembershipTypeId = u.MembershipTypeId;
                //userDB.MembershipTypes = (ICollection<MembershipType>)db.MembershipTypes.ToList().Where(n => n.Id.Equals(u.MembershipTypeId));
                userDB.Disable = u.Disable;

            }

            db.SaveChanges();

            return RedirectToAction("Index", "User");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(string id)
        {
            if ((null == id) || (0 == id.Length))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ApplicationUser user = db.Users.Find(id);

            UserViewModel model = new UserViewModel(user);
            model.MembershipTypes = db.MembershipTypes.ToList();

            return View(model);
        }

        /// <summary>
        /// Delete GET
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(string id)
        {
            if ((null == id) || (0 == id.Length))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ApplicationUser user = db.Users.Find(id);

            UserViewModel model = new UserViewModel(user);
            model.MembershipTypes = db.MembershipTypes.ToList();

            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(String id)
        {
            if ((null == id) || (0 == id.Length))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ApplicationUser user = db.Users.Find(id);

            user.Disable = true;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult Edit(string Id)
        {
            if (null == Id)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ApplicationUser user = db.Users.Find(Id);

            if (null == user)
            {
                return HttpNotFound();
            }

            UserViewModel model = new UserViewModel(user);

            model.MembershipTypes = db.MembershipTypes.ToList();

            return View(model);
        }
    }
}