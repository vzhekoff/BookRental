using BookRental.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace BookRental.Extensions
{
    public static class ThumbnailExtension
    {
        public static IEnumerable<ThumbnailModel> GetBookThumbnail(this List<ThumbnailModel> thumbnails,
            ApplicationDbContext db = null,
            string search = null)
        {
            List<ThumbnailModel> l = null;

            try
            {
                if (null == db)
                {
                    db = ApplicationDbContext.Create();
                }

                if ((null != search) && (search.Trim().Length > 0))
                {
                    search = search.Trim().ToLower();

                    l = (from b in db.Books
                         select new ThumbnailModel
                         {
                             BookId = b.Id,
                             Title = b.Title,
                             Description = b.Description,
                             ImageUrl = b.ImageUrl,
                             Link = "/BookDetail/Index/" + b.Id
                         }
                  ).Where(t => t.Title.ToLower().Contains(search)).ToList();

                }
                else
                {
                    l = (from b in db.Books
                         select new ThumbnailModel
                         {
                             BookId = b.Id,
                             Title = b.Title,
                             Description = b.Description,
                             ImageUrl = b.ImageUrl,
                             Link = "/BookDetail/Index/" + b.Id
                         }
                        ).ToList();

                }

                l.OrderBy(b => b.Title);
            }
            catch (Exception ex)
            {
                //Debug.Write(ex);

            }

            thumbnails = l;

            return l;
        }
    }
}