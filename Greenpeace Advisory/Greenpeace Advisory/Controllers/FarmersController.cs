using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Greenpeace_Advisory.Models;

namespace Greenpeace_Advisory.Controllers
{
    [Authorize]
    public class FarmersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Farmers
        public async Task<ActionResult> Index(string SearchFarmer)
        {
            var farmers = await db.Farmers.ToListAsync();

            if (!string.IsNullOrEmpty(SearchFarmer))
            {
                farmers = db.Farmers.Where(r => r.LastName.Contains(SearchFarmer)).ToList();
            }
          
            return View(farmers);
        }

        // GET: Farmers/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Farmer farmer = await db.Farmers.FindAsync(id);
            if (farmer == null)
            {
                return HttpNotFound();
            }

            ViewBag.ContactDetails = db.ContactDetails.Where(m => m.FarmerId == id).ToList();
            return View(farmer);
        }

        // GET: Farmers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Farmers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "FarmerId,LastName,FirstName,MiddleName")] Farmer farmer)
        {
            if (ModelState.IsValid)
            {
                db.Farmers.Add(farmer);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(farmer);
        }

        // GET: Farmers/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Farmer farmer = await db.Farmers.FindAsync(id);
            if (farmer == null)
            {
                return HttpNotFound();
            }
            return View(farmer);
        }

        // POST: Farmers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "FarmerId,LastName,FirstName,MiddleName")] Farmer farmer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(farmer).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(farmer);
        }

        // GET: Farmers/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Farmer farmer = await db.Farmers.FindAsync(id);
            if (farmer == null)
            {
                return HttpNotFound();
            }
            return View(farmer);
        }

        // POST: Farmers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Farmer farmer = await db.Farmers.FindAsync(id);
            db.Farmers.Remove(farmer);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
