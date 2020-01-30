﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookRental.ViewModels;
using BookRental.Extensions;
using BookRental.Models;

namespace BookRental.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string search = null)
        {
            var list = new List<ThumbnailModel>();
            var thumbnails = list.GetBookThumbnail(ApplicationDbContext.Create(), search);
            var count = thumbnails.Count() / 4;
            var model = new List<ThumbnailBoxViewModel>();

            for (int i = 0; i <= count; i++)
            {
                model.Add(new ThumbnailBoxViewModel
                {
                    Thumbnails = thumbnails.Skip(i * 4).Take(4)
                });
            }

            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}