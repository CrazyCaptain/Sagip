using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Greenpeace_Advisory.Models;

namespace Greenpeace_Advisory.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class RegisterStaffController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: RegisterStaff
        //[Authorize(Roles = "SuperAdmin")]
        //[Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            RoleManager<ApplicationRole> _roleManager = new RoleManager<ApplicationRole>(
            new RoleStore<ApplicationRole>(new ApplicationDbContext()));

            var users = _roleManager.Roles.Single(x => x.Name == "Staff").Users;

            //  List<ApplicationUser> userList = db.Users.ToList();
            List<RegisterViewModel> model = new List<RegisterViewModel>();

            foreach (IdentityUserRole user in users)
            {
                ApplicationUser targetUser = db.Users.Find(user.UserId);
                if (targetUser.UserName!="sa")
                model.Add(new RegisterViewModel { Username = targetUser.UserName, FirstName = targetUser.FirstName, LastName = targetUser.LastName, Email = targetUser.Email });
            }

            return View(model);
        }

        // GET: RegisterStaff/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = db.Users.Where(r=>r.UserName==id).Single();
            if (user == null)
            {
                return HttpNotFound();
            }
            RegisterViewModel model = new RegisterViewModel() { Username = user.UserName, FirstName = user.FirstName, LastName = user.LastName, Email = user.Email };
          
            return View(model);
        }


        // GET: RegisterStaff/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RegisterStaff/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RegisterViewModel model)
        {
            if (ModelState.IsValid && model.Password != "" && model.Password != null)
            {
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
       
                var user = new ApplicationUser (){ UserName = model.Username, Email = model.Email,FirstName=model.FirstName,LastName=model.LastName};
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    result = UserManager.AddToRole(user.Id, "Staff");
                 
                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }
   
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // GET: RegisterStaff/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = db.Users.Where(r => r.UserName == id).Single();
            if (user == null)
            {
                return HttpNotFound();
            }
            RegisterViewModel model = new RegisterViewModel() { Username = user.UserName, FirstName = user.FirstName, LastName = user.LastName, Email = user.Email };
          
            return View(model);
        }


        // POST: RegisterStaff/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RegisterViewModel registerViewModel,string oldPassword)
        {
            if (ModelState.IsValid)
            {
                UserManager<ApplicationUser> _userManager = new UserManager<ApplicationUser>(
      new UserStore<ApplicationUser>(new ApplicationDbContext()));

                ApplicationUser user = db.Users.Where(r => r.UserName == registerViewModel.Username).Single();
                user.FirstName = registerViewModel.FirstName;
                user.LastName = registerViewModel.LastName;
                user.Email = registerViewModel.Email;
                if (registerViewModel.Password != "" && registerViewModel.Password != null)
                {             
                        var result = _userManager.ChangePassword(user.Id, oldPassword, registerViewModel.Password);
                        if (!result.Succeeded)
                        {
                           AddErrors(result);
                           return View(registerViewModel);
                        }
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(registerViewModel);
        }

        // GET: RegisterStaff/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = db.Users.Where(r => r.UserName == id).Single();
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(new RegisterViewModel() { FirstName = user.FirstName, Username = user.UserName, LastName = user.LastName, Email = user.Email });
        }

        // POST: RegisterStaff/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            if (ModelState.IsValid)
            {
                UserManager<ApplicationUser> _userManager = new UserManager<ApplicationUser>(
          new UserStore<ApplicationUser>(new ApplicationDbContext()));

                ApplicationUser user = db.Users.Where(r => r.UserName == id).Single();
               // var result = await _userManager.DeleteAsync(user);
                db.Users.Remove(user);
                db.SaveChanges();
                //if (result.Succeeded)
                //{
                //    return RedirectToAction("Index", "Home");
                //}
            }
            return RedirectToAction("Index");
         
        }


        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
