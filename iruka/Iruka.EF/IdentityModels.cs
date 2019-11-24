using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Iruka.EF.Model;
using System;

namespace Iruka.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string Certificate { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Picture { get; set; }
        public bool isActive { get; set; }
        public DateTime CreatedDate { get; set; }

        public ApplicationUser()
        {
            isActive = true;
        }

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
        public IDbSet<Test> Tests { get; set; }
        public IDbSet<Product> Product { get; set; }
        public IDbSet<Event> Event { get; set; }
        public IDbSet<IdentityUserRole> UserRoles { get; set; }
        public IDbSet<Coupon> Coupons { get; set; }
        public IDbSet<Transaction> Transactions { get; set; }

        public ApplicationDbContext() : base("DefaultConnection")
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

       
    }
}