﻿using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BookRental.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FName { get; set; }

        public string LName { get; set; }

        public string Phone { get; set; }

        public DateTime BDay { get; set; }

        public bool Disable { get; set; }

        public int MembershipTypeId { get; set; }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        /// <summary>
        /// 
        /// </summary>
        public DbSet<Book> Books { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<Genre> Genres { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<MembershipType> MembershipTypes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<BookRent> BookRental { get; set; }

        /// <summary>
        /// Ctr
        /// </summary>
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}