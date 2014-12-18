using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Collections.Generic;

namespace Greenpeace_Advisory.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

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
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        //public DbSet<FeedbackAPIModel> feedTest { get; set; }
        public DbSet<Feedback> Feedback { get; set; }
        public DbSet<Recipient> Recipients { get; set; }
        public DbSet<Advisory> Advisory { get; set; }
        public DbSet<Farmer> Farmers { get; set; }
        public DbSet<ContactDetail> ContactDetails { get; set; }
      
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

    }




    public class IdentityManager
    {
        RoleManager<ApplicationRole> _roleManager = new RoleManager<ApplicationRole>(
        new RoleStore<ApplicationRole>(new ApplicationDbContext()));

        UserManager<ApplicationUser> _userManager = new UserManager<ApplicationUser>(
            new UserStore<ApplicationUser>(new ApplicationDbContext()));

        ApplicationDbContext _db = new ApplicationDbContext();

        public bool RoleExists(string name)
        {
            return _roleManager.RoleExists(name);
        }

        public bool CreateRole(string name, string description)
        {
            // Swap ApplicationRole for IdentityRole:
            var idResult = _roleManager.Create(new ApplicationRole(name, description));
            return idResult.Succeeded;
        }


        public void ClearUserRoles(string userId)
        {
            var user = _userManager.FindById(userId);
            var currentRoles = new List<IdentityUserRole>();

            currentRoles.AddRange(user.Roles);

            _userManager.RemoveFromRole(userId, "SuperAdmin");
            _userManager.RemoveFromRole(userId, "Admin");
            _userManager.RemoveFromRole(userId, "Staff");

        }
    }
}