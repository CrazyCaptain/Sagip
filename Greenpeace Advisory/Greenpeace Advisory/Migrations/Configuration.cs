namespace Greenpeace_Advisory.Migrations
{
    using Greenpeace_Advisory.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Greenpeace_Advisory.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Greenpeace_Advisory.Models.ApplicationDbContext context)
        {
            bool success = false;

            var idManager = new IdentityManager();
            success = idManager.CreateRole("SuperAdmin", "SuperAdministrator");
            if (!success == true) return;

            success = idManager.CreateRole("Admin", "Administrator");
            if (!success == true) return;

            success = idManager.CreateRole("Staff", "Staff");
            if (!success) return;

            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            //UserManager.UserValidator = new UserValidator<ApplicationUser>(UserManager) { AllowOnlyAlphanumericUserNames = false };

            //set a new password
            string password = "";
            //Create User=Admin with password=12345678
            var newUser = new ApplicationUser()
            {
                UserName = "",
                Email = ""

            };
            var adminresult = UserManager.Create(newUser, password);
            //            Add User Admin to Role Admin
            if (adminresult.Succeeded)
            {
                IdentityResult result = UserManager.AddToRole(newUser.Id, "SuperAdmin");
                result = UserManager.AddToRole(newUser.Id, "Admin");
                result = UserManager.AddToRole(newUser.Id, "Staff");

            }


            //context.Advisory.AddOrUpdate(m => m.AdvisoryId,
            //    new Advisory { AdvisoryId = 1 , Message = "Nondescript.", TimeStamp = DateTime.Now }
            //);

            //for (int i = 1; i < 40; i++)
            //{
            //    Recipient r = new Recipient();
            //    r.RecipientId = i;
            //    r.AdvisoryId = 1;
            //    r.Status = "Sent";
            //    r.ContactId = 0;
            //    context.Recipients.AddOrUpdate(r);    
            //}

            context.SaveChanges();

            base.Seed(context);
        }
    }
}
